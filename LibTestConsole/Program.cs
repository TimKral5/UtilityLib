using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;

using UtilityLib.Converter.CSF;
namespace LibTestConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			settings.Add("setting1", "value1");
			settings.Add("setting2", "value2");
			string result = CsfConverter.ConvertToString(settings);
			Console.WriteLine(result);

			Dictionary<string, string> resultDict = CsfConverter.ConvertToDictionary(result);

			foreach (KeyValuePair<string, string> item in resultDict)
			{
				Console.WriteLine("Setting: ["+item.Key+"], Value: ["+item.Value+"]");
			}
		}
	}
}
