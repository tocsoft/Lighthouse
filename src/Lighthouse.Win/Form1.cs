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
		string viewerUrl;
		string editUrl;
		string xmlToLoad;
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

			viewer.Load += viewer_Load;

			_device.ProgramStopped += _device_ProgramStopped;

			viewer.DocumentCompleted += viewer_DocumentCompleted;
			editer.DocumentCompleted += editer_DocumentCompleted;


		}

		void viewer_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{

			var xml = editer.GetScriptManager.CallFunction("getXml", new object[] { });
			viewer.GetScriptManager.CallFunction("loadXml", new object[] { xml.ToString() });
		}
		void editer_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{

			editer.GetScriptManager.CallFunction("loadXml", new object[] { xmlToLoad });
		}

		void viewer_Load(object sender, EventArgs e)
		{

		}

		void _device_ProgramStopped(object sender, EventArgs e)
		{

			runToolStripMenuItem.Enabled = false;

			stopToolStripMenuItem.Enabled = _device.IsProgramRunning;
			runToolStripMenuItem.Enabled = !_device.IsProgramRunning;
			viewer.Hide();
			editer.Show();
		}
		
		private void Form1_Load(object sender, EventArgs e)
		{
			FileInfo file = new FileInfo("blockly\\edit.html");
			editUrl = "file:///" + file.FullName.Replace("\\", "/").Replace(" ", "%20");
			editer.Navigate(editUrl);

			 file = new FileInfo("blockly\\view.html");
			 viewerUrl = "file:///" + file.FullName.Replace("\\", "/").Replace(" ", "%20");
			 viewer.Navigate(viewerUrl);

			
			_device.Refresh();
		}

		private void runToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!_device.IsProgramRunning)
			{

				editer.Hide();
				viewer.Show();
				viewer.Navigate(viewerUrl);

				stopToolStripMenuItem.Enabled = true;
				runToolStripMenuItem.Enabled = false;
				var val = editer.GetScriptManager.CallFunction("getCode", new object[] { });
				var code = val.ToString();
				_device.RunProgram(code);



			}
		}

		private void stopToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_device.IsProgramRunning) {
				_device.StopProgram();
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
