using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.IO;

namespace TransferSSS {
	class Program {
		[STAThread]
		static void Main(string[] args) {
			Options o = new Options();
			if (o.ShowDialog() == DialogResult.OK) {
				if (o.Copy_std || o.Copy_exp) {
					if (!new FileInfo("MiscData[80].brres").Exists) {
						MessageBox.Show("Error: cannot find a MiscData[80].brres file to use as a base.");
						return;
					}
					if (o.Common5 == null && o.Mu_menumain == null) {
						MessageBox.Show("Error: no common5 or mu_menumain found to replace icons in.");
						return;
					}
				}
				if (o.RSBE01 != null) {
					if (!new FileInfo("Codeset.txt").Exists) {
						MessageBox.Show("Error: cannot find a Codeset.txt file to add to the GCT.");
						return;
					}
				}

				ResourceNode toBrres_node = NodeFactory.FromFile(null, "MiscData[80].brres");

				if (o.RSBE01 != null) GCT.add(o.RSBE01.FullName, "Codeset.txt", "RSBE01.gct");

				if (o.Copy_std || o.Copy_exp) {
					if (o.Common5 != null) {
						ResourceNode fromBrres_common5 = NodeFactory.FromFile(null, o.Common5.FullName);
						ResourceNode fromBrres_common5_node = fromBrres_common5.FindChild("sc_selmap_en", false).FindChild("MiscData[80]", false);
						SSS.copy(fromBrres_common5_node, toBrres_node, o);
						fromBrres_common5_node.ReplaceRaw(toBrres_node.WorkingSource.Address, toBrres_node.WorkingSource.Length);
						fromBrres_common5.Merge();
						fromBrres_common5.Export("common5.pac");
					}
					if (o.Mu_menumain != null) {
						ResourceNode fromBrres_mu_menumain = NodeFactory.FromFile(null, o.Mu_menumain.FullName);
						ResourceNode fromBrres_mu_menumain_node = fromBrres_mu_menumain.FindChild("MiscData[0]", false);
						if (o.Common5 == null) {
							SSS.copy(fromBrres_mu_menumain_node, toBrres_node, o);
						}
						fromBrres_mu_menumain_node.ReplaceRaw(toBrres_node.WorkingSource.Address, toBrres_node.WorkingSource.Length);
						fromBrres_mu_menumain.Merge();
						fromBrres_mu_menumain.Export("mu_menumain.pac");
					}
				}

				MessageBox.Show("Remember to copy the new files to the correct locations.", "Finished");
			}
		}
	}
}
