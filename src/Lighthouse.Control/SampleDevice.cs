using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	public class SampleDevice : Arduino
	{
		
		public Led Led1 { get; private set; }
		public Led Led2 { get; private set; }
		public RgbLed Rgb { get; private set; }

		public Button Switch1 { get; private set; }
		public Button Button1 { get; private set; }

		public VariableInput Knob { get; private set; }
		public VariableInput Light { get; private set; }

		protected override void Initialize()
		{
			RegisterCompontent(Rgb = new RgbLed("RGB"));

			RegisterCompontent(Led1 = new Led("LED1"));

			RegisterCompontent(Led2 = new Led("LED2"));

			RegisterCompontent(Switch1 = new Button("SWITCH1"));
			RegisterCompontent(Button1 = new Button("BUTTON1"));

			RegisterCompontent(Light = new VariableInput("LIGHT"));
			RegisterCompontent(Knob = new VariableInput("KNOB"));
		}
	}
}
