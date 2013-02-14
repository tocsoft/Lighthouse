using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xilium.CefGlue.WindowsForms;

namespace Lighthouse.App
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			CefWebBrowser browser = new CefWebBrowser();
			browser.Dock = DockStyle.Fill;
			
			//browser.Browser.GetMainFrame().V8Context.GetGlobal().

			Controls.Add(browser);
		}
	}
}
