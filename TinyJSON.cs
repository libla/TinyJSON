using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace TinyJSON
{
	public class Node
	{
		public enum Kind
		{
			NONE,
			NULL,
			BOOLEAN,
			INT,
			UINT,
			DOUBLE,
			STRING,
			ARRAY,
			TABLE,
		}
		public virtual Kind GetKind()
		{
			return Kind.NONE;
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
		public virtual bool IsUInt()
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
		protected virtual bool AsBool()
		{
			throw new InvalidCastException();
		}
		protected virtual int AsInt()
		{
			throw new InvalidCastException();
		}
		protected virtual uint AsUInt()
		{
			throw new InvalidCastException();
		}
		protected virtual double AsNumber()
		{
			throw new InvalidCastException();
		}
		protected virtual string AsString()
		{
			throw new InvalidCastException();
		}
		protected virtual List<Node> AsArray()
		{
			throw new InvalidCastException();
		}
		protected virtual Dictionary<string, Node> AsTable()
		{
			throw new InvalidCastException();
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
				return node.AsBool();
			return true;
		}
		public static bool operator false(Node node)
		{
			if (!node.IsAny())
				return true;
			if (node.IsNull())
				return true;
			if (node.IsBool())
				return !node.AsBool();
			return false;
		}
		public static explicit operator bool(Node node)
		{
			return node.AsBool();
		}
		public static explicit operator int(Node node)
		{
			return node.AsInt();
		}
		public static explicit operator uint(Node node)
		{
			return node.AsUInt();
		}
		public static explicit operator double(Node node)
		{
			return node.AsNumber();
		}
		public static explicit operator string(Node node)
		{
			return node.AsString();
		}
		public static explicit operator List<Node>(Node node)
		{
			return node.AsArray();
		}
		public static explicit operator Dictionary<string, Node>(Node node)
		{
			return node.AsTable();
		}

		public static implicit operator Node(bool b)
		{
			return new BoolNode(b);
		}
		public static implicit operator Node(int i)
		{
			return new IntNode(i);
		}
		public static implicit operator Node(uint u)
		{
			return new UIntNode(u);
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

		public static Node NewUInt(uint u)
		{
			UIntNode node = new UIntNode(u);
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

		protected static EmptyNode empty = new EmptyNode();

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
			public override Kind GetKind()
			{
				return Kind.NULL;
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
			protected bool b;
			public BoolNode(bool b)
			{
				this.b = b;
			}
			public override Kind GetKind()
			{
				return Kind.BOOLEAN;
			}
			public override string ToString()
			{
				return b.ToString();
			}
			public override bool IsBool()
			{
				return true;
			}
			protected override bool AsBool()
			{
				return b;
			}
		}

		protected class IntNode : Node
		{
			protected int i;
			public IntNode(int i)
			{
				this.i = i;
			}
			public override Kind GetKind()
			{
				return Kind.INT;
			}
			public override string ToString()
			{
				return i.ToString();
			}
			public override bool IsInt()
			{
				return true;
			}
			public override bool IsUInt()
			{
				return i >= 0;
			}
			public override bool IsNumber()
			{
				return true;
			}
			protected override int AsInt()
			{
				return i;
			}
			protected override uint AsUInt()
			{
				if (i >= 0)
					return (uint)i;
				throw new InvalidCastException();
			}
			protected override double AsNumber()
			{
				return (double)i;
			}
		}

		protected class UIntNode : Node
		{
			protected uint u;
			public UIntNode(uint u)
			{
				this.u = u;
			}
			public override Kind GetKind()
			{
				return Kind.UINT;
			}
			public override string ToString()
			{
				return u.ToString();
			}
			public override bool IsInt()
			{
				return u <= (uint)int.MaxValue;
			}
			public override bool IsUInt()
			{
				return true;
			}
			public override bool IsNumber()
			{
				return true;
			}
			protected override int AsInt()
			{
				if (u <= (uint)int.MaxValue)
					return (int)u;
				throw new InvalidCastException();
			}
			protected override uint AsUInt()
			{
				return u;
			}
			protected override double AsNumber()
			{
				return (double)u;
			}
		}

		protected class NumberNode : Node
		{
			protected double d;
			public NumberNode(double d)
			{
				this.d = d;
			}
			public override Kind GetKind()
			{
				return Kind.DOUBLE;
			}
			public override string ToString()
			{
				return d.ToString();
			}
			public override bool IsNumber()
			{
				return true;
			}
			protected override double AsNumber()
			{
				return (double)d;
			}
		}

		protected class StringNode : Node
		{
			protected string s;
			public StringNode(string s)
			{
				this.s = s;
			}
			public override Kind GetKind()
			{
				return Kind.STRING;
			}
			public override string ToString()
			{
				return s.ToString();
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
			protected List<Node> list;
			public ArrayNode()
			{
				list = new List<Node>();
			}
			public ArrayNode(List<Node> lst)
			{
				list = lst;
			}
			public override Kind GetKind()
			{
				return Kind.ARRAY;
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
					list[index] = value;
				}
			}
		}

		protected class TableNode : Node
		{
			protected Dictionary<string, Node> table;
			public TableNode()
			{
				table = new Dictionary<string, Node>();
			}
			public TableNode(Dictionary<string, Node> tb)
			{
				table = tb;
			}
			public override Kind GetKind()
			{
				return Kind.TABLE;
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
						table[key] = value;
					}
				}
			}
		}
	}
	public struct Parser
	{
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

		private static byte[] utf8_lead_bits = {0x00, 0xc0, 0xe0};

		private static double[] e = { // 1e-0...1e308: 309 * 8 bytes = 2472 bytes
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

		private static double POW10(int x)
		{
			if (x < -308)
				return 0;
			if (x > 308)
				return double.PositiveInfinity;
			return (x >= 0) ? e[x] : 1.0 / e[-x];
		}

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

		private static Tokens[] ascii_token = {
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

			CLOSE,
			__
		}

		private static TokenStates[,] token_state_table = {
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

		private static TokenActions[,] token_action_table = {
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

		private static WordStates[,] word_state_table = {
			/*
				The token state table takes the current state and the current symbol, and sometimes do an action.
				A JSON text is accepted if at the end of the text the state is OK.
						      [              ]              {              }             int           float          null           true           false          string           :              ,
			*/
			/* OE */	{WordStates.__, WordStates.__, WordStates.__, WordStates.CLOSE, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.CO, WordStates.__, WordStates.__},
			/* CO */	{WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.OV, WordStates.__},
			/* OV */	{WordStates.ON, WordStates.__, WordStates.ON, WordStates.__, WordStates.ON, WordStates.ON, WordStates.ON, WordStates.ON, WordStates.ON, WordStates.ON, WordStates.__, WordStates.__},
			/* ON */	{WordStates.__, WordStates.__, WordStates.__, WordStates.CLOSE, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.KY},
			/* KY */	{WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.CO, WordStates.__, WordStates.__},

			/* AE */	{WordStates.AN, WordStates.CLOSE, WordStates.AN, WordStates.__, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.__, WordStates.__},
			/* AN */	{WordStates.__, WordStates.CLOSE, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.__, WordStates.AV},
			/* AV */	{WordStates.AN, WordStates.__, WordStates.AN, WordStates.__, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.AN, WordStates.__, WordStates.__},
		};

		private static void _ArrayProc(Node node, string key, Node value)
		{
			((List<Node>)node).Add(value);
		}

		private static void _TableProc(Node node, string key, Node value)
		{
			((Dictionary<string, Node>)node)[key] = value;
		}

		private static Proc ArrayProc = _ArrayProc;
		private static Proc TableProc = _TableProc;

		private delegate void Proc(Node node, string key, Node value);
		private struct NodeProc
		{
			public Node node;
			public Proc proc;
			public WordStates state;
		}

		private struct Context
		{
			public byte[] input;
			public int outset;
			public int cursor;
			public TokenStates token_state;
			public string error;
			public Node root;
			public List<NodeProc> nodeprocs;
			public byte[] buffer;
			public int bufferused;
			public string key;

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

		private Context context;

		private bool Value(Node value)
		{
			if (context.root == null)
			{
				context.root = value;
				return true;
			}
			else if (context.nodeprocs.Count > 0)
			{
				NodeProc nodeproc = context.nodeprocs[context.nodeprocs.Count - 1];
				nodeproc.proc(nodeproc.node, context.key, value);
				return true;
			}
			context.Error();
			return false;
		}

		private bool ParseWord(Words word)
		{
			WordStates state = WordStates.__;
			if (context.nodeprocs.Count != 0)
			{
				NodeProc nodeproc = context.nodeprocs[context.nodeprocs.Count - 1];
				state = word_state_table[(int)(nodeproc.state), (int)word];
				if (state == WordStates.__)
				{
					context.Error();
					return false;
				}
				else if (state == WordStates.CLOSE)
				{
					context.nodeprocs.RemoveAt(context.nodeprocs.Count - 1);
					return true;
				}
				else
				{
					nodeproc.state = state;
					context.nodeprocs[context.nodeprocs.Count - 1] = nodeproc;
				}
			}
			switch (word)
			{
			case Words.ARRAY_BEGIN:
				{
					Node newnode = Node.NewArray();
					if (!Value(newnode))
						return false;
					NodeProc nodeproc;
					nodeproc.node = newnode;
					nodeproc.proc = ArrayProc;
					nodeproc.state = WordStates.AE;
					context.nodeprocs.Add(nodeproc);
				}
				break;
			case Words.OBJECT_BEGIN:
				{
					Node newnode = Node.NewTable();
					if (!Value(newnode))
						return false;
					NodeProc nodeproc;
					nodeproc.node = newnode;
					nodeproc.proc = TableProc;
					nodeproc.state = WordStates.OE;
					context.nodeprocs.Add(nodeproc);
				}
				break;
			case Words.INTEGER:
				{
					uint n = 0;
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
								if (minus)
								{
									if (n > int.MaxValue / 10 || (n == int.MaxValue / 10 && c > int.MaxValue % 10))
									{
										use_double = true;
										d = n;
										d = d * 10 + c;
										continue;
									}
								}
								else
								{
									if (n > uint.MaxValue / 10 || (n == uint.MaxValue / 10 && c > uint.MaxValue % 10))
									{
										use_double = true;
										d = n;
										d = d * 10 + c;
										continue;
									}
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
						if (!Value(Node.NewNumber(minus ? -d : d)))
							return false;
					}
					else
					{
						if (minus)
						{
							if (!Value(Node.NewInt(-(int)n)))
								return false;
						}
						else
						{
							if (!Value(Node.NewUInt(n)))
								return false;
						}
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
						if (!Value((minus ? -d : d) * POW10((dot < 0 ? 0 : -dot) + exp)))
							return false;
					}
					else
					{
						if (!Value(Node.NewNumber((minus ? -n : n) * POW10((dot < 0 ? 0 : -dot) + exp))))
							return false;
					}
				}
				break;
			case Words.NULL:
				{
					if (!Value(Node.NewNull()))
						return false;
				}
				break;
			case Words.TRUE:
				{
					if (!Value(Node.NewBool(true)))
						return false;
				}
				break;
			case Words.FALSE:
				{
					if (!Value(Node.NewBool(false)))
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
									if (unicode < 0x80)
									{
										trail = 0;
									}
									else if (unicode < 0x800)
									{
										trail = 1;
									}
									else
									{
										trail = 2;
									}
									context.BufferAdd((byte)((unicode >> (trail * 6)) | utf8_lead_bits[trail]));
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
						context.key = str;
					}
					else
					{
						if (!Value(Node.NewString(str)))
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
			TokenStates state = context.token_state;
			context.token_state = token_state_table[(int)state, (int)token];
			TokenActions action = token_action_table[(int)state, (int)token];
			if (context.token_state == TokenStates.__)
			{
				context.token_state = state;
				context.Error();
				return false;
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
			int count = context.input.Length;
			while (context.cursor < count)
			{
				byte c = context.input[context.cursor++];
				if (!DoToken(c))
				{
					return false;
				}
			}
			if (!DoToken(0))
			{
				return false;
			}
			if (context.token_state != TokenStates.OK)
			{
				context.Error();
				return false;
			}
			return true;
		}

		public string Error()
		{
			return context.error;
		}

		public Node Load(string input)
		{
			return Load(Encoding.UTF8.GetBytes(input));
		}

		public Node Load(byte[] input)
		{
			context.input = input;
			context.outset = 0;
			context.cursor = 0;
			context.token_state = TokenStates.OK;
			context.error = null;
			context.root = null;
			context.key = null;
			if (context.nodeprocs == null)
			{
				context.nodeprocs = new List<NodeProc>(16);
			}
			else
			{
				context.nodeprocs.Clear();
			}
			if (context.buffer == null)
			{
				context.buffer = new byte[1024];
			}
			context.bufferused = 0;
			if (input.Length >= 3 && input[0] == 0xEF && input[1] == 0xBB && input[2] == 0xBF)
			{
				context.cursor += 3;
			}
			if (!ParseToken())
			{
				return null;
			}
			return context.root;
		}
	}

	public class Printer
	{
		private Writer writer;
		private Dictionary<Node, bool> writed;
		public Printer()
		{
			writer = new Writer(false);
			writed = new Dictionary<Node, bool>();
		}
		public Printer(bool pretty)
		{
			if (pretty)
			{
				writer = new PrettyWriter(false);
			}
			else
			{
				writer = new Writer(false);
			}
			writed = new Dictionary<Node, bool>();
		}
		public Printer(bool pretty, bool escape)
		{
			if (pretty)
			{
				writer = new PrettyWriter(escape);
			}
			else
			{
				writer = new Writer(escape);
			}
			writed = new Dictionary<Node, bool>();
		}
		private bool Write(Node node)
		{
			switch (node.GetKind())
			{
			case Node.Kind.NULL:
				writer.WriteNull();
				return true;
			case Node.Kind.BOOLEAN:
				writer.WriteBool((bool)node);
				return true;
			case Node.Kind.INT:
				writer.WriteInt((int)node);
				return true;
			case Node.Kind.UINT:
				writer.WriteUInt((uint)node);
				return true;
			case Node.Kind.DOUBLE:
				writer.WriteNumber((double)node);
				return true;
			case Node.Kind.STRING:
				writer.WriteString((string)node);
				return true;
			case Node.Kind.ARRAY:
				{
					if (writed.ContainsKey(node))
						return false;
					writed.Add(node, true);
					List<Node> list = (List<Node>)node;
					writer.WriteNewArray(list.Count);
					bool first = true;
					for (int i = 0, j = list.Count; i < j; ++i)
					{
						if (first)
						{
							first = false;
						}
						else
						{
							writer.WriteComma();
						}
						writer.WriteIndent();
						if (!Write(list[i]))
							return false;
					}
					writer.WriteCloseArray(list.Count);
					return true;
				}
			case Node.Kind.TABLE:
				{
					if (writed.ContainsKey(node))
						return false;
					writed.Add(node, true);
					Dictionary<string, Node> table = (Dictionary<string, Node>)node;
					writer.WriteNewTable(table.Count);
					bool first = true;
					foreach (KeyValuePair<string, Node> kv in table)
					{
						if (first)
						{
							first = false;
						}
						else
						{
							writer.WriteComma();
						}
						writer.WriteIndent();
						writer.WriteTableKey(kv.Key);
						if (!Write(kv.Value))
							return false;
					}
					writer.WriteCloseTable(table.Count);
					return true;
				}
			}
			return false;
		}
		public string String(Node node)
		{
			writer.Reset();
			bool result = Write(node);
			writed.Clear();
			if (!result)
				return null;
			return writer.ToString();
		}
		public byte[] Bytes(Node node)
		{
			string str = String(node);
			if (str == null)
				return null;
			return Encoding.UTF8.GetBytes(str);
		}

		private static char[] escapes = {
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

		private class Writer
		{
			protected bool escape;
			protected StringBuilder stream;
			protected StringBuilder buffer;
			public Writer(bool esc)
			{
				escape = esc;
				stream = new StringBuilder();
				buffer = new StringBuilder();
			}
			public override string ToString()
			{
				return stream.ToString();
			}
			public virtual void Reset()
			{
				stream.Length = 0;
			}
			public virtual void WriteNull()
			{
				stream.Append("null");
			}
			public virtual void WriteBool(bool b)
			{
				stream.Append(b ? "true" : "false");
			}
			public virtual void WriteInt(int i)
			{
				stream.Append(i.ToString());
			}
			public virtual void WriteUInt(uint u)
			{
				stream.Append(u.ToString());
			}
			public virtual void WriteNumber(double d)
			{
				stream.Append(d.ToString());
			}
			public virtual void WriteString(string s)
			{
				buffer.Length = 0;
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
							buffer.Append(string.Format("{0,2:X2}", (uint)c));
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
						buffer.Append(string.Format("{0,4:X4}", (uint)c));
					}
					else
					{
						buffer.Append(c);
					}
				}
				buffer.Append('"');
				stream.Append(buffer.ToString());
			}
			public virtual void WriteNewArray(int count)
			{
				stream.Append('[');
			}
			public virtual void WriteCloseArray(int count)
			{
				stream.Append(']');
			}
			public virtual void WriteNewTable(int count)
			{
				stream.Append('{');
			}
			public virtual void WriteCloseTable(int count)
			{
				stream.Append('}');
			}
			public virtual void WriteTableKey(string s)
			{
				WriteString(s);
				stream.Append(':');
			}
			public virtual void WriteIndent()
			{
			}
			public virtual void WriteComma()
			{
				stream.Append(',');
			}
		}

		private class PrettyWriter : Writer
		{
			protected int depth;
			public PrettyWriter(bool escape)
				: base(escape)
			{
				depth = 0;
			}
			public override void Reset()
			{
				depth = 0;
				base.Reset();
			}
			public override void WriteNewArray(int count)
			{
				++depth;
				base.WriteNewArray(count);
			}
			public override void WriteCloseArray(int count)
			{
				--depth;
				if (count != 0)
					WriteIndent();
				base.WriteCloseArray(count);
			}
			public override void WriteNewTable(int count)
			{
				++depth;
				base.WriteNewTable(count);
			}
			public override void WriteCloseTable(int count)
			{
				--depth;
				if (count != 0)
					WriteIndent();
				base.WriteCloseTable(count);
			}
			public override void WriteTableKey(string s)
			{
				base.WriteTableKey(s);
				stream.Append(' ');
			}
			public override void WriteIndent()
			{
				stream.Append('\n');
				for (int i = 0; i < depth; ++i)
				{
					stream.Append('\t');
				}
			}
		}
	}
}