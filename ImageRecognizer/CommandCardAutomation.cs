using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

using System.Diagnostics;

namespace ImageRecognizer
{
	public class CommandCardAutomation : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblRecognitionOutput;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox imgRecognized;
		private System.Timers.Timer timer1;
		private System.Windows.Forms.Button debugButton;
		private System.Windows.Forms.GroupBox groupBox2;
		private Image currentImage;
		private bool recognizing = false;
		private System.Windows.Forms.PictureBox previewPanel;

		private CatalogManager.CardCatalog cardCatalog;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CommandCardAutomation()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			timer1.Enabled = false;
			InitializeFileSystemWatcher();
			cardCatalog = new CatalogManager.CardCatalog();

			LoadConfiguration();
		}

		private void InitializeFileSystemWatcher()
		{
			FileSystemWatcher watcher = new FileSystemWatcher(Environment.CurrentDirectory, "capture.jpg");
			watcher.Changed += new FileSystemEventHandler(watcher_Changed);
			watcher.NotifyFilter = NotifyFilters.LastWrite  | NotifyFilters.FileName ;
			watcher.EnableRaisingEvents = true;
		}

		private void LoadConfiguration()
		{
			if(! File.Exists("CardCatalog.xml"))
				MessageBox.Show("Error: Could not load CardCatalog.xml.  The application will not respond to commands.", "Could not load configuration");
			else
				cardCatalog.ReadXml("CardCatalog.xml");
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CommandCardAutomation));
			this.lblRecognitionOutput = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.previewPanel = new System.Windows.Forms.PictureBox();
			this.imgRecognized = new System.Windows.Forms.PictureBox();
			this.timer1 = new System.Timers.Timer();
			this.debugButton = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
			this.SuspendLayout();
			// 
			// lblRecognitionOutput
			// 
			this.lblRecognitionOutput.Location = new System.Drawing.Point(8, 208);
			this.lblRecognitionOutput.Name = "lblRecognitionOutput";
			this.lblRecognitionOutput.Size = new System.Drawing.Size(280, 23);
			this.lblRecognitionOutput.TabIndex = 2;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.previewPanel);
			this.groupBox1.Location = new System.Drawing.Point(2, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(288, 200);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			// 
			// previewPanel
			// 
			this.previewPanel.Location = new System.Drawing.Point(8, 16);
			this.previewPanel.Name = "previewPanel";
			this.previewPanel.Size = new System.Drawing.Size(272, 176);
			this.previewPanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.previewPanel.TabIndex = 0;
			this.previewPanel.TabStop = false;
			// 
			// imgRecognized
			// 
			this.imgRecognized.Location = new System.Drawing.Point(306, 16);
			this.imgRecognized.Name = "imgRecognized";
			this.imgRecognized.Size = new System.Drawing.Size(264, 176);
			this.imgRecognized.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.imgRecognized.TabIndex = 5;
			this.imgRecognized.TabStop = false;
			// 
			// timer1
			// 
			this.timer1.Interval = 4000;
			this.timer1.SynchronizingObject = this;
			this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
			// 
			// debugButton
			// 
			this.debugButton.Location = new System.Drawing.Point(496, 240);
			this.debugButton.Name = "debugButton";
			this.debugButton.TabIndex = 6;
			this.debugButton.Text = "Debug";
			this.debugButton.Click += new System.EventHandler(this.debugButton_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(296, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(280, 200);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			// 
			// CommandCardAutomation
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 277);
			this.Controls.Add(this.debugButton);
			this.Controls.Add(this.imgRecognized);
			this.Controls.Add(this.lblRecognitionOutput);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CommandCardAutomation";
			this.Text = "Image Recognition";
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new CommandCardAutomation());
		}

		private void Recognize()
		{
			if(currentImage == null)
				return;

			try
			{
				recognizing = true;
				using(Image recognitionImage = CommandCardRecognition.Clean(currentImage, false, imgRecognized))
				{
					if(recognitionImage != null)
					{
						if(imgRecognized.Image != null)
							imgRecognized.Image.Dispose();
						imgRecognized.Image = (Image) recognitionImage.Clone();
						this.Refresh();
					}

					using(Bitmap tempBitmap = new Bitmap(recognitionImage))
					{
						double width = tempBitmap.Width;
						double height = tempBitmap.Height;
						double horizontalResolution = 10;
						double verticalResolution = 2;
						double horizontalIncrement = width / horizontalResolution;
						double verticalIncrement = height / verticalResolution;
						double sampleRadius = 10;

						int recognizedNumber = 0;
						int currentDigit = 19;
						string oldRecognition = lblRecognitionOutput.Text;
						lblRecognitionOutput.Text = "";
						for(double yPos = (verticalIncrement / 2); yPos < height; yPos += verticalIncrement)
						{
							for(double xPos = (horizontalIncrement / 2); xPos < width; xPos += horizontalIncrement)
							{
								if(CommandCardRecognition.GetSpreadSample(xPos, yPos, sampleRadius, tempBitmap) < 0.5)
								{
									recognizedNumber += (int) Math.Pow(2, currentDigit);
									lblRecognitionOutput.Text += "1";
								}
								else
									lblRecognitionOutput.Text += "0";

								currentDigit--;
							}
						}

						lblRecognitionOutput.Text += ": " + recognizedNumber;

						// There was a change
						if(lblRecognitionOutput.Text != oldRecognition)
						{
							string filename = "";

							DataRow[] foundCards = 
								cardCatalog.Tables["CommandCards"].Select("Id = " + recognizedNumber);

							if(foundCards.Length > 0)
							{
								CatalogManager.CardCatalog.CommandCard foundCard = 
									(CatalogManager.CardCatalog.CommandCard) foundCards[0];
								filename = foundCard.Command;

								try
								{
									Process p = new Process();
									ProcessStartInfo startInfo = new ProcessStartInfo();
									startInfo.UseShellExecute = true;
									startInfo.FileName = filename;
									p.StartInfo = startInfo;
									p.Start();
								}
								catch(Exception e)
								{
									MessageBox.Show("Error starting " + filename + ": " + e.Message);
								}
							}
						}
					}
				}
			}
			catch { }
			finally { recognizing = false; }
		}

		private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			StartRecognition();
		}

		private void StartRecognition()
		{
			if(! recognizing)
			{
				try
				{
					using (Image camImage = Image.FromStream(new FileStream(@"capture.jpg", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
					{
						if(currentImage != null)
							currentImage.Dispose();
						currentImage = (Image) camImage.Clone();
						if(previewPanel != null)
							previewPanel.Image = currentImage;
					}

					Recognize();
					this.Refresh();
				}
				catch {}
			}
		}

		private void debugButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				CommandCardRecognition.Clean(currentImage, true, imgRecognized);
			}
			catch {}
		}

		private void watcher_Changed(object sender, FileSystemEventArgs e)
		{
			StartRecognition();
		}
	}
}
