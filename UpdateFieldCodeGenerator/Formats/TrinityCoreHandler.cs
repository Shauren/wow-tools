using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UpdateFieldCodeGenerator.Formats
{
    public class TrinityCoreHandler : UpdateFieldHandlerBase
    {
        private readonly Type _updateFieldType = CppTypes.CreateType("UpdateField", "T", "Bit");
        private readonly Type _arrayUpdateFieldType = CppTypes.CreateType("UpdateFieldArray", "T", "Size", "PrimaryBit", "FirstElementBit");
        private readonly Type _dynamicUpdateFieldType = CppTypes.CreateType("DynamicUpdateField", "T", "Bit");

        private UpdateFieldFlag _allUsedFlags;
        private readonly IDictionary<int, UpdateFieldFlag> _flagByUpdateBit = new Dictionary<int, UpdateFieldFlag>();

        public TrinityCoreHandler() : base(new StreamWriter("UpdateFields.cpp"), new StreamWriter("UpdateFields.h"))
        {
        }

        public override void OnStructureBegin(Type structureType, bool create, bool writeUpdateMasks)
        {
            base.OnStructureBegin(structureType, create, writeUpdateMasks);
            _allUsedFlags = UpdateFieldFlag.None;
            _flagByUpdateBit.Clear();
            _flagByUpdateBit[0] = UpdateFieldFlag.None;

            var structureName = RenameType(structureType);

            if (!_create)
            {
                _header.WriteLine($"struct {structureName}");
                _header.WriteLine("{");
            }

            if (_create)
                _source.WriteLine($"void {structureName}::WriteCreate(ByteBuffer& data, EnumClassFlag<UpdateFieldFlag> flags, Player const* target) const");
            else
                _source.WriteLine($"void {structureName}::WriteUpdate(ByteBuffer& data, EnumClassFlag<UpdateFieldFlag> flags, Player const* target) const");

            _source.WriteLine("{");
        }

        public override void OnStructureEnd(bool needsFlush, bool hadArrayFields)
        {
            ++_bitCounter;
            if (!_create)
            {
                if (_writeUpdateMasks)
                    _header.WriteLine($"    UpdateMask<{_bitCounter}> m_changesMask;");
                _header.WriteLine("}");
                _header.WriteLine();
                _header.Flush();
            }

            if (!_create && _writeUpdateMasks)
            {
                if (_allUsedFlags != UpdateFieldFlag.None)
                {
                    var bitMaskByFlag = new Dictionary<UpdateFieldFlag, BitArray>();
                    for (var i = 0; i < _bitCounter; ++i)
                    {
                        if (_flagByUpdateBit.TryGetValue(i, out var flag) && flag != UpdateFieldFlag.None)
                        {
                            for (var j = 0; j < 8; ++j)
                                if ((flag & (UpdateFieldFlag)(1 << j)) != UpdateFieldFlag.None)
                                    bitMaskByFlag.ComputeIfAbsent((UpdateFieldFlag)(1 << j), k => new BitArray(_bitCounter)).Set(i, true);
                        }
                        else
                            bitMaskByFlag.ComputeIfAbsent(UpdateFieldFlag.None, k => new BitArray(_bitCounter)).Set(i, true);
                    }

                    var noneFlags = new int[(_bitCounter + 31) / 32];
                    bitMaskByFlag[UpdateFieldFlag.None].CopyTo(noneFlags, 0);

                    _source.WriteLine($"    UpdateMask<{_bitCounter}> allowedMaskForTarget({{ {string.Join(", ", noneFlags.Select(v => "0x" + v.ToString("X8")))} }});");
                    for (var j = 0; j < 8; ++j)
                    {
                        if ((_allUsedFlags & (UpdateFieldFlag)(1 << j)) != UpdateFieldFlag.None)
                        {
                            var flagArray = new int[(_bitCounter + 31) / 32];
                            bitMaskByFlag[(UpdateFieldFlag)(1 << j)].CopyTo(flagArray, 0);
                            _source.WriteLine($"    if (flags.HasFlag(UpdateFieldFlag::{(UpdateFieldFlag)(1 << j)}))");
                            _source.WriteLine($"        allowedMaskForTarget |= {{ {string.Join(", ", flagArray.Select(v => "0x" + v.ToString("X8")))} }}");
                            _source.WriteLine();
                        }
                    }

                    _source.WriteLine($"    UpdateMask<{_bitCounter}> changesMask = m_changesMask & allowedMaskForTarget;");
                    _source.WriteLine();
                }
                else
                {
                    _source.WriteLine($"    UpdateMask<{_bitCounter}> const& changesMask = m_changesMask;");
                    _source.WriteLine();
                }
                var maskBlocks = (_bitCounter + 31) / 32;
                if (maskBlocks > 1 || hadArrayFields)
                {
                    if (maskBlocks >= 32)
                    {
                        _source.WriteLine($"    for (std::size_t i = 0; i < {maskBlocks / 32}; ++i)");
                        _source.WriteLine($"        data << uint32(changesMask.GetBlocksMask(i));");
                        if ((maskBlocks % 32) != 0)
                            _source.WriteLine($"    data.WriteBits(changesMask.GetBlocksMask({maskBlocks / 32}), {maskBlocks % 32});");
                    }
                    else
                        _source.WriteLine($"    data.WriteBits(changesMask.GetBlocksMask(0), {maskBlocks});");

                    if (maskBlocks > 1)
                    {
                        _source.WriteLine($"    for (std::size_t i = 0; i < {maskBlocks}; ++i)");
                        _source.WriteLine("        if (changesMask.GetBlock(i))");
                        _source.WriteLine("            data.WriteBits(changesMask.GetBlock(i), 32);");
                    }
                    else
                    {
                        _source.WriteLine("    if (changesMask.GetBlock(0))");
                        _source.WriteLine("        data.WriteBits(changesMask.GetBlock(0), 32);");
                    }
                }
                else
                    _source.WriteLine($"    data.WriteBits(changesMask.GetBlock(0), {_bitCounter});");

                _source.WriteLine();
            }

            PostProcessFieldWrites();

            foreach (var (_, _, Write) in _fieldWrites)
                Write();

            if (needsFlush)
                _source.WriteLine($"{GetIndent()}data.FlushBits();");

            _source.WriteLine("}");
            _source.WriteLine();
            _source.Flush();
        }

        public override IReadOnlyList<FlowControlBlock> OnField(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow)
        {
            _allUsedFlags |= updateField.Flag;

            var flowControl = new List<FlowControlBlock>();
            if (_create && updateField.Flag != UpdateFieldFlag.None)
                flowControl.Add(new FlowControlBlock { Statement = $"if (flags.HasFlag({updateField.Flag.ToFlagsExpression("UpdateFieldFlag::")}))" });

            var type = updateField.Type;
            var nameUsedToWrite = name;
            var arrayLoopBlockIndex = -1;
            var indexLetter = 'i';
            if (type.IsArray)
            {
                flowControl.Add(new FlowControlBlock { Statement = $"for (std::size_t {indexLetter} = 0; {indexLetter} < {updateField.Size}; ++{indexLetter})" });
                nameUsedToWrite += $"[{indexLetter}]";
                type = type.GetElementType();
                arrayLoopBlockIndex = flowControl.Count;
                ++indexLetter;
            }
            if (typeof(DynamicUpdateField).IsAssignableFrom(type))
            {
                flowControl.Add(new FlowControlBlock { Statement = $"for (std::size_t {indexLetter} = 0; {indexLetter} < {name}.size(); ++{indexLetter})" });
                if (!_create)
                    flowControl.Add(new FlowControlBlock { Statement = $"if ({name}.HasChanged({indexLetter}))" });

                nameUsedToWrite += $"[{indexLetter}]";
                type = type.GenericTypeArguments[0];
                ++indexLetter;
            }
            if (typeof(BlzVectorField).IsAssignableFrom(type))
            {
                flowControl.Add(new FlowControlBlock { Statement = $"for (std::size_t {indexLetter} = 0; {indexLetter} < {name}.size(); ++{indexLetter})" });
                nameUsedToWrite += $"[{indexLetter}]";
                type = type.GenericTypeArguments[0];
                ++indexLetter;
            }

            if (!_create && _writeUpdateMasks)
            {
                var newField = false;
                var nameForIndex = updateField.SizeForField != null ? updateField.SizeForField.Name : name;
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

                if (_flagByUpdateBit.ContainsKey(bitIndex[0]))
                    _flagByUpdateBit[bitIndex[0]] |= updateField.Flag;
                else
                    _flagByUpdateBit[bitIndex[0]] = updateField.Flag;

                if (updateField.Type.IsArray)
                {
                    flowControl.Insert(0, new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[0]}])" });
                    if (newField)
                    {
                        bitIndex.AddRange(Enumerable.Range(_bitCounter + 1, updateField.Size));
                        _bitCounter += updateField.Size;
                    }
                    flowControl.Insert(arrayLoopBlockIndex + 1, new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[1]} + i])" });
                    for (var i = 0; i < updateField.Size; ++i)
                        _flagByUpdateBit[bitIndex[1] + i] = updateField.Flag;
                }
                else
                {
                    flowControl.Insert(0, new FlowControlBlock { Statement = $"if (changesMask[{_blockGroupBit}])" });
                    flowControl.Insert(1, new FlowControlBlock { Statement = $"if (changesMask[{bitIndex[0]}])" });
                }

                _previousFieldCounters = bitIndex;
            }

            _fieldWrites.Add((name, false, () =>
            {
                WriteControlBlocks(_source, flowControl, previousControlFlow);
                WriteField(nameUsedToWrite, type, updateField.BitSize);
                _indent = 1;
            }
            ));

            if (!_create && updateField.SizeForField == null)
                WriteFieldDeclaration(name, updateField);

            return flowControl;
        }

        public override IReadOnlyList<FlowControlBlock> OnDynamicFieldSizeCreate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow)
        {
            var flowControl = new List<FlowControlBlock>();

            var nameUsedToWrite = name;
            if (updateField.Type.IsArray)
            {
                flowControl.Add(new FlowControlBlock { Statement = $"for (std::size_t i = 0; i < {updateField.Size}; ++i)" });
                nameUsedToWrite += "[i]";
            }

            _fieldWrites.Add((name, true, () =>
            {
                WriteControlBlocks(_source, flowControl, previousControlFlow);
                _source.WriteLine($"{GetIndent()}data << uint32({nameUsedToWrite}.size());");
                _indent = 1;
            }));
            return flowControl;
        }

        public override IReadOnlyList<FlowControlBlock> OnDynamicFieldSizeUpdate(string name, UpdateField updateField, IReadOnlyList<FlowControlBlock> previousControlFlow)
        {
            var flowControl = new List<FlowControlBlock>();

            var nameUsedToWrite = name;
            if (updateField.Type.IsArray)
            {
                flowControl.Add(new FlowControlBlock { Statement = $"for (std::size_t i = 0; i < {updateField.Size}; ++i)" });
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
                _source.WriteLine($"{GetIndent()}{nameUsedToWrite}.WriteUpdateMask(data);");
                _indent = 1;
            }));
            return flowControl;
        }

        private void WriteField(string name, Type type, int bitSize)
        {
            _source.Write(GetIndent());
            if (name.EndsWith(".size()"))
            {
                if (_create)
                    _source.WriteLine($"data << uint32({name});");
                else
                    _source.WriteLine($"data.WriteBits({name}, 32);");
                return;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Object:
                    if (type == typeof(WowGuid))
                        _source.WriteLine($"data << {name};");
                    else if (type == typeof(Bits))
                        _source.WriteLine($"data.WriteBits({name}, {bitSize});");
                    else if (type == typeof(Vector2))
                        _source.WriteLine($"data << {name};");
                    else if (type == typeof(Quaternion))
                    {
                        _source.WriteLine($"data << float({name}->x);");
                        _source.WriteLine($"{GetIndent()}data << float({name}->y);");
                        _source.WriteLine($"{GetIndent()}data << float({name}->z);");
                        _source.WriteLine($"{GetIndent()}data << float({name}->w);");
                    }
                    else if (_create)
                        _source.WriteLine($"{name}.WriteCreate(data, flags, target);");
                    else
                        _source.WriteLine($"{name}.WriteUpdate(data, flags, target);");
                    break;
                case TypeCode.Boolean:
                    _source.WriteLine($"data.WriteBit({name});");
                    break;
                case TypeCode.SByte:
                    _source.WriteLine($"data << int8({name});");
                    break;
                case TypeCode.Byte:
                    _source.WriteLine($"data << uint8({name});");
                    break;
                case TypeCode.Int16:
                    _source.WriteLine($"data << int16({name});");
                    break;
                case TypeCode.UInt16:
                    _source.WriteLine($"data << uint16({name});");
                    break;
                case TypeCode.Int32:
                    _source.WriteLine($"data << int32({name});");
                    break;
                case TypeCode.UInt32:
                    _source.WriteLine($"data << uint32({name});");
                    break;
                case TypeCode.Int64:
                    _source.WriteLine($"data << int64({name});");
                    break;
                case TypeCode.UInt64:
                    _source.WriteLine($"data << uint64({name});");
                    break;
                case TypeCode.Single:
                    _source.WriteLine($"data << float({name});");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        private void WriteFieldDeclaration(string name, UpdateField declarationType)
        {
            var fieldGeneratedType = CppTypes.GetCppType(declarationType.Type);
            if (_writeUpdateMasks)
            {
                var bit = CppTypes.CreateConstantForTemplateParameter(_fieldBitIndex[name][0]);
                if (fieldGeneratedType.IsArray)
                {
                    if (typeof(DynamicUpdateField).IsAssignableFrom(fieldGeneratedType.GetElementType()))
                        fieldGeneratedType = _arrayUpdateFieldType.MakeGenericType(
                            _dynamicUpdateFieldType.MakeGenericType(fieldGeneratedType.GetElementType().GenericTypeArguments[0], CppTypes.CreateConstantForTemplateParameter(-1)),
                            CppTypes.CreateConstantForTemplateParameter(declarationType.Size),
                            bit,
                            CppTypes.CreateConstantForTemplateParameter(_fieldBitIndex[name][1]));
                    else
                        fieldGeneratedType = _arrayUpdateFieldType.MakeGenericType(fieldGeneratedType.GetElementType(),
                            CppTypes.CreateConstantForTemplateParameter(declarationType.Size),
                            bit,
                            CppTypes.CreateConstantForTemplateParameter(_fieldBitIndex[name][1]));
                }
                else if (typeof(DynamicUpdateField).IsAssignableFrom(declarationType.Type))
                    fieldGeneratedType = _dynamicUpdateFieldType.MakeGenericType(fieldGeneratedType.GenericTypeArguments[0], bit);
                else
                    fieldGeneratedType = _updateFieldType.MakeGenericType(CppTypes.GetCppType(declarationType.Type), bit);

                _header.WriteLine($"    {TypeHandler.GetFriendlyName(fieldGeneratedType)} {name};");
            }
            else if (fieldGeneratedType.IsArray)
                _header.WriteLine($"    {TypeHandler.GetFriendlyName(fieldGeneratedType.GetElementType())} {name}[{declarationType.Size}];");
            else
                _header.WriteLine($"    {TypeHandler.GetFriendlyName(fieldGeneratedType)} {name};");
        }

        public override void FinishControlBlocks(IReadOnlyList<FlowControlBlock> previousControlFlow)
        {
            _fieldWrites.Add((string.Empty, false, () =>
            {
                FinishControlBlocks(_source, previousControlFlow);
            }));
        }

        public override void FinishBitPack()
        {
            _fieldWrites.Add((string.Empty, false, () =>
            {
                _source.WriteLine($"{GetIndent()}data.FlushBits();");
            }));
        }
    }
}
