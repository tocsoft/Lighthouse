using Jurassic;
using Lighthouse.Control;
using Noesis.Javascript;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			var device = new SampleDevice();
			device.Refresh();

			string color = device.Rgb.Color;

			string[] colors = new string[]{
				"#ff0000",
				"#00ff00",
				"#0000ff",
				"#ff00ff",
				"#00ffff",
				"#ffff00"
			};

			var times = DateTime.Now;
			var pos = 0;

			while (true) {

				if (device.Light.Value < 125)
					device.Led1.IsOn = true;
				else
					device.Led1.IsOn = false;
			}
			//while (true) {
			//	device.Rgb.IsOn = device.Switch1.IsOn;
			//	var tmpC = device.Rgb.Color;
			//	var tmpNow = DateTime.Now;
			//	if (tmpNow.Subtract(times).Seconds > 2)
			//	{
			//		times = tmpNow;
			//		pos = (pos + 1) % colors.Length;
			//		device.Rgb.Color = colors[pos];
			//	}


			//	device.Flush();
			//}
			string prog = @"

if(device.Knob.Value > 230){
	device.Led1.IsFlashing = true;
	device.Led2.IsFlashing = true;
}else{
	device.Led1.IsFlashing = false;
	device.Led2.IsFlashing = false;

	if(device.Knob.Value < 75){
		device.Led1.IsOn = false;
		device.Led2.IsOn = false;
	}
	if(device.Knob.Value > 75){
		device.Led1.IsOn = true;
	}
	if(device.Knob.Value > 140){
		device.Led1.IsOn = true;
	}
}

";
			
			device.RunProgram(prog);
			while (device.IsProgramRunning)
			{
				//hold loop
			}
			Console.ReadLine();
			device.Disconnect();
		}
	}
}
