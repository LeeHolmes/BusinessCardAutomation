using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace PlaylistGenerator
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class PlaylistGenerator : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button generate;
		private ArrayList musicDirectories;
		private System.Windows.Forms.ListBox generatedPlaylists;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label id;
		private System.Windows.Forms.TextBox idText;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PlaylistGenerator()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			musicDirectories = new ArrayList();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PlaylistGenerator));
			this.generate = new System.Windows.Forms.Button();
			this.generatedPlaylists = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.id = new System.Windows.Forms.Label();
			this.idText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// generate
			// 
			this.generate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.generate.Location = new System.Drawing.Point(344, 240);
			this.generate.Name = "generate";
			this.generate.TabIndex = 1;
			this.generate.Text = "Generate";
			this.generate.Click += new System.EventHandler(this.generate_Click);
			// 
			// generatedPlaylists
			// 
			this.generatedPlaylists.AllowDrop = true;
			this.generatedPlaylists.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.generatedPlaylists.Location = new System.Drawing.Point(16, 32);
			this.generatedPlaylists.Name = "generatedPlaylists";
			this.generatedPlaylists.Size = new System.Drawing.Size(400, 186);
			this.generatedPlaylists.TabIndex = 2;
			this.generatedPlaylists.DragDrop += new System.Windows.Forms.DragEventHandler(this.generatedPlaylists_DragDrop);
			this.generatedPlaylists.DragEnter += new System.Windows.Forms.DragEventHandler(this.generatedPlaylists_DragEnter);
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label1.Location = new System.Drawing.Point(88, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(248, 23);
			this.label1.TabIndex = 3;
			this.label1.Text = "Drop directories here to add Playlist Commands";
			// 
			// id
			// 
			this.id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.id.Location = new System.Drawing.Point(152, 244);
			this.id.Name = "id";
			this.id.Size = new System.Drawing.Size(72, 16);
			this.id.TabIndex = 4;
			this.id.Text = "Starting ID:";
			// 
			// idText
			// 
			this.idText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.idText.Location = new System.Drawing.Point(224, 240);
			this.idText.Name = "idText";
			this.idText.TabIndex = 5;
			this.idText.Text = "1";
			// 
			// PlaylistGenerator
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 273);
			this.Controls.Add(this.idText);
			this.Controls.Add(this.id);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.generatedPlaylists);
			this.Controls.Add(this.generate);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "PlaylistGenerator";
			this.Text = "Playlist Generator";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new PlaylistGenerator());
		}

		private void generatedPlaylists_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop, false))
				e.Effect = DragDropEffects.All;
		}

		private void generatedPlaylists_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);

			foreach( string file in files )
				ParseDirectory(file);
		}

		private void ParseDirectory(string directory)
		{
			if(! Directory.Exists(directory))
				return;

			// Parse the subdirectories for playlists
			if(! generatedPlaylists.Items.Contains(directory))
			{
				foreach(string subDirectory in Directory.GetDirectories(directory))
					ParseDirectory(subDirectory);
			}

			// Check if the directory has music directly in it.
			// If so, add it to the "playlists to be created"
			if(GetMediaFiles(directory).Count > 0)
			{
				if(! generatedPlaylists.Items.Contains(directory.ToUpper()))
					generatedPlaylists.Items.Add(directory.ToUpper());
			}
		}

		private void generate_Click(object sender, System.EventArgs e)
		{
			int id = -1;
			try
			{
				id = Int32.Parse(idText.Text);
			}
			catch { id = -1; }

			if((id > (int) (Math.Pow(2, 21) - 1)) || (id < 0))
			{
				MessageBox.Show("Starting ID must be greater than zero, and less than " + (int) (Math.Pow(2, 21) - 1), "Invalid ID");
				return;
			}

			string longestCommonPrefix = GetLongestCommonPrefix(generatedPlaylists.Items);
			GeneratePlaylists(longestCommonPrefix, generatedPlaylists.Items, id);
			
			MessageBox.Show("These directories have now been exported as playlists to CardCatalog_PlaylistExport.xml.  Import them to your catalog by pasting the XML into CardCatalog.xml", "Generation Complete.");
		}

		private void GeneratePlaylists(string longestCommonPrefix, IList items, int startingId)
		{
			if(! Directory.Exists("Helpers"))
				Directory.CreateDirectory("Helpers");
			if(! Directory.Exists(@"Helpers\Playlists"))
				Directory.CreateDirectory(@"Helpers\Playlists");

			int id = startingId;
			string exportXml = "";
			string cardFormat = @"  <CommandCards>
    <Id>{0}</Id>
    <Description>{1}</Description>
    <Command>{2}</Command>
  </CommandCards>
";

			System.Xml.XmlDocument document = new System.Xml.XmlDocument();
			System.Xml.XmlText friendlyXml = null;
			System.Xml.XmlText playlistXml = null;
			foreach(string currentDirectory in items)
			{
				string friendlyName = currentDirectory.Remove(0, longestCommonPrefix.Length);
				friendlyName = friendlyName.TrimStart('\\');
				friendlyName = friendlyName.Replace(@"\", " - ");
				string playlistName = @"Helpers\Playlists\" + friendlyName + ".wpl";
				friendlyXml = document.CreateTextNode(friendlyName);
				playlistXml = document.CreateTextNode(playlistName);

				GeneratePlaylist(friendlyName, playlistName, currentDirectory);
				exportXml += String.Format(
					cardFormat, 
					id, 
					friendlyXml.OuterXml, 
					playlistXml.OuterXml);
				id++;
			}

			string filename = "CardCatalog_PlaylistExport.xml";
			// Prompt them to overwrite the playlist if it exists
			if(File.Exists(filename))
			{
				DialogResult result = MessageBox.Show("Exported Catalog " + filename + " exists.  Overwrite?", 
					"Exported Catalog Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if(result != DialogResult.Yes)
					return;
			}

			if(File.Exists(filename))
				File.Delete(filename);
			using(TextWriter outputFile = File.CreateText(filename))
			{
				outputFile.Write(exportXml);
			}
		}

		private void GeneratePlaylist(string friendlyName, string filename, string fileDirectory)
		{
			// Prompt them to overwrite the playlist if it exists
			if(File.Exists(filename))
			{
				DialogResult result = MessageBox.Show("Playlist " + filename + " exists.  Overwrite?", 
					"Playlist Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if(result != DialogResult.Yes)
					return;
			}

			string playlistTemplate = @"<?wpl version=""1.0""?>
<smil>
	<head>
		<meta name=""Generator"" content=""PlaylistGenerator""/>
		<author/>
		<title>{0}</title>
    </head>
    <body>
        <seq>
		{1}
		</seq>
	</body>
</smil>";
			string mediaItems = "";
			ArrayList mediaList = GetMediaFiles(fileDirectory);
			foreach(string mediaItem in mediaList)
			{
				mediaItems += String.Format("<media src=\"{0}\" />\n", mediaItem);
			}

			TextWriter outputFile = File.CreateText(filename);
			outputFile.Write(String.Format(playlistTemplate, friendlyName, mediaItems));
			outputFile.Close();
		}

		private ArrayList GetMediaFiles(string directory)
		{
			ArrayList mediaList = new ArrayList();
			mediaList.AddRange(Directory.GetFiles(directory, "*.mp3"));
			mediaList.AddRange(Directory.GetFiles(directory, "*.wma"));
			mediaList.AddRange(Directory.GetFiles(directory, "*.wav"));

			return mediaList;
		}

		private string GetLongestCommonPrefix(IList items)
		{
			string longest = null;

			foreach(string currentItem in items)
			{
				// Store the first string, if required
				if(longest == null)
				{
					longest = currentItem;
					continue;
				}

				// Keep on pulling directories from the end of "longest"
				// until "longest" is a prefix of "current."
				// If we empty the string, then return the empty string
				while((! currentItem.StartsWith(longest)) && (longest.Length > 0))
				{
					int lastBackSlash = longest.LastIndexOf(@"\");
					if(lastBackSlash >= 0)
						longest = longest.Substring(0, lastBackSlash);
					else
						longest = "";
				}

				// If we're here, longest is empty, or we have a substring.
				if(longest.Length == 0)
					break;
			}

			return longest;
		}
	}
}
