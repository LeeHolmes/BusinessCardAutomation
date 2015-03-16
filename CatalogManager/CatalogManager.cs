using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Printing;

namespace CatalogManager
{
	public class CatalogUI : System.Windows.Forms.Form
	{
		private System.Windows.Forms.DataGrid cardCatalog;
		private CatalogManager.CardCatalog cardCatalog1;
		private System.Windows.Forms.DataGridTableStyle CommandCards;
		private System.Windows.Forms.DataGridTextBoxColumn idTextBoxColumn;
		private System.Windows.Forms.DataGridTextBoxColumn descTextBoxColumn;
		private System.Windows.Forms.DataGridTextBoxColumn commandTextBoxColumn;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button printButton;

		private PrintDocument printDoc;
		PrintPositionPicker picker;
		private int currentPrintPage;
		private int currentPrintRow;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CatalogUI()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			ResizeColumns();
			
			this.Resize += new EventHandler(CatalogManager_Resize);
			cardCatalog1.Tables["CommandCards"].ColumnChanging += new DataColumnChangeEventHandler(CatalogUI_ColumnChanging);
			cardCatalog1.Tables["CommandCards"].RowChanging += new DataRowChangeEventHandler(CatalogUI_RowChanging);

			if(System.IO.File.Exists("CardCatalog.xml"))
				cardCatalog1.ReadXml("CardCatalog.xml");
			cardCatalog.Refresh();

			printDoc = new PrintDocument();
			picker = new PrintPositionPicker();

			printDoc.PrintPage += new PrintPageEventHandler(PrintDoc_PrintPage);
		}

		private void ResizeColumns()
		{
			int widthSum = (idTextBoxColumn.Width + descTextBoxColumn.Width);
			if(widthSum < (cardCatalog.Width - 40))
				commandTextBoxColumn.Width = cardCatalog.Width - widthSum - 40; 
			else
				commandTextBoxColumn.Width = 300;
			cardCatalog.Refresh();
		}

