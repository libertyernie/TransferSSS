using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TransferSSS {
	public partial class Options : Form {
		private FileInfo _common5, _mu_menumain, _rsbe01, _info;
		#region Accessors for each file (Common5, RSBE01, etc.)
		public FileInfo Common5 {
			get {
				return (common5_label.Checked ? _common5 : null);
			}
			set {
				if (value != null && value.Exists) {
					_common5 = value;
				} else {
					_common5 = null;
				}
				if (_common5 != null) {
					common5_label.Text = _common5.FullName;
					common5_label.Enabled = true;
				} else {
					common5_label.Text = "No common5 selected";
					common5_label.Enabled = false;
				}
			}
		}
		public FileInfo Mu_menumain {
			get {
				return (mu_menumain_label.Checked ? _mu_menumain : null);
			}
			set {
				if (value != null && value.Exists) {
					_mu_menumain = value;
				} else {
					_mu_menumain = null;
				}
				if (_mu_menumain != null) {
					mu_menumain_label.Text = _mu_menumain.FullName;
					mu_menumain_label.Enabled = true;
				} else {
					mu_menumain_label.Text = "No mu_menumain selected";
					mu_menumain_label.Enabled = false;
				}
			}
		}
		public FileInfo RSBE01 {
			get {
				return (addCodes.Checked ? _rsbe01 : null);
			}
			set {
				if (value != null && value.Exists) {
					_rsbe01 = value;
				} else {
					_rsbe01 = null;
				}
				if (_rsbe01 != null) {
					addCodes.Text = "Add from Codeset.txt to "+_rsbe01.FullName;
					addCodes.Enabled = true;
				} else {
					addCodes.Text = "No RSBE01.gct selected";
					addCodes.Enabled = false;
				}
			}
		}
		public FileInfo Info {
			get {
				return (updateSongTitles.Checked ? _info : null);
			}
			set {
				if (value != null && value.Exists) {
					_info = value;
				} else {
					_info = null;
				}
				if (_info != null) {
					updateSongTitles.Text = _info.FullName;
					updateSongTitles.Enabled = true;
				} else {
					updateSongTitles.Text = "No info.pac selected";
					updateSongTitles.Enabled = false;
				}
			}
		}
		#endregion

		#region Accessors for number spinners
		public int Prevbase_width_std {
			get {
				return (int)prevbase_width_std.Value;
			}
		}
		public int Prevbase_height_std {
			get {
				return (int)prevbase_height_std.Value;
			}
		}
		public int Prevbase_width_exp {
			get {
				return (int)prevbase_width_exp.Value;
			}
		}
		public int Prevbase_height_exp {
			get {
				return (int)prevbase_height_exp.Value;
			}
		}
		public int Frontstname_width {
			get {
				return (int)frontstname_width.Value;
			}
		}
		public int Frontstname_height {
			get {
				return (int)frontstname_height.Value;
			}
		}
		#endregion

		public bool Copy_std {
			get {
				return copy_std.Checked;
			}
		}
		public bool Copy_exp {
			get {
				return copy_exp.Checked;
			}
		}

		public Options() {
			InitializeComponent();

			Common5 = Mu_menumain = RSBE01 = Info = null;

			#region Scan for files in current folder and root of current drive
			string[] common5_folders = {
								"private/wii/app/RSBE/pf/system", "brawlmods/textures/system",
								"brawlmods/vbrawl/system", "brawlmods/brawlminus/system",
								"/private/wii/app/RSBE/pf/system", "/brawlmods/textures/system",
								"/brawlmods/vbrawl/system", "/brawlmods/brawlminus/system"};
			foreach (string folder in common5_folders) {
				string file_string = folder + "/common5.pac";
				FileInfo file = new FileInfo(file_string);
				if (file.Exists) {
					Common5 = file;
					break;
				}
			}

			string[] mu_menumain_folders = {
								"private/wii/app/RSBE/pfmenu2", "private/wii/app/RSBE/pf/menu2",
								"brawlmods/textures/menu2", "brawlmods/brawlminus/menu2",
								"/private/wii/app/RSBE/pfmenu2", "/private/wii/app/RSBE/pf/menu2",
								"/brawlmods/textures/menu2", "/brawlmods/brawlminus/menu2"};
			foreach (string folder in mu_menumain_folders) {
				string file_string = folder + "/mu_menumain.pac";
				FileInfo file = new FileInfo(file_string);
				if (file.Exists) {
					Mu_menumain = file;
					break;
				}
			}

			string[] rsbe01_folders = {
								"data/gecko/codes", "codes",
								"brawlmods/vbrawl", "brawlmods/brawlminus",
								"/data/gecko/codes", "/codes",
								"/brawlmods/vbrawl", "/brawlmods/brawlminus",};
			foreach (string folder in rsbe01_folders) {
				string file_string = folder + "/RSBE01.gct";
				FileInfo file = new FileInfo(file_string);
				if (file.Exists) {
					RSBE01 = file;
					break;
				}
			}

			string[] info_folders = {
								"private/wii/app/RSBE/pf/info2",
								"brawlmods/music",
								"/private/wii/app/RSBE/pf/info2",
								"/brawlmods/music",};
			foreach (string folder in info_folders) {
				string file_string = folder + "/info.pac";
				FileInfo file = new FileInfo(file_string);
				if (file.Exists) {
					Info = file;
					break;
				}
			}
			#endregion

			if (!new FileInfo("MiscData[80].brres").Exists) {
				MessageBox.Show("Cannot find a MiscData[80].brres file to use as a base.");
				common5_label.Enabled = mu_menumain_label.Enabled =
					common5_label.Checked = mu_menumain_label.Checked = 
					copy_std.Checked = copy_exp.Checked = false;
				// copy_std and copy_exp will be disabled automatically by unchecking common5_label and mu_menumain_label.
			}
			if (!new FileInfo("Codeset.txt").Exists) {
				MessageBox.Show("Cannot find a Codeset.txt file to add to the GCT.");
				addCodes.Enabled = addCodes.Checked = false;
			}
			if (!new FileInfo("MiscData[140].msbin").Exists) {
				MessageBox.Show("Cannot find a MiscData[140].msbin file to get custom song titles from.");
				updateSongTitles.Enabled = updateSongTitles.Checked = false;
			}
		}

		private void common5_button_Click(object sender, EventArgs e) {
			if (Common5 != null) {
				openFileDialog1.InitialDirectory = Common5.DirectoryName;
			} else if (Mu_menumain != null) {
				openFileDialog1.InitialDirectory = Mu_menumain.Directory.Parent.FullName;
			}
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				Common5 = new FileInfo(openFileDialog1.FileName);
			}
		}

		private void mu_menumain_change_Click(object sender, EventArgs e) {
			if (Mu_menumain != null) {
				openFileDialog1.InitialDirectory = Mu_menumain.DirectoryName;
			} else if (Common5 != null) {
				openFileDialog1.InitialDirectory = Common5.Directory.Parent.Parent.FullName;
			}
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				Mu_menumain = new FileInfo(openFileDialog1.FileName);
			}
		}

		private void rsbe01_button_Click(object sender, EventArgs e) {
			if (RSBE01 != null) {
				openFileDialog1.InitialDirectory = RSBE01.DirectoryName;
			}
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				RSBE01 = new FileInfo(openFileDialog1.FileName);
			}
		}

		private void info_button_Click(object sender, EventArgs e) {
			if (Info != null) {
				openFileDialog1.InitialDirectory = Info.DirectoryName;
			}
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				Info = new FileInfo(openFileDialog1.FileName);
			}
		}

		private void btnOkay_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnAbout_Click(object sender, EventArgs e) {
			const string about = "This program will perform the following tasks:\n" +
				" * Read the custom stage icons/names/portraits from common5.pac and/or mu_menumain.pac\n" +
				" * Resize the images and copy them into a new, stage-expansion-enabled BRRES, using MiscData[80].brres as a base\n" +
				" * Insert the new BRRES into copies of common5 and/or mu_menumain, which are saved to the current directory\n" +
				"\n" +
				"When using the File Patch Code, resizing the images is required to keep the filesize below a certain level.";
			MessageBox.Show(this, about);
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			Close();
		}

		private void updateAll() {
			if (common5_label.Checked || mu_menumain_label.Checked) {
				copy_std.Enabled = copy_exp.Enabled = true;
				// update things individually
				prevbase_width_std.Enabled = prevbase_height_std.Enabled = copy_std.Checked;
				prevbase_width_exp.Enabled = prevbase_height_exp.Enabled = copy_exp.Checked;
				frontstname_width.Enabled = frontstname_height.Enabled = (copy_exp.Checked || copy_std.Checked);
			} else {
				copy_std.Enabled = copy_exp.Enabled =
				prevbase_width_std.Enabled = prevbase_height_std.Enabled =
				prevbase_width_exp.Enabled = prevbase_height_exp.Enabled =
				frontstname_width.Enabled = frontstname_height.Enabled = false;
			}
		}

		private void copy_std_CheckedChanged(object sender, EventArgs e) {
			updateAll();
		}

		private void copy_exp_CheckedChanged(object sender, EventArgs e) {
			updateAll();
		}

		private void common5_label_CheckedChanged(object sender, EventArgs e) {
			updateAll();
		}

		private void mu_menumain_label_CheckedChanged(object sender, EventArgs e) {
			updateAll();
		}
	}
}
