using System;
using System.Linq;

namespace UpdateFieldCodeGenerator
{
    public static class EnumExtensions
    {
        public static string ToFlagsExpression<T>(this T flags, string separator = " | ",
            string flagPrefix = "", string flagSuffix = "",
            string expressionPrefix = "", string expressionSuffix = "") where T : Enum
        {
            var flagsStr = flags.ToString();
            var flagParts = flagsStr.Split(',');
            if (flagParts.Length > 1)
                return $"{expressionPrefix}{string.Join(separator, flagParts.Select(flag => flagPrefix + flag.Trim() + flagSuffix))}{expressionSuffix}";

            return flagPrefix + flagsStr + flagSuffix;
        }
    }
}
