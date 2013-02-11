
using Noesis.Javascript;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{

	public class SampleDevice : Arduino
	{
		public SampleDevice(Guid Id)
			: base(Id)
		{
			Leds = new Dictionary<string, Led>();
			Leds.Add("RGB", Rgb = new RgbLed(this, "RGB"));
			Leds.Add("LED1", Led1 = new Led(this, "LED1"));
			Leds.Add("LED2", Led2 = new RgbLed(this, "LED2"));
			//_Servo = new Servo(this);
			Buttons = new Dictionary<string, Button>();
			Buttons.Add("SWITCH1", Switch1 = new Button(this, "SWITCH1"));
			Buttons.Add("BUTTON1", Button1 = new Button(this, "BUTTON1"));
		}

		public SampleDevice() : this(Guid.Empty) { }

		public Led Led1 { get; private set; }
		public Led Led2 { get; private set; }
		public Led Rgb { get; private set; }

		public Button Switch1 { get; private set; }
		public Button Button1 { get; private set; }
		
		public Dictionary<string, Led> Leds { get; private set; }
		public Dictionary<string, Button> Buttons { get; private set; }

	}
    public abstract class Arduino: IDisposable
    {
		SerialPort _serialPort;
		Guid _id;
		Guid _reportedId;
		public Arduino(Guid Id) { 
			//connectes to devide witch specific ID
			_id = Id;

			StatusUpdate += (s, e) =>
			{
				if (e.Device == "SYSTEM" && e.Property == "ID")
				{
					_reportedId = new Guid(e.Value);
				}
				
			};


			_worker.DoWork += _worker_DoWork;
			_worker.WorkerSupportsCancellation = true;
			_worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
		}

		void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			
			if(ProgramStopped != null)
				ProgramStopped.Invoke(this, new EventArgs());
		}

		public Arduino()
			: this(Guid.Empty)
		{
			
		}

		BackgroundWorker _worker = new BackgroundWorker();
		public void RunProgram(string javascript) { 
			//this will "program" the device with this code and have it run

			_worker.RunWorkerAsync(javascript);
		}
		public void StopProgram() {
			_worker.CancelAsync();
		}
		public bool IsProgramRunning { get { return _worker.IsBusy; } }
		void _worker_DoWork(object sender, DoWorkEventArgs e)
		{
			var javascript = (string)e.Argument;

			

			using (JavascriptContext context = new JavascriptContext())
			{
				// Setting the externals parameters of the context
				context.SetParameter("device", this);
				
				var lastRun = DateTime.Now.Ticks;
				while (!_worker.CancellationPending)
				{
					var now = DateTime.Now.Ticks;
					if((now - lastRun) > TimeSpan.TicksPerSecond * 5/*num secs*/){
						//every 1000 times round the loop run a refresh
						this.Refresh();
						lastRun = now;
					}

					
					// Running the script
					context.Run(javascript);
					this.Flush();
					
				}

			}

		}



		public void Refresh() 
		{
			SendCommand("STATUS");
			Flush();
		}

		public event EventHandler<StatusEventArgs> StatusUpdate;
		public event EventHandler ProgramStopped;


		void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			var sp = (sender as SerialPort);
			if (e.EventType == SerialData.Chars) {
				StringBuilder sb = new StringBuilder();
				while (sp.IsOpen && sp.BytesToRead > 0)
				{
					var chr = (char)sp.ReadChar();
					Console.Write(chr);
					if (chr == '\n' || chr == '\r')
					{
						var line = sb.ToString().Trim();
						if (line.Length > 0)
						{

							if (line.StartsWith("#"))
							{
								//Console.WriteLine(line);
							}
							else
							{
								if (StatusUpdate != null)
								{
									var parts = line.Split(new char[] { ' ' }, 3);
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
							}
						}

						sb.Clear();
					}
					else { sb.Append(chr); }

				}

			}
		}


		public void Connect() {

			var allPorts = SerialPort.GetPortNames();

			foreach (var port in allPorts) {

				SerialPort sp = null;
				try
				{
					_reportedId = Guid.Empty;
					sp = new SerialPort(port, 9600);
					sp.DataReceived += sp_DataReceived;
					sp.DtrEnable = true;
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

		StringBuilder cmdBuffer = new StringBuilder();
		object cmdLocker = new object();
		internal void SendCommand(string cmd, params object[] args)
		{
			args = args ?? new object[] { };
			lock (cmdLocker)
			{
				var pkt = cmd + " " + string.Join(" ", args);
				//Console.WriteLine(">" + pkt);
				cmdBuffer.AppendLine(pkt);

			}
		}

		public void Flush() {
			lock (cmdLocker)
			{
				if (cmdBuffer.Length > 0)
				{
					if (!IsConnected)
					{
						Disconnect();
						Connect();
						if (!IsConnected)
							throw new Exception("Serial port not connected");
					}
					_serialPort.Write(cmdBuffer.ToString());
					cmdBuffer.Clear();
				}
			}
		}



		public void Dispose()
		{
			Disconnect();
		}

	}
	public class StatusEventArgs : EventArgs
	{
		public string Device { get; set; }
		public string Property { get; set; }
		public string Value { get; set; }
	}
	//public class Servo
	//{
	//	private Arduino _arduino;
	//	private string _commandSet;

	//	internal Servo(Arduino arduino)
	//	{
	//		_arduino = arduino;
	//		//TODO to speed things up we could query full state on init and
			
	//	}

		
	//	public bool IsSweeping
	//	{
	//		get
	//		{
	//			var mode = _arduino.SendCommand("SERVO", "GET", "MODE");
	//			return mode == "SWEEP";
	//		}
	//		set
	//		{
	//			_arduino.SendCommand("SERVO", "SET", "MODE", (value ? "SWEEP" : "STATIC"));
	//		}
	//	}
	//	public int CurrentAngle
	//	{
	//		get
	//		{
	//			return int.Parse(_arduino.SendCommand("SERVO", "GET", "ANGLE"));
	//		}
	//		set
	//		{
	//			_arduino.SendCommand("SERVO", "SET", "ANGLE", value);
	//		}
	//	}
		
	//	public int MinAngle
	//	{
	//		get
	//		{
	//			return int.Parse(_arduino.SendCommand("SERVO", "GET", "MIN"));
	//		}
	//		set
	//		{
	//			_arduino.SendCommand("SERVO", "SET", "MIN", value);
	//		}
	//	}
	//	public int MaxAngle
	//	{
	//		get
	//		{
	//			return int.Parse(_arduino.SendCommand("SERVO", "GET", "MAX"));
	//		}
	//		set
	//		{
	//			_arduino.SendCommand("SERVO", "SET", "MAX", value);
	//		}
	//	}
	//	public int Speed
	//	{
	//		get
	//		{
	//			return int.Parse(_arduino.SendCommand("SERVO", "GET", "SPEED"));
	//		}
	//		set
	//		{
	//			_arduino.SendCommand("SERVO", "SET", "SPEED", value);
	//		}
	//	}
	//}


	public class Button
	{
		private Arduino _arduino;
		private string _commandSet;
		private string _device;

		internal Button(Arduino arduino, string device)
		{
			_arduino = arduino;
			_device = device;

			_arduino.StatusUpdate += (s, e) =>
			{
				if (e.Device == _device &&
				e.Property == "STATE") {
					IsOn = (e.Value == "ON");

//					Console.WriteLine("{0} - {1}",_device, IsOn);
				}
			};
		}

		public bool IsOn
		{
			get;
			private set;
		}
	}

	public class Led
	{
		private Arduino _arduino;
		private string _deviceRef;

		internal Led(Arduino arduino, string deviceRef)
		{
			_arduino = arduino;
			_deviceRef = deviceRef;
			//TODO to speed things up we could query full state on init and

			_arduino.StatusUpdate += (s, e) =>
			{
				if (e.Device == deviceRef &&
				e.Property == "STATE")
				{
					_isOn = (e.Value == "ON");
				//	Console.WriteLine("{0} - {1}", deviceRef, _isOn);
				}
			};
		}
		bool _isOn;
		public bool IsOn
		{
			get
			{
				return _isOn;
			}
			set
			{
				if (_isOn != value)
				{
					_isOn = value;
					_arduino.SendCommand(_deviceRef, "STATE", (value ? "ON" : "OFF"));
				}
			}
		}
	}
	public class RgbLed : Led
	{
		private Arduino _arduino;
		private string _device;
		internal RgbLed(Arduino arduino, string device)
			: base(arduino, device)
		{
			_arduino = arduino;
			_device = device;
			//TODO to speed things up we could query full state on init and
			_arduino.StatusUpdate += (s, e) =>
			{
				if (e.Device == _device &&
				e.Property == "COLOR")
				{
					var parts = e.Value.Split(' ');
					int red = int.Parse(parts[0]);
					int green = int.Parse(parts[1]);
					int blue = int.Parse(parts[2]);
					_Color = Color.FromArgb(red, green, blue); 
				} 
			};
		}

		Color _Color;
		public Color Color
		{
			get
			{
				return _Color;
			}
			set
			{
				if (_Color != value)
				{
					_Color = value;
					_arduino.SendCommand(_device, "COLOR", value.R, value.G, value.B);
				}
			}
		}
	}
}
