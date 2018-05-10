using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

namespace TinyJSON
{
	public sealed partial class JSON
	{
		public abstract class Object : IEnumerable<string>
		{
			protected abstract IEnumerator<string> Keys();
			public abstract Type Type(string key);

			public abstract bool Get(string key, ref bool b);
			public abstract bool Get(string key, ref int i);
			public abstract bool Get(string key, ref double d);
			public abstract bool Get(string key, ref string s);
			public abstract bool Get(string key, ref Object o);
			public abstract bool Get(string key, ref Array a);

			public Value this[string key]
			{
				get
				{
					switch (Type(key))
					{
					case JSON.Type.Bool:
						{
							bool b = false;
							if (Get(key, ref b))
								return b;
						}
						break;
					case JSON.Type.Int:
						{
							int i = 0;
							if (Get(key, ref i))
								return i;
						}
						break;
					case JSON.Type.Double:
						{
							double d = 0;
							if (Get(key, ref d))
								return d;
						}
						break;
					case JSON.Type.String:
						{
							string s = null;
							if (Get(key, ref s))
								return s;
						}
						break;
					case JSON.Type.Object:
						{
							Object o = null;
							if (Get(key, ref o))
								return o;
						}
						break;
					case JSON.Type.Array:
						{
							Array a = null;
							if (Get(key, ref a))
								return a;
						}
						break;
					}
					return Value.Null;
				}
			}

