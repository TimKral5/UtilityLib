using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLib.Console
{
	public static class CT
	{
		public static void Configure(string title="", ConsoleColor fColor=ConsoleColor.White, ConsoleColor bgColor=ConsoleColor.Black)
		{
			System.Console.Title = title;
			System.Console.ForegroundColor = fColor;
			System.Console.BackgroundColor = bgColor;
		}
	}
}
