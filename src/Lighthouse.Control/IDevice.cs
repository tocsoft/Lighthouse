using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	public interface IDevice
	{

		event EventHandler ComponentAdded;

		IEnumerable<Led> Leds { get; }
		IEnumerable<Button> Inputs { get; }
		IEnumerable<VariableInput> Ranges { get; }
		IEnumerable<Buzzer> Buzzers { get; }

		Led Led(string key);
		Button Input(string key);
		VariableInput Range(string key);
		Buzzer Buzzer(string key);

		void Refresh();
		event EventHandler ComponentStatusUpdated;
	}
}
