using Lighthouse.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lighthouse.Win
{
	public class BlocklyDebugger : IDebugger
	{
		private WebKit.WebKitBrowser _browser;
		private Arduino _device;

		public BlocklyDebugger(WebKit.WebKitBrowser browser, Arduino device) {
			_browser = browser;
			_device = device;
			Speed = 500;
		}

		public int Speed { get; set; }
		public void RunHighLight(int handle) {

		}
		public void Enter(int handle)
		{
			try
			{
				Action act = () => {
					try
					{

						var ret = _browser.GetScriptManager.CallFunction("highlight", new object[] { handle });
					}
					catch { }
				};
				_browser.Invoke(act);
				if (Speed > 0)
				{
					Thread.Sleep(Speed);
				}
			}
			catch { }
		}

		public void Exit(int handle)
		{

		}
	}
}
