
using Lighthouse.Control;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Web;

namespace Lighthouse.Web.Hubs
{
	[HubName("arduino")]
	public class Arduino : Hub
	{
		static Lighthouse.Control.SampleDevice _device = new Control.SampleDevice();

		
		public Arduino():base() {
			//Device.StatusUpdated += (s, e) =>
			//{
			//	Clients.Caller.updateState(Device);
			//};
		}


		public void Refresh()
		{
			Device.Refresh();

		}

		public SampleDevice Device { get { return _device; } }



		public bool GetLedIsOn(string name)
		{
			if (!_device.IsConnected)
				_device.Connect();

			return _device.Leds[name].IsOn;
		}
		public bool GetButtonIsOn(string name)
		{
			if (!_device.IsConnected)
				_device.Connect();

			return _device.Buttons[name].IsOn;
		}
		public void SetLedIsOn(string name, bool state)
		{
			if (!_device.IsConnected)
				_device.Connect();

			_device.Leds[name].IsOn = state;
		}

		//public void SetLedColor(string color)
		//{

		//	if (!_device.IsConnected)
		//		_device.Connect();

		//	//_device.Led.Color = ColorTranslator.FromHtml(color);
		//}

		//public string GetLedColor()
		//{

		//	if (!_device.IsConnected)
		//		_device.Connect();

		//	return ColorTranslator.ToHtml(_device.Led.Color);

		//}


	}
}