using System.Numerics;
using CactusLang.Model.Types;
using CactusLang.Semantics.Types;

namespace CactusLang.Util;

public static class IntegerParser {
    public static BigInteger? Parse(string input) {
        string prefix = "";

        if (input.Length > 2)
            prefix = input[0..2];
                
        try {
            switch (prefix) {
                default:
                    return BigInteger.Parse(input);
                case PrimitiveType.INT_PREFIX_HEX:
                    return ParseToBaseTwoPow(prefix[2..], 4);
                case PrimitiveType.INT_PREFIX_BIN:
                    return ParseToBaseTwoPow(prefix[2..], 1);
                case PrimitiveType.INT_PREFIX_OCT:
                    return ParseToBaseTwoPow(prefix[2..], 3);
            }
        }
        catch (Exception e) {
            return null;
        }
    }

    public static BaseType GetType(BigInteger value) {
        //8-bit
        if (value >= byte.MinValue && value <= byte.MaxValue)
            return PrimitiveType.U08;
        if (value >= sbyte.MinValue && value <= sbyte.MaxValue)
            return PrimitiveType.I08;
        //16-bit
        if (value >= ushort.MinValue && value <= ushort.MaxValue)
            return PrimitiveType.U16;
        if (value >= short.MinValue && value <= short.MaxValue)
            return PrimitiveType.I16;
        //32-bit
        if (value >= uint.MinValue && value <= uint.MaxValue)
            return PrimitiveType.U32;
        if (value >= int.MinValue && value <= int.MaxValue)
            return PrimitiveType.I32;
        //64-bit
        if (value >= ulong.MinValue && value <= ulong.MaxValue)
            return PrimitiveType.U64;
        if (value >= long.MinValue && value <= long.MaxValue)
            return PrimitiveType.I64;

        return PrimitiveType.I64;
    }

    private static BigInteger? ParseToBaseTwoPow(string input, int basePow) {
        BigInteger value = 0;

        for (int i = 0; i < input.Length; i++) {
            char digit = input[i];
            int digVal =  GetDigitValue(digit);
            if (digVal == -1) return null;
            value <<= basePow;
            value |= digVal;
        }

        return value;
    }

    private static int GetDigitValue(char digit) {
        if ( digit >='0' && digit <= '9') 
            return  digit - '0';
        
        char lwr = char.ToLower(digit);
        
        if  (lwr >= 'a' && lwr <= 'f')
            return  lwr - 'a' + 10;
        return -1;
    }
}