using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	public class Buzzer : BaseComponent
	{
		public Buzzer(byte componentAddress, string componentName) : base(componentAddress, componentName) { }

		public bool IsOn
		{
			get
			{
				return Get(0x01) == 1;
			}
			set
			{
				Set(0x01, (byte)(value ? 1 : 0));
			}
		}

		public int Tone
		{
			get
			{
				return Get(0x11);
			}
			set
			{
				Set(0x11, (byte)value);
			}
		}

	}
}
