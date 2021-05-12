using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using System.Numerics;

using UtilityLib.Converter.CSF;
using static UtilityLib.Superior.SuperiorClass;
namespace LibTestConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			object TEST(object zero)
			{
				return 1;
			}
			ST_METHOD_SET("x", TEST);
			ST_METHOD_EXECUTE("x", null, out object result);
			int res = (int)result;
			Console.WriteLine(res);
		}
	}
}
