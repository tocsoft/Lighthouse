using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lighthouse.Win
{
	public partial class Form1 : Form
	{
		WebKit.WebKitBrowser editer;
		WebKit.WebKitBrowser viewer;
		Control.SampleDevice _device = new Control.SampleDevice();
		string editUrl;
		string xmlToLoad;
		BlocklyDebugger debugger;
		Lighthouse.Control.JavascriptRunner runner;
		public Form1()
		{
			InitializeComponent();


			viewer = new WebKit.WebKitBrowser();
			viewer.Dock = DockStyle.Fill;
			viewer.Hide();
			panel1.Controls.Add(viewer);

			editer = new WebKit.WebKitBrowser();
			editer.Dock = DockStyle.Fill;
			panel1.Controls.Add(editer);

			debugger = new BlocklyDebugger(viewer, _device);
			runner=  new Control.JavascriptRunner(_device, debugger);
			viewer.Load += viewer_Load;

			runner.Stopped += _device_ProgramStopped;

			viewer.DocumentCompleted += viewer_DocumentCompleted;
			editer.DocumentCompleted += editer_DocumentCompleted;

			this.FormClosed += (s, e) =>
			{
				editer.Dispose();
				viewer.Dispose();
			};
		}

		void viewer_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{

			var xml = editer.GetScriptManager.CallFunction("getXml", new object[] { });
			var val = viewer.GetScriptManager.CallFunction("setupViewMode", new object[] { xml.ToString() });
			//var val = viewer.GetScriptManager.CallFunction("getCode", new object[] { });
			var code = val.ToString();

			runner.Run(code);
		}

		void editer_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			editer.GetScriptManager.CallFunction("setupEditMode", new object[] { xmlToLoad });
		}

		void viewer_Load(object sender, EventArgs e)
		{

		}

		void _device_ProgramStopped(object sender, EventArgs e)
		{

			runToolStripMenuItem.Enabled = false;

			stopToolStripMenuItem.Enabled = runner.IsRunning;
			runToolStripMenuItem.Enabled = !runner.IsRunning;
			viewer.Hide();
			editer.Show();
		}
		
		private void Form1_Load(object sender, EventArgs e)
		{
			FileInfo file = new FileInfo("blockly\\edit.html");
			editUrl = "file:///" + file.FullName.Replace("\\", "/").Replace(" ", "%20");
			editer.Navigate(editUrl);
			viewer.Navigate(editUrl);

			
			_device.Refresh();
		}

		private void runToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!runner.IsRunning)
			{

				editer.Hide();
				viewer.Show();
				viewer.Navigate(editUrl);


				stopToolStripMenuItem.Enabled = true;
				runToolStripMenuItem.Enabled = false;
				



			}
		}



		private void stopToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (runner.IsRunning)
			{
				runner.Stop();
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {

				var xml = editer.GetScriptManager.CallFunction("getXml", new object[] { });
				File.WriteAllText(saveFileDialog1.FileName, xml.ToString());
			}
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				xmlToLoad = File.ReadAllText(openFileDialog1.FileName);
				editer.Navigate(editUrl);
			}
		}



		
	}
}
