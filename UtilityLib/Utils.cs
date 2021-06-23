using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityLib.ProgramEngine;

namespace UtilityLib.Static
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

		
	}
}
