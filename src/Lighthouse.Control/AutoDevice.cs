using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lighthouse.Control
{
	public  class AutoDevice : BaseDevice
	{
		public event EventHandler ComponentAdded;
		bool compAdded = false;
		protected override void UnknownDevice(byte componentAddress, byte propertyAddress, byte value)
		{
			IComponent newComp = null;
			if (componentAddress >= 0x10 && componentAddress <= 0x1F)
			{
				var itm = new Button(componentAddress, "INPUT" + (componentAddress - 0x10).ToString());
				RegisterCompontent(itm);
				newComp = itm;

			}
			else if (componentAddress >= 0x20 && componentAddress <= 0x2F)
			{
				var itm = new Led(componentAddress, "LED" + (componentAddress - 0x20).ToString());
				RegisterCompontent(itm);
				newComp = itm;

			}
			else if (componentAddress >= 0x30 && componentAddress <= 0x3F)
			{
				var itm = new VariableInput(componentAddress, "RANGE" + (componentAddress - 0x30).ToString());
				RegisterCompontent(itm);
				newComp = itm;
			}
			else if (componentAddress >= 0x40 && componentAddress <= 0x4F)
			{
				var itm = new Buzzer(componentAddress, "BUZZER" + (componentAddress - 0x40).ToString());
				RegisterCompontent(itm);
				newComp = itm;
			}

			if (newComp != null)
			{
				compAdded = true;
				if (ComponentAdded != null)
					ComponentAdded.Invoke(this, new EventArgs());
				newComp.UpdateProperty(propertyAddress, value);
			}
		}

		public void AwaitComponents() {
			while (!compAdded) {
				Thread.Sleep(50);
			}

		}

		protected override void Setup()
		{
			//don't do anything just use unknow device to add stuff
		}

		public override VariableInput Range(string key)
		{
			VariableInput ret = base.Range(key);
			int count = 0;
			while (ret == null)
			{
				count++;
				if (count > 40)
					return null;
				Thread.Sleep(50);
				ret = base.Range(key);
			}
			return ret;
		}
		public override Button Input(string key)
		{
			Button ret = base.Input(key);
			int count = 0;
			while (ret == null)
			{
				count++;
				if (count > 40)
					return null;
				Thread.Sleep(50);
				ret = base.Input(key);

			}
			return ret;
		}
		public override Led Led(string key)
		{
			Led ret = base.Led(key);
			int count = 0;
			while (ret == null)
			{
				count++;
				if (count > 40)
					return null;
				Thread.Sleep(50);
				ret = base.Led(key);

			}
			return ret;
		}
		public override Buzzer Buzzer(string key)
		{
			Buzzer ret = base.Buzzer(key);
			int count = 0;
			while (ret == null)
			{
				count++;
				if (count > 40)
					return null;
				Thread.Sleep(50);
				ret = base.Buzzer(key);

			}
			return ret;
		}

	}
}
