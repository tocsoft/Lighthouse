using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	public class Button : BaseComponent
	{
		public Button(string componentAddress) : base(componentAddress) { }

		public bool IsOn {
			get {
				return Get("ON") == 1;
			}
		}
	}
}
