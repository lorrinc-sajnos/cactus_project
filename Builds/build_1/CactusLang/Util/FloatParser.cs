using System.Globalization;
using CactusLang.Model.Types;
using CactusLang.Semantics.Types;

namespace CactusLang.Util;

public class FloatParser {
    
    public static FloatLiteralResult Parse(string rawText) {
        FloatType type =  FloatType.DOUBLE;
        string text = rawText;
        char lastChar = text.ToLower()[^1];
        
        if(lastChar==PrimitiveType.FLOAT_POSTFIX_SINGLE) {
            type = FloatType.SINGLE;
            text = rawText.Substring(0, text.Length - 1);
        }
        else if(lastChar==PrimitiveType.FLOAT_POSTFIX_DOUBLE) {
            type = FloatType.DOUBLE;
            text = rawText.Substring(0, text.Length - 1);
        }

        double value;
        try {
            value = double.Parse(text, CultureInfo.InvariantCulture);
        }
        catch (Exception e) {
            return FloatLiteralResult.NAN;
        }

        if (type == FloatType.SINGLE) {
            return new FloatLiteralResult((float)value);
        }
        
        if (type == FloatType.DOUBLE) {
            return new FloatLiteralResult((double)value);
        }
        
        return FloatLiteralResult.NAN;
    }
}


public enum FloatType {
    NAN, SINGLE, DOUBLE
}

public struct FloatLiteralResult {
    public double Value { get; private set; }
    public float FloatValue { get; private set; }
    public double DoubleValue { get; private set; }
    public FloatType Type { get; private set; }

    public FloatLiteralResult(float value) {
        Value = value;
        Type = FloatType.SINGLE;
        FloatValue = value;
        DoubleValue = double.NaN;
    }
    public FloatLiteralResult(double value) {
        Value = value;
        Type = FloatType.DOUBLE;
        FloatValue = float.NaN;
        DoubleValue = value;
    }


    public static FloatLiteralResult NAN = new FloatLiteralResult() {
    Type = FloatType.NAN, 
    Value = double.NaN, 
    FloatValue = float.NaN ,  
    DoubleValue = double.NaN 
    };
}