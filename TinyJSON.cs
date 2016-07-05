using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

namespace TinyJSON
{
	#region 默认JSON结点实现类
	public class Node
	{
		public enum Type
		{
			NONE,
			NULL,
			BOOLEAN,
			INT,
			DOUBLE,
			STRING,
			ARRAY,
			TABLE,
		}

		protected Node() { }

		public virtual Type TypeOf()
		{
			return Type.NONE;
		}

		public virtual bool IsAny()
		{
			return true;
		}

		public virtual bool IsNull()
		{
			return false;
		}

		public virtual bool IsBool()
		{
			return false;
		}

		public virtual bool IsInt()
		{
			return false;
		}

		public virtual bool IsNumber()
		{
			return false;
		}

		public virtual bool IsString()
		{
			return false;
		}

		public virtual bool IsArray()
		{
			return false;
		}

		public virtual bool IsTable()
		{
			return false;
		}

		protected virtual bool? AsBool()
		{
			return null;
		}

		protected virtual int? AsInt()
		{
			return null;
		}

		protected virtual double? AsNumber()
		{
			return null;
		}

		protected virtual string AsString()
		{
			return null;
		}

		protected virtual List<Node> AsArray()
		{
			return null;
		}

		protected virtual Dictionary<string, Node> AsTable()
		{
			return null;
		}

		public virtual void Clear()
		{
			throw new NotImplementedException();
		}

		public virtual int Count
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public virtual void Add(Node node)
		{
			throw new NotImplementedException();
		}

