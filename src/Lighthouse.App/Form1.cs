using CefSharp;
using CefSharp.WinForms;
using Lighthouse.App.blockly;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;


namespace Lighthouse.App
{
	

	public partial class Form1 : Form
	{
		Editor editor = new Editor();


		public Form1()
		{
			InitializeComponent();
			editor.Dock = DockStyle.Fill;
			splitContainer1.Panel1.Controls.Add(editor);
			editor.Stopped += editor_Stopped;
			editor.Device = new Lighthouse.Control.AutoDevice();
		}

		void editor_Stopped(object sender, EventArgs e)
		{
			if (!this.IsDisposed)
			{
				this.Invoke((MethodInvoker)delegate
				{
					stopToolStripMenuItem1.Visible = false;
					runToolStripMenuItem1.Visible = true;
				});
			}
		}


		protected override void OnClosed(EventArgs e)
		{
			editor.Shutdown();
			base.OnClosed(e);
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			devicePreview1.Device = editor.Device;
		}

		private void runToolStripMenuItem_Click(object sender, EventArgs e)
		{
			editor.Run();
		}

		private void stopToolStripMenuItem_Click(object sender, EventArgs e)
		{
			editor.Stop();
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			editor.ShowDebugger();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{

			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

				editor.Xml = File.ReadAllText(openFileDialog1.FileName);

			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

				File.WriteAllText(saveFileDialog1.FileName, editor.Xml);

			}
		}

		private void runToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			stopToolStripMenuItem1.Visible = true;
			runToolStripMenuItem1.Visible = false;
			editor.Run();
		}

		private void stopToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			editor.Stop();
		}



	}
}
