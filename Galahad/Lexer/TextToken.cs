using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galahad.Lexer;
public class TextToken(int line, int column, int length, string value) : LexerToken(line, column, length, value)
{
}
