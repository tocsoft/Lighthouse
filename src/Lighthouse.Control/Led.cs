using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	public class Led : BaseComponent
	{
		public Led(string componentAddress) : base(componentAddress) { }

		public bool IsOn
		{
			get
			{
				return Get("ON") == 1;
			}
			set
			{
				Set("ON", (byte)(value ? 1 : 0));
			}
		}

		public bool IsFlashing
		{
			get
			{
				return Get("FLASH") == 1;
			}
			set
			{
				Set("FLASH", (byte)(value ? 1 : 0));
			}
		}
	}
}
