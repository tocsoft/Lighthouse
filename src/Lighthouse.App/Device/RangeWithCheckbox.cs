using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lighthouse.App.Device
{
	public partial class RangeWithCheckbox : UserControl
	{
		public RangeWithCheckbox()
		{
			InitializeComponent();
			numericUpDown1.ValueChanged+=numericUpDown1_ValueChanged;
			label1.CheckedChanged += label1_CheckedChanged;
		}

		void label1_CheckedChanged(object sender, EventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged.Invoke(this, e);
		}

		void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged.Invoke(this, e);
		}

		public int Value {
			get {
				return (int)numericUpDown1.Value;
			}
			set{
				numericUpDown1.Value = value;
			}
		}
		public override string Text
		{
			get
			{
				return label1.Text;
			}
			set
			{
				label1.Text = value;
			}
		}

		public bool Checked {
			get {
				return label1.Checked;
			}
			set {
				label1.Checked = value;
			}
		}
		public event EventHandler ValueChanged;
	}

}
