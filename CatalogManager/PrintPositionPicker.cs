using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace CatalogManager
{
	/// <summary>
	/// Summary description for PrintPositionPicker.
	/// </summary>
	public class PrintPositionPicker : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label promptLabel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Panel panelPick02;
		private System.Windows.Forms.Panel panelPick01;
		private System.Windows.Forms.Panel panelPick11;
		private System.Windows.Forms.Panel panelPick00;
		private System.Windows.Forms.Panel panelPick10;
		private System.Windows.Forms.Panel panelPick04;
		private System.Windows.Forms.Panel panelPick14;
		private System.Windows.Forms.Panel panelPick03;
		private System.Windows.Forms.Panel panelPick13;
		private System.Windows.Forms.Panel panelPick;
		private System.Windows.Forms.Panel panelPick12;

		private Panel chosenPanel;
		
		private int xPosition, yPosition;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PrintPositionPicker()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			xPosition = 0; yPosition = 0;
			chosenPanel = panelPick00;
			HighlightCurrent();
			
			panelPick00.Click += new EventHandler(PanelPick00_Click);
			panelPick10.Click += new EventHandler(PanelPick10_Click);
			panelPick01.Click += new EventHandler(PanelPick01_Click);
			panelPick11.Click += new EventHandler(PanelPick11_Click);
			panelPick02.Click += new EventHandler(PanelPick02_Click);
			panelPick12.Click += new EventHandler(PanelPick12_Click);
			panelPick03.Click += new EventHandler(PanelPick03_Click);
			panelPick13.Click += new EventHandler(PanelPick13_Click);
			panelPick04.Click += new EventHandler(PanelPick04_Click);
			panelPick14.Click += new EventHandler(PanelPick14_Click);
		}

		public int XPosition { get { return xPosition; } }
		public int YPosition { get { return yPosition; } }

		private void HighlightCurrent()
		{
			chosenPanel.BackColor = Color.White;

			if((xPosition == 0) && (yPosition == 0))
				chosenPanel = panelPick00;
			if((xPosition == 1) && (yPosition == 0))
				chosenPanel = panelPick10;
			if((xPosition == 0) && (yPosition == 1))
				chosenPanel = panelPick01;
			if((xPosition == 1) && (yPosition == 1))
				chosenPanel = panelPick11;
			if((xPosition == 0) && (yPosition == 2))
				chosenPanel = panelPick02;
			if((xPosition == 1) && (yPosition == 2))
				chosenPanel = panelPick12;
			if((xPosition == 0) && (yPosition == 3))
				chosenPanel = panelPick03;
			if((xPosition == 1) && (yPosition == 3))
				chosenPanel = panelPick13;
			if((xPosition == 0) && (yPosition == 4))
				chosenPanel = panelPick04;
			if((xPosition == 1) && (yPosition == 4))
				chosenPanel = panelPick14;

			chosenPanel.BackColor = Color.FromArgb(240, 240, 240);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.promptLabel = new System.Windows.Forms.Label();
			this.panelPick = new System.Windows.Forms.Panel();
			this.panelPick04 = new System.Windows.Forms.Panel();
			this.panelPick14 = new System.Windows.Forms.Panel();
			this.panelPick03 = new System.Windows.Forms.Panel();
			this.panelPick13 = new System.Windows.Forms.Panel();
			this.panelPick02 = new System.Windows.Forms.Panel();
			this.panelPick12 = new System.Windows.Forms.Panel();
			this.panelPick01 = new System.Windows.Forms.Panel();
			this.panelPick11 = new System.Windows.Forms.Panel();
			this.panelPick00 = new System.Windows.Forms.Panel();
			this.panelPick10 = new System.Windows.Forms.Panel();
			this.okButton = new System.Windows.Forms.Button();
			this.panelPick.SuspendLayout();
			this.SuspendLayout();
			// 
			// promptLabel
			// 
			this.promptLabel.Location = new System.Drawing.Point(64, 8);
			this.promptLabel.Name = "promptLabel";
			this.promptLabel.Size = new System.Drawing.Size(168, 23);
			this.promptLabel.TabIndex = 0;
			this.promptLabel.Text = "Start Printing at Which Card?";
			// 
			// panelPick
			// 
			this.panelPick.BackColor = System.Drawing.Color.White;
			this.panelPick.Controls.Add(this.panelPick04);
			this.panelPick.Controls.Add(this.panelPick14);
			this.panelPick.Controls.Add(this.panelPick03);
			this.panelPick.Controls.Add(this.panelPick13);
			this.panelPick.Controls.Add(this.panelPick02);
			this.panelPick.Controls.Add(this.panelPick12);
			this.panelPick.Controls.Add(this.panelPick01);
			this.panelPick.Controls.Add(this.panelPick11);
			this.panelPick.Controls.Add(this.panelPick00);
			this.panelPick.Controls.Add(this.panelPick10);
			this.panelPick.Location = new System.Drawing.Point(40, 32);
			this.panelPick.Name = "panelPick";
			this.panelPick.Size = new System.Drawing.Size(200, 288);
			this.panelPick.TabIndex = 1;
			// 
			// panelPick04
			// 
			this.panelPick04.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelPick04.Location = new System.Drawing.Point(8, 232);
			this.panelPick04.Name = "panelPick04";
			this.panelPick04.Size = new System.Drawing.Size(88, 48);
			this.panelPick04.TabIndex = 11;
			// 
			// panelPick14
			// 
			this.panelPick14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelPick14.Location = new System.Drawing.Point(104, 232);
			this.panelPick14.Name = "panelPick14";
			this.panelPick14.Size = new System.Drawing.Size(88, 48);
			this.panelPick14.TabIndex = 12;
			// 
			// panelPick03
			// 
			this.panelPick03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelPick03.Location = new System.Drawing.Point(8, 176);
			this.panelPick03.Name = "panelPick03";
			this.panelPick03.Size = new System.Drawing.Size(88, 48);
			this.panelPick03.TabIndex = 9;
			// 
			// panelPick13
			// 
			this.panelPick13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelPick13.Location = new System.Drawing.Point(104, 176);
			this.panelPick13.Name = "panelPick13";
			this.panelPick13.Size = new System.Drawing.Size(88, 48);
			this.panelPick13.TabIndex = 10;
			// 
			// panelPick02
			// 
			this.panelPick02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelPick02.Location = new System.Drawing.Point(8, 120);
			this.panelPick02.Name = "panelPick02";
			this.panelPick02.Size = new System.Drawing.Size(88, 48);
			this.panelPick02.TabIndex = 7;
			// 
			// panelPick12
			// 
			this.panelPick12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelPick12.Location = new System.Drawing.Point(104, 120);
			this.panelPick12.Name = "panelPick12";
			this.panelPick12.Size = new System.Drawing.Size(88, 48);
			this.panelPick12.TabIndex = 8;
			// 
			// panelPick01
			// 
			this.panelPick01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelPick01.Location = new System.Drawing.Point(8, 64);
			this.panelPick01.Name = "panelPick01";
			this.panelPick01.Size = new System.Drawing.Size(88, 48);
			this.panelPick01.TabIndex = 5;
			// 
			// panelPick11
			// 
			this.panelPick11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelPick11.Location = new System.Drawing.Point(104, 64);
			this.panelPick11.Name = "panelPick11";
			this.panelPick11.Size = new System.Drawing.Size(88, 48);
			this.panelPick11.TabIndex = 6;
			// 
			// panelPick00
			// 
			this.panelPick00.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelPick00.Location = new System.Drawing.Point(8, 8);
			this.panelPick00.Name = "panelPick00";
			this.panelPick00.Size = new System.Drawing.Size(88, 48);
			this.panelPick00.TabIndex = 3;
			// 
			// panelPick10
			// 
			this.panelPick10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelPick10.Location = new System.Drawing.Point(104, 8);
			this.panelPick10.Name = "panelPick10";
			this.panelPick10.Size = new System.Drawing.Size(88, 48);
			this.panelPick10.TabIndex = 4;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(208, 336);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 2;
			this.okButton.Text = "Ok";
			// 
			// PrintPositionPicker
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 365);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.panelPick);
			this.Controls.Add(this.promptLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "PrintPositionPicker";
			this.Text = "Print Configuration";
			this.panelPick.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void PanelPick00_Click(object sender, EventArgs e)
		{
			this.xPosition = 0;
			this.yPosition = 0;
			HighlightCurrent();
			this.Refresh();
		}
		private void PanelPick10_Click(object sender, EventArgs e)
		{
			this.xPosition = 1;
			this.yPosition = 0;
			HighlightCurrent();
			this.Refresh();
		}
		private void PanelPick01_Click(object sender, EventArgs e)
		{
			this.xPosition = 0;
			this.yPosition = 1;
			HighlightCurrent();
			this.Refresh();
		}
		private void PanelPick11_Click(object sender, EventArgs e)
		{
			this.xPosition = 1;
			this.yPosition = 1;
			HighlightCurrent();
			this.Refresh();
		}
		private void PanelPick02_Click(object sender, EventArgs e)
		{
			this.xPosition = 0;
			this.yPosition = 2;
			HighlightCurrent();
			this.Refresh();
		}
		private void PanelPick12_Click(object sender, EventArgs e)
		{
			this.xPosition = 1;
			this.yPosition = 2;
			HighlightCurrent();
			this.Refresh();
		}
		private void PanelPick03_Click(object sender, EventArgs e)
		{
			this.xPosition = 0;
			this.yPosition = 3;
			HighlightCurrent();
			this.Refresh();
		}
		private void PanelPick13_Click(object sender, EventArgs e)
		{
			this.xPosition = 1;
			this.yPosition = 3;
			HighlightCurrent();
			this.Refresh();
		}
		private void PanelPick04_Click(object sender, EventArgs e)
		{
			this.xPosition = 0;
			this.yPosition = 4;
			HighlightCurrent();
			this.Refresh();
		}
		private void PanelPick14_Click(object sender, EventArgs e)
		{
			this.xPosition = 1;
			this.yPosition = 4;
			HighlightCurrent();
			this.Refresh();
		}

	}
}
