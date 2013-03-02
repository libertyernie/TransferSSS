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
		private FileInfo _common5, _mu_menumain;
		public FileInfo Common5 {
			get {
				return _common5;
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
					common5_label.Text = "No file selected";
					common5_label.Enabled = false;
				}
			}
		}
		public FileInfo Mu_menumain {
			get {
				return _mu_menumain;
			}
			set {
				if (value != null && value.Exists) {
					_mu_menumain = value;
				} else {
					_mu_menumain = null;
				}
				if (_common5 != null) {
					mu_menumain_label.Text = _mu_menumain.FullName;
					mu_menumain_label.Enabled = true;
				} else {
					mu_menumain_label.Text = "No file selected";
					mu_menumain_label.Enabled = false;
				}
			}
		}

		public Options() {
			InitializeComponent();

			Common5 = Mu_menumain = null;

			string[] common5_folders = { "/private/wii/app/RSBE/pf/system", "/brawlmods/textures/system",
								"/brawlmods/vbrawl/system", "/brawlmods/brawlminus/system" };
			foreach (string folder in common5_folders) {
				string file_string = folder + "/common5.pac";
				FileInfo file = new FileInfo(file_string);
				if (file.Exists) {
					Common5 = file;
					break;
				}
			}

			string[] mu_menumain_folders = { "/private/wii/app/RSBE/pfmenu2", "/private/wii/app/RSBE/pf/menu2",
								"/brawlmods/textures/menu2", "/brawlmods/brawlminus/menu2" };
			foreach (string folder in mu_menumain_folders) {
				string file_string = folder + "/mu_menumain.pac";
				FileInfo file = new FileInfo(file_string);
				if (file.Exists) {
					Mu_menumain = file;
					break;
				}
			}
		}

		private void common5_button_Click(object sender, EventArgs e) {
			openFileDialog1.InitialDirectory = Common5.DirectoryName;
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				common5_label.Text = openFileDialog1.FileName;
			}
		}
	}
}
