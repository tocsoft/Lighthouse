using Noesis.Javascript;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	public class JavascriptRunner
	{
		internal bool _stop = false;
		BackgroundWorker _worker = new BackgroundWorker();
		private IDebugger _debugger;
		private string _code;

		public JavascriptRunner(Arduino arduino, IDebugger debugger) {
			_debugger = debugger;
			_worker.DoWork += _worker_DoWork;
			_worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
			_arduino = arduino;
		}

		void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (Stopped != null)
				Stopped.Invoke(this, new EventArgs());
		}
		JavascriptContext _context = null;
		void _worker_DoWork(object sender, DoWorkEventArgs e)
		{
			var system = new SystemUtilities(this);
			var debugger = new Debugger(_debugger, this);

			while (!_stop)
			{
				using (_context = new JavascriptContext())
				{
					// Setting the externals parameters of the context
					_context.SetParameter("device", _arduino);
					_context.SetParameter("system", system);
					_context.SetParameter("__debugger__", debugger);

					try
					{
						_context.Run(_code);
					}
					catch (StopExecutionExecption) { 
						//we can safly ignore this caue we triggered it :)
					}

				}
			}
			_context = null;
		}

		public bool IsRunning { get { return _worker.IsBusy; } }

		public event EventHandler Stopped;
		private Arduino _arduino;

		public void Run(string code)
		{
			if (!_worker.IsBusy)
			{
				_stop = false;
				_code = code;
				_worker.RunWorkerAsync();
			}
		}


		public void Stop()
		{
			_stop = true;
		}

		 internal class SystemUtilities
		{
			 JavascriptRunner _runner;
			 public SystemUtilities( JavascriptRunner runner)
			 {
				_runner = runner;
			}


			public void Sleep(int milliseconds) {
				var target = DateTime.Now.AddMilliseconds(milliseconds);
				while (DateTime.Now < target)
				{
					if (_runner._stop)
						throw new StopExecutionExecption();
				}
			}
		}
		internal class Debugger : IDebugger
		{
			 JavascriptRunner _runner;
			 IDebugger _externalDebug;
			public Debugger( IDebugger externalDebug, JavascriptRunner runner){
				_externalDebug = externalDebug;
				_runner = runner;
			}

			public void Enter(int handle)
			{
				if (_runner._stop)
					throw new StopExecutionExecption();
				
				_externalDebug.Enter(handle);

				if (_runner._stop)
					throw new StopExecutionExecption();
			}

			public void Exit(int handle)
			{
				if (_runner._stop)
					throw new StopExecutionExecption();
				
				_externalDebug.Exit(handle);

				if (_runner._stop)
					throw new StopExecutionExecption();
			}
		}
		internal class StopExecutionExecption: Exception{
	
		}
	}
}
