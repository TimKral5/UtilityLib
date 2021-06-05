using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLib.Superior
{
	public static class SuperiorClass
	{
		static Dictionary<string, object> objects = new Dictionary<string, object>();
		static Dictionary<string, Func<object, object>> methods = new Dictionary<string, System.Func<object, object>>();

		//ST_OBJECT Get
		public static T ST_OBJECT_GET<T>(string name)
		{
			objects.TryGetValue(name, out object result);
			return (T)result;
		}
		//ST_OBJECT Set
		/// <summary>
		/// stores the the entered value (<paramref name="value"/>) under the entered index (<paramref name="name"/>).
		/// </summary>
		/// <param name="name">index under wich the entered value (<paramref name="value"/>) is saved</param>
		/// <param name="value">value used for processing</param>
		/// <returns>bool that marks is the process was successful</returns>
		public static void ST_OBJECT_SET(string name, object value)
		{
			objects.Add(name, value);
		}

		//ST_METHOD Get
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name">the index of the method</param>
		/// <returns>the method that is saved under the entered index (<paramref name="name"/>).</returns>
		public static Func<object, object> ST_METHOD_GET(string name)
		{
			methods.TryGetValue(name, out Func<object, object> result);
			return result;
		}
		//ST_METHOD Set
		public static void ST_METHOD_SET(string name, Func<object, object> method)
		{
			methods.Add(name, method);
		}
		//ST_METHOD Execute
		public static bool ST_METHOD_EXECUTE(string name, object input, out object output)
		{
			bool result = methods.TryGetValue(name, out Func<object, object> method);
			if (!result) throw new ArgumentException();
			output = method(input);
			return true;
		}
		public static bool ST_METHOD_EXECUTE<outT>(string name, object input, out outT output)
		{
			bool result = methods.TryGetValue(name, out Func<object, object> method);
			if (!result) throw new ArgumentException();
			output = (outT)method(input);
			return true;
		}
	}
}
