using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	public class VariableInput : BaseComponent
	{
		public VariableInput(string componentAddress) : base(componentAddress) { }

		public byte Value {
			get {
				return Get("VALUE");
			}
		}
	}
}
