using System;
using System.Linq;

namespace UpdateFieldCodeGenerator
{
    public static class EnumExtensions
    {
        public static string ToFlagsExpression<T>(this T flags, string flagPrefix = "", string expressionPrefix = "", string expressionSuffix = "") where T : Enum
        {
            var flagsStr = flags.ToString();
            var flagParts = flagsStr.Split(',');
            if (flagParts.Length > 1)
                return $"{expressionPrefix}{string.Join(" | ", flagParts.Select(flag => flagPrefix + flag.Trim()))}{expressionSuffix}";

            return flagsStr;
        }
    }
}
