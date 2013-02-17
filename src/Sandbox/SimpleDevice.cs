using Lighthouse.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandbox
{
	public class SimpleDevice : BaseDevice
	{
		protected override void UnknownDevice(byte componentAddress, byte propertyAddress, byte value)
		{
			
		}
		protected override void Setup()
		{

			//RegisterCompontent(new Led("RGB"));

			RegisterCompontent(new Button(0x11,"SWITCH1"));
			RegisterCompontent(new Button(0x12, "BUTTON1"));

			RegisterCompontent(new Led(0x21, "LED1"));
			RegisterCompontent(new Led(0x22, "LED2"));

			RegisterCompontent(new VariableInput(0x31, "KNOB"));
			RegisterCompontent(new VariableInput(0x32, "LIGHT"));


			
		}
	}
}
