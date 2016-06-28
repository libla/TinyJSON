using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TinyJSON.UnitTests
{
	[TestClass()]
	public class ParserUnitTests
	{
		[TestMethod()]
		public void LoadUnitTest()
		{
			string right = @"{
				""array"": [1.44, 2, 3],
				""object"": {""key1"":""value1"", ""key2"":256},
				""string"": ""The quick brown fox \""jumps\"" over the lazy dog "",
				""unicode"": ""\u3041 Men\u54c8 sesiu00f3n"",
				""int"": 65536,
				""float"": -3.1415926e-1,
				""bool"": true,
				""null"": null
			}";
			string wrong = @"[,]";
			Parser parser = new Parser();
			Printer printer = new Printer(new Options { pretty = true });
			Assert.IsNotNull(parser.Load(right));
			Assert.IsNull(parser.Load(wrong));
			Assert.IsNotNull(printer.Bytes(parser.Load(right)));
			Assert.IsNotNull(parser.Load(printer.String(parser.Load(right))));
			Assert.IsNotNull(printer.Format(right));
			Assert.IsNull(printer.Format(wrong));
		}
	}
}
