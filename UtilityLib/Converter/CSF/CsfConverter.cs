using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLib.Converter.CSF
{
	public static class CsfConverter
	{
		public static string ConvertToString(Dictionary<string, string> data)
		{
			string result = "";
			foreach (KeyValuePair<string, string> item in data)
			{
				result += '"' + item.Key + "\" = \"" + item.Value + "\"\n";
			}
			return result;
		}

		public static Dictionary<string, string> ConvertToDictionary(string converted)
		{
			Dictionary<string, string> result = new Dictionary<string, string>();
			string[] settings = converted.Split('\n');

			foreach (string item in settings)
			{
				if (item != string.Empty)
				{
					if (!item.StartsWith("#"))
					{
						string[] parameters = item.Split('=');
						string propName = parameters[0].Split('"')[1];
						string propData = parameters[1].Split('"')[1];

						result.Add(propName, propData);
					}
				}
			}

			return result;
		}
	}
}
