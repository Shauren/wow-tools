using System.Reflection;
using System.Runtime.CompilerServices;

namespace UpdateFieldCodeGenerator
{
    public class UpdateField
    {
        public Type Type { get; }
        public UpdateFieldFlag Flag { get; }
        public int Size { get; }
        public int BitSize { get; }
        public int Order { get; }
        public CustomUpdateFieldFlag CustomFlag { get; }
        public IReadOnlyCollection<Condition> Conditions { get; }
        public string Comment { get; }
        public FieldInfo SizeForField { get; }
        public string UpdateBitGroup { get; }

        public UpdateField(Type type, UpdateFieldFlag flag, int size = 0, int bitSize = 0, CustomUpdateFieldFlag customFlag = CustomUpdateFieldFlag.None,
            IReadOnlyCollection<Condition> conditions = null, string comment = null, string updateBitGroup = null, [CallerLineNumber] int order = 0)
        {
            if (size == 0 && type.IsArray)
                throw new ArgumentException($"Given type is an array with 0 length ({type.Name})", nameof(type));
            if (size != 0 && !type.IsArray)
                throw new ArgumentException($"Given type is not an array ({type.Name})", nameof(type));

            if (type == typeof(Bits) && (bitSize == 0 || bitSize > 32))
                throw new ArgumentOutOfRangeException(nameof(bitSize), bitSize, "must be in range 1-32");

            (Type, Flag, Size, BitSize, CustomFlag, Conditions, Comment, Order, UpdateBitGroup) = (type, flag, size, bitSize, customFlag, conditions ?? new List<Condition>(), comment, order, updateBitGroup);
        }

        public UpdateField(Type type, UpdateFieldFlag flag, FieldInfo sizeForField, int size = 0, int bitSize = 0, CustomUpdateFieldFlag customFlag = CustomUpdateFieldFlag.None,
            IReadOnlyCollection<Condition> conditions = null, string comment = null, string updateBitGroup = null, [CallerLineNumber] int order = 0)
            : this(type, flag, size, bitSize, customFlag, conditions, comment, updateBitGroup, order)
        {
            SizeForField = sizeForField ?? throw new ArgumentNullException(nameof(sizeForField));
        }

        public record struct Condition(FieldInfo FieldToCompare, string OperatorAndConstant);
    }
}
