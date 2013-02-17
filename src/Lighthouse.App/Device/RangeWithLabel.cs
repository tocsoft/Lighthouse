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
	public partial class RangeWithLabel : UserControl
	{
		public RangeWithLabel()
		{
			InitializeComponent();
			numericUpDown1.ValueChanged+=numericUpDown1_ValueChanged;
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
		public event EventHandler ValueChanged;
	}

}
