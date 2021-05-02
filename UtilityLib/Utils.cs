using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityLib.ProgramEngine;

namespace UtilityLib.Extension
{
	public static class Utils
	{
		public enum UL_ProgEngineRunModes {
			DEFAULT = 1,
			FAST = 2
		}

		#region ProgEngine
		private static ProgEngine _progEngine;

		public static void UL_GenerateProgEngine(object obj)
		{
			_progEngine = new ProgEngine(obj);
		}

		public static void UL_ProgEngine_Run(int fixedUpdatesPerSec = 60, int normalUpdateDelay = 5)
		{
			_progEngine.CompleteStart(fixedUpdatesPerSec, normalUpdateDelay);
		}

		public static void UL_ProgEngine_Run(UL_ProgEngineRunModes mode)
		{
			if (mode == UL_ProgEngineRunModes.DEFAULT)
			{
				_progEngine.CompleteStart(60, 5);
			}
		}

		public static void UL_ProgEngine_Kill()
		{
			_progEngine.CompleteStop();
		}
		
		#endregion

		#region GetCentered(inputString, seperator1, seperator2);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="inputString">String to operate with</param>
		/// <param name="seperator1">1st Seperator</param>
		/// <param name="seperator2">2nd Seperator</param>
		/// <returns><paramref name="string"/> between <paramref name="seperator1"/> and <paramref name="seperator2"/></returns>
		public static string UL_GetCentered(string inputString, string seperator1, string seperator2) { return _GetCentered(inputString, seperator1, seperator2); }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="inputString">String to operate with</param>
		/// <param name="seperator1">1st Seperator</param>
		/// <param name="seperator2">2nd Seperator</param>
		/// <returns><paramref name="string"/> between <paramref name="seperator1"/> and <paramref name="seperator2"/></returns>
		public static string UL_GetCentered(string inputString, char seperator1, char seperator2) { return _GetCentered(inputString, seperator1.ToString(), seperator2.ToString()); }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="inputString">String to operate with</param>
		/// <param name="seperator1">1st Seperator</param>
		/// <param name="seperator2">2nd Seperator</param>
		/// <returns><paramref name="string"/> between <paramref name="seperator1"/> and <paramref name="seperator2"/></returns>
		public static string UL_GetCentered(string inputString, string seperator1, char seperator2) { return _GetCentered(inputString, seperator1, seperator2.ToString()); }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="inputString">String to operate with</param>
		/// <param name="seperator1">1st Seperator</param>
		/// <param name="seperator2">2nd Seperator</param>
		/// <returns><paramref name="string"/> between <paramref name="seperator1"/> and <paramref name="seperator2"/></returns>
		public static string UL_GetCentered(string inputString, char seperator1, string seperator2) { return _GetCentered(inputString, seperator1.ToString(), seperator2); }

		private static string _GetCentered(string inputString, string seperator1, string seperator2)
		{
			return inputString.Split(seperator1)[1].Split(seperator2)[0];
		}
		#endregion
	}
}
