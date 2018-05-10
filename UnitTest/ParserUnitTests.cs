using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TinyJSON.UnitTests
{
	[TestClass()]
	public class ParserUnitTests
	{
		[TestMethod]
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
			JSON json = new JSON();
			Assert.IsNotNull(json.Parse(right));
			Assert.IsNull(json.Parse(wrong));
			Assert.IsNotNull(json.Dump(json.Parse(right)));
			Assert.IsNotNull(json.Parse(json.Dump(json.Parse(right))));
		}

		[TestMethod]
		public void ReflectUnitTest()
		{
			string right = @"{
				""array"": [1.44, 2, 3],
				""object"": {""key1"":""value1"", ""key2"":256},
				""string"": ""The quick brown fox \""jumps\"" over the lazy dog "",
				""unicode"": ""\u3041 Men\u54c8 sesiu00f3n"",
				""int"": 65536,
				""float"": -3.1415926e-1,
				""bool"": true
			}";

			ReflectTest test = new ReflectTest();
			JSON json = new JSON();
			JSON.Value? value = json.Parse(right);
			Assert.IsTrue(value.HasValue);
			Assert.IsNotNull(value.Value.AsObject());
			Assert.IsTrue(test.Read(value.Value.AsObject()));
			Assert.AreEqual(test.array.Length, 3);
			Assert.AreEqual(test.obj.key1, "value1");
			Assert.AreEqual(test.obj.key2, 256);
			Assert.AreEqual(test.str, @"The quick brown fox ""jumps"" over the lazy dog ");
			Assert.AreEqual(test.i, 65536);
			Assert.AreEqual(test.f, -3.1415926e-1);
			Assert.AreEqual(test.b, true);
		}

		private class ReflectTest : JSON.Readable
		{
			public double[] array;
			[JSON.Entity(Name = "object")] public ReflectSubTest obj;
			[JSON.Entity(Name = "string")] public string str;
			public string unicode;
			[JSON.Entity(Name = "int")] public int i;
			[JSON.Entity(Name = "float")] public double f;
			[JSON.Entity(Name = "bool")] public bool b;

			public bool Read(JSON.Object o)
			{
				return o.Help(this);
			}
		}

		private class ReflectSubTest : JSON.Readable
		{
			public string key1;
			public int key2;

			public bool Read(JSON.Object o)
			{
				return o.Help(this);
			}
		}
	}
}
