/*
 * oIo — 11/21/2010 — 10:25 AM — http://msdn.microsoft.com/en-us/library/ms404304.aspx
 */
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Cor3.Forms.Controls
{
	[System.Drawing.ToolboxBitmapAttribute(typeof(System.Windows.Forms.NumericUpDown))]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip|ToolStripItemDesignerAvailability.MenuStrip)]
	public class ToolStripUpDown : ToolStripControlHost
	{
		public decimal MaxValue
		{
			get { return this.NumericUpDownControl.Maximum; }
			set { this.NumericUpDownControl.Maximum = value; }
		}
		public decimal MinValue
		{
			get { return this.NumericUpDownControl.Minimum; }
			set { this.NumericUpDownControl.Minimum = value; }
		}
		public decimal Value
		{
			get { return NumericUpDownControl.Value; }
			set { NumericUpDownControl.Value = value; }
		}
		public NumericUpDown NumericUpDownControl {
			get { return base.Control as NumericUpDown; }
		}
		
		public new bool AutoSize
		{
			get { return NumericUpDownControl.AutoSize; }
			set { base.AutoSize = NumericUpDownControl.AutoSize = value; }
		}

		public new HorizontalAlignment TextAlign
		{
			get { return NumericUpDownControl.TextAlign; }
			set { NumericUpDownControl.TextAlign = value; }
		}
		public bool ThousandsSeparator
		{
			get { return NumericUpDownControl.ThousandsSeparator; }
			set { NumericUpDownControl.ThousandsSeparator = value; }
		}
		public event EventHandler UpDownTextChanged
		{
			add { NumericUpDownControl.TextChanged += value; }
			remove { NumericUpDownControl.TextChanged -= value; }
		}
		public event EventHandler ValueChanged
		{
			add { NumericUpDownControl.ValueChanged += value; }
			remove { NumericUpDownControl.ValueChanged -= value; }
		}
		public ToolStripUpDown() : base(new NumericUpDown())
		{
			
		}
	}
}
