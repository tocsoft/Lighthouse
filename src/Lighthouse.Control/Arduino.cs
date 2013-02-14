
using Noesis.Javascript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lighthouse.Control
{

	
    public abstract class Arduino: IDisposable
    {
		SerialPort _serialPort;
		Guid _id;
		Guid _reportedId;
		bool refreshed = false;
		List<IComponent> Components = new List<IComponent>();
		protected void RegisterCompontent(IComponent component){
			Components.Add(component);
			component.PropertyChanged += component_PropertChanged;
		}

		public Arduino(Guid Id) { 
			//connectes to devide witch specific ID
			_id = Id;

			StatusUpdate += Arduino_StatusUpdate;
			_spProcessor.WorkerSupportsCancellation = true;
			_spProcessor.DoWork += _spProcessor_DoWork;
			_spProcessor.RunWorkerAsync();
			Initialize();
		}

		void _spProcessor_DoWork(object sender, DoWorkEventArgs e)
		{
			while(!_spProcessor.CancellationPending){
				if (_serialPort != null)
				{
					processFromPort(_serialPort);
				}
			}
		}

		void Arduino_StatusUpdate(object sender, StatusEventArgs e)
		{
			if (e.Device == "SYSTEM" && e.Property == "ID")
			{
				refreshed = true;
				try
				{
					_reportedId = new Guid(e.Value);
				}
				catch { }
			}

			foreach (var c in Components.Where(x => x.ComponentAddress == e.Device))
			{
				c.UpdateProperty(e.Property, byte.Parse(e.Value));
			}
		}

		protected abstract void Initialize();

		
		public Arduino()
			: this(Guid.Empty)
		{
			
		}

		BackgroundWorker _spProcessor = new BackgroundWorker();
		
		
		StringBuilder _lineBuffer = new StringBuilder();
		private void processFromPort(SerialPort sp){
			lock (sp)
			{
				while(sp.BytesToRead > 0)
				{

					var c = (Char)sp.ReadChar();
					if (c != '\n')
					{
						_lineBuffer.Append(c);
					}
					else
					{
						if (StatusUpdate != null)
						{
							var parts = _lineBuffer.ToString().Trim().Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
							if (parts.Count() == 3)
							{
								StatusUpdate.Invoke(this,
									new StatusEventArgs
									{
										Device = parts[0],
										Property = parts[1],
										Value = parts[2]
									});
							}
						}
						_lineBuffer.Clear();
					}
				}
			}
			foreach (var line in "")
			{



			}

			
		}
		public void Refresh()
		{
			EnsureConnection();
				refreshed = false;
				//StreamReader sr = new StreamReader(_serialPort.BaseStream, Encoding.ASCII);
				//_serialPort.DiscardInBuffer();
				_serialPort.DiscardOutBuffer();
				_serialPort.WriteLine("STATUS");
				Thread.Sleep(10);
				processFromPort(_serialPort);
				//var line = sr.ReadLine();
				
				//_serialPort
			
		}

		public event EventHandler<StatusEventArgs> StatusUpdate;


		void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			
			var sp = (sender as SerialPort);
			processFromPort(sp);
		}


		public void Connect() {

			var allPorts = SerialPort.GetPortNames();

			foreach (var port in allPorts) {

				SerialPort sp = null;
				try
				{
					_reportedId = Guid.Empty;
					sp = new SerialPort(port, 57600);
					sp.DataReceived += sp_DataReceived;
					sp.DtrEnable = true;
					sp.RtsEnable = true;
					sp.Open();

					//clear buffer before opp to make sure we only recieve the right stuff back
					
						sp.WriteLine("STATUS");

					if (_id == Guid.Empty || _reportedId == _id)
					{
						_serialPort = sp;
						return;
					}
					else
					{
						sp.DataReceived -= sp_DataReceived;
						sp.Close();
					}
					
				}
				catch {
					if (sp != null)
					{
						if (sp.IsOpen) {
							sp.Close();
						}
					}
				}
			}
		}


		public void Disconnect()
		{
			if (_serialPort != null)
			{
				_serialPort.DataReceived -= sp_DataReceived;
				_serialPort.Close();
				_serialPort.Dispose();
				_serialPort = null;
			}
		}

		public bool IsConnected {get{ return _serialPort != null && _serialPort.IsOpen;}}


		//public void Flush()
		//{

		//	StringBuilder cmdBuffer = new StringBuilder();

		//	bool propertyUpdated = false;
		//	foreach (var comp in Components)
		//	{
		//		foreach (var property in comp.UpdatedProperties())
		//		{
		//			propertyUpdated = true;
		//			cmdBuffer.AppendFormat("{0} {1} {2}", comp.ComponentAddress, property.Key, property.Value);
		//			cmdBuffer.AppendLine();
		//		}
		//	}
		//	if (propertyUpdated)
		//	{
		//		if (!IsConnected)
		//		{
		//			Disconnect();
		//			Connect();
		//			if (!IsConnected)
		//				throw new Exception("Serial port not connected");
		//		}
		//		_serialPort.Write(cmdBuffer.ToString());
		//		cmdBuffer.Clear();
		//	}
		//}
		private void EnsureConnection(){
				if (!IsConnected)
				{
					Disconnect();
					Connect();
					if (!IsConnected)
						throw new Exception("Serial port not connected");
				}
		}
		void component_PropertChanged(object sender, PropertyChangedEventArgs e)
		{
			EnsureConnection();
			var comp = sender as IComponent;
			
			
				lock (_serialPort)
				{
					_serialPort.Write(string.Format("{0} {1} {2}", comp.ComponentAddress, e.Property, e.Value));
				}
				processFromPort(_serialPort);
		}

		public void Dispose()
		{
			Disconnect();
			_spProcessor.CancelAsync();
		}

	}
	public class StatusEventArgs : EventArgs
	{
		public string Device { get; set; }
		public string Property { get; set; }
		public string Value { get; set; }
	}

	internal class Codebase {
		public IDebugger Debugger { get; set; }
		public string Code { get; set; }
	}
}
