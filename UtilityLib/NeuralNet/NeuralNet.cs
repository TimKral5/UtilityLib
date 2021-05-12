using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLib.NeuralNet
{

	public class Perceptor
	{
		

		public enum Mode
		{
			SIGMOID = 1
		}

	}

	public class NeuralNet
	{
		public int InputPerceptorCount { get; set; }
		public int OutPerceptorCount { get; set; }
		public Perceptor[][] NormalPerceptors { get; set; }

		public static void SetValues(int inputPerceptorCount, int outputPerceptorCount, Perceptor[,] normalPerceptors)
		{

		}
	}
}
