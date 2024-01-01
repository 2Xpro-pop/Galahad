using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galahad.Lexer;
public abstract class LexerToken(int line, int column, int length, string value)
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

    public override string ToString()
    {

        return $"{Line}:{Column} {Value}";
    }
}
