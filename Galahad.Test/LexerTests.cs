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

        while (true)
        {

            var token = await lexerReader.ReadAsync();

            if (token is null)
            {

                break;
            }

            Console.WriteLine(token);
        }
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

        Assert.AreEqual(216, tokens.Count);
        Assert.AreEqual(16, tokens.Count(x => x.Type == LexerTokenType.NewLine));
    }
}