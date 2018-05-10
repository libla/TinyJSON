using System;
using System.Reflection;
using System.Collections.Generic;

namespace TinyJSON
{
	public sealed partial class JSON
	{
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
		public class EntityAttribute : Attribute
		{
			public string Name;
			public bool Required;

			public EntityAttribute()
			{
				Name = null;
				Required = false;
			}
		}
	}

	public static partial class JSONExt
	{
		public static bool Help<T>(this JSON.Object o, ref T value) where T : struct, JSON.Readable
		{
			return ReadHelper<T>.Read(o, ref value);
		}

		public static bool Help<T>(this JSON.Object o, T value) where T : class, JSON.Readable
		{
			return ReadHelper<T>.Read(o, ref value);
		}

		#region 自动映射Object和指定数据类型
		private static bool Get(JSON.Object o, string key, ref bool b)
		{
			return o.Get(key, ref b);
		}

		private static bool Get(JSON.Object o, string key, ref int i)
		{
			return o.Get(key, ref i);
		}

		private static bool Get(JSON.Object o, string key, ref double d)
		{
			return o.Get(key, ref d);
		}

		private static bool Get(JSON.Object o, string key, ref string s)
		{
			return o.Get(key, ref s);
		}

		private static bool Get(JSON.Object o, string key, ref JSON.Object oo)
		{
			return o.Get(key, ref oo);
		}

		private static bool Get(JSON.Object o, string key, ref JSON.Array a)
		{
			return o.Get(key, ref a);
		}

		private static bool GetEnum(JSON.Object o, Type type, string key, ref object value)
		{
			string result = default(string);
			if (!o.Get(key, ref result))
				return false;
			try
			{
				value = Enum.Parse(type, result);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		private static bool GetEnumNullable(JSON.Object o, Type type, string key, ref object value)
		{
			string result = default(string);
			if (!o.Get(key, ref result))
				return false;
			if (string.IsNullOrEmpty(result))
			{
				value = Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(type));
				return true;
			}
			object tmp;
			try
			{
				tmp = Enum.Parse(type, result);
			}
			catch (Exception)
			{
				return false;
			}
			value = Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(type), tmp);
			return true;
		}

		private static bool GetEnumArray(JSON.Object o, Type type, string key, ref object value)
		{
			JSON.Array result = default(JSON.Array);
			if (!o.Get(key, ref result))
				return false;
			int count = result.Count;
			Array array = Array.CreateInstance(type, count);
			for (int i = 0; i < count; ++i)
			{
				string tmp1 = default(string);
				if (!o.Get(key, ref tmp1))
					return false;
				object tmp2;
				try
				{
					tmp2 = Enum.Parse(type, tmp1);
				}
				catch (Exception)
				{
					return false;
				}
				array.SetValue(tmp2, i);
			}
			value = array;
			return true;
		}

		private static bool GetType(JSON.Object o, Type type, string key, ref object value)
		{
			JSON.Object result = default(JSON.Object);
			if (!o.Get(key, ref result))
				return false;
			JSON.Readable tmp;
			try
			{
				tmp = (JSON.Readable)Activator.CreateInstance(type);
			}
			catch (Exception)
			{
				return false;
			}
			if (!tmp.Read(result))
				return false;
			value = tmp;
			return true;
		}