		private void CatalogManager_Resize(object sender, EventArgs e)
		{
			ResizeColumns();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CatalogUI));
			this.cardCatalog = new System.Windows.Forms.DataGrid();
			this.cardCatalog1 = new CatalogManager.CardCatalog();
			this.CommandCards = new System.Windows.Forms.DataGridTableStyle();
			this.idTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
			this.descTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
			this.commandTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
			this.saveButton = new System.Windows.Forms.Button();
			this.printButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.cardCatalog)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cardCatalog1)).BeginInit();
			this.SuspendLayout();
			// 
			// cardCatalog
			// 
			this.cardCatalog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.cardCatalog.BackgroundColor = System.Drawing.Color.LightGray;
			this.cardCatalog.DataMember = "";
			this.cardCatalog.DataSource = this.cardCatalog1.CommandCards;
			this.cardCatalog.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.cardCatalog.Location = new System.Drawing.Point(8, 8);
			this.cardCatalog.Name = "cardCatalog";
			this.cardCatalog.PreferredColumnWidth = 100;
			this.cardCatalog.Size = new System.Drawing.Size(552, 220);
			this.cardCatalog.TabIndex = 0;
			this.cardCatalog.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																									this.CommandCards});
			// 
			// cardCatalog1
			// 
			this.cardCatalog1.DataSetName = "CardCatalog";
			this.cardCatalog1.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// CommandCards
			// 
			this.CommandCards.AlternatingBackColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(200)), ((System.Byte)(220)));
			this.CommandCards.DataGrid = this.cardCatalog;
			this.CommandCards.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																										   this.idTextBoxColumn,
																										   this.descTextBoxColumn,
																										   this.commandTextBoxColumn});
			this.CommandCards.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.CommandCards.MappingName = "CommandCards";
			// 
			// idTextBoxColumn
			// 
			this.idTextBoxColumn.Format = "";
			this.idTextBoxColumn.FormatInfo = null;
			this.idTextBoxColumn.HeaderText = "Id";
			this.idTextBoxColumn.MappingName = "Id";
			this.idTextBoxColumn.Width = 60;
			// 
			// descTextBoxColumn
			// 
			this.descTextBoxColumn.Format = "";
			this.descTextBoxColumn.FormatInfo = null;
			this.descTextBoxColumn.HeaderText = "Description";
			this.descTextBoxColumn.MappingName = "Description";
			this.descTextBoxColumn.Width = 150;
			// 
			// commandTextBoxColumn
			// 
			this.commandTextBoxColumn.Format = "";
			this.commandTextBoxColumn.FormatInfo = null;
			this.commandTextBoxColumn.HeaderText = "Command";
			this.commandTextBoxColumn.MappingName = "Command";
			this.commandTextBoxColumn.Width = 300;
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(486, 240);
			this.saveButton.Name = "saveButton";
			this.saveButton.TabIndex = 1;
			this.saveButton.Text = "Save";
			this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// printButton
			// 
			this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.printButton.Location = new System.Drawing.Point(376, 240);
			this.printButton.Name = "printButton";
			this.printButton.Size = new System.Drawing.Size(88, 23);
			this.printButton.TabIndex = 2;
			this.printButton.Text = "Print Selected";
			this.printButton.Click += new System.EventHandler(this.PrintButton_Click);
			// 
			// CatalogUI
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(568, 269);
			this.Controls.Add(this.printButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.cardCatalog);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CatalogUI";
			this.Text = "Command Card Catalog Management";
			((System.ComponentModel.ISupportInitialize)(this.cardCatalog)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cardCatalog1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new CatalogUI());
		}

		private void SaveButton_Click(object sender, System.EventArgs e)
		{
			cardCatalog1.WriteXml("CardCatalog.xml");			
		}

		private void CatalogUI_ColumnChanging(object sender, DataColumnChangeEventArgs e)
		{
			if(e.Column.ColumnName == "Id")
			{
				UInt64 id = (UInt64) e.ProposedValue;

				if(((int) id > ((int) (Math.Pow(2, 20) - 1))) || (id < 0))
				{
					e.Row.RowError = "The Id column contains an error";
					e.Row.SetColumnError(e.Column, "Id must be less than " + ((int) (Math.Pow(2, 20) - 1)));
					return;
				}

				foreach(CardCatalog.CommandCard currentCard in cardCatalog1.Tables["CommandCards"].Rows)
				{
					if(currentCard.Id == id)
					{
						e.Row.RowError = "The Id column contains an error";
						e.Row.SetColumnError(e.Column, "Id " + id + " already exists.");
						return;
					}
				}
			}
		}

		private void CatalogUI_RowChanging(object sender, DataRowChangeEventArgs e)
		{
			if(e.Row.HasErrors)
			{
				throw new Exception("This row contains errors.");
			}
		}

		private void PrintButton_Click(object sender, System.EventArgs e)
		{
			this.currentPrintPage = 1;
			this.currentPrintRow = 0;

			DialogResult result = picker.ShowDialog();

			if(result == DialogResult.OK)
			{
//				PrintPreviewDialog dlg = new PrintPreviewDialog();
//				dlg.Document = printDoc;
//				dlg.ShowDialog();

				printDoc.Print();
			}
		}

		private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
		{
			Graphics printGraphics = e.Graphics;
			PrinterBounds printerBounds = new PrinterBounds(e);
			printGraphics.PageUnit = GraphicsUnit.Pixel;

			int topMargin = (int) ((0.5 - ((double) printerBounds.HardMarginTop / 100)) * e.PageSettings.PrinterResolution.Y) ;
			int leftMargin = (int) ((0.75 - ((double) printerBounds.HardMarginLeft / 100)) * e.PageSettings.PrinterResolution.X);
			int cardWidth = (int) (3.5 * e.PageSettings.PrinterResolution.X);
			int cardHeight = (int) (2 * e.PageSettings.PrinterResolution.Y);
			
			int horizontalPosition = 0;
			int verticalPosition = 0;

			if(this.currentPrintPage == 1)
			{
				horizontalPosition = picker.XPosition;
				verticalPosition = picker.YPosition;
			}

			e.HasMorePages = false;
			for(int currentRow = this.currentPrintRow; currentRow < cardCatalog1.Tables["CommandCards"].Rows.Count; currentRow++)
			{
				if(cardCatalog.IsSelected(currentRow))
				{
					CatalogManager.CardCatalog.CommandCard currentCard =
						(CatalogManager.CardCatalog.CommandCard) cardCatalog1.Tables["CommandCards"].Rows[currentRow];

					string description = currentCard.Description;
					UInt64 id = currentCard.Id;

					PrintCard(description, (int) id, 
						printGraphics, topMargin, 
						leftMargin, cardWidth, 
						cardHeight, horizontalPosition, 
						verticalPosition);

					horizontalPosition++;
					if(horizontalPosition > 1)
					{
						verticalPosition++;
						horizontalPosition = 0;
						if(verticalPosition > 4)
						{
							// We need to move on to another page
							this.currentPrintPage++;
							this.currentPrintRow = (currentRow + 1);

							// But make sure more are selected
							bool foundMore = false;
							currentRow++;
							while(currentRow < cardCatalog1.Tables["CommandCards"].Rows.Count)
							{
								if(cardCatalog.IsSelected(currentRow))
								{
									foundMore = true;
									break;
								}
								currentRow++;
							}
							if(foundMore)
								e.HasMorePages = true;

							break;
						}
					}
				}
			}
		}

		// Print a specific card to a specific position on paper
		private void PrintCard(string description, int id, 
			Graphics printGraphics, int topMargin, 
			int leftMargin, int cardWidth, 
			int cardHeight, int horizontalPosition, 
			int verticalPosition)
		{
			int hOffset = leftMargin + (horizontalPosition * cardWidth) + 80;
			int vOffset = topMargin + (verticalPosition * cardHeight) + 80;

			// Draw the text
			String textToPrint = description;
			Font printFont = new Font("Arial Black", 9);
			SizeF stringLength = printGraphics.MeasureString(textToPrint, printFont);

			// Trim, if necessary
			while(stringLength.Width > (cardWidth - 160 - 40))
			{
				textToPrint = textToPrint.Remove(textToPrint.Length - 6, 6) + "...";
				stringLength = printGraphics.MeasureString(textToPrint, printFont);
			}

			int textLeft = hOffset + 20 + (int) (((cardWidth - 160 - 20) - stringLength.Width) / 2);
			int textTop = 100 + vOffset;
			printGraphics.DrawString(textToPrint, printFont, Brushes.Black, textLeft, textTop);

			// Draw the box
			Pen pen = new Pen(Color.Black, 40);
			printGraphics.DrawRectangle(pen, hOffset, vOffset, cardWidth - 160, cardHeight - 160);
			printGraphics.DrawLine(pen, hOffset, vOffset + 300, hOffset + cardWidth - 160, vOffset + 300);

			// Draw the code
			int xPos = 0;
			int yPos = 0;
			int squareSize = ((cardWidth - 160) / 10);
			int vBitOffset = 300 + (((cardHeight - 160 - 300) - (2 * squareSize)) / 2);
			int hBitOffset = 0;

			for(int currentBit = 19; currentBit >= 0; currentBit--)
			{
				if((id & (int) Math.Pow(2, currentBit)) == (int) Math.Pow(2, currentBit))
				{
					printGraphics.FillRectangle(new SolidBrush(Color.Black), 
						(xPos * squareSize) + hOffset + hBitOffset,
						(yPos * squareSize) + vOffset + vBitOffset,
						squareSize,
						squareSize);
				}
				xPos++;
				if(xPos >= 10)
				{
					yPos++;
					xPos = 0;
				}
			}
		}
	}
}
