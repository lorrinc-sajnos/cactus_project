using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class CharLiteral :  LiteralExpression {
    public char CharValue {get; private set;}
    public override string Value => $"\'{CharValue}\'";
    PrimitiveType  _type;   //TODO float literal!
    public PrimitiveType Type => _type;

    public CharLiteral(PrimitiveType type, string value) {
        CharValue = value[0];
        _type = type;
    }

    public override BaseType GetResultType() => _type;
}