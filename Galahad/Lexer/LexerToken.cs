using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galahad.Lexer;
public class LexerToken(int line, int column, int length, string value, LexerTokenType type)
{
    public int Line
    {
        get;
    } = line;
    public int Column
    {
        get;
    } = column;
    public int Length
    {
        get;
    } = length;

    public string Value
    {
        get;
    } = value;

    public LexerTokenType Type
    {
        get;
    } = type;



    public override string ToString()
    {
        return $"LexerToken: {Type} {Value} {Line}:{Column} {Length}";
    }
}
