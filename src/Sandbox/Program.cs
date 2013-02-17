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
			var device = new AutoDevice();
			device.Refresh();

			while (true)
			{
				var val1 = device.Input("INPUT1").IsOn;
				device.Buzzer("BUZZER1").IsOn = val1;
				
				device.Buzzer("BUZZER1").Tone = device.Range("RANGE1").Value;

			}
			Console.ReadLine();
			device.Disconnect();
		}
	}
}
