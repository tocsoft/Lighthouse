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
			device.Connect();

			
			string prog = @"

device.Led1.IsOn = device.Switch1.IsOn;

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
