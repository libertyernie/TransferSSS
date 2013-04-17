using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TransferSSS {
	public partial class AboutDialog : Form {
		public AboutDialog() {
			InitializeComponent();
			webBrowser1.Document.Write(string.Empty);
			webBrowser1.DocumentText = Properties.Resources.help;
		}

		public static void About() {
			new AboutDialog().Show();
		}
	}
}
