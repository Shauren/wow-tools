using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UpdateFieldCodeGenerator.Formats
{
    public class WowPacketParserHandler : UpdateFieldHandlerBase
    {
        public WowPacketParserHandler() : base(new StreamWriter("UpdateFieldsHandler.cs"), null)
        {
        }

        public override void OnStructureBegin(Type structureType, bool create, bool writeUpdateMasks)
        {
            base.OnStructureBegin(structureType, create, writeUpdateMasks);
            var structureName = RenameType(structureType);

            if (_create)
            {
                _header = new StreamWriter($"{structureName}.cs");
                _header.WriteLine("using WowPacketParser.Misc;");
                _header.WriteLine();
                _header.WriteLine($"namespace WowPacketParserModule.V8_0_1_27101.UpdateFields.V8_1_5_29495");
                _header.WriteLine("{");
                _header.WriteLine($"    public class {structureName} : I{structureName}");
                _header.WriteLine("    {");
            }

            if (_create)
                _source.WriteLine($"public static {structureName} ReadCreate{structureName}(Packet packet, UpdateFieldFlag flags, params object[] indexes)");
            else
                _source.WriteLine($"public static void ReadUpdate{structureName}(Packet packet, {structureName} data, params object[] indexes)");

            _source.WriteLine("{");
            if (_create)
                _source.WriteLine($"    var data = new {structureName}();");
        }

        public override void OnStructureEnd(bool needsFlush, bool hadArrayFields)
        {
            if (_create)
            {
                _header.WriteLine("    }");
                _header.WriteLine("}");
                _header.WriteLine();
                _header.Close();
                _header = null;
            }

            if (!_create && _writeUpdateMasks)
            {
                ++_bitCounter;
                var maskBlocks = (_bitCounter + 31) / 32;
                _source.WriteLine($"    var rawChangesMask = new int[{maskBlocks}];");
                if (maskBlocks > 1 || hadArrayFields)
                {
                    _source.WriteLine($"    var rawMaskMask = new int[{(maskBlocks + 31) / 32}]");
                    if (maskBlocks >= 32)
                    {
                        _source.WriteLine($"    for (var i = 0; i < {maskBlocks / 32}; ++i)");
                        _source.WriteLine($"        rawMaskMask[i] = packet.ReadInt32();");
                        if ((maskBlocks % 32) != 0)
                            _source.WriteLine($"    rawMaskMask[{maskBlocks / 32}] = (int)packet.ReadBits({maskBlocks % 32});");
                    }
                    else
                        _source.WriteLine($"    rawMaskMask[0] = (int)packet.ReadBits({maskBlocks});");

                    _source.WriteLine("    var maskMask = new BitArray(rawMaskMask);");
                    if (maskBlocks > 1)
                    {
                        _source.WriteLine($"    for (var i = 0; i < {maskBlocks}; ++i)");
                        _source.WriteLine("        if (maskMask[i])");
                        _source.WriteLine("            rawChangesMask[i] = (int)packet.ReadBits(32);");
                    }
                    else
                    {
                        _source.WriteLine("    if (maskMask[0])");
                        _source.WriteLine("        rawChangesMask[0] = (int)packet.ReadBits(32);");
                    }
                }
                else
                    _source.WriteLine($"    rawChangesMask[0] = (int)packet.ReadBits({_bitCounter});");

                _source.WriteLine("    var changesMask = new BitArray(rawChangesMask);");
                _source.WriteLine();
            }

            PostProcessFieldWrites();

            foreach (var (_, _, Write) in _fieldWrites)
                Write();

            if (needsFlush)
                _source.WriteLine("    packet.ResetBitReader();");

            if (_create)
                _source.WriteLine("    return data;");

            _source.WriteLine("}");
            _source.WriteLine();
            _source.Flush();
        }

        public override IReadOnlyList<FlowControlBlock> OnField(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow)
        {
            name = RenameField(name);

            var flowControl = new List<FlowControlBlock>();
            if (_create && updateField.Flag != UpdateFieldFlag.None)
                flowControl.Add(new FlowControlBlock { Statement = $"if ((flags & {updateField.Flag.ToFlagsExpression("UpdateFieldFlag.", "(", ")")}) != UpdateFieldFlag.None)" });

            var type = updateField.Type;
            var outputFieldName = name;
            var nextIndex = string.Empty;
            var declarationType = updateField.Type;
            var declarationSettable = true;
            var arrayLoopBlockIndex = -1;
            var indexLetter = 'i';
            if (type.IsArray)
            {
                flowControl.Add(new FlowControlBlock { Statement = $"for (var {indexLetter} = 0; {indexLetter} < {updateField.Size}; ++{indexLetter})" });
                outputFieldName += $"[{indexLetter}]";
                type = type.GetElementType();
                nextIndex += ", " + indexLetter;
                declarationSettable = false;
                arrayLoopBlockIndex = flowControl.Count;
                ++indexLetter;
            }
            if (typeof(DynamicUpdateField).IsAssignableFrom(type))
            {
                flowControl.Add(new FlowControlBlock { Statement = $"for (var {indexLetter} = 0; {indexLetter} < data.{outputFieldName}.Count; ++{indexLetter})" });
                if (!_create)
                    flowControl.Add(new FlowControlBlock { Statement = $"if (data.{name}.UpdateMask[{indexLetter}])" });

                outputFieldName += $"[{indexLetter}]";
                type = type.GenericTypeArguments[0];
                nextIndex += ", " + indexLetter;
                declarationSettable = false;
                ++indexLetter;
            }
            if (typeof(BlzVectorField).IsAssignableFrom(type))
            {
                flowControl.Add(new FlowControlBlock { Statement = $"for (var {indexLetter} = 0; {indexLetter} < data.{outputFieldName}.Count; ++{indexLetter})" });
                outputFieldName += $"[{indexLetter}]";
                type = type.GenericTypeArguments[0];
                nextIndex += ", " + indexLetter;
                declarationType = type.MakeArrayType();
                ++indexLetter;
            }
            if (typeof(Bits).IsAssignableFrom(type))
            {
                declarationType = typeof(uint);
            }

            if (!_create && _writeUpdateMasks)
            {
                var newField = false;
                var nameForIndex = updateField.SizeForField != null ? RenameField(updateField.SizeForField.Name) : name;
                if (!_fieldBitIndex.TryGetValue(nameForIndex, out var bitIndex))
                {
                    bitIndex = new List<int>();
                    if (flowControl.Count == 0 || !FlowControlBlock.AreChainsAlmostEqual(previousControlFlow, flowControl))
                    {
                        if (!updateField.Type.IsArray)
                        {
                            ++_nonArrayBitCounter;
                            if (_nonArrayBitCounter == 32)
                            {
                                _blockGroupBit = ++_bitCounter;
                                _nonArrayBitCounter = 1;
                            }
                        }

                        bitIndex.Add(++_bitCounter);
                    }
                    else
                    {
                        if (_previousFieldCounters == null || _previousFieldCounters.Count == 1)
                            throw new Exception("Expected previous field to have been an array");

                        bitIndex.Add(_previousFieldCounters[0]);
                    }

                    _fieldBitIndex[nameForIndex] = bitIndex;
                    newField = true;
                }

                if (updateField.Type.IsArray)
                {
                    flowControl.Insert(0, new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[0]}])" });
                    if (newField)
                    {
                        bitIndex.AddRange(Enumerable.Range(_bitCounter + 1, updateField.Size));
                        _bitCounter += updateField.Size;
                    }
                    flowControl.Insert(arrayLoopBlockIndex + 1, new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[1]} + i])" });
                }
                else
                {
                    flowControl.Insert(0, new FlowControlBlock { Statement = $"if (changesMask[{_blockGroupBit}])" });
                    flowControl.Insert(1, new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[0]}])" });
                }

                _previousFieldCounters = bitIndex;
            }

            Type interfaceType = null;
            if (updateField.SizeForField != null)
            {
                type = (updateField.SizeForField.GetValue(null) as UpdateField).Type;
                type = type.GenericTypeArguments[0];
                interfaceType = TypeHandler.ConvertToInterfaces(type, rawName => RenameType(rawName));
            }

            _fieldWrites.Add((name, false, () =>
            {
                WriteControlBlocks(_source, flowControl, previousControlFlow);
                WriteField(name, outputFieldName, type, updateField.BitSize, nextIndex, interfaceType);
                _indent = 1;
            }));

            if (_create && updateField.SizeForField == null)
                WriteFieldDeclaration(name, updateField, declarationType, declarationSettable);

            return flowControl;
        }

        public override IReadOnlyList<FlowControlBlock> OnDynamicFieldSizeCreate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow)
        {
            name = RenameField(name);
            var flowControl = new List<FlowControlBlock>();

            var nameUsedToWrite = name;
            if (updateField.Type.IsArray)
            {
                flowControl.Add(new FlowControlBlock { Statement = $"for (var i = 0; i < {updateField.Size}; ++i)" });
                nameUsedToWrite += "[i]";
            }

            _fieldWrites.Add((name, true, () =>
            {
                WriteControlBlocks(_source, flowControl, previousControlFlow);
                _source.WriteLine($"{GetIndent()}data.{nameUsedToWrite}.Resize(packet.ReadUInt32());");
                _indent = 1;
            }));
            return flowControl;
        }

        public override IReadOnlyList<FlowControlBlock> OnDynamicFieldSizeUpdate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow)
        {
            name = RenameField(name);
            var flowControl = new List<FlowControlBlock>();

            var nameUsedToWrite = name;
            if (updateField.Type.IsArray)
            {
                flowControl.Add(new FlowControlBlock { Statement = $"for (var i = 0; i < {updateField.Size}; ++i)" });
                nameUsedToWrite += "[i]";
            }

            if (_writeUpdateMasks)
            {
                if (!_fieldBitIndex.TryGetValue(name, out var bitIndex))
                {
                    bitIndex = new List<int>();
                    if (flowControl.Count == 0 || !FlowControlBlock.AreChainsAlmostEqual(previousControlFlow, flowControl))
                    {
                        if (!updateField.Type.IsArray)
                        {
                            ++_nonArrayBitCounter;
                            if (_nonArrayBitCounter == 32)
                            {
                                _blockGroupBit = ++_bitCounter;
                                _nonArrayBitCounter = 1;
                            }
                        }

                        bitIndex.Add(++_bitCounter);
                    }
                    else
                    {
                        if (_previousFieldCounters == null || _previousFieldCounters.Count == 1)
                            throw new Exception("Expected previous field to have been an array");

                        bitIndex.Add(_previousFieldCounters[0]);
                    }

                    _fieldBitIndex[name] = bitIndex;
                }

                if (updateField.Type.IsArray)
                {
                    flowControl.Insert(0, new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[0]}])" });
                    bitIndex.AddRange(Enumerable.Range(_bitCounter + 1, updateField.Size));
                    flowControl.Add(new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[1]} + i])" });
                    _bitCounter += updateField.Size;
                }
                else
                {
                    flowControl.Insert(0, new FlowControlBlock { Statement = $"if (changesMask[{_blockGroupBit}])" });
                    flowControl.Insert(1, new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[0]}])" });
                }

                _previousFieldCounters = bitIndex;
            }

            _fieldWrites.Add((name, true, () =>
            {
                WriteControlBlocks(_source, flowControl, previousControlFlow);
                _source.WriteLine($"{GetIndent()}data.{name}.ReadUpdateMask(packet);");
                _indent = 1;
            }));
            return flowControl;
        }

        private void WriteField(string name, string outputFieldName, Type type, int bitSize, string nextIndex, Type interfaceType)
        {
            _source.Write(GetIndent());
            if (name.EndsWith(".size()"))
            {
                outputFieldName = outputFieldName.Substring(0, outputFieldName.Length - 7);
                var sizeRead = _create ? "packet.ReadUInt32()" : "packet.ReadBits(32)";
                var interfaceName = RenameType(TypeHandler.GetFriendlyName(interfaceType));
                if (_create)
                    _source.WriteLine($"data.{outputFieldName} = new {interfaceName}[{sizeRead}];");
                else
                    _source.WriteLine($"data.{outputFieldName} = Enumerable.Range(0, {sizeRead}).Select(x => new {RenameType(TypeHandler.GetFriendlyName(type))}()).Cast<{interfaceName}>().ToArray();");
                return;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Object:
                    if (type == typeof(WowGuid))
                        _source.WriteLine($"data.{outputFieldName} = packet.ReadPackedGuid128(\"{name}\", indexes{nextIndex});");
                    else if (type == typeof(Bits))
                        _source.WriteLine($"data.{outputFieldName} = packet.ReadBits(\"{name}\", {bitSize});");
                    else if (type == typeof(Vector2))
                        _source.WriteLine($"data.{outputFieldName} = packet.ReadVector2(\"{name}\", indexes{nextIndex});");
                    else if (type == typeof(Quaternion))
                        _source.WriteLine($"data.{outputFieldName} = packet.ReadQuaternion(\"{name}\", indexes{nextIndex});");
                    else if (_create)
                        _source.WriteLine($"data.{outputFieldName} = ReadCreate{RenameType(type)}(packet, flags, indexes, \"{name}\"{nextIndex});");
                    else
                        _source.WriteLine($"ReadUpdate{RenameType(type)}(packet, data.{outputFieldName}, indexes, \"{name}\"{nextIndex});");
                    break;
                case TypeCode.Boolean:
                    _source.WriteLine($"data.{outputFieldName} = packet.ReadBit(\"{name}\", indexes{nextIndex});");
                    break;
                case TypeCode.SByte:
                    _source.WriteLine($"data.{outputFieldName} = packet.ReadSByte(\"{name}\", indexes{nextIndex});");
                    break;
                case TypeCode.Byte:
                    _source.WriteLine($"data.{outputFieldName} = packet.ReadByte(\"{name}\", indexes{nextIndex});");
                    break;
                case TypeCode.Int16:
                    _source.WriteLine($"data.{outputFieldName} = packet.ReadInt16(\"{name}\", indexes{nextIndex});");
                    break;
                case TypeCode.UInt16:
                    _source.WriteLine($"data.{outputFieldName} = packet.ReadUInt16(\"{name}\", indexes{nextIndex});");
                    break;
                case TypeCode.Int32:
                    _source.WriteLine($"data.{outputFieldName} = packet.ReadInt32(\"{name}\", indexes{nextIndex});");
                    break;
                case TypeCode.UInt32:
                    _source.WriteLine($"data.{outputFieldName} = packet.ReadUInt32(\"{name}\", indexes{nextIndex});");
                    break;
                case TypeCode.Int64:
                    _source.WriteLine($"data.{outputFieldName} = packet.ReadInt64(\"{name}\", indexes{nextIndex});");
                    break;
                case TypeCode.UInt64:
                    _source.WriteLine($"data.{outputFieldName} = packet.ReadUInt64(\"{name}\", indexes{nextIndex});");
                    break;
                case TypeCode.Single:
                    _source.WriteLine($"data.{outputFieldName} = packet.ReadSingle(\"{name}\", indexes{nextIndex});");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        private void WriteFieldDeclaration(string name, UpdateField updateField, Type declarationType, bool declarationSettable)
        {
            declarationType = TypeHandler.ConvertToInterfaces(declarationType, rawName => RenameType(rawName));
            _header.Write($"        public {TypeHandler.GetFriendlyName(declarationType)} {name} {{ get;{(declarationSettable ? " set;" : "")} }}");
            if (typeof(DynamicUpdateField).IsAssignableFrom(updateField.Type))
                _header.Write($" = new {TypeHandler.GetFriendlyName(declarationType)}();");
            else if (updateField.Type.IsArray)
                _header.Write($" = new {TypeHandler.GetFriendlyName(declarationType.GetElementType())}[{updateField.Size}];");

            _header.WriteLine();
        }

        protected override string RenameType(Type type)
        {
            return RenameType(type.Name);
        }

        private string RenameType(string name)
        {
            if (name.StartsWith("CG") && char.IsUpper(name[2]))
                name = name.Substring(2);
            if (name.EndsWith("_C"))
                name = name.Substring(0, name.Length - 2);
            if (name.StartsWith("JamMirror"))
                name = name.Substring(9);
            return name;
        }

        protected override string RenameField(string name)
        {
            name = name.Replace("m_", "");
            return char.ToUpperInvariant(name[0]) + name.Substring(1);
        }

        public override void FinishControlBlocks(IReadOnlyList<FlowControlBlock> previousControlFlow)
        {
            _fieldWrites.Add((string.Empty, false, () =>
            {
                FinishControlBlocks(_source, previousControlFlow);
            }));
        }
    }
}
