using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	public class VariableInput : BaseComponent
	{
		public VariableInput(byte componentAddress, string componentName) : base(componentAddress, componentName) { }

		public int Value {
			get {
				return Get(0x11);
			}
			set {
				Set(0x11, (byte)value);
			}
		}
	}
}
