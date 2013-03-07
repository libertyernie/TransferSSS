using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.IO;

namespace TransferSSS {
	class Program {
		[STAThread]
		static void Main(string[] args) {
			ResourceNode toBrres_node = null;
			try {
				toBrres_node = NodeFactory.FromFile(null, "MiscData[80].brres");
			} catch (FileNotFoundException) {
				MessageBox.Show("Error: cannot find a MiscData[80].brres file to use as a base.");
				return;
			}
			Options o = new Options();
			if (o.ShowDialog() == DialogResult.OK) {
				/*MessageBox.Show(
					o.Copy_std + "," +
					o.Prevbase_width_std + "," +
					o.Prevbase_height_std + "," +
					o.Copy_exp + "," +
					o.Prevbase_width_exp + "," +
					o.Prevbase_height_exp + "," +
					o.Frontstname_width + "," +
					o.Frontstname_height);*/

				GCT.add("codes/RSBE01.gct", "Codeset.txt");
				Console.ReadLine();
				return;

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

				MessageBox.Show("Remember to copy the new files to the correct locations.", "Finished");
			}
		}
	}
}