		private static bool GetTypeNullable(JSON.Object o, Type type, string key, ref object value)
		{
			JSON.Object result = default(JSON.Object);
			if (!o.Get(key, ref result))
				return false;
			if (result == null)
			{
				value = Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(type));
				return true;
			}
			JSON.Readable tmp;
			try
			{
				tmp = (JSON.Readable)Activator.CreateInstance(type);
			}
			catch (Exception)
			{
				return false;
			}
			if (!tmp.Read(result))
				return false;
			value = Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(type), tmp);
			return true;
		}

		private static bool GetTypeArray(JSON.Object o, Type type, string key, ref object value)
		{
			JSON.Array result = default(JSON.Array);
			if (!o.Get(key, ref result))
				return false;
			int count = result.Count;
			Array array = Array.CreateInstance(type, count);
			for (int i = 0; i < count; ++i)
			{
				JSON.Object tmp1 = default(JSON.Object);
				if (!o.Get(key, ref tmp1))
					return false;
				JSON.Readable tmp2;
				try
				{
					tmp2 = (JSON.Readable)Activator.CreateInstance(type);
				}
				catch (Exception)
				{
					return false;
				}
				if (!tmp2.Read(tmp1))
					return false;
				array.SetValue(tmp2, i);
			}
			value = array;
			return true;
		}

		private enum Modify
		{
			None,
			Array,
			Option,
		}

		private static MethodInfo FindReadValue(Type type, out bool needtype)
		{
			MethodInfo method = typeof(JSONExt).GetMethod("Get",
														BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null,
														new[] {typeof(JSON.Object), typeof(string), type.MakeByRefType()}, null);
			if (method != null)
			{
				needtype = false;
				return method;
			}
			needtype = true;
			Modify modify = Modify.None;
			if (type.IsArray)
			{
				if (type.GetArrayRank() > 1)
					return null;
				modify = Modify.Array;
				type = type.GetElementType();
			}
			else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				modify = Modify.Option;
				type = type.GetGenericArguments()[0];
			}
			if (type.IsEnum)
			{
				return typeof(JSONExt).GetMethod(
					modify == Modify.Array ? "GetEnumArray" : (modify == Modify.Option ? "GetEnumNullable" : "GetEnum"),
					BindingFlags.Static | BindingFlags.NonPublic, null,
					new[] {typeof(JSON.Object), typeof(Type), typeof(string), typeof(object).MakeByRefType()}, null);
			}
			if (typeof(JSON.Readable).IsAssignableFrom(type) && type.GetConstructor(Type.EmptyTypes) != null)
			{
				return typeof(JSONExt).GetMethod(
					modify == Modify.Array ? "GetTypeArray" : (modify == Modify.Option ? "GetTypeNullable" : "GetType"),
					BindingFlags.Static | BindingFlags.NonPublic, null,
					new[] {typeof(JSON.Object), typeof(Type), typeof(string), typeof(object).MakeByRefType()}, null);
			}
			return null;
		}

		private static string GetName(MemberInfo member)
		{
			var attrs = member.GetCustomAttributes(typeof(JSON.EntityAttribute), true);
			if (attrs.Length != 0)
			{
				string s = ((JSON.EntityAttribute)attrs[0]).Name;
				if (!string.IsNullOrEmpty(s))
					return s;
			}
			return member.Name;
		}

		private static bool IsRequired(MemberInfo member)
		{
			var attrs = member.GetCustomAttributes(typeof(JSON.EntityAttribute), true);
			return attrs.Length != 0 && ((JSON.EntityAttribute)attrs[0]).Required;
		}

		private static class ReadHelper<T>
		{
			private delegate bool Reader(JSON.Object o, ref T value);
			private static readonly List<Reader> readers;

			static ReadHelper()
			{
				readers = new List<Reader>();
				foreach (MemberInfo member in typeof(T).GetMembers(BindingFlags.Public | BindingFlags.Instance))
				{
					Type type;
					if (member.MemberType == MemberTypes.Field)
					{
						type = ((FieldInfo)member).FieldType;
					}
					else
					{
						if (member.MemberType != MemberTypes.Property)
							continue;
						PropertyInfo property = (PropertyInfo)member;
						if (property.GetSetMethod() == null || property.GetGetMethod() == null)
							continue;
						type = property.PropertyType;
					}
					bool needtype;
					MethodInfo method = FindReadValue(type, out needtype);
					if (method == null)
					{
						string name = member.Name;
						readers.Add((JSON.Object o, ref T value) => { throw new MissingFieldException(typeof(T).FullName, name); });
					}
					else
					{
						string name = GetName(member);
						bool require = IsRequired(member);
						if (needtype)
						{
							if (member.MemberType == MemberTypes.Field)
							{
								FieldInfo field = (FieldInfo)member;
								readers.Add((JSON.Object o, ref T value) =>
								{
									object[] args = new object[4];
									args[0] = o;
									args[1] = type;
									args[2] = name;
									args[3] = field.GetValue(value);
									bool result = (bool)method.Invoke(null, args);
									if (!result)
										return !require;
									field.SetValue(value, args[3]);
									return true;
								});
							}
							else
							{
								PropertyInfo property = (PropertyInfo)member;
								MethodInfo setter = property.GetSetMethod();
								MethodInfo getter = property.GetGetMethod();
								readers.Add((JSON.Object o, ref T value) =>
								{
									object[] args = new object[4];
									args[0] = o;
									args[1] = type;
									args[2] = name;
									args[3] = getter.Invoke(value, null);
									bool result = (bool)method.Invoke(null, args);
									if (!result)
										return !require;
									setter.Invoke(value, new[] {args[3]});
									return true;
								});
							}
						}
						else
						{
							if (member.MemberType == MemberTypes.Field)
							{
								FieldInfo field = (FieldInfo)member;
								readers.Add((JSON.Object o, ref T value) =>
								{
									object[] args = new object[3];
									args[0] = o;
									args[1] = name;
									args[2] = field.GetValue(value);
									bool result = (bool)method.Invoke(null, args);
									if (!result)
										return !require;
									field.SetValue(value, args[2]);
									return true;
								});
							}
							else
							{
								PropertyInfo property = (PropertyInfo)member;
								MethodInfo setter = property.GetSetMethod();
								MethodInfo getter = property.GetGetMethod();
								readers.Add((JSON.Object o, ref T value) =>
								{
									object[] args = new object[4];
									args[0] = o;
									args[1] = name;
									args[2] = getter.Invoke(value, null);
									bool result = (bool)method.Invoke(null, args);
									if (!result)
										return !require;
									setter.Invoke(value, new[] {args[2]});
									return true;
								});
							}
						}
					}
				}
			}

			public static bool Read(JSON.Object o, ref T value)
			{
				for (int i = 0, j = readers.Count; i < j; ++i)
				{
					if (!readers[i](o, ref value))
						return false;
				}
				return true;
			}
		}
		#endregion
	}
}