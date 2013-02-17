using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	public class Button : BaseComponent
	{
		public Button(byte componentAddress, string componentName) : base(componentAddress, componentName) { }

		public bool IsOn {
			get {
				return Get(0x01) == 1;
			}
			set
			{
				Set(0x01, (byte)(value ? 1 : 0));
			}
		}
	}
}
