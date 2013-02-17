using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	public abstract class BaseDevice : Arduino, IDevice
	{

		Dictionary<string, Led> Leds = new Dictionary<string, Led>();
		Dictionary<string, Button> Inputs = new Dictionary<string, Button>();
		Dictionary<string, VariableInput> Ranges = new Dictionary<string, VariableInput>();
		Dictionary<string, Buzzer> Buzzers = new Dictionary<string, Buzzer>();

		protected override void Initialize()
		{
			this.StatusUpdate += BaseDevice_StatusUpdate;
			Setup();
		}


		
		protected abstract void Setup();

		void BaseDevice_StatusUpdate(object sender, StatusEventArgs e)
		{
			if (this._componentStatusUpdated != null)
				this._componentStatusUpdated.Invoke(this, e);
		}

		public void RegisterCompontent(VariableInput item)
		{
			Ranges.Add(((IComponent)item).ComponentName, item);
			base.RegisterCompontent(item);
		}
		public void RegisterCompontent(Led item)
		{
			Leds.Add(((IComponent)item).ComponentName, item);
			base.RegisterCompontent(item);
		}
		public void RegisterCompontent(Button item)
		{
			Inputs.Add(((IComponent)item).ComponentName, item);
			base.RegisterCompontent(item);
		}
		public void RegisterCompontent(Buzzer item)
		{
			Buzzers.Add(((IComponent)item).ComponentName, item);
			base.RegisterCompontent(item);
		}


		event EventHandler _componentStatusUpdated;
		public event EventHandler ComponentStatusUpdated
		{
			add
			{
				_componentStatusUpdated += value;
			}
			remove
			{
				_componentStatusUpdated -= value;
			}
		}






		public virtual Led Led(string key)
		{
			if (Leds.ContainsKey(key))
				return Leds[key];
			else
				return null;
		}

		public virtual  Button Input(string key)
		{
			if (Inputs.ContainsKey(key))
				return Inputs[key];
			else
				return null;
		}

		public virtual VariableInput Range(string key)
		{
			if (Ranges.ContainsKey(key))
				return Ranges[key];
			else
				return null;
		}

		public virtual Buzzer Buzzer(string key)
		{
			if (Buzzers.ContainsKey(key))
				return Buzzers[key];
			else
				return null;
		}


		IEnumerable<Led> IDevice.Leds
		{
			get { return this.Leds.Values; }
		}

		IEnumerable<Button> IDevice.Inputs
		{
			get { return this.Inputs.Values; }
		}

		IEnumerable<VariableInput> IDevice.Ranges
		{
			get { return this.Ranges.Values; }
		}
		IEnumerable<Buzzer> IDevice.Buzzers
		{
			get { return this.Buzzers.Values; }
		}
	}
}
