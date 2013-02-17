using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Net;
using System.Reflection;
using System.IO;
using Lighthouse.Control;
using System.Threading;

namespace Lighthouse.App.blockly
{
	public partial class Editor : UserControl
	{
		public class Proxy
		{
			private Editor _editor;
			public int _highlighted;
			public int highlighted()
			{
				return _highlighted;
			}
			public Proxy(Editor editor) { _editor = editor; }
			public string getXml() { return _editor.Xml ?? "<xml />"; }

			public void setupComplete() {
				_editor.isLoading = false;
				_editor.SetupComplete(); 
			}
			public void updateXml(string xml)
			{
				if (!_editor.isLoading)
				{
					if (_editor._xml != xml)
					{
						_editor._xml = xml;
						if (_editor.XmlChanged != null)
						{
							_editor.XmlChanged.Invoke(_editor, new EventArgs());
						}
					}
				}
			}

			public void run(string code) {
				if (!_editor.IsRunning && _editor.runner != null)
					_editor.runner.Run(code);
			}
		}

		private class RequestProxy : IRequestHandler
		{
			private string appPrefix;

			public RequestProxy(string appPrefix)
			{
				// TODO: Complete member initialization
				this.appPrefix = appPrefix;
			}

			bool IRequestHandler.OnBeforeBrowse(IWebBrowser browser, IRequest request,
								   NavigationType naigationvType, bool isRedirect)
			{
				return false;
			}

			bool IRequestHandler.OnBeforeResourceLoad(IWebBrowser browser,
											 IRequestResponse requestResponse)
			{
				IRequest request = requestResponse.Request;
				if (request.Url.StartsWith(appPrefix))
				{
					var url = request.Url.Substring(appPrefix.Length);

					url = url.Split('?')[0];
					var ext = Path.GetExtension(url);
					string mimeType = "";
					if (ext == "")
					{
						ext = ".html";
						url += ".html";
					}
					switch (ext.ToLower())
					{
						case ".html":
							mimeType = "text/html";
							break;
						case ".png":
							mimeType = "image/png";
							break;
						case ".js":
							mimeType = "application/javascript";
							break;
							break;
						case ".gif":
							mimeType = "image/gif";
							break;
							break;
						case ".css":
							mimeType = "text/css";
							break;
						case ".wav":
							mimeType = "audio/wav";
							break;
						default:
							mimeType = "application/octet-stream";
							break;
					}
					var str = GetFile(url);
					if (str != null)
						requestResponse.RespondWith(str, mimeType);

				}


				return false;
			}

			Stream GetFile(string url)
			{
				var full = "Lighthouse.App.blockly." + url.Replace("/", ".");
				var res = Assembly.GetExecutingAssembly().GetManifestResourceNames();
				if (res.Contains(full))
				{
					return Assembly.GetExecutingAssembly().GetManifestResourceStream(full);
				}
				return null;
			}

			void IRequestHandler.OnResourceResponse(IWebBrowser browser, string url,
										   int status, string statusText,
										   string mimeType, WebHeaderCollection headers)
			{
			}
		}

		internal class EditorDebugger : IDebugger {
			private Editor _editor;
			public EditorDebugger(Editor editor) {
				_editor = editor;
			}
			public void Enter(int handle)
			{
				_editor.javascriptProxy._highlighted = handle;
				//_editor.browser.ExecuteScript("highlight("+handle.ToString()+")");
				Thread.Sleep(250);
			}

			public void Exit(int handle)
			{
			}
		}

		private string _xml;
		bool isLoading = false;
		public string Xml
		{
			get { return _xml; }
			set
			{
				if (_xml != value)
				{
					if (!IsRunning)
					{
						isLoading = true;
						_xml = value;
						browser.Reload(true);
					}
					else
					{
						throw new Exception("Can't set xml while app running");
					}
				}
			}
		}

		private class Keyboard : IKeyboardHandler {
			public Editor _editor;
			public Keyboard(Editor editor) {
				_editor = editor;

			}

			IEnumerable<ToolStripMenuItem> _menuItems = null;
			private IEnumerable<ToolStripMenuItem> MenuItems {
				get {
					if (_menuItems == null)
						_menuItems = items().ToList();
					return _menuItems;
				}
			}
			private IEnumerable<ToolStripMenuItem> items()
			{
				foreach (var itm in  _editor.FindForm().MainMenuStrip.Items.Cast<ToolStripItem>())
				{
					ToolStripDropDownItem mnu = itm as ToolStripDropDownItem;
					if (mnu != null)
					{
						foreach (var i in items(mnu))
							yield return i;
					}

						var mn = itm as ToolStripMenuItem;
						if(mn != null)
							yield return mn;
					
				}

			}
			private IEnumerable<ToolStripMenuItem> items(ToolStripDropDownItem menuItem)
			{


				foreach (var itm in menuItem.DropDownItems.Cast<ToolStripItem>())
				{
					ToolStripDropDownItem mnu = itm as ToolStripDropDownItem;
					if (mnu != null)
					{
						foreach (var i in items(mnu))
							yield return i;
					}

						var mn = itm as ToolStripMenuItem;
						if (mn != null)
							yield return mn;
					
				}
			}
			public bool OnKeyEvent(IWebBrowser browser, KeyType type, int code, int modifiers, bool isSystemKey, bool isAfterJavaScript)
			{
				Keys key = (System.Windows.Forms.Control.ModifierKeys | (Keys)code);

				var itm = MenuItems.FirstOrDefault(x => x.ShortcutKeys == key && x.Enabled == true);
				if (itm != null)
				{
					_editor.Invoke((MethodInvoker)delegate
					{
						itm.PerformClick();
					});
					return true;
				}
				return false;
			}
		}
		
		public event EventHandler XmlChanged;

		private CefSharp.WinForms.WebView browser;
		private Proxy javascriptProxy;
		const string appPrefix = "http://app/";

		JavascriptRunner runner;
		EditorDebugger debugger;

		public Editor()
		{
			InitializeComponent();


			browser = new WebView(appPrefix + "host.html", new BrowserSettings() { WebSecurityDisabled = true });
			browser.Dock = DockStyle.Fill;
			browser.RequestHandler = new RequestProxy(appPrefix);
			Controls.Add(browser);

			javascriptProxy = new Proxy(this);
			debugger = new EditorDebugger(this);
			runner = new JavascriptRunner(debugger);

			browser.KeyboardHandler = new Keyboard(this);
			browser.RegisterJsObject("app", javascriptProxy);
			runner.Stopped += runner_Stopped;


		}


		public void Shutdown(){
			runner.Stop();
			browser.Stop();
			browser.Dispose();
		}
		public event EventHandler Stopped;

		void runner_Stopped(object sender, EventArgs e)
		{
			//tell webkit we've stopped so it can shift back to edit ode
			browser.ExecuteScript("stop()");
			if (Stopped != null)
				Stopped.Invoke(this, e);
		}

		public IDevice Device
		{
			get { return runner.Device; }
			set
			{
				if (!runner.IsRunning)
				{
					runner.Device = value;
				}
				else
				{
					throw new Exception("Can't swap device while app running");
				}
			}
		}

		public bool IsRunning
		{
			get
			{
				return runner.IsRunning;
			}
		}
		public bool CanRun
		{
			get
			{
				return runner.Device != null;
			}
		}

		public void Run()
		{
			if (!IsRunning && CanRun)
				browser.ExecuteScript("run()");
		}

		public void Stop()
		{
			if (IsRunning)
			{
				runner.Stop();
			}
		}

		public void ShowDebugger() {

			browser.ShowDevTools();
		}

		private void SetupComplete()
		{
		}

	}
}