		public virtual Node this[int index]
		{
			get
			{
				return empty;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public virtual Node this[string key]
		{
			get
			{
				return empty;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public static bool operator true(Node node)
		{
			if (!node.IsAny())
				return false;
			if (node.IsNull())
				return false;
			if (node.IsBool())
				return node.AsBool() ?? false;
			return true;
		}

		public static bool operator false(Node node)
		{
			if (!node.IsAny())
				return true;
			if (node.IsNull())
				return true;
			if (node.IsBool())
				return !(node.AsBool() ?? false);
			return false;
		}

		public static explicit operator bool(Node node)
		{
			bool? result = node.AsBool();
			if (!result.HasValue)
				throw new InvalidCastException();
			return result.Value;
		}

		public static explicit operator int(Node node)
		{
			int? result = node.AsInt();
			if (!result.HasValue)
				throw new InvalidCastException();
			return result.Value;
		}

		public static explicit operator double(Node node)
		{
			double? result = node.AsNumber();
			if (!result.HasValue)
				throw new InvalidCastException();
			return result.Value;
		}

		public static explicit operator string(Node node)
		{
			string result = node.AsString();
			if (result == null)
				throw new InvalidCastException();
			return result;
		}

		public static explicit operator List<Node>(Node node)
		{
			List<Node> result = node.AsArray();
			if (result == null)
				throw new InvalidCastException();
			return result;
		}

		public static explicit operator Dictionary<string, Node>(Node node)
		{
			Dictionary<string, Node> result = node.AsTable();
			if (result == null)
				throw new InvalidCastException();
			return result;
		}

		public static implicit operator Node(bool b)
		{
			return new BoolNode(b);
		}

		public static implicit operator Node(int i)
		{
			return new IntNode(i);
		}

		public static implicit operator Node(double d)
		{
			return new NumberNode(d);
		}

		public static implicit operator Node(string s)
		{
			return new StringNode(s);
		}

		public static implicit operator Node(List<Node> list)
		{
			return new ArrayNode(list);
		}

		public static implicit operator Node(Dictionary<string, Node> table)
		{
			return new TableNode(table);
		}

		public static Node NewNull()
		{
			NullNode node = new NullNode();
			return node;
		}

		public static Node NewBool(bool b)
		{
			BoolNode node = new BoolNode(b);
			return node;
		}

		public static Node NewInt(int i)
		{
			IntNode node = new IntNode(i);
			return node;
		}

		public static Node NewNumber(double d)
		{
			NumberNode node = new NumberNode(d);
			return node;
		}

		public static Node NewString(string s)
		{
			StringNode node = new StringNode(s);
			return node;
		}

		public static Node NewArray()
		{
			return new ArrayNode();
		}

		public static Node NewTable()
		{
			return new TableNode();
		}

		protected static readonly EmptyNode empty = new EmptyNode();

		protected class EmptyNode : Node
		{
			public override string ToString()
			{
				return "";
			}
			public override bool IsAny()
			{
				return false;
			}
		}

		protected class NullNode : Node
		{
			public override Type TypeOf()
			{
				return Type.NULL;
			}
			public override string ToString()
			{
				return "null";
			}
			public override bool IsNull()
			{
				return true;
			}
		}

		protected class BoolNode : Node
		{
			protected readonly bool b;
			public BoolNode(bool b)
			{
				this.b = b;
			}
			public override Type TypeOf()
			{
				return Type.BOOLEAN;
			}
			public override string ToString()
			{
				return b ? "true" : "false";
			}
			public override bool IsBool()
			{
				return true;
			}
			protected override bool? AsBool()
			{
				return b;
			}
		}

		protected class IntNode : Node
		{
			protected readonly int i;
			public IntNode(int i)
			{
				this.i = i;
			}
			public override Type TypeOf()
			{
				return Type.INT;
			}
			public override string ToString()
			{
				return i.ToString(CultureInfo.InvariantCulture);
			}
			public override bool IsInt()
			{
				return true;
			}
			public override bool IsNumber()
			{
				return true;
			}
			protected override int? AsInt()
			{
				return i;
			}
			protected override double? AsNumber()
			{
				return i;
			}
		}

		protected class NumberNode : Node
		{
			protected readonly double d;
			public NumberNode(double d)
			{
				this.d = d;
			}
			public override Type TypeOf()
			{
				return Type.DOUBLE;
			}
			public override string ToString()
			{
				return d.ToString(CultureInfo.InvariantCulture);
			}
			public override bool IsNumber()
			{
				return true;
			}
			protected override double? AsNumber()
			{
				return d;
			}
		}

		protected class StringNode : Node
		{
			protected readonly string s;
			public StringNode(string s)
			{
				this.s = s;
			}
			public override Type TypeOf()
			{
				return Type.STRING;
			}
			public override string ToString()
			{
				return s;
			}
			public override bool IsString()
			{
				return true;
			}
			protected override string AsString()
			{
				return s;
			}
		}

		protected class ArrayNode : Node
		{
			protected readonly List<Node> list;
			public ArrayNode()
			{
				list = new List<Node>();
			}
			public ArrayNode(List<Node> lst)
			{
				list = lst;
			}
			public override Type TypeOf()
			{
				return Type.ARRAY;
			}
			public override string ToString()
			{
				return list.ToString();
			}
			public override bool IsArray()
			{
				return true;
			}
			protected override List<Node> AsArray()
			{
				return list;
			}
			public override void Clear()
			{
				list.Clear();
			}
			public override int Count
			{
				get
				{
					return list.Count;
				}
			}
			public override void Add(Node node)
			{
				if (node == empty)
					throw new ArgumentException();
				if (node == null)
					node = NewNull();
				list.Add(node);
			}
			public override Node this[int index]
			{
				get
				{
					if (index < 0 || index >= list.Count)
						return empty;
					return list[index];
				}
				set
				{
					if (value == null)
					{
						list[index] = NewNull();
					}
					else
					{
						if (value == empty)
							throw new ArgumentException();
						list[index] = value;
					}
				}
			}
		}

		protected class TableNode : Node
		{
			protected readonly Dictionary<string, Node> table;
			public TableNode()
			{
				table = new Dictionary<string, Node>();
			}
			public TableNode(Dictionary<string, Node> tb)
			{
				table = tb;
			}
			public override Type TypeOf()
			{
				return Type.TABLE;
			}
			public override string ToString()
			{
				return table.ToString();
			}
			public override bool IsTable()
			{
				return true;
			}
			protected override Dictionary<string, Node> AsTable()
			{
				return table;
			}
			public override void Clear()
			{
				table.Clear();
			}
			public override int Count
			{
				get
				{
					return table.Count;
				}
			}
			public override Node this[string key]
			{
				get
				{
					Node node;
					if (!table.TryGetValue(key, out node))
						return empty;
					return node;
				}
				set
				{
					if (value == null)
					{
						table.Remove(key);
					}
					else
					{
						if (value == empty)
							throw new ArgumentException();
						table[key] = value;
					}
				}
			}
		}
	}
	#endregion

	#region 自定义事件接口
	public interface Handler
	{
		bool StartArray();
		bool StartTable();
		bool EndArray();
		bool EndTable();
		bool Null();
		bool Key(string key);
		bool Bool(bool b);
		bool String(string s);
		bool Double(double d);
		bool Int(int i);
	}

	public interface Writer
	{
		bool Write(Handler handler);
	}
	#endregion

	public struct Parser
	{
		#region 解析选项
		public struct Options
		{
			public bool comment;
		}

		public Parser(Options options)
		{
			context = new Context {allowcomment = options.comment};
		}
		#endregion

		#region 错误相关
		public enum Errors
		{
			NONE,
			INVALID_CHAR,
			INVALID_KEYWORD,
			INVALID_ESCAPE_SEQUENCE,
			INVALID_UNICODE_SEQUENCE,
			INVALID_NUMBER,
			NESTING_DEPTH_REACHED,
			UNBALANCED_COLLECTION,
			EXPECTED_KEY,
			EXPECTED_COLON,
			OUT_OF_MEMORY,
		};

		public Errors Error()
		{
			return context.error;
		}

		public static string Error(Errors e)
		{
			return null;
		}
		#endregion

		#region 加速Pow运算
		private static readonly double[] e = { // 1e-0...1e308: 309 * 8 bytes = 2472 bytes
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

		#region 内部分类、状态和状态表
		private enum Tokens
		{
			SPACE = 0,	/* space */
			WHITE,		/* other whitespace */
			LCURB,		/* {  */
			RCURB,		/* } */
			LSQRB,		/* [ */
			RSQRB,		/* ] */
			COLON,		/* : */
			COMMA,		/* , */
			QUOTE,		/* " */
			BACKS,		/* \ */
			SLASH,		/* / */
			PLUS,		/* + */
			MINUS,		/* - */
			POINT,		/* . */
			ZERO,		/* 0 */
			DIGIT,		/* 123456789 */
			LOW_A,		/* a */
			LOW_B,		/* b */
			LOW_C,		/* c */
			LOW_D,		/* d */
			LOW_E,		/* e */
			LOW_F,		/* f */
			LOW_L,		/* l */
			LOW_N,		/* n */
			LOW_R,		/* r */
			LOW_S,		/* s */
			LOW_T,		/* t */
			LOW_U,		/* u */
			ABCDF,		/* ABCDF */
			E,			/* E */
			ETC,		/* everything else */
			STAR,		/* * */
			__
		};

		private static readonly Tokens[] ascii_token = {
			/*
				This array maps the 128 ASCII characters into character classes.
				The remaining Unicode characters should be mapped to C_ETC.
				Non-whitespace control characters are errors.
			*/
			Tokens.WHITE, Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,
			Tokens.__,    Tokens.WHITE, Tokens.WHITE, Tokens.__,    Tokens.__,    Tokens.WHITE, Tokens.__,    Tokens.__,
			Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,
			Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,    Tokens.__,

			Tokens.SPACE, Tokens.ETC,   Tokens.QUOTE, Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,
			Tokens.ETC,   Tokens.ETC,   Tokens.STAR,  Tokens.PLUS,  Tokens.COMMA, Tokens.MINUS, Tokens.POINT, Tokens.SLASH,
			Tokens.ZERO,  Tokens.DIGIT, Tokens.DIGIT, Tokens.DIGIT, Tokens.DIGIT, Tokens.DIGIT, Tokens.DIGIT, Tokens.DIGIT,
			Tokens.DIGIT, Tokens.DIGIT, Tokens.COLON, Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,

			Tokens.ETC,   Tokens.ABCDF, Tokens.ABCDF, Tokens.ABCDF, Tokens.ABCDF, Tokens.E,     Tokens.ABCDF, Tokens.ETC,
			Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,
			Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,
			Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.LSQRB, Tokens.BACKS, Tokens.RSQRB, Tokens.ETC,   Tokens.ETC,

			Tokens.ETC,   Tokens.LOW_A, Tokens.LOW_B, Tokens.LOW_C, Tokens.LOW_D, Tokens.LOW_E, Tokens.LOW_F, Tokens.ETC,
			Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.LOW_L, Tokens.ETC,   Tokens.LOW_N, Tokens.ETC,
			Tokens.ETC,   Tokens.ETC,   Tokens.LOW_R, Tokens.LOW_S, Tokens.LOW_T, Tokens.LOW_U, Tokens.ETC,   Tokens.ETC,
			Tokens.ETC,   Tokens.ETC,   Tokens.ETC,   Tokens.LCURB, Tokens.ETC,   Tokens.RCURB, Tokens.ETC,   Tokens.ETC,
		};

		private enum TokenStates
		{
			OK = 0,	/* any		*/
			ST,		/* string	*/
			ES,		/* escape	*/
			U1,		/* \u1		*/
			U2,		/* \u2		*/
			U3,		/* \u3		*/
			U4,		/* \u4		*/
			MI,		/* minus	*/
			ZE,		/* zero		*/
			IT,		/* integer	*/
			FT,		/* float	*/
			E1,		/* e		*/
			E2,		/* ex		*/
			E3,		/* exp		*/
			T1,		/* tr		*/
			T2,		/* tru		*/
			T3,		/* true		*/
			T4,		/* true|	*/
			F1,		/* fa		*/
			F2,		/* fal		*/
			F3,		/* fals		*/
			F4,		/* false	*/
			F5,		/* false|	*/
			N1,		/* nu		*/
			N2,		/* nul		*/
			N3,		/* null		*/
			N4,		/* null|	*/
			C1,		/* /		*/
			C2,		/* /*		*/
			C3,		/* *		*/
			FX,		/* *.* *eE*	*/
			__
		}

		private enum TokenActions
		{
			AB,		/* [		*/
			AE,		/* ]		*/
			OB,		/* {		*/
			OE,		/* }		*/
			CO,		/* colon	*/
			CM,		/* comma	*/
			BG,		/* begin	*/
			IT,		/* integer	*/
			FT,		/* number	*/
			NU,		/* null		*/
			TR,		/* true		*/
			FL,		/* false	*/
			ST,		/* string	*/
			IT_AE,
			IT_OE,
			IT_CM,
			FT_AE,
			FT_OE,
			FT_CM,
			NU_AE,
			NU_OE,
			NU_CM,
			TR_AE,
			TR_OE,
			TR_CM,
			FL_AE,
			FL_OE,
			FL_CM,
			__
		}

		private enum Words
		{
			ARRAY_BEGIN = 0,	/* [		*/
			ARRAY_END,			/* ]		*/
			OBJECT_BEGIN,		/* {		*/
			OBJECT_END,			/* }		*/
			INTEGER,			/* integer	*/
			FLOAT,				/* number	*/
			NULL,				/* null		*/
			TRUE,				/* true		*/
			FALSE,				/* false	*/
			STRING,				/* string	*/
			COLON,				/* :		*/
			COMMA,				/* ,		*/
		};

		private enum WordStates
		{
			OE = 0,		/* empty object	*/
			CO,			/* need colon	*/
			OV,			/* object value	*/
			ON,			/* object next	*/
			KY,			/* need key		*/

			AE,			/* empty array	*/
			AN,			/* array next	*/
			AV,			/* array value	*/

			OC,
			AC,
			__
		}

		private static readonly TokenStates[,] token_state_table = {
			/*
				The token state table takes the current state and the current symbol, and returns either a new state.
				A JSON text is accepted if at the end of the text the state is OK.
							  space           white             {               }               [               ]               :               ,               "               \               /               +               -               .               0              1-9              a               b               c               d               e               f               l               n               r               s               t               u             ABCDF             E              etc              *
			*/
			/* OK */	{TokenStates.OK, TokenStates.OK, TokenStates.OK, TokenStates.OK, TokenStates.OK, TokenStates.OK, TokenStates.OK, TokenStates.OK, TokenStates.ST, TokenStates.__, TokenStates.C1, TokenStates.__, TokenStates.MI, TokenStates.__, TokenStates.ZE, TokenStates.IT, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.F1, TokenStates.__, TokenStates.N1, TokenStates.__, TokenStates.__, TokenStates.T1, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* ST */	{TokenStates.ST, TokenStates.__, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.OK, TokenStates.ES, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST},
			/* ES */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.ST, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.ST, TokenStates.__, TokenStates.ST, TokenStates.ST, TokenStates.__, TokenStates.ST, TokenStates.U1, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* U1 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.U2, TokenStates.U2, TokenStates.U2, TokenStates.U2, TokenStates.U2, TokenStates.U2, TokenStates.U2, TokenStates.U2, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.U2, TokenStates.U2, TokenStates.__, TokenStates.__},
			/* U2 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.U3, TokenStates.U3, TokenStates.U3, TokenStates.U3, TokenStates.U3, TokenStates.U3, TokenStates.U3, TokenStates.U3, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.U3, TokenStates.U3, TokenStates.__, TokenStates.__},
			/* U3 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.U4, TokenStates.U4, TokenStates.U4, TokenStates.U4, TokenStates.U4, TokenStates.U4, TokenStates.U4, TokenStates.U4, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.U4, TokenStates.U4, TokenStates.__, TokenStates.__},
			/* U4 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.ST, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.ST, TokenStates.ST, TokenStates.__, TokenStates.__},
			/* MI */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.ZE, TokenStates.IT, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* ZE */	{TokenStates.OK, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.__, TokenStates.C1, TokenStates.__, TokenStates.__, TokenStates.FX, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* IT */	{TokenStates.OK, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.__, TokenStates.C1, TokenStates.__, TokenStates.__, TokenStates.FX, TokenStates.IT, TokenStates.IT, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.E1, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.E1, TokenStates.__, TokenStates.__},
			/* FT */	{TokenStates.OK, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.__, TokenStates.C1, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.FT, TokenStates.FT, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.E1, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.E1, TokenStates.__, TokenStates.__},
			/* E1 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.E2, TokenStates.E2, TokenStates.__, TokenStates.E3, TokenStates.E3, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* E2 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.E3, TokenStates.E3, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* E3 */	{TokenStates.OK, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.__, TokenStates.C1, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.E3, TokenStates.E3, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* T1 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.T2, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* T2 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.T3, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* T3 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.T4, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* T4 */	{TokenStates.OK, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.__, TokenStates.C1, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* F1 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.F2, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* F2 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.F3, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* F3 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.F4, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* F4 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.F5, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* F5 */	{TokenStates.OK, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.__, TokenStates.C1, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* N1 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.N2, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* N2 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.N3, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* N3 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.N4, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* N4 */	{TokenStates.OK, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.OK, TokenStates.__, TokenStates.__, TokenStates.C1, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
			/* C1 */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.C2},
			/* C2 */	{TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C3},
			/* C3 */	{TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.OK, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C2, TokenStates.C3},
			/* FX */	{TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.FT, TokenStates.FT, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__, TokenStates.__},
		};

		private static readonly TokenActions[,] token_action_table = {
			/*
				The token state table takes the current state and the current symbol, and sometimes do an action.
				A JSON text is accepted if at the end of the text the state is OK.
							  space            white              {                }                [                ]                :                ,                "                \                /                +                -                .                0               1-9               a                b                c                d                e                f                l                n                r                s                t                u              ABCDF              E               etc               *
			*/
			/* OK */	{TokenActions.__, TokenActions.__, TokenActions.OB, TokenActions.OE, TokenActions.AB, TokenActions.AE, TokenActions.CO, TokenActions.CM, TokenActions.BG, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.BG, TokenActions.__, TokenActions.BG, TokenActions.BG, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.BG, TokenActions.__, TokenActions.BG, TokenActions.__, TokenActions.__, TokenActions.BG, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* ST */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.ST, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* ES */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* U1 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* U2 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* U3 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* U4 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* MI */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* ZE */	{TokenActions.IT, TokenActions.IT, TokenActions.__, TokenActions.IT_OE, TokenActions.__, TokenActions.IT_AE, TokenActions.__, TokenActions.IT_CM, TokenActions.__, TokenActions.__, TokenActions.IT, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* IT */	{TokenActions.IT, TokenActions.IT, TokenActions.__, TokenActions.IT_OE, TokenActions.__, TokenActions.IT_AE, TokenActions.__, TokenActions.IT_CM, TokenActions.__, TokenActions.__, TokenActions.IT, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* FT */	{TokenActions.FT, TokenActions.FT, TokenActions.__, TokenActions.FT_OE, TokenActions.__, TokenActions.FT_AE, TokenActions.__, TokenActions.FT_CM, TokenActions.__, TokenActions.__, TokenActions.FT, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* E1 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* E2 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* E3 */	{TokenActions.FT, TokenActions.FT, TokenActions.__, TokenActions.FT_OE, TokenActions.__, TokenActions.FT_AE, TokenActions.__, TokenActions.FT_CM, TokenActions.__, TokenActions.__, TokenActions.FT, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* T1 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* T2 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* T3 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* T4 */	{TokenActions.TR, TokenActions.TR, TokenActions.__, TokenActions.TR_OE, TokenActions.__, TokenActions.TR_AE, TokenActions.__, TokenActions.TR_CM, TokenActions.__, TokenActions.__, TokenActions.TR, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* F1 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* F2 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* F3 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* F4 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* F5 */	{TokenActions.FL, TokenActions.FL, TokenActions.__, TokenActions.FL_OE, TokenActions.__, TokenActions.FL_AE, TokenActions.__, TokenActions.FL_CM, TokenActions.__, TokenActions.__, TokenActions.FL, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* N1 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* N2 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* N3 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* N4 */	{TokenActions.NU, TokenActions.NU, TokenActions.__, TokenActions.NU_OE, TokenActions.__, TokenActions.NU_AE, TokenActions.__, TokenActions.NU_CM, TokenActions.__, TokenActions.__, TokenActions.NU, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* C1 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* C2 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* C3 */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
			/* FX */	{TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__, TokenActions.__},
		};

		private static readonly WordStates[,] word_state_table = {
			/*
				The token state table takes the current state and the current symbol, and sometimes do an action.
				A JSON text is accepted if at the end of the text the state is OK.
						      [              ]              {              }             int           float          null           true           false          string           :              ,
			*/
			/* OE */	{WordStates.__, WordStates.__, WordStates.__, WordStates.OC, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.CO, WordStates.__, WordStates.__},
			/* CO */	{WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.OV, WordStates.__},
			/* OV */	{WordStates.ON, WordStates.__, WordStates.ON, WordStates.__, WordStates.ON, WordStates.ON, WordStates.ON, WordStates.ON, WordStates.ON, WordStates.ON, WordStates.__, WordStates.__},
			/* ON */	{WordStates.__, WordStates.__, WordStates.__, WordStates.OC, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.KY},
			/* KY */	{WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.CO, WordStates.__, WordStates.__},

			/* AE */	{WordStates.AN, WordStates.AC, WordStates.AN, WordStates.__, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.__, WordStates.__},
			/* AN */	{WordStates.__, WordStates.AC, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.AV},
			/* AV */	{WordStates.AN, WordStates.__, WordStates.AN, WordStates.__, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.__, WordStates.__},
		};
		#endregion

		#region 默认解析处理，生成Node
		private class NodeHandler : Stack<Node>, Handler
		{
			private string key;
			public Node root;

			private bool Value(Node node)
			{
				if (root == null)
				{
					root = node;
				}
				else if (Count > 0)
				{
					Node last = Peek();
					switch (last.TypeOf())
					{
						case Node.Type.ARRAY:
							last.Add(node);
							break;
						case Node.Type.TABLE:
							last[key] = node;
							break;
						default:
							return false;
					}
				}
				return true;
			}

			public bool StartArray()
			{
				Node newnode = Node.NewArray();
				if (!Value(newnode))
					return false;
				Push(newnode);
				return true;
			}

			public bool StartTable()
			{
				Node newnode = Node.NewTable();
				if (!Value(newnode))
					return false;
				Push(newnode);
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
				return Value(Node.NewNull());
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
		}
		#endregion

		#region 内部类
		private struct Context
		{
			public Handler handler;
			public byte[] input;
			public int start;
			public int upon;
			public int outset;
			public int cursor;
			public TokenStates tokenstate;
			public Errors error;
			public Stack<WordStates> wordstates;
			public byte[] buffer;
			public int bufferused;
			public bool allowcomment;

			public void BufferAdd(byte c)
			{
				if (bufferused == buffer.Length)
				{
					int capacity = bufferused * 2;
					byte[] newbuffer = new byte[capacity];
					Array.Copy(buffer, newbuffer, bufferused);
					buffer = newbuffer;
				}
				buffer[bufferused++] = c;
			}

			public void Error()
			{
			}
		}
		#endregion

		#region 内部变量
		private Context context;
		#endregion

		#region 内部解析实现
		private bool ParseWord(Words word)
		{
			WordStates state = WordStates.__;
			if (context.wordstates.Count != 0)
			{
				WordStates oldstate = context.wordstates.Peek();
				state = word_state_table[(int)oldstate, (int)word];
				if (state == WordStates.__)
				{
					context.Error();
					return false;
				}
				context.wordstates.Pop();
				context.wordstates.Push(state);
			}
			switch (word)
			{
				case Words.ARRAY_BEGIN:
					{
						if (!context.handler.StartArray())
							return false;
						context.wordstates.Push(WordStates.AE);
					}
					break;
				case Words.OBJECT_BEGIN:
					{
						if (!context.handler.StartTable())
							return false;
						context.wordstates.Push(WordStates.OE);
					}
					break;
				case Words.ARRAY_END:
					{
						if (!context.handler.EndArray())
							return false;
						context.wordstates.Pop();
					}
					break;
				case Words.OBJECT_END:
					{
						if (!context.handler.EndTable())
							return false;
						context.wordstates.Pop();
					}
					break;
				case Words.INTEGER:
					{
						int n = 0;
						double d = 0;
						bool minus = false;
						bool use_double = false;
						for (int i = context.outset; i < context.cursor; ++i)
						{
							byte c = context.input[i];
							if (c >= (byte)'0' && c <= (byte)'9')
							{
								c -= (byte)'0';
								if (use_double)
								{
									d = d * 10 + c;
								}
								else
								{
									if (n > int.MaxValue / 10 || (n == int.MaxValue / 10 && c > int.MaxValue % 10))
									{
										use_double = true;
										d = n;
										d = d * 10 + c;
										continue;
									}
									n = n * 10 + c;
								}
							}
							else if (c == '-')
							{
								minus = true;
							}
						}
						if (use_double)
						{
							if (!context.handler.Double(minus ? -d : d))
								return false;
						}
						else
						{
							if (!context.handler.Int(minus ? -n : n))
								return false;
						}
					}
					break;
				case Words.FLOAT:
					{
						int n = 0;
						double d = 0;
						int dot = -1;
						int exp = 0;
						bool minus = false;
						bool use_double = false;
						for (int i = context.outset; i < context.cursor; ++i)
						{
							byte c = context.input[i];
							if (c >= (byte)'0' && c <= (byte)'9')
							{
								c -= (byte)'0';
								if (use_double)
								{
									d = d * 10 + c;
								}
								else
								{
									if (n > int.MaxValue / 10 || (n == int.MaxValue / 10 && c > int.MaxValue % 10))
									{
										use_double = true;
										d = n;
										d = d * 10 + c;
									}
									else
									{
										n = n * 10 + c;
									}
								}
								if (dot >= 0)
								{
									++dot;
								}
							}
							else if (c == '-')
							{
								minus = true;
							}
							else if (c == '.')
							{
								dot = 0;
							}
							else if (c == 'e' || c == 'E')
							{
								bool expminus = false;
								for (int j = i; j < context.cursor; ++j)
								{
									c = context.input[j];
									if (c >= (byte)'0' && c <= (byte)'9')
									{
										exp = exp * 10 + (c - '0');
									}
									else if (c == '-')
									{
										expminus = true;
									}
								}
								exp = expminus ? -exp : exp;
								break;
							}
						}
						if (use_double)
						{
							if (!context.handler.Double((minus ? -d : d) * Pow10((dot < 0 ? 0 : -dot) + exp)))
								return false;
						}
						else
						{
							if (!context.handler.Double((minus ? -n : n) * Pow10((dot < 0 ? 0 : -dot) + exp)))
								return false;
						}
					}
					break;
				case Words.NULL:
					{
						if (!context.handler.Null())
							return false;
					}
					break;
				case Words.TRUE:
					{
						if (!context.handler.Bool(true))
							return false;
					}
					break;
				case Words.FALSE:
					{
						if (!context.handler.Bool(false))
							return false;
					}
					break;
				case Words.STRING:
					{
						context.bufferused = 0;
						for (int i = context.outset; i < context.cursor; ++i)
						{
							byte c = context.input[i];
							if (c == (byte)'\\')
							{
								switch (context.input[++i])
								{
									case (byte)'b':
										context.BufferAdd((byte)'\b');
										break;
									case (byte)'f':
										context.BufferAdd((byte)'\f');
										break;
									case (byte)'n':
										context.BufferAdd((byte)'\n');
										break;
									case (byte)'r':
										context.BufferAdd((byte)'\r');
										break;
									case (byte)'t':
										context.BufferAdd((byte)'\t');
										break;
									case (byte)'"':
										context.BufferAdd((byte)'"');
										break;
									case (byte)'\\':
										context.BufferAdd((byte)'\\');
										break;
									case (byte)'/':
										context.BufferAdd((byte)'/');
										break;
									case (byte)'u':
										{
											int unicode = 0;
											for (int j = 1; j <= 4; ++j)
											{
												c = context.input[i + j];
												if (c >= (byte)'a')
													c -= (byte)'a' - 10;
												else if (c >= (byte)'A')
													c -= (byte)'A' - 10;
												else
													c -= (byte)'0';
												unicode <<= 4;
												unicode |= c;
											}
											i += 4;
											int trail;
											byte leadbit;
											if (unicode < 0x80)
											{
												trail = 0;
												leadbit = 0x00;
											}
											else if (unicode < 0x800)
											{
												trail = 1;
												leadbit = 0xc0;
											}
											else
											{
												trail = 2;
												leadbit = 0xe0;
											}
											context.BufferAdd((byte)((unicode >> (trail * 6)) | leadbit));
											for (int j = trail * 6 - 6; j >= 0; j -= 6)
											{
												context.BufferAdd((byte)(((unicode >> j) & 0x3F) | 0x80));
											}
										}
										break;
								}
							}
							else
							{
								context.BufferAdd(c);
							}
						}
						string str = Encoding.UTF8.GetString(context.buffer, 0, context.bufferused);
						if (state == WordStates.CO)
						{
							if (!context.handler.Key(str))
								return false;
						}
						else
						{
							if (!context.handler.String(str))
								return false;
						}
					}
					break;
			}
			return true;
		}

		private bool DoToken(byte c)
		{
			Tokens token;
			if (c >= 128)
			{
				token = Tokens.ETC;
			}
			else
			{
				token = ascii_token[c];
				if (token == Tokens.__)
				{
					context.Error();
					return false;
				}
			}
			TokenStates state = context.tokenstate;
			context.tokenstate = token_state_table[(int)state, (int)token];
			TokenActions action = token_action_table[(int)state, (int)token];
			if (context.tokenstate == TokenStates.__)
			{
				context.tokenstate = state;
				context.Error();
				return false;
			}
			if (context.tokenstate == TokenStates.C1)
			{
				if (!context.allowcomment)
				{
					context.Error();
					return false;
				}
			}
			switch (action)
			{
				case TokenActions.AB:
					{
						context.outset = context.cursor - 1;
						if (!ParseWord(Words.ARRAY_BEGIN))
						{
							return false;
						}
					}
					break;
				case TokenActions.AE:
					{
						context.outset = context.cursor - 1;
						if (!ParseWord(Words.ARRAY_END))
						{
							return false;
						}
					}
					break;
				case TokenActions.OB:
					{
						context.outset = context.cursor - 1;
						if (!ParseWord(Words.OBJECT_BEGIN))
						{
							return false;
						}
					}
					break;
				case TokenActions.OE:
					{
						context.outset = context.cursor - 1;
						if (!ParseWord(Words.OBJECT_END))
						{
							return false;
						}
					}
					break;
				case TokenActions.CO:
					{
						context.outset = context.cursor - 1;
						if (!ParseWord(Words.COLON))
						{
							return false;
						}
					}
					break;
				case TokenActions.CM:
					{
						context.outset = context.cursor - 1;
						if (!ParseWord(Words.COMMA))
						{
							return false;
						}
					}
					break;
				case TokenActions.BG:
					{
						context.outset = context.cursor - 1;
					}
					break;
				case TokenActions.IT:
					{
						if (!ParseWord(Words.INTEGER))
						{
							return false;
						}
					}
					break;
				case TokenActions.FT:
					{
						if (!ParseWord(Words.FLOAT))
						{
							return false;
						}
					}
					break;
				case TokenActions.NU:
					{
						if (!ParseWord(Words.NULL))
						{
							return false;
						}
					}
					break;
				case TokenActions.TR:
					{
						if (!ParseWord(Words.TRUE))
						{
							return false;
						}
					}
					break;
				case TokenActions.FL:
					{
						if (!ParseWord(Words.FALSE))
						{
							return false;
						}
					}
					break;
				case TokenActions.ST:
					{
						++context.outset;
						--context.cursor;
						bool result = ParseWord(Words.STRING);
						++context.cursor;
						if (!result)
						{
							return false;
						}
					}
					break;
				case TokenActions.IT_AE:
					{
						--context.cursor;
						bool result = ParseWord(Words.INTEGER);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.ARRAY_END))
						{
							return false;
						}
					}
					break;
				case TokenActions.IT_OE:
					{
						--context.cursor;
						bool result = ParseWord(Words.INTEGER);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.OBJECT_END))
						{
							return false;
						}
					}
					break;
				case TokenActions.IT_CM:
					{
						--context.cursor;
						bool result = ParseWord(Words.INTEGER);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.COMMA))
						{
							return false;
						}
					}
					break;
				case TokenActions.FT_AE:
					{
						--context.cursor;
						bool result = ParseWord(Words.FLOAT);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.ARRAY_END))
						{
							return false;
						}
					}
					break;
				case TokenActions.FT_OE:
					{
						--context.cursor;
						bool result = ParseWord(Words.FLOAT);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.OBJECT_END))
						{
							return false;
						}
					}
					break;
				case TokenActions.FT_CM:
					{
						--context.cursor;
						bool result = ParseWord(Words.FLOAT);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.COMMA))
						{
							return false;
						}
					}
					break;
				case TokenActions.NU_AE:
					{
						--context.cursor;
						bool result = ParseWord(Words.NULL);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.ARRAY_END))
						{
							return false;
						}
					}
					break;
				case TokenActions.NU_OE:
					{
						--context.cursor;
						bool result = ParseWord(Words.NULL);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.OBJECT_END))
						{
							return false;
						}
					}
					break;
				case TokenActions.NU_CM:
					{
						--context.cursor;
						bool result = ParseWord(Words.NULL);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.COMMA))
						{
							return false;
						}
					}
					break;
				case TokenActions.TR_AE:
					{
						--context.cursor;
						bool result = ParseWord(Words.TRUE);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.ARRAY_END))
						{
							return false;
						}
					}
					break;
				case TokenActions.TR_OE:
					{
						--context.cursor;
						bool result = ParseWord(Words.TRUE);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.OBJECT_END))
						{
							return false;
						}
					}
					break;
				case TokenActions.TR_CM:
					{
						--context.cursor;
						bool result = ParseWord(Words.TRUE);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.COMMA))
						{
							return false;
						}
					}
					break;
				case TokenActions.FL_AE:
					{
						--context.cursor;
						bool result = ParseWord(Words.FALSE);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.ARRAY_END))
						{
							return false;
						}
					}
					break;
				case TokenActions.FL_OE:
					{
						--context.cursor;
						bool result = ParseWord(Words.FALSE);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.OBJECT_END))
						{
							return false;
						}
					}
					break;
				case TokenActions.FL_CM:
					{
						--context.cursor;
						bool result = ParseWord(Words.FALSE);
						++context.cursor;
						context.outset = context.cursor - 1;
						if (!result || !ParseWord(Words.COMMA))
						{
							return false;
						}
					}
					break;
			}
			return true;
		}

		private bool ParseToken()
		{
			while (context.cursor < context.upon)
			{
				byte c = context.input[context.cursor++];
				if (!DoToken(c))
					return false;
			}
			if (!DoToken(0))
				return false;
			if (context.tokenstate != TokenStates.OK)
			{
				context.Error();
				return false;
			}
			if (context.wordstates.Count != 0)
			{
				context.Error();
				return false;
			}
			return true;
		}
		#endregion

		#region 对外解析接口
		public Node Load(string input)
		{
			return Load(Encoding.UTF8.GetBytes(input));
		}

		public Node Load(byte[] input)
		{
			NodeHandler handler = new NodeHandler();
			if (!Load(input, handler))
				return null;
			return handler.root;
		}

		public Node Load(byte[] input, int start)
		{
			NodeHandler handler = new NodeHandler();
			if (!Load(input, start, handler))
				return null;
			return handler.root;
		}

		public Node Load(byte[] input, int start, int count)
		{
			NodeHandler handler = new NodeHandler();
			if (!Load(input, start, count, handler))
				return null;
			return handler.root;
		}

		public bool Load(string input, Handler handler)
		{
			return Load(Encoding.UTF8.GetBytes(input), handler);
		}

		public bool Load(byte[] input, Handler handler)
		{
			int start = 0;
			if (input.Length >= 3 && input[0] == 0xEF && input[1] == 0xBB && input[2] == 0xBF)
			{
				start += 3;
			}
			return Load(input, start, handler);
		}

		public bool Load(byte[] input, int start, Handler handler)
		{
			return Load(input, start, input.Length - start, handler);
		}

		public bool Load(byte[] input, int start, int count, Handler handler)
		{
			context.handler = handler;
			context.input = input;
			context.outset = 0;
			context.start = context.cursor = start;
			context.upon = start + count;
			context.tokenstate = TokenStates.OK;
			context.error = Errors.NONE;
			if (context.wordstates == null)
			{
				context.wordstates = new Stack<WordStates>(16);
			}
			else
			{
				context.wordstates.Clear();
			}
			if (context.buffer == null)
			{
				context.buffer = new byte[1024];
			}
			context.bufferused = 0;
			if (!ParseToken())
			{
				return false;
			}
			return true;
		}
		#endregion
	}

	public struct Printer
	{
		#region 序列化选项
		public struct Options
		{
			public bool pretty;
			public bool escape;
		}

		public Printer(Options options)
		{
			buffer = options.pretty ? new PrettyBuffer() : new Buffer();
			buffer.escape = options.escape;
		}
		#endregion

		#region 转义字符表
		private static readonly char[] escapes = {
			/*
				This array maps the 128 ASCII characters into character escape.
				'u' indicate must transform to \u00xx.
			*/
			'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'b', 't', 'n', 'u', 'f',  'r', 'u', 'u',
			'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u',  'u', 'u', 'u',
			'#', '#', '"', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#',  '#', '#', '#',
			'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#',  '#', '#', '#',
			'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#',  '#', '#', '#',
			'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '\\', '#', '#', '#',
			'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#',  '#', '#', '#',
			'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#',  '#', '#', '#',
		};
		#endregion

		#region 内部类
		private class Buffer : Handler
		{
			public bool escape;
			protected bool empty;
			protected int depth;
			protected bool table;
			protected readonly StringBuilder buffer;

			public Buffer()
			{
				escape = false;
				empty = true;
				depth = 0;
				buffer = new StringBuilder();
			}
			private void WriteString(string s)
			{
				buffer.Append('"');
				for (int i = 0, j = s.Length; i < j; ++i)
				{
					char c = s[i];
					if (c < 128)
					{
						char esc = escapes[c];
						if (esc == '#')
						{
							buffer.Append(c);
						}
						else if (esc == 'u')
						{
							buffer.Append("\\u00");
							buffer.Append(((uint)c).ToString("X2", CultureInfo.InvariantCulture));
						}
						else
						{
							buffer.Append('\\');
							buffer.Append(esc);
						}
					}
					else if (escape)
					{
						buffer.Append("\\u");
						buffer.Append(((uint)c).ToString("X4", CultureInfo.InvariantCulture));
					}
					else
					{
						buffer.Append(c);
					}
				}
				buffer.Append('"');
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
				return buffer.ToString();
			}
			public virtual void Reset()
			{
				empty = true;
				depth = 0;
				buffer.Length = 0;
			}
			public bool Null()
			{
				Prefix();
				empty = false;
				table = false;
				buffer.Append("null");
				return true;
			}
			public bool Bool(bool b)
			{
				Prefix();
				empty = false;
				table = false;
				buffer.Append(b ? "true" : "false");
				return true;
			}
			public bool Int(int i)
			{
				Prefix();
				empty = false;
				table = false;
				buffer.Append(i.ToString(CultureInfo.InvariantCulture));
				return true;
			}
			public bool Double(double d)
			{
				Prefix();
				empty = false;
				table = false;
				buffer.Append(d.ToString(CultureInfo.InvariantCulture));
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
			public virtual bool StartArray()
			{
				Prefix();
				++depth;
				empty = true;
				table = false;
				buffer.Append('[');
				return true;
			}
			public virtual bool EndArray()
			{
				--depth;
				if (!empty)
					WriteIndent();
				empty = false;
				buffer.Append(']');
				return true;
			}
			public virtual bool StartTable()
			{
				Prefix();
				++depth;
				empty = true;
				table = false;
				buffer.Append('{');
				return true;
			}
			public virtual bool EndTable()
			{
				--depth;
				if (!empty)
					WriteIndent();
				empty = false;
				buffer.Append('}');
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
			protected virtual void WriteIndent()
			{
			}
			protected virtual void WriteComma()
			{
				buffer.Append(',');
			}
			protected virtual void WriteColon()
			{
				buffer.Append(':');
			}
		}

		private class PrettyBuffer : Buffer
		{
			protected override void WriteColon()
			{
				buffer.Append(" : ");
			}
			protected override void WriteIndent()
			{
				buffer.Append('\n');
				for (int i = 0, j = depth; i < j; ++i)
				{
					buffer.Append('\t');
				}
			}
		}
		#endregion

		#region 内部变量
		private Buffer buffer;
		private void Reset()
		{
			if (buffer == null)
				buffer = new Buffer { escape = false };
			buffer.Reset();
		}
		#endregion

		#region 默认序列化实现，序列化Node
		private class NodeWriter : Writer
		{
			private readonly Node rootnode;
			private readonly Dictionary<Node, bool> writed;

			public NodeWriter(Node node)
			{
				rootnode = node;
				writed = new Dictionary<Node, bool>();
			}

			public bool Write(Handler handler)
			{
				return Write(handler, rootnode);
			}

			private bool Write(Handler handler, Node node)
			{
				switch (node.TypeOf())
				{
					case Node.Type.NULL:
						return handler.Null();
					case Node.Type.BOOLEAN:
						return handler.Bool((bool)node);
					case Node.Type.INT:
						return handler.Int((int)node);
					case Node.Type.DOUBLE:
						return handler.Double((double)node);
					case Node.Type.STRING:
						return handler.String((string)node);
					case Node.Type.ARRAY:
						{
							if (writed.ContainsKey(node))
								return false;
							writed.Add(node, true);
							List<Node> list = (List<Node>)node;
							if (!handler.StartArray())
								return false;
							for (int i = 0, j = list.Count; i < j; ++i)
							{
								if (!Write(handler, list[i]))
									return false;
							}
							return handler.EndArray();
						}
					case Node.Type.TABLE:
						{
							if (writed.ContainsKey(node))
								return false;
							writed.Add(node, true);
							Dictionary<string, Node> table = (Dictionary<string, Node>)node;
							if (!handler.StartTable())
								return false;
							foreach (KeyValuePair<string, Node> kv in table)
							{
								if (!handler.Key(kv.Key))
									return false;
								if (!Write(handler, kv.Value))
									return false;
							}
							return handler.EndTable();
						}
				}
				return false;
			}
		}
		#endregion

		#region 对外序列化接口
		public string String(Writer writer)
		{
			Reset();
			if (!writer.Write(buffer))
				return null;
			return buffer.ToString();
		}

		public string String(Node node)
		{
			return String(new NodeWriter(node));
		}

		public byte[] Bytes(Writer writer)
		{
			string str = String(writer);
			if (str == null)
				return null;
			return Encoding.UTF8.GetBytes(str);
		}

		public byte[] Bytes(Node node)
		{
			string str = String(node);
			if (str == null)
				return null;
			return Encoding.UTF8.GetBytes(str);
		}

		public string Format(string input)
		{
			Reset();
			Parser parser = new Parser();
			if (!parser.Load(input, buffer))
				return null;
			return buffer.ToString();
		}
		#endregion
	}
}