			public IEnumerator<string> GetEnumerator()
			{
				return Keys();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		public abstract class Array : IEnumerable<Value>
		{
			public abstract int Count { get; }
			public abstract Type Type(int index);

			public abstract bool Get(int index, ref bool b);
			public abstract bool Get(int index, ref int i);
			public abstract bool Get(int index, ref double d);
			public abstract bool Get(int index, ref string s);
			public abstract bool Get(int index, ref Object o);
			public abstract bool Get(int index, ref Array a);

			public Value this[int index]
			{
				get
				{
					switch (Type(index))
					{
					case JSON.Type.Bool:
						{
							bool b = false;
							if (Get(index, ref b))
								return b;
						}
						break;
					case JSON.Type.Int:
						{
							int i = 0;
							if (Get(index, ref i))
								return i;
						}
						break;
					case JSON.Type.Double:
						{
							double d = 0;
							if (Get(index, ref d))
								return d;
						}
						break;
					case JSON.Type.String:
						{
							string s = null;
							if (Get(index, ref s))
								return s;
						}
						break;
					case JSON.Type.Object:
						{
							Object o = null;
							if (Get(index, ref o))
								return o;
						}
						break;
					case JSON.Type.Array:
						{
							Array a = null;
							if (Get(index, ref a))
								return a;
						}
						break;
					}
					return Value.Null;
				}
			}

			public Enumerator GetEnumerator()
			{
				return new Enumerator(this);
			}

			IEnumerator<Value> IEnumerable<Value>.GetEnumerator()
			{
				return GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public struct Enumerator : IEnumerator<Value>
			{
				private readonly Array array;
				private int index;

				internal Enumerator(Array array)
				{
					this.array = array;
					this.index = -1;
				}

				public void Dispose() {}

				public bool MoveNext()
				{
					if (array == null)
						return false;
					if (++index >= array.Count)
						return false;
					return true;
				}

				public void Reset()
				{
					index = -1;
				}

				public Value Current
				{
					get { return array[index]; }
				}

				object IEnumerator.Current
				{
					get { return Current; }
				}
			}
		}

		public struct Value
		{
			public Type Type { get; private set; }
			private bool b;
			private int i;
			private double d;
			private string s;
			private Object o;
			private Array a;

			public static readonly Value Null = new Value {Type = Type.Null};

			public static implicit operator Value(bool b)
			{
				return new Value {Type = Type.Bool, b = b};
			}

			public static implicit operator Value(bool? b)
			{
				return b.HasValue ? new Value {Type = Type.Bool, b = b.Value} : Null;
			}

			public static implicit operator Value(int i)
			{
				return new Value {Type = Type.Int, i = i};
			}

			public static implicit operator Value(int? i)
			{
				return i.HasValue ? new Value {Type = Type.Int, i = i.Value} : Null;
			}

			public static implicit operator Value(double d)
			{
				return new Value {Type = Type.Double, d = d};
			}

			public static implicit operator Value(double? d)
			{
				return d.HasValue ? new Value {Type = Type.Double, d = d.Value} : Null;
			}

			public static implicit operator Value(string s)
			{
				return s != null ? new Value {Type = Type.String, s = s} : Null;
			}

			public static implicit operator Value(Object o)
			{
				return o != null ? new Value {Type = Type.Object, o = o} : Null;
			}

			public static implicit operator Value(Array a)
			{
				return a != null ? new Value {Type = Type.Array, a = a} : Null;
			}

			public static explicit operator bool(Value v)
			{
				bool? result = v.AsBool();
				if (result.HasValue)
					return result.Value;
				throw new InvalidCastException();
			}

			public static explicit operator int(Value v)
			{
				int? result = v.AsInt();
				if (result.HasValue)
					return result.Value;
				throw new InvalidCastException();
			}

			public static explicit operator double(Value v)
			{
				double? result = v.AsDouble();
				if (result.HasValue)
					return result.Value;
				throw new InvalidCastException();
			}

			public static explicit operator string(Value v)
			{
				if (v.Type == Type.Null)
					return null;
				string result = v.AsString();
				if (result != null)
					return result;
				throw new InvalidCastException();
			}

			public static explicit operator Object(Value v)
			{
				if (v.Type == Type.Null)
					return null;
				Object result = v.AsObject();
				if (result != null)
					return result;
				throw new InvalidCastException();
			}

			public static explicit operator Array(Value v)
			{
				if (v.Type == Type.Null)
					return null;
				Array result = v.AsArray();
				if (result != null)
					return result;
				throw new InvalidCastException();
			}

			public bool? AsBool()
			{
				if (Type == Type.Bool)
					return b;
				return null;
			}

			public int? AsInt()
			{
				if (Type == Type.Int)
					return i;
				double r = Math.Round(d);
				if (Type == Type.Double && Math.Abs(d - r) < double.Epsilon && r >= int.MinValue && r <= int.MaxValue)
					return (int)r;
				return null;
			}

			public double? AsDouble()
			{
				if (Type == Type.Int)
					return i;
				if (Type == Type.Double)
					return d;
				return null;
			}

			public string AsString()
			{
				return Type == Type.String ? s : null;
			}

			public Object AsObject()
			{
				return Type == Type.Object ? o : null;
			}

			public Array AsArray()
			{
				return Type == Type.Array ? a : null;
			}
		}

		public enum Type
		{
			Null,
			Bool,
			Int,
			Double,
			String,
			Object,
			Array,
		}
		
		#region 自定义事件接口
		public interface Handler
		{
			bool StartArray();
			bool StartTable();
			bool EndArray();
			bool EndTable();
			bool Key(string key);
			bool Null();
			bool Bool(bool b);
			bool String(string s);
			bool Double(double d);
			bool Int(int i);
			bool Flush();
		}

		public interface Writer
		{
			bool Write(Handler handler);
		}

		public delegate void ValueAction(Value value);
		public delegate bool WriteAction(Handler handler);
		#endregion

		#region 选项
		public struct ParseOptions
		{
			public bool strict;
			public bool comment;
			public bool eof;
		}

		public struct DumpOptions
		{
			public bool pretty;
			public bool escape;
		}
		#endregion

		#region 默认接口
		public static Handler DefaultHandler(ValueAction action)
		{
			return new NodeHandler {result = action};
		}

		public static Writer DefaultWriter(Value value)
		{
			return new NodeWriter(value);
		}

		public static Object From(Dictionary<string, Value> dict)
		{
			return new NodeObject(dict);
		}

		public static Array From(List<Value> list)
		{
			return new NodeArray(list);
		}
		#endregion

		#region 对外解析接口
		public bool Parse(TextReader reader, Handler handler, ParseOptions options)
		{
			input = reader;
			token = input.Read();
			if (!Skip())
				return false;
			if (!ReadValue(handler))
				return false;
			if (!handler.Flush())
				return false;
			return !options.eof || Flush();
		}

		public bool Parse(TextReader reader, Handler handler)
		{
			return Parse(reader, handler, new ParseOptions {strict = false, comment = true, eof = false});
		}

		public bool Parse(string text, Handler handler, ParseOptions options)
		{
			return Parse(new StringReader(text), handler, options);
		}

		public bool Parse(string text, Handler handler)
		{
			return Parse(new StringReader(text), handler, new ParseOptions {strict = false, comment = true, eof = true});
		}

		public Value? Parse(TextReader reader, ParseOptions options)
		{
			NodeHandler handler = new NodeHandler();
			return Parse(reader, handler, options) ? handler.root : null;
		}

		public Value? Parse(TextReader reader)
		{
			NodeHandler handler = new NodeHandler();
			return Parse(reader, handler) ? handler.root : null;
		}

		public Value? Parse(string text, ParseOptions options)
		{
			return Parse(new StringReader(text), options);
		}

		public Value? Parse(string text)
		{
			return Parse(new StringReader(text));
		}
		#endregion

		#region 对外序列化接口
		private Buffer normalbuffer;
		private Buffer prettybuffer;

		public bool Dump(Writer writer, Handler handler)
		{
			return writer.Write(handler);
		}

		public bool Dump(TextWriter output, Writer writer, DumpOptions options)
		{
			Buffer buffer;
			if (options.pretty)
			{
				if (prettybuffer == null)
					prettybuffer = new PrettyBuffer();
				buffer = prettybuffer;
			}
			else
			{
				if (normalbuffer == null)
					normalbuffer = new Buffer();
				buffer = normalbuffer;
			}
			buffer.escape = options.escape;
			buffer.writer = output;
			buffer.Reset();
			bool result = Dump(writer, buffer);
			buffer.writer = null;
			return result;
		}

		public bool Dump(TextWriter output, Writer writer)
		{
			return Dump(output, writer, new DumpOptions {pretty = false, escape = false});
		}

		public string Dump(Writer writer, DumpOptions options)
		{
			StringWriter output = new StringWriter();
			return Dump(output, writer, options) ? output.ToString() : null;
		}

		public string Dump(Writer writer)
		{
			StringWriter output = new StringWriter();
			return Dump(output, writer) ? output.ToString() : null;
		}

		public bool Dump(WriteAction writer, Handler handler)
		{
			return new ActionWriter(writer).Write(handler);
		}

		public bool Dump(TextWriter output, WriteAction writer, DumpOptions options)
		{
			return Dump(output, new ActionWriter(writer), options);
		}

		public bool Dump(TextWriter output, WriteAction writer)
		{
			return Dump(output, new ActionWriter(writer));
		}

		public string Dump(WriteAction writer, DumpOptions options)
		{
			return Dump(new ActionWriter(writer), options);
		}

		public string Dump(WriteAction writer)
		{
			return Dump(new ActionWriter(writer));
		}

		public bool Dump(Value value, Handler handler)
		{
			return new NodeWriter(value).Write(handler);
		}

		public bool Dump(Value? value, Handler handler)
		{
			return value.HasValue && new NodeWriter(value.Value).Write(handler);
		}

		public bool Dump(TextWriter output, Value value, DumpOptions options)
		{
			return Dump(output, new NodeWriter(value), options);
		}

		public bool Dump(TextWriter output, Value? value, DumpOptions options)
		{
			return value.HasValue && Dump(output, new NodeWriter(value.Value), options);
		}

		public bool Dump(TextWriter output, Value value)
		{
			return Dump(output, new NodeWriter(value));
		}

		public bool Dump(TextWriter output, Value? value)
		{
			return value.HasValue && Dump(output, new NodeWriter(value.Value));
		}

		public string Dump(Value value, DumpOptions options)
		{
			return Dump(new NodeWriter(value), options);
		}

		public string Dump(Value? value, DumpOptions options)
		{
			return value.HasValue ? Dump(new NodeWriter(value.Value), options) : null;
		}

		public string Dump(Value value)
		{
			return Dump(new NodeWriter(value));
		}

		public string Dump(Value? value)
		{
			return value.HasValue ? Dump(new NodeWriter(value.Value)) : null;
		}
		#endregion

		#region 解析实现
		private int token;
		private TextReader input;
		private StringBuilder builder;

		private bool ReadValue(Handler handler)
		{
			switch (token)
			{
			case '{':
				return ReadTable(handler);
			case '[':
				return ReadArray(handler);
			case 't':
			case 'f':
				return ReadBool(handler);
			case 'n':
				return ReadNull(handler);
			case '"':
			case '\'':
				return ReadString(handler);
			case '-':
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				return ReadNumber(handler);
			case '/':
				Next();
				switch (token)
				{
				case '/':
					return ReadCommentLine();
				case '*':
					return ReadComments();
				}
				return false;
			}
			return false;
		}

		private bool ReadTable(Handler handler)
		{
			if (!handler.StartTable())
				return false;
			while (true)
			{
				Next();
				if (!Skip())
					return false;
				if (token == '}')
				{
					Next();
					if (!handler.EndTable())
						return false;
					break;
				}
				if (!ReadKey(handler))
					return false;
				if (!Skip())
					return false;
				if (token != ':')
					return false;
				Next();
				if (!Skip())
					return false;
				if (!ReadValue(handler))
					return false;
				if (!Skip())
					return false;
				if (token == '}')
				{
					Next();
					if (!handler.EndTable())
						return false;
					break;
				}
				if (token != ',')
					return false;
			}
			return true;
		}

		private bool ReadArray(Handler handler)
		{
			if (!handler.StartArray())
				return false;
			while (true)
			{
				Next();
				if (!Skip())
					return false;
				if (token == ']')
				{
					Next();
					if (!handler.EndArray())
						return false;
					break;
				}
				if (!ReadValue(handler))
					return false;
				if (!Skip())
					return false;
				if (token == ']')
				{
					Next();
					if (!handler.EndArray())
						return false;
					break;
				}
				if (token != ',')
					return false;
			}
			return true;
		}

		private bool ReadBool(Handler handler)
		{
			if (token == 't')
			{
				if (Next() == 'r' && Next() == 'u' && Next() == 'e')
				{
					Next();
					return handler.Bool(true);
				}
			}
			else
			{
				if (Next() == 'a' && Next() == 'l' && Next() == 's' && Next() == 'e')
				{
					Next();
					return handler.Bool(false);
				}
			}
			return false;
		}

		private bool ReadNull(Handler handler)
		{
			if (Next() == 'u' && Next() == 'l' && Next() == 'l')
			{
				Next();
				return handler.Null();
			}
			return false;
		}

		private bool ReadNumber(Handler handler)
		{
			bool minus = false;
			if (token == '-')
			{
				minus = true;
				Next();
			}
			if (token < '0' || token > '9')
				return false;
			uint u = 0;
			double d = 0;
			bool isfloat = false;
			while (token >= '0' && token <= '9')
			{
				if (isfloat)
				{
					d = d * 10 + (token - '0');
				}
				else
				{
					uint uu = (uint)(token - '0');
					if (u > int.MaxValue / 10 || (u == int.MaxValue / 10 && uu > int.MaxValue % 10))
					{
						isfloat = true;
						d = u;
						continue;
					}
					u = u * 10 + uu;
				}
				Next();
			}
			int dot = 0;
			if (token == '.')
			{
				if (!isfloat)
				{
					isfloat = true;
					d = u;
				}
				Next();
				while (token >= '0' && token <= '9')
				{
					d = d * 10 + (token - '0');
					--dot;
					Next();
				}
				if (dot == 0)
					return false;
			}
			int exp = 0;
			if (token == 'E' || token == 'e')
			{
				if (!isfloat)
				{
					isfloat = true;
					d = u;
				}
				bool expMinus = false;
				Next();
				if (token == '+')
				{
					Next();
				}
				else if (token == '-')
				{
					expMinus = true;
					Next();
				}
				if (token < '0' || token > '9')
					return false;
				while (token >= '0' && token <= '9')
				{
					exp = exp * 10 + (token - '0');
					Next();
				}
				if (expMinus)
					exp = -exp;
			}
			return isfloat ? handler.Double((minus ? -d : d) * Pow10(dot + exp)) : handler.Int(minus ? (int)-u : (int)u);
		}

		private string ReadString()
		{
			char symbol = (char)token;
			if (builder == null)
				builder = new StringBuilder();
			else
				builder.Length = 0;
			while (true)
			{
				Next();
				if (token == symbol)
					break;
				if (token == -1)
					return null;
				if (token == '\\')
				{
					Next();
					switch (token)
					{
					case 'b':
						builder.Append('\b');
						break;
					case 'f':
						builder.Append('\f');
						break;
					case 'n':
						builder.Append('\n');
						break;
					case 'r':
						builder.Append('\r');
						break;
					case 't':
						builder.Append('\t');
						break;
					case '"':
						builder.Append('"');
						break;
					case '\'':
						builder.Append('\'');
						break;
					case '\\':
						builder.Append('\\');
						break;
					case '/':
						builder.Append('/');
						break;
					case 'u':
						{
							int unicode = 0;
							for (int i = 0; i < 4; ++i)
							{
								int c = Next();
								if (c >= 'a')
									c -= 'a' - 10;
								else if (c >= 'A')
									c -= 'A' - 10;
								else if (c >= '0' && c <= '9')
									c -= '0';
								else
									return null;
								unicode <<= 4;
								unicode |= c;
							}
							builder.Append((char)unicode);
						}
						break;
					default:
						return null;
					}
				}
				else
				{
					builder.Append((char)token);
				}
			}
			Next();
			return builder.ToString();
		}

		private bool ReadString(Handler handler)
		{
			string str = ReadString();
			return str != null && handler.String(str);
		}

		private bool ReadKey(Handler handler)
		{
			if (token == '"' || token == '\'')
			{
				string str = ReadString();
				return str != null && handler.Key(str);
			}
			if (token == '_' || (token >= 'a' && token <= 'z') || (token >= 'A' && token <= 'Z'))
			{
				if (builder == null)
					builder = new StringBuilder();
				else
					builder.Length = 0;
				while (true)
				{
					builder.Append((char)token);
					Next();
					if (token == '_' || (token >= 'a' && token <= 'z') || (token >= 'A' && token <= 'Z') || (token >= '0' && token <= '9'))
						continue;
					break;
				}
				return handler.Key(builder.ToString());
			}
			return false;
		}

		private bool ReadCommentLine()
		{
			while (true)
			{
				Next();
				if (token == '\n')
				{
					Next();
					return true;
				}
				if (token == -1)
					return true;
			}
		}

		private bool ReadComments()
		{
			while (true)
			{
				Next();
				if (token == '*')
				{
					Next();
					if (token == '/')
					{
						Next();
						return true;
					}
				}
				if (token == -1)
					return false;
			}
		}

		private bool Flush()
		{
			if (!Skip())
				return false;
			input = null;
			return token == -1 || token == '\0';
		}

		private bool Skip()
		{
			while (token == ' ' || token == '\n' || token == '\r' || token == '\t')
			{
				token = input.Read();
			}
			if (token == '/')
			{
				switch (input.Peek())
				{
				case '*':
					input.Read();
					while (true)
					{
						token = input.Read();
						if (token == -1)
							return false;
						if (token == '*')
						{
							if (input.Peek() == '/')
							{
								input.Read();
								token = input.Read();
								break;
							}
						}
					}
					return Skip();
				case '/':
					input.Read();
					while (true)
					{
						token = input.Read();
						if (token == '\r' || token == '\n' || token == -1)
							break;
					}
					return Skip();
				}
			}
			return true;
		}

		private int Next()
		{
			token = input.Read();
			return token;
		}
		#endregion

		#region 加速Pow运算
		private static readonly double[] e =
		{
			// 1e-0...1e308: 309 * 8 bytes = 2472 bytes
			1e+0,  
			1e+1,  1e+2,  1e+3,  1e+4,  1e+5,  1e+6,  1e+7,  1e+8,  1e+9,  1e+10, 1e+11, 1e+12, 1e+13, 1e+14, 1e+15, 1e+16, 1e+17, 1e+18, 1e+19, 1e+20, 1e+21, 1e+22, 1e+23, 1e+24, 1e+25, 1e+26, 1e+27, 1e+28, 
			1e+29, 1e+30, 1e+31, 1e+32, 1e+33, 1e+34, 1e+35, 1e+36, 1e+37, 1e+38, 1e+39, 1e+40, 1e+41, 1e+42, 1e+43, 1e+44, 1e+45, 1e+46, 1e+47, 1e+48, 1e+49, 1e+50, 1e+51, 1e+52, 1e+53, 1e+54, 1e+55, 1e+56, 
			1e+57, 1e+58, 1e+59, 1e+60, 1e+61, 1e+62, 1e+63, 1e+64, 1e+65, 1e+66, 1e+67, 1e+68, 1e+69, 1e+70, 1e+71, 1e+72, 1e+73, 1e+74, 1e+75, 1e+76, 1e+77, 1e+78, 1e+79, 1e+80, 1e+81, 1e+82, 1e+83, 1e+84, 
			1e+85, 1e+86, 1e+87, 1e+88, 1e+89, 1e+90, 1e+91, 1e+92, 1e+93, 1e+94, 1e+95, 1e+96, 1e+97, 1e+98, 1e+99, 1e+100,1e+101,1e+102,1e+103,1e+104,1e+105,1e+106,1e+107,1e+108,1e+109,1e+110,1e+111,1e+112,
			1e+113,1e+114,1e+115,1e+116,1e+117,1e+118,1e+119,1e+120,1e+121,1e+122,1e+123,1e+124,1e+125,1e+126,1e+127,1e+128,1e+129,1e+130,1e+131,1e+132,1e+133,1e+134,1e+135,1e+136,1e+137,1e+138,1e+139,1e+140,
			1e+141,1e+142,1e+143,1e+144,1e+145,1e+146,1e+147,1e+148,1e+149,1e+150,1e+151,1e+152,1e+153,1e+154,1e+155,1e+156,1e+157,1e+158,1e+159,1e+160,1e+161,1e+162,1e+163,1e+164,1e+165,1e+166,1e+167,1e+168,
			1e+169,1e+170,1e+171,1e+172,1e+173,1e+174,1e+175,1e+176,1e+177,1e+178,1e+179,1e+180,1e+181,1e+182,1e+183,1e+184,1e+185,1e+186,1e+187,1e+188,1e+189,1e+190,1e+191,1e+192,1e+193,1e+194,1e+195,1e+196,
			1e+197,1e+198,1e+199,1e+200,1e+201,1e+202,1e+203,1e+204,1e+205,1e+206,1e+207,1e+208,1e+209,1e+210,1e+211,1e+212,1e+213,1e+214,1e+215,1e+216,1e+217,1e+218,1e+219,1e+220,1e+221,1e+222,1e+223,1e+224,
			1e+225,1e+226,1e+227,1e+228,1e+229,1e+230,1e+231,1e+232,1e+233,1e+234,1e+235,1e+236,1e+237,1e+238,1e+239,1e+240,1e+241,1e+242,1e+243,1e+244,1e+245,1e+246,1e+247,1e+248,1e+249,1e+250,1e+251,1e+252,
			1e+253,1e+254,1e+255,1e+256,1e+257,1e+258,1e+259,1e+260,1e+261,1e+262,1e+263,1e+264,1e+265,1e+266,1e+267,1e+268,1e+269,1e+270,1e+271,1e+272,1e+273,1e+274,1e+275,1e+276,1e+277,1e+278,1e+279,1e+280,
			1e+281,1e+282,1e+283,1e+284,1e+285,1e+286,1e+287,1e+288,1e+289,1e+290,1e+291,1e+292,1e+293,1e+294,1e+295,1e+296,1e+297,1e+298,1e+299,1e+300,1e+301,1e+302,1e+303,1e+304,1e+305,1e+306,1e+307,1e+308,
		};

		private static double Pow10(int x)
		{
			if (x < -308)
				return 0;
			if (x > 308)
				return double.PositiveInfinity;
			return x >= 0 ? e[x] : 1.0 / e[-x];
		}
		#endregion

		#region 默认解析生成Node
		private class NodeObject : Object
		{
			public readonly Dictionary<string, Value> dict;

			public NodeObject()
			{
				dict = new Dictionary<string, Value>();
			}

			public NodeObject(Dictionary<string, Value> dict)
			{
				this.dict = dict;
			}

			protected override IEnumerator<string> Keys()
			{
				return new Enumerator(dict);
			}

			public override Type Type(string key)
			{
				Value value;
				return dict.TryGetValue(key, out value) ? value.Type : JSON.Type.Null;
			}

			public override bool Get(string key, ref bool b)
			{
				Value value;
				if (dict.TryGetValue(key, out value))
				{
					bool? result = value.AsBool();
					if (result.HasValue)
					{
						b = result.Value;
						return true;
					}
				}
				return false;
			}

			public override bool Get(string key, ref int i)
			{
				Value value;
				if (dict.TryGetValue(key, out value))
				{
					int? result = value.AsInt();
					if (result.HasValue)
					{
						i = result.Value;
						return true;
					}
				}
				return false;
			}

			public override bool Get(string key, ref double d)
			{
				Value value;
				if (dict.TryGetValue(key, out value))
				{
					double? result = value.AsDouble();
					if (result.HasValue)
					{
						d = result.Value;
						return true;
					}
				}
				return false;
			}

			public override bool Get(string key, ref string s)
			{
				Value value;
				if (dict.TryGetValue(key, out value))
				{
					string result = value.AsString();
					if (result != null)
					{
						s = result;
						return true;
					}
				}
				return false;
			}

			public override bool Get(string key, ref Object o)
			{
				Value value;
				if (dict.TryGetValue(key, out value))
				{
					Object result = value.AsObject();
					if (result != null)
					{
						o = result;
						return true;
					}
				}
				return false;
			}

			public override bool Get(string key, ref Array a)
			{
				Value value;
				if (dict.TryGetValue(key, out value))
				{
					Array result = value.AsArray();
					if (result != null)
					{
						a = result;
						return true;
					}
				}
				return false;
			}

			private class Enumerator : IEnumerator<string>
			{
				private readonly Dictionary<string, Value> dict;
				private Dictionary<string, Value>.Enumerator enumerator;

				public Enumerator(Dictionary<string, Value> dict)
				{
					this.dict = dict;
					enumerator = dict.GetEnumerator();
				}

				public void Dispose()
				{
					enumerator.Dispose();
				}

				public bool MoveNext()
				{
					return enumerator.MoveNext();
				}

				public void Reset()
				{
					enumerator.Dispose();
					enumerator = dict.GetEnumerator();
				}

				public string Current
				{
					get { return enumerator.Current.Key; }
				}

				object IEnumerator.Current
				{
					get { return Current; }
				}
			}
		}

		private class NodeArray : Array
		{
			public readonly List<Value> list;

			public NodeArray()
			{
				list = new List<Value>();
			}

			public NodeArray(List<Value> list)
			{
				this.list = list;
			}

			public override int Count
			{
				get { return list.Count; }
			}

			public override Type Type(int index)
			{
				return list[index].Type;
			}

			public override bool Get(int index, ref bool b)
			{
				bool? result = list[index].AsBool();
				if (result.HasValue)
				{
					b = result.Value;
					return true;
				}
				return false;
			}

			public override bool Get(int index, ref int i)
			{
				int? result = list[index].AsInt();
				if (result.HasValue)
				{
					i = result.Value;
					return true;
				}
				return false;
			}

			public override bool Get(int index, ref double d)
			{
				double? result = list[index].AsDouble();
				if (result.HasValue)
				{
					d = result.Value;
					return true;
				}
				return false;
			}

			public override bool Get(int index, ref string s)
			{
				string result = list[index].AsString();
				if (result != null)
				{
					s = result;
					return true;
				}
				return false;
			}

			public override bool Get(int index, ref Object o)
			{
				Object result = list[index].AsObject();
				if (result != null)
				{
					o = result;
					return true;
				}
				return false;
			}

			public override bool Get(int index, ref Array a)
			{
				Array result = list[index].AsArray();
				if (result != null)
				{
					a = result;
					return true;
				}
				return false;
			}
		}

		private class NodeHandler : Stack<Value>, Handler
		{
			private string key;
			public Value? root;
			public ValueAction result;

			private bool Value(Value value)
			{
				if (root == null)
				{
					root = value;
				}
				else if (Count > 0)
				{
					Value last = Peek();
					switch (last.Type)
					{
					case Type.Object:
						{
							NodeObject node = last.AsObject() as NodeObject;
							if (node != null)
								node.dict[key] = value;
						}
						break;
					case Type.Array:
						{
							NodeArray node = last.AsArray() as NodeArray;
							if (node != null)
								node.list.Add(value);
						}
						break;
					default:
						return false;
					}
				}
				return true;
			}

			public bool StartArray()
			{
				NodeArray node = new NodeArray();
				if (!Value(node))
					return false;
				Push(node);
				return true;
			}

			public bool StartTable()
			{
				NodeObject node = new NodeObject();
				if (!Value(node))
					return false;
				Push(node);
				return true;
			}

			public bool EndArray()
			{
				try
				{
					Pop();
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}

			public bool EndTable()
			{
				try
				{
					Pop();
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}

			public bool Null()
			{
				return Value(JSON.Value.Null);
			}

			public bool Key(string key)
			{
				this.key = key;
				return true;
			}

			public bool Bool(bool b)
			{
				return Value(b);
			}

			public bool String(string s)
			{
				return Value(s);
			}

			public bool Double(double d)
			{
				return Value(d);
			}

			public bool Int(int i)
			{
				return Value(i);
			}

			public bool Flush()
			{
				if (Count != 0)
					return false;
				if (result != null && root.HasValue)
					result(root.Value);
				return true;
			}
		}
		#endregion

		#region 序列化实现
		private class Buffer : Handler
		{
			public bool escape;
			public TextWriter writer;
			protected bool empty;
			protected int depth;
			protected bool table;

			public Buffer()
			{
				escape = false;
				empty = true;
				depth = 0;
			}

			private void WriteString(string s)
			{
				writer.Write('"');
				for (int i = 0, j = s.Length; i < j; ++i)
				{
					char c = s[i];
					if (c < 128)
					{
						char esc = escapes[c];
						if (esc == '#')
						{
							writer.Write(c);
						}
						else if (esc == 'u')
						{
							writer.Write("\\u00");
							writer.Write(((uint)c).ToString("X2", CultureInfo.InvariantCulture));
						}
						else
						{
							writer.Write('\\');
							writer.Write(esc);
						}
					}
					else if (escape)
					{
						writer.Write("\\u");
						writer.Write(((uint)c).ToString("X4", CultureInfo.InvariantCulture));
					}
					else
					{
						writer.Write(c);
					}
				}
				writer.Write('"');
			}

			protected virtual void Prefix()
			{
				if (depth > 0)
				{
					if (!table)
					{
						if (!empty)
							WriteComma();
						WriteIndent();
					}
				}
			}

			public override string ToString()
			{
				return writer.ToString();
			}

			public virtual void Reset()
			{
				empty = true;
				depth = 0;
			}

			public virtual bool StartArray()
			{
				Prefix();
				++depth;
				empty = true;
				table = false;
				writer.Write('[');
				return true;
			}

			public virtual bool EndArray()
			{
				--depth;
				if (!empty)
					WriteIndent();
				empty = false;
				writer.Write(']');
				return true;
			}

			public virtual bool StartTable()
			{
				Prefix();
				++depth;
				empty = true;
				table = false;
				writer.Write('{');
				return true;
			}

			public virtual bool EndTable()
			{
				--depth;
				if (!empty)
					WriteIndent();
				empty = false;
				writer.Write('}');
				return true;
			}

			public virtual bool Key(string s)
			{
				table = true;
				if (!empty)
					WriteComma();
				WriteIndent();
				WriteString(s);
				WriteColon();
				return true;
			}

			public bool Null()
			{
				Prefix();
				empty = false;
				table = false;
				writer.Write("null");
				return true;
			}

			public bool Bool(bool b)
			{
				Prefix();
				empty = false;
				table = false;
				writer.Write(b ? "true" : "false");
				return true;
			}

			public bool String(string s)
			{
				Prefix();
				empty = false;
				table = false;
				WriteString(s);
				return true;
			}

			public bool Double(double d)
			{
				Prefix();
				empty = false;
				table = false;
				writer.Write(d.ToString(CultureInfo.InvariantCulture));
				return true;
			}

			public bool Int(int i)
			{
				Prefix();
				empty = false;
				table = false;
				writer.Write(i.ToString(CultureInfo.InvariantCulture));
				return true;
			}

			public bool Flush()
			{
				return depth == 0;
			}

			protected virtual void WriteIndent()
			{
			}

			protected virtual void WriteComma()
			{
				writer.Write(',');
			}

			protected virtual void WriteColon()
			{
				writer.Write(':');
			}
		}

		private class PrettyBuffer : Buffer
		{
			protected override void WriteColon()
			{
				writer.Write(" : ");
			}

			protected override void WriteIndent()
			{
				writer.Write('\n');
				for (int i = 0, j = depth; i < j; ++i)
				{
					writer.Write('\t');
				}
			}
		}
		#endregion

		#region 转义字符表
		private static readonly char[] escapes =
		{
			/*
				This array maps the 128 ASCII characters into character escape.
				'u' indicate must transform to \u00xx.
			*/
			'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'b', 't', 'n', 'u', 'f', 'r', 'u', 'u',
			'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u',
			'#', '#', '"', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#',
			'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#',
			'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#',
			'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '\\', '#', '#', '#',
			'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#',
			'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#',
		};

		#endregion

		#region 默认序列化行为
		private class NodeWriter : Writer
		{
			private readonly Value root;
			private readonly Dictionary<object, bool> writed;

			public NodeWriter(Value value)
			{
				root = value;
				writed = new Dictionary<object, bool>();
			}

			public bool Write(Handler handler)
			{
				return Write(handler, root) && handler.Flush();
			}

			private bool Write(Handler handler, Value value)
			{
				switch (value.Type)
				{
				case Type.Null:
					return handler.Null();
				case Type.Bool:
					return handler.Bool((bool)value);
				case Type.Int:
					return handler.Int((int)value);
				case Type.Double:
					return handler.Double((double)value);
				case Type.String:
					return handler.String((string)value);
				case Type.Array:
					{
						Array arr = (Array)value;
						if (writed.ContainsKey(arr))
							return false;
						writed.Add(arr, true);
						if (!handler.StartArray())
							return false;
						for (int i = 0, j = arr.Count; i < j; ++i)
						{
							if (!Write(handler, arr[i]))
								return false;
						}
						return handler.EndArray();
					}
				case Type.Object:
					{
						Object obj = (Object)value;
						if (writed.ContainsKey(obj))
							return false;
						writed.Add(obj, true);
						if (!handler.StartTable())
							return false;
						foreach (string key in obj)
						{
							if (!handler.Key(key))
								return false;
							if (!Write(handler, obj[key]))
								return false;
						}
						return handler.EndTable();
					}
				}
				return false;
			}
		}

		private class ActionWriter : Writer
		{
			private readonly WriteAction func;

			public ActionWriter(WriteAction func)
			{
				this.func = func;
			}

			public bool Write(Handler handler)
			{
				return func(handler);
			}
		}
		#endregion
		
		public interface Readable
		{
			bool Read(Object o);
		}

		public interface Writeable
		{
			Object Write();
		}

		public static bool Read<T>(Object o, ref T value) where T : Readable, new()
		{
			T tmp = new T();
			if (!tmp.Read(o))
				return false;
			value = tmp;
			return true;
		}

		public static Object Write<T>(ref T value) where T : Writeable
		{
			return value.Write();
		}

		public static Object Write<T>(T value) where T : Writeable
		{
			return Write(ref value);
		}
	}
	
	public static partial class JSONExt
	{
		public static bool Convert<T>(this JSON.Object o, ref T value) where T : JSON.Readable, new()
		{
			return JSON.Read(o, ref value);
		}

		public static T Convert<T>(this JSON.Object o) where T : JSON.Readable, new()
		{
			T value = default(T);
			if (JSON.Read(o, ref value))
				return value;
			throw new FormatException();
		}

		public static JSON.Object Convert<T>(this T value) where T : JSON.Writeable
		{
			return JSON.Write(ref value);
		}

		public static bool Get(this JSON.Object o, string key, ref byte value)
		{
			int tmp = default(int);
			if (!o.Get(key, ref tmp))
				return false;
			if (tmp < byte.MinValue || tmp > byte.MaxValue)
				return false;
			value = (byte)tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref sbyte value)
		{
			int tmp = default(int);
			if (!o.Get(key, ref tmp))
				return false;
			if (tmp < sbyte.MinValue || tmp > sbyte.MaxValue)
				return false;
			value = (sbyte)tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref short value)
		{
			int tmp = default(int);
			if (!o.Get(key, ref tmp))
				return false;
			if (tmp < short.MinValue || tmp > short.MaxValue)
				return false;
			value = (short)tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref ushort value)
		{
			int tmp = default(int);
			if (!o.Get(key, ref tmp))
				return false;
			if (tmp < ushort.MinValue || tmp > ushort.MaxValue)
				return false;
			value = (ushort)tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref uint value)
		{
			int tmpi = default(int);
			if (o.Get(key, ref tmpi))
			{
				if (tmpi < 0)
					return false;
				value = (uint)tmpi;
				return true;
			}
			double tmpd = default(double);
			if (o.Get(key, ref tmpd))
			{
				double r = Math.Round(tmpd);
				if (Math.Abs(tmpd - r) < double.Epsilon && r >= uint.MinValue && r <= uint.MaxValue)
				{
					value = (uint)r;
					return true;
				}
			}
			return false;
		}

		public static bool Get(this JSON.Object o, string key, ref float value)
		{
			double tmp = default(double);
			if (!o.Get(key, ref tmp))
				return false;
			if (tmp < float.MinValue || tmp > float.MaxValue)
				return false;
			value = (float)tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref decimal value)
		{
			double tmp = default(double);
			if (!o.Get(key, ref tmp))
				return false;
			try
			{
				value = (decimal)tmp;
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool Get(this JSON.Object o, string key, ref JSON.Value value)
		{
			value = o[key];
			return true;
		}

		public static bool Get<T>(this JSON.Object o, string key, ref T value) where T : JSON.Readable, new()
		{
			if (typeof(T).IsEnum)
			{
				string tmp = default(string);
				if (!o.Get(key, ref tmp))
					return false;
				try
				{
					value = (T)Enum.Parse(typeof(T), tmp);
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}
			else
			{
				JSON.Object tmp = default(JSON.Object);
				return o.Get(key, ref tmp) && JSON.Read(tmp, ref value);
			}
		}

		public static bool Get(this JSON.Object o, string key, ref bool? value)
		{
			if (o.Type(key) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			bool tmp = default(bool);
			if (!o.Get(key, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref bool[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			bool[] result = new bool[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref byte? value)
		{
			if (o.Type(key) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			byte tmp = default(byte);
			if (!o.Get(key, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref byte[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			byte[] result = new byte[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref sbyte? value)
		{
			if (o.Type(key) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			sbyte tmp = default(sbyte);
			if (!o.Get(key, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref sbyte[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			sbyte[] result = new sbyte[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref short? value)
		{
			if (o.Type(key) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			short tmp = default(short);
			if (!o.Get(key, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref short[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			short[] result = new short[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref ushort? value)
		{
			if (o.Type(key) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			ushort tmp = default(ushort);
			if (!o.Get(key, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref ushort[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			ushort[] result = new ushort[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref int? value)
		{
			if (o.Type(key) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			int tmp = default(int);
			if (!o.Get(key, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref int[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			int[] result = new int[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref uint? value)
		{
			if (o.Type(key) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			uint tmp = default(uint);
			if (!o.Get(key, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref uint[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			uint[] result = new uint[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref float? value)
		{
			if (o.Type(key) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			float tmp = default(float);
			if (!o.Get(key, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref float[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			float[] result = new float[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref decimal? value)
		{
			if (o.Type(key) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			decimal tmp = default(decimal);
			if (!o.Get(key, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref decimal[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			decimal[] result = new decimal[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref double? value)
		{
			if (o.Type(key) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			double tmp = default(double);
			if (!o.Get(key, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref double[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			double[] result = new double[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref string[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			string[] result = new string[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref JSON.Object[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			JSON.Object[] result = new JSON.Object[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref JSON.Array[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			JSON.Array[] result = new JSON.Array[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref JSON.Value? value)
		{
			if (o.Type(key) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			JSON.Value tmp = default(JSON.Value);
			if (!o.Get(key, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Object o, string key, ref JSON.Value[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			JSON.Value[] result = new JSON.Value[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get<T>(this JSON.Object o, string key, ref T? value) where T : struct, JSON.Readable
		{
			if (o.Type(key) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			T tmp = default(T);
			if (!Get(o, key, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get<T>(this JSON.Object o, string key, ref T[] value) where T : JSON.Readable, new()
		{
			JSON.Array array = default(JSON.Array);
			if (!o.Get(key, ref array))
				return false;
			T[] result = new T[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref byte value)
		{
			int tmp = default(int);
			if (!a.Get(index, ref tmp))
				return false;
			if (tmp < byte.MinValue || tmp > byte.MaxValue)
				return false;
			value = (byte)tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref sbyte value)
		{
			int tmp = default(int);
			if (!a.Get(index, ref tmp))
				return false;
			if (tmp < sbyte.MinValue || tmp > sbyte.MaxValue)
				return false;
			value = (sbyte)tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref short value)
		{
			int tmp = default(int);
			if (!a.Get(index, ref tmp))
				return false;
			if (tmp < short.MinValue || tmp > short.MaxValue)
				return false;
			value = (short)tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref ushort value)
		{
			int tmp = default(int);
			if (!a.Get(index, ref tmp))
				return false;
			if (tmp < ushort.MinValue || tmp > ushort.MaxValue)
				return false;
			value = (ushort)tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref uint value)
		{
			int tmpi = default(int);
			if (a.Get(index, ref tmpi))
			{
				if (tmpi < 0)
					return false;
				value = (uint)tmpi;
				return true;
			}
			double tmpd = default(double);
			if (a.Get(index, ref tmpd))
			{
				double r = Math.Round(tmpd);
				if (Math.Abs(tmpd - r) < double.Epsilon && r >= uint.MinValue && r <= uint.MaxValue)
				{
					value = (uint)r;
					return true;
				}
			}
			return false;
		}

		public static bool Get(this JSON.Array a, int index, ref float value)
		{
			double tmp = default(double);
			if (!a.Get(index, ref tmp))
				return false;
			if (tmp < float.MinValue || tmp > float.MaxValue)
				return false;
			value = (float)tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref decimal value)
		{
			double tmp = default(double);
			if (!a.Get(index, ref tmp))
				return false;
			try
			{
				value = (decimal)tmp;
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool Get(this JSON.Array a, int index, ref JSON.Value value)
		{
			value = a[index];
			return true;
		}

		public static bool Get<T>(this JSON.Array a, int index, ref T value) where T : JSON.Readable, new()
		{
			if (typeof(T).IsEnum)
			{
				string tmp = default(string);
				if (!a.Get(index, ref tmp))
					return false;
				try
				{
					value = (T)Enum.Parse(typeof(T), tmp);
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}
			else
			{
				JSON.Object tmp = default(JSON.Object);
				return a.Get(index, ref tmp) && JSON.Read(tmp, ref value);
			}
		}

		public static bool Get(this JSON.Array a, int index, ref bool? value)
		{
			if (a.Type(index) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			bool tmp = default(bool);
			if (!a.Get(index, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref bool[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			bool[] result = new bool[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref byte? value)
		{
			if (a.Type(index) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			byte tmp = default(byte);
			if (!a.Get(index, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref byte[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			byte[] result = new byte[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref sbyte? value)
		{
			if (a.Type(index) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			sbyte tmp = default(sbyte);
			if (!a.Get(index, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref sbyte[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			sbyte[] result = new sbyte[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref short? value)
		{
			if (a.Type(index) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			short tmp = default(short);
			if (!a.Get(index, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref short[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			short[] result = new short[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref ushort? value)
		{
			if (a.Type(index) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			ushort tmp = default(ushort);
			if (!a.Get(index, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref ushort[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			ushort[] result = new ushort[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref int? value)
		{
			if (a.Type(index) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			int tmp = default(int);
			if (!a.Get(index, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref int[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			int[] result = new int[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref uint? value)
		{
			if (a.Type(index) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			uint tmp = default(uint);
			if (!a.Get(index, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref uint[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			uint[] result = new uint[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref float? value)
		{
			if (a.Type(index) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			float tmp = default(float);
			if (!a.Get(index, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref float[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			float[] result = new float[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref decimal? value)
		{
			if (a.Type(index) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			decimal tmp = default(decimal);
			if (!a.Get(index, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref decimal[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			decimal[] result = new decimal[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref double? value)
		{
			if (a.Type(index) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			double tmp = default(double);
			if (!a.Get(index, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref double[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			double[] result = new double[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref string[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			string[] result = new string[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref JSON.Object[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			JSON.Object[] result = new JSON.Object[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref JSON.Array[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			JSON.Array[] result = new JSON.Array[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref JSON.Value? value)
		{
			if (a.Type(index) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			JSON.Value tmp = default(JSON.Value);
			if (!a.Get(index, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get(this JSON.Array a, int index, ref JSON.Value[] value)
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			JSON.Value[] result = new JSON.Value[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}

		public static bool Get<T>(this JSON.Array a, int index, ref T? value) where T : struct, JSON.Readable
		{
			if (a.Type(index) == JSON.Type.Null)
			{
				value = null;
				return true;
			}
			T tmp = default(T);
			if (!Get(a, index, ref tmp))
				return false;
			value = tmp;
			return true;
		}

		public static bool Get<T>(this JSON.Array a, int index, ref T[] value) where T : JSON.Readable, new()
		{
			JSON.Array array = default(JSON.Array);
			if (!a.Get(index, ref array))
				return false;
			T[] result = new T[array.Count];
			for (int i = 0; i < result.Length; ++i)
			{
				if (!array.Get(i, ref result[i]))
					return false;
			}
			value = result;
			return true;
		}
	}
}
