using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lighthouse.Control;

namespace Lighthouse.App.Device
{
	public partial class DevicePreview : UserControl
	{
		public DevicePreview()
		{
			InitializeComponent();
		}

		IDevice _device;
		public IDevice Device
		{
			get { return _device; }
			set
			{
				if (_device != null)
					Remove(_device);
				_device = value;
				if (_device != null)
					LoadDevice(_device);
			}
		}
		bool loaded = false;
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			loaded = true;
			if (_device != null)
				LoadDevice(_device);
		}

		public void Refesh() {
			//sync device properties
		}

		Dictionary<Lighthouse.Control.Button, CheckBox> _buttons = new Dictionary<Control.Button, CheckBox>();
		Dictionary<Lighthouse.Control.Led, CheckBox> _leds = new Dictionary<Control.Led, CheckBox>();
		Dictionary<Lighthouse.Control.VariableInput, RangeWithLabel> _inputs = new Dictionary<Control.VariableInput, RangeWithLabel>();
		Dictionary<Lighthouse.Control.Buzzer, RangeWithCheckbox> _buzzers = new Dictionary<Control.Buzzer, RangeWithCheckbox>();

		private void Remove(IDevice dev) {
			dev.ComponentAdded -= dev_ComponentAdded;
			foreach (var button in dev.Inputs.Where(x=>_buttons.ContainsKey(x)))
			{

				((Lighthouse.Control.IComponent)(button)).PropertyChanged -= DevicePreview_PropertyChanged;
				
				Controls.Remove(_buttons[button]);
				_buttons.Remove(button);
			}
			foreach (var led in dev.Leds.Where(x => _leds.ContainsKey(x)))
			{
				((Lighthouse.Control.IComponent)(led)).PropertyChanged -= DevicePreview_PropertyChanged;

				Controls.Remove(_leds[led]);
				_leds.Remove(led);
			}
			foreach (var item in dev.Ranges.Where(x => _inputs.ContainsKey(x)))
			{
				((Lighthouse.Control.IComponent)(item)).PropertyChanged -= DevicePreview_PropertyChanged;

				Controls.Remove(_inputs[item]);
				_inputs.Remove(item);
			}
			foreach (var item in dev.Buzzers.Where(x => _buzzers.ContainsKey(x)))
			{
				((Lighthouse.Control.IComponent)(item)).PropertyChanged -= DevicePreview_PropertyChanged;

				Controls.Remove(_buzzers[item]);
				_buzzers.Remove(item);
			}
		}

		void dev_ComponentAdded(object sender, EventArgs e)
		{
			this.Invoke((MethodInvoker)delegate
			{
				Device = sender as IDevice;
			});
		}

		private void LoadDevice(IDevice dev)
		{
			if (loaded)
			{
				foreach (var button in dev.Inputs)
				{
					((Lighthouse.Control.IComponent)(button)).PropertyChanged += DevicePreview_PropertyChanged;

					CheckBox cb = new CheckBox()
					{
						Text = button.ComponentName,
						Checked = button.IsOn,
						Dock = DockStyle.Top
					};
					cb.CheckedChanged += (s, e) =>
					{
						button.IsOn = cb.Checked;
					};
					Controls.Add(cb);
					_buttons.Add(button, cb);

				}
				foreach (var led in dev.Leds)
				{
					((Lighthouse.Control.IComponent)(led)).PropertyChanged += DevicePreview_PropertyChanged;

					CheckBox cb = new CheckBox()
					{
						Text = led.ComponentName,
						Checked = led.IsOn,
						Dock = DockStyle.Top

					};

					cb.CheckedChanged += (s, e) =>
					{
						led.IsOn = cb.Checked;
					};
					Controls.Add(cb);
					_leds.Add(led, cb);

				}
				foreach (var item in dev.Ranges)
				{
					((Lighthouse.Control.IComponent)(item)).PropertyChanged += DevicePreview_PropertyChanged;

					RangeWithLabel cb = new RangeWithLabel()
					{
						Text = item.ComponentName,
						Value = item.Value,
						Dock = DockStyle.Top
					};

					cb.ValueChanged += (s, e) =>
					{
						item.Value = (byte)cb.Value;
					};
					Controls.Add(cb);
					_inputs.Add(item, cb);

				}


				foreach (var item in dev.Buzzers)
				{
					((Lighthouse.Control.IComponent)(item)).PropertyChanged += DevicePreview_PropertyChanged;

					RangeWithCheckbox cb = new RangeWithCheckbox()
					{
						Text = item.ComponentName,
						Value = item.Tone,
						Checked = item.IsOn,
						Dock = DockStyle.Top
					};

					cb.ValueChanged += (s, e) =>
					{
						item.Tone = (byte)cb.Value;
						item.IsOn = cb.Checked;
					};
					Controls.Add(cb);
					_buzzers.Add(item, cb);

				}

				dev.ComponentAdded += dev_ComponentAdded;

				dev.Refresh();
			}
		}
		private void RefreshDevice()
		{
			foreach (var button in Device.Inputs)
			{
				_buttons[button].Checked = button.IsOn;
			}
			foreach (var itm in Device.Leds)
			{
				_leds[itm].Checked = itm.IsOn;
			}
			foreach (var itm in Device.Ranges)
			{
				_inputs[itm].Value = itm.Value;
			}
			foreach (var itm in Device.Buzzers)
			{
				_buzzers[itm].Value = itm.Tone;
				_buzzers[itm].Checked = itm.IsOn;
			}
		}

		

		void DevicePreview_PropertyChanged(object sender, Control.PropertyChangedEventArgs e)
		{
			this.Invoke((MethodInvoker)delegate
			{
				RefreshDevice();
			});
		}

	}
}
