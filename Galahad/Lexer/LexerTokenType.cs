namespace Galahad.Lexer;

public enum LexerTokenType
{
    Assignment,
    Colon,
    AttributePrefix,
    CloseBrace,
    OpenBrace,
    CloseBracket,
    OpenBracket,
    DoubleQuote,
    OpenAngleBracket,
    CloseAngleBracket,
    SelfClosingTag,
    OpenParenthesis,
    CloseParenthesis,
    Text,
    NewLine,
    Whitespace,
    Dot,
    Comma,
}