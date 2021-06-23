using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace UtilityLib.ProgramEngine
{
	#region Attributes
	/// <summary>
	/// Declares that the class with this attribute is runnable and contains methods with one of these attributes: 
	/// <seealso cref="StartAttribute"/>, <seealso cref="AwakeAttribute"/>, 
	/// <seealso cref="UpdateAttribute"/>, <seealso cref="FixedUpdateAttribute"/>, 
	/// <seealso cref="LateUpdateAttribute"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class RunnableAttribute : Attribute
	{}

	/// <summary>
	/// Declares, that the method with this attribute is executeable using the ProgEngine. For fuhrter information see: <see cref="ProgEngine.Run()"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class StartAttribute : Attribute 
	{}

	/// <summary>
	/// Declares, that the method with this attribute is executeable using the ProgEngine. For fuhrter information see: <see cref="ProgEngine.Run()"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class AwakeAttribute : Attribute
	{}

	/// <summary>
	/// Declares, that the method with this attribute is executeable using the ProgEngine. For fuhrter information see: <see cref="ProgEngine.Run()"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class UpdateAttribute : Attribute
	{}

	/// <summary>
	/// Declares, that the method with this attribute is executeable using the ProgEngine. For fuhrter information see: <see cref="ProgEngine.Run()"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class FixedUpdateAttribute : Attribute
	{}

	/// <summary>
	/// Declares, that the method with this attribute is executeable using the ProgEngine. For fuhrter information see: <see cref="ProgEngine.Run()"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class LateUpdateAttribute : Attribute
	{}
	#endregion

	[Runnable]
	public abstract class BasicProgram
	{
		[Awake]
		public abstract void Awake();
		[Start]
		public abstract void Start();
		[Update]
		public abstract void Update();
		[LateUpdate]
		public abstract void LateUpdate();
		[FixedUpdate]
		public abstract void FixedUpdate();
	}

	public partial class BasicBehaviour
	{
		//public Func func;
	}

	/// <summary>
	/// Engine for running class with Runnable-Attribute.
	/// </summary>
	public class ProgEngine
	{
		public static class ThreadTaskManager
		{
			private static ThreadStart updateFunc;
			private static ThreadStart fixedUpdateFunc;

			private static Thread updateThread;
			private static Thread fixedUpdateThread;

			public static void SetThreadMethods(ThreadStart _updateMethod, ThreadStart _fixedUpdateMethod)
			{
				updateFunc = _updateMethod;
				fixedUpdateFunc = _fixedUpdateMethod;
			}

			private static bool _running;
			public static bool Running { get { return _running; } }
			
			public static void GenerateThreads()
			{
				updateThread = new Thread(updateFunc);
				fixedUpdateThread = new Thread(fixedUpdateFunc);
			}
			public static void StartThreads()
			{
				_running = true;
				updateThread.Start();
				fixedUpdateThread.Start();
			}
			public static void StopThreads()
			{
				_running = false;
			}
		}

		public virtual void updateT()
		{
			while (ThreadTaskManager.Running)
			{
				Thread.Sleep(UpdateDelay);
				if (UpdateMethod != null) UpdateMethod.Invoke(runnableClass, null);
				if (LateUpdateMethod != null) LateUpdateMethod.Invoke(runnableClass, null);
			}
		}

		public static int fixedUpdatesPerSecond;
		public static int UpdateDelay;

		public virtual void fixedUpdateT()
		{
			while (ThreadTaskManager.Running)
			{
				Thread.Sleep(1000 / fixedUpdatesPerSecond);
				if (FixedUpdateMethod != null) FixedUpdateMethod.Invoke(runnableClass, null);
			}
		}

		public virtual void CompleteStop()
		{
			ThreadTaskManager.StopThreads();
		}

		/// <summary>
		/// runs the program with the entered properties or else runs it with the predefined properties
		/// </summary>
		/// <param name="fixedUpdatesPerSecond">that many fixedUpdates per seconds are executed</param>
		/// <param name="UpdateDelay"></param>
		/// <returns></returns>
		public virtual void CompleteStart(int fixedUpdatesPerSecond=60, int UpdateDelay = 5)
		{

			TypeInfo typeInfo = runnableClass.GetType().GetTypeInfo();
			var attrs = typeInfo.GetCustomAttributes();
			bool testResult = false;
			foreach (var attr in attrs)
			{
				if (attr.GetType() == typeof(RunnableAttribute))
				{
					testResult = true;
					break;
				}
			}
			if (!testResult) return;

			MethodInfo[] methods = runnableClass.GetType().GetMethods();

			MethodInfo finalStartMethod = null;
			MethodInfo finalAwakeMethod = null;
			MethodInfo finalFixedUpdateMethod = null;
			MethodInfo finalUpdateMethod = null;
			MethodInfo finalLateUpdateMethod = null;

			foreach (MethodInfo method in methods)
			{
				foreach (var attr in method.GetCustomAttributes())
				{
					if (attr.GetType() == typeof(StartAttribute))
					{
						finalStartMethod = method;
					}
					else if (attr.GetType() == typeof(AwakeAttribute))
					{
						finalAwakeMethod = method;
					}
					else if (attr.GetType() == typeof(UpdateAttribute))
					{
						finalUpdateMethod = method;
					}
					else if (attr.GetType() == typeof(FixedUpdateAttribute))
					{
						finalFixedUpdateMethod = method;
					}
					else if (attr.GetType() == typeof(LateUpdateAttribute))
					{
						finalLateUpdateMethod = method;
					}
				}
			}

			AwakeMethod = finalAwakeMethod;
			StartMethod = finalStartMethod;
			UpdateMethod = finalUpdateMethod;
			FixedUpdateMethod = finalFixedUpdateMethod;
			LateUpdateMethod = finalLateUpdateMethod;

			if (AwakeMethod != null) AwakeMethod.Invoke(runnableClass, null);
			if (StartMethod != null) StartMethod.Invoke(runnableClass, null);

			ProgEngine.fixedUpdatesPerSecond = fixedUpdatesPerSecond;
			ProgEngine.UpdateDelay = UpdateDelay;

			ThreadTaskManager.SetThreadMethods(updateT, fixedUpdateT);
			ThreadTaskManager.GenerateThreads();
			ThreadTaskManager.StartThreads();
		}

		public virtual void ExecAwake()
		{
			if (AwakeMethod != null) AwakeMethod.Invoke(runnableClass, null);
		}
		public virtual void ExecStart()
		{
			if (StartMethod != null) StartMethod.Invoke(runnableClass, null);
		}
		public virtual void ExecUpdate()
		{
			if (UpdateMethod != null) UpdateMethod.Invoke(runnableClass, null);
		}
		public virtual void ExecFixedUpdate()
		{
			if (FixedUpdateMethod != null) FixedUpdateMethod.Invoke(runnableClass, null);
		}
		public void ExecLateUpdate()
		{
			if (LateUpdateMethod != null) LateUpdateMethod.Invoke(runnableClass, null);
		}

		public enum Method {
			START = 1,
			AWAKE = 2,
			UPDATE = 3,
			FIXED_UPDATE = 4,
			LATE_UPDATE = 5,
		}

		public object runnableClass;

		private MethodInfo StartMethod;
		private MethodInfo AwakeMethod;
		private MethodInfo UpdateMethod;
		private MethodInfo FixedUpdateMethod;
		private MethodInfo LateUpdateMethod;

		public virtual bool this[Method method] { 
			get 
			{
				switch (method)
				{
					case Method.AWAKE:
						if (AwakeMethod != null) AwakeMethod.Invoke(runnableClass, null); return true;
					case Method.START:
						if (StartMethod != null) StartMethod.Invoke(runnableClass, null); return true;
					case Method.UPDATE:
						if (UpdateMethod != null) UpdateMethod.Invoke(runnableClass, null); return true;
					case Method.FIXED_UPDATE:
						if (FixedUpdateMethod != null) FixedUpdateMethod.Invoke(runnableClass, null); return true;
					case Method.LATE_UPDATE:
						if (LateUpdateMethod != null) LateUpdateMethod.Invoke(runnableClass, null); return true;
					default: return false;
				}
			} 
		}

		public ProgEngine() {}

		public ProgEngine(object _runnableClass)
		{
			runnableClass = _runnableClass;
		}

		/// <summary>
		/// Scans the runnableClass for the attributes 
		/// <seealso cref="AwakeAttribute"/>, <seealso cref="StartAttribute"/>, 
		/// <seealso cref="UpdateAttribute"/>, <seealso cref="FixedUpdateAttribute"/>
		/// and <seealso cref="LateUpdateAttribute"/> and executes the methods with the following attributes in following order:
		/// <seealso cref="AwakeAttribute"/>, <seealso cref="StartAttribute"/>.
		/// </summary>
		/// <returns>Bool that marks, if any error happened.</returns>
		public virtual void Initialize(int fixedUpdatesPerSecond = 60, int generalDeltaTime = 5)
		{
			TypeInfo typeInfo = runnableClass.GetType().GetTypeInfo();
			var attrs = typeInfo.GetCustomAttributes();
			bool testResult = false;
			foreach (var attr in attrs)
			{
				if (attr.GetType() == typeof(RunnableAttribute))
				{
					testResult = true;
					break;
				}
			}
			if (!testResult) return;

			MethodInfo[] methods = runnableClass.GetType().GetMethods();

			MethodInfo finalStartMethod = null;
			MethodInfo finalAwakeMethod = null;
			MethodInfo finalFixedUpdateMethod = null;
			MethodInfo finalUpdateMethod = null;
			MethodInfo finalLateUpdateMethod = null;

			foreach (MethodInfo method in methods)
			{
				foreach (var attr in method.GetCustomAttributes())
				{
					if (attr.GetType() == typeof(StartAttribute))
					{
						finalStartMethod = method;
					}
					else if (attr.GetType() == typeof(AwakeAttribute))
					{
						finalAwakeMethod = method;
					}
					else if (attr.GetType() == typeof(UpdateAttribute))
					{
						finalUpdateMethod = method;
					}
					else if (attr.GetType() == typeof(FixedUpdateAttribute))
					{
						finalFixedUpdateMethod = method;
					}
					else if (attr.GetType() == typeof(LateUpdateAttribute))
					{
						finalLateUpdateMethod = method;
					}
				}
			}

			AwakeMethod = finalAwakeMethod;
			StartMethod = finalStartMethod;
			UpdateMethod = finalUpdateMethod;
			FixedUpdateMethod = finalFixedUpdateMethod;
			LateUpdateMethod = finalLateUpdateMethod;

			ProgEngine.fixedUpdatesPerSecond = fixedUpdatesPerSecond;
			ProgEngine.UpdateDelay = generalDeltaTime;

			Run();
		}

		public virtual void Run()
		{
			if (AwakeMethod != null) AwakeMethod.Invoke(runnableClass, null);
			if (StartMethod != null) StartMethod.Invoke(runnableClass, null);
		}

		/// <summary>
		/// Useable after running <seealso cref="ProgEngine.Run()"/>. Executes methods with following attributes in following order: 
		/// <seealso cref="UpdateAttribute"/>, <seealso cref="LateUpdateAttribute"/>.
		/// </summary>
		/// <returns>Bool that marks, if any error happened.</returns>
		public virtual void Update()
		{
			if (UpdateMethod != null) UpdateMethod.Invoke(runnableClass, null);
			if (LateUpdateMethod != null) LateUpdateMethod.Invoke(runnableClass, null);
		}

		/// <summary>
		/// Useable after running <seealso cref="ProgEngine.Run()"/>. Executes method with the attribute: <seealso cref="FixedUpdateAttribute"/>.
		/// </summary>
		/// <returns>Bool that marks, if any error happened.</returns>
		public virtual void FixedUpdate()
		{
			if (FixedUpdateMethod != null) FixedUpdateMethod.Invoke(runnableClass, null);
		}
	}
}
