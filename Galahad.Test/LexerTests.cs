using System.Diagnostics;
using System.Text;
using Galahad.Lexer;

namespace Galahad.Test;

[TestClass]
public class LexerTests
{
    [TestMethod]
    public async Task LexerReaderSimpleTest()
    {
        var lexerReader = new LexerReader(new MemoryStream(Encoding.UTF8.GetBytes("a = 1")));

        var aToken = await lexerReader.ReadAsync();
        Assert.IsNotNull(aToken);
        Assert.AreEqual(LexerTokenType.Text, aToken.Type);
        Assert.AreEqual("a", aToken.Value);

        var firstWhiteSpaceToken = await lexerReader.ReadAsync();
        Assert.IsNotNull(firstWhiteSpaceToken);
        Assert.AreEqual(LexerTokenType.Whitespace, firstWhiteSpaceToken.Type);
        Assert.AreEqual(" ", firstWhiteSpaceToken.Value);

        var assignmentToken = await lexerReader.ReadAsync();
        Assert.IsNotNull(assignmentToken);
        Assert.AreEqual(LexerTokenType.Assignment, assignmentToken.Type);
        Assert.AreEqual("=", assignmentToken.Value);

        var secondWhiteSpaceToken = await lexerReader.ReadAsync();
        Assert.IsNotNull(secondWhiteSpaceToken);
        Assert.AreEqual(LexerTokenType.Whitespace, secondWhiteSpaceToken.Type);
        Assert.AreEqual(" ", secondWhiteSpaceToken.Value);

        var oneToken = await lexerReader.ReadAsync();
        Assert.IsNotNull(oneToken);
        Assert.AreEqual(LexerTokenType.Text, oneToken.Type);
        Assert.AreEqual("1", oneToken.Value);

        var newLineToken = await lexerReader.ReadAsync();
        Assert.IsNotNull(newLineToken);
        Assert.AreEqual(LexerTokenType.NewLine, newLineToken.Type);
    }

    [TestMethod]
    public async Task HardTest()
    {
        var galahad = @"
@using Galahad alias x 
@using Galahad.AbstractPresentation

<UserControl>
	<Grid RowDefinations=""[*, Auto]"">
		<TextBox x:Name=""text"" Grid.Row=""0""
                   [{StyleSelector ""TextBlock""}] = ""{Style TextBox , FontSize = {DynamicResource ""FontSize""}}""
                   [{StyleSelector ""TextBlock:Focused""}] = ""{Style TextBox, Foreground = Green}""
                   [{StyleSelector ""TextBlock:Invalid""}] = ""{Style TextBox, Foreground = Red}""
                   Behaviors = ""[{IsEmalBehavior}]""
                   [""FontSize""] = ""double(14)""
                   [""Text""] = ""Hello World""
                   Text=""{DynamicResource ""Text""}"" />
	</Grid>
</UserControl>";
        var lexerReader = new LexerReader(new MemoryStream(Encoding.UTF8.GetBytes(galahad)));

        var tokens = await lexerReader.ReadAllAsync().ToListAsync();

#if DEBUG
        if (Debugger.IsAttached)
        {
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }
#endif
        Assert.AreEqual(16, tokens.Count(x => x.Type == LexerTokenType.NewLine));
        Assert.AreEqual(4, tokens.Count(x => x.Type == LexerTokenType.Comma));
        Assert.AreEqual(7, tokens.Count(x => x.Type == LexerTokenType.OpenBracket));
        Assert.AreEqual(7, tokens.Count(x => x.Type == LexerTokenType.CloseBracket));
        Assert.AreEqual(5, tokens.Count(x => x.Type == LexerTokenType.OpenAngleBracket));
        Assert.AreEqual(3, tokens.Count(x => x.Value.Equals("StyleSelector")));
        Assert.AreEqual(2, tokens.Count(x => x.Value.Equals("DynamicResource")));
    }
}