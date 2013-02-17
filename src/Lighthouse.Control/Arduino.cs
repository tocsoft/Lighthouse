
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
		public event EventHandler ComponentAdded;

		protected void RegisterCompontent(IComponent component){
			Components.Add(component);
			component.PropertySet += component_PropertChanged;
			if (ComponentAdded != null)
				ComponentAdded.Invoke(this, new EventArgs());

		}

		public Arduino(Guid Id) { 
			//connectes to devide witch specific ID
			_id = Id;

			_spProcessor.WorkerSupportsCancellation = true;
			_spProcessor.DoWork += _spProcessor_DoWork;
			_spProcessor.RunWorkerAsync();
			Initialize();
		}

		void _spProcessor_DoWork(object sender, DoWorkEventArgs e)
		{
			while(!_spProcessor.CancellationPending){
				
					processFromPort(_serialPort);
				
			}
		}


		protected abstract void Initialize();

		
		public Arduino()
			: this(Guid.Empty)
		{
			
		}

		BackgroundWorker _spProcessor = new BackgroundWorker();

		object readSpLock = new object();
		private void processFromPort(SerialPort sp)
		{
			if (sp != null && sp.IsOpen)
				if (sp != null && sp.IsOpen)
					lock (readSpLock)
					{
						if (sp.BytesToRead < 5)
							return;
						//this will block until a 0x06 is found or buffer size becomes less then 3.
						while (sp.ReadByte() != 0x06)
						{
							//This will trash any preamble junk in the serial buffer
							//but we need to make sure there is enough in the buffer to process while we trash the rest
							//if the buffer becomes too empty, we will escape and try again on the next call
							if (sp.BytesToRead < 5)
								return;
						}
						

						byte componentAddress = (byte)sp.ReadByte();
						byte propertyAddress = (byte)sp.ReadByte();
						byte value = (byte)sp.ReadByte();

						int cs = sp.ReadByte();

						byte calc_CS = 0;
						calc_CS ^= componentAddress;
						calc_CS ^= propertyAddress;
						calc_CS ^= value;

						if (cs == calc_CS)
						{
							//recieved correctly 
							//send update
							var comps = Components.Where(x => x.ComponentAddress == componentAddress);
							foreach (var c in comps)
							{
								c.UpdateProperty(propertyAddress, value);
							}
							if (!comps.Any()) {
								UnknownDevice(componentAddress, propertyAddress, value);
							}
						}
					}
		}

		protected abstract void UnknownDevice(byte componentAddress, byte propertyAddress, byte value);
		
		public void Refresh()
		{
			if (EnsureConnection())
			{

				SendCmd(0x01, 0x01, 0x01);

				
			}
			
		}

		public event EventHandler<StatusEventArgs> StatusUpdate;

		public void Connect() {

			var allPorts = SerialPort.GetPortNames();

			foreach (var port in allPorts) {

				SerialPort sp = null;
				try
				{
					_reportedId = Guid.Empty;
					sp = new SerialPort(port, 57600);
					sp.DtrEnable = true;
					sp.RtsEnable = true;
					sp.Open();

					
					if (_id == Guid.Empty || _reportedId == _id)
					{
						_serialPort = sp;
						return;
					}
					else
					{
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
				try
				{
					_serialPort.Close();
					_serialPort.Dispose();
				}
				catch { }
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
		private bool EnsureConnection(){
				if (!IsConnected)
				{
					Disconnect();
					Connect();
					return IsConnected;
				}
				return true;
		}
		void component_PropertChanged(object sender, PropertyChangedEventArgs e)
		{
			if (EnsureConnection())
			{
				
					var comp = sender as IComponent;

					SendCmd(comp.ComponentAddress, e.Property, e.Value);

				
			}

		}

		private void SendCmd(byte component, byte prop, byte value) {
			try
			{
				lock (_serialPort)
				{

					byte calc_CS = 0;
					calc_CS ^= component;
					calc_CS ^= prop;
					calc_CS ^= value;

					_serialPort.Write(new byte[]{
								0x06,
								component,
								prop, 
								value,
								calc_CS
							}, 0, 5);
				}
			}
			catch
			{
				Disconnect();
			}
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
