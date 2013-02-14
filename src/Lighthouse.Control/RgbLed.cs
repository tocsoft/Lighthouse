using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	public class RgbLed : Led
	{
		public RgbLed(string componentAddress) : base(componentAddress) { }

		public string Color
		{
			get 
			{

				return ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(
					Get("RED"),
					Get("GREEN"),
					Get("BLUE")));
			}
			set 
			{
				var c = ColorTranslator.FromHtml(value);
				Set("RED", c.R);
				Set("GREEN", c.G);
				Set("BLUE", c.B);
			}
		}
	}
}
