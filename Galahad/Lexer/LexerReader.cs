using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galahad.Lexer;

public sealed class LexerReader(Stream stream) : IDisposable
{
    private readonly List<LexerToken> _tokens = [];

    private readonly StreamReader _reader = new(stream);
    private int _lineNumber = 0;

    public void Dispose()
    {
        _reader.Dispose();
        stream.Dispose();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns LexerToken null if stream ended</returns>
    public async ValueTask<LexerToken?> ReadAsync()
    {
        if (_tokens.Count > 0)
        {
            return GetLastLexerToken();
        }

        var line = await _reader.ReadLineAsync();

        if (line is null)
        {
            return null;
        }

        Volatile.Write(ref _lineNumber, _lineNumber + 1);

        ParseLine(line, _lineNumber);

        return GetLastLexerToken();
    }

    public async IAsyncEnumerable<LexerToken> ReadAllAsync()
    {
        while (true)
        {
            var token = await ReadAsync();

            if (token is null)
            {
                break;
            }

            yield return token;
        }
    }

    /// <summary>
    /// Parse line and add tokens to _tokens
    /// </summary>
    /// <param name="line"></param>
    private void ParseLine(string line, int numberLine)
    {
        if (line.Length == 0)
        {
            _tokens.Add(new(numberLine, 1, 0, "", LexerTokenType.NewLine));
            return;
        }
        var buffer = new StringBuilder();
        for (var i = 0; i < line.Length; i++)
        {
            buffer.Append(line[i]);

            if (IsToken(buffer, "=", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, "=", LexerTokenType.Assignment));
                continue;
            }
            if (IsToken(buffer, ":", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, ":", LexerTokenType.Colon));
                continue;
            }
            if (IsToken(buffer, "@", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, "@", LexerTokenType.AttributePrefix));
                continue;
            }
            if (IsToken(buffer, "}", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, "}", LexerTokenType.CloseBrace));
                continue;
            }
            if (IsToken(buffer, "{", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, "{", LexerTokenType.OpenBrace));
                continue;
            }
            if (IsToken(buffer, "[", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, "[", LexerTokenType.OpenBracket));
                continue;
            }
            if (IsToken(buffer, "]", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, "]", LexerTokenType.CloseBracket));
                continue;
            }
            if (IsToken(buffer, "\"", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, "\"", LexerTokenType.DoubleQuote));
                continue;
            }
            if (IsToken(buffer, "<", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, "<", LexerTokenType.OpenAngleBracket));
                continue;
            }
            if (IsToken(buffer, ">", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, ">", LexerTokenType.CloseAngleBracket));
                continue;
            }
            if (IsToken(buffer, "/>", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 2, "/>", LexerTokenType.SelfClosingTag));
                continue;
            }
            if (IsToken(buffer, "(", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, "(", LexerTokenType.OpenParenthesis));
                continue;
            }
            if (IsToken(buffer, ")", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, ")", LexerTokenType.CloseParenthesis));
                continue;
            }
            if (IsToken(buffer, ".", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, ".", LexerTokenType.Dot));
                continue;
            }
            if (IsToken(buffer, ",", i))
            {
                buffer.Clear();
                _tokens.Add(new(numberLine, i + 1, 1, ",", LexerTokenType.Comma));
                continue;
            }
            if (line[i] is ' ' or '\t')
            {
                if (buffer.Length > 1)
                {

                    var index = 2 + i - buffer.Length;

                    buffer.Remove(buffer.Length - 1, 1);
                    _tokens.Add(new(numberLine, index, buffer.Length, buffer.ToString(), LexerTokenType.Text));

                    buffer.Clear();
                    buffer.Append(line[i]);
                }

                ReadAllWhiteSpaces(buffer, line, ref i);
                _tokens.Add(new(numberLine, i + 1, buffer.Length, buffer.ToString(), LexerTokenType.Whitespace));

                buffer.Clear();
            }
        }

        if (buffer.Length > 0)
        {
            _tokens.Add(new(numberLine, line.Length - buffer.Length + 1, buffer.Length, buffer.ToString(), LexerTokenType.Text));
        }

        _tokens.Add(new(numberLine, line.Length, 0, "", LexerTokenType.NewLine));
    }

    private bool IsToken(StringBuilder builder, string exptected, int column)
    {
        if (builder.ToString() == exptected)
        {
            return true;
        }

        if (builder.ToString().EndsWith(exptected))
        {
            var index = 2 + column - builder.Length;
            builder.Remove(builder.Length - exptected.Length, exptected.Length);
            _tokens.Add(new(_lineNumber, index, builder.Length, builder.ToString(), LexerTokenType.Text));
            return true;
        }
        return false;
    }

    private static void ReadAllWhiteSpaces(StringBuilder builder, string line, ref int column)
    {
        for (var i = column + 1; i < line.Length; i++)
        {
            if (line[i] is ' ' or '\t')
            {
                builder.Append(line[i]);
            }
            else
            {
                column = i - 1;
                return;
            }
        }
    }

    private LexerToken? GetLastLexerToken()
    {
        if (_tokens.Count > 0)
        {
            var token = _tokens[0];
            _tokens.RemoveAt(0);
            return token;
        }

        return null;
    }
}
