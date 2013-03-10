using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace TransferSSS {
	class Program {
		[STAThread]
		static void Main(string[] args) {
			Options o = new Options();
			if (o.ShowDialog() == DialogResult.OK) {
				List<string> filesCreated = new List<string>();

				if (o.Copy_std || o.Copy_exp) {
					if (!new FileInfo("MiscData[80].brres").Exists) {
						MessageBox.Show("Error: cannot find a MiscData[80].brres file to use as a base.");
						return;
					}
					/*if (o.Common5 == null && o.Mu_menumain == null) {
						MessageBox.Show("Error: copying stage icons selected, but no common5 or mu_menumain found to replace icons in.");
						return;
					}*/
				}
				if (o.RSBE01 != null) {
					if (!new FileInfo("Codeset.txt").Exists) {
						MessageBox.Show("Error: cannot find a Codeset.txt file to add to the GCT.");
						return;
					}
				}
				if (o.Info != null) {
					if (!new FileInfo("MiscData[140].msbin").Exists) {
						MessageBox.Show("Error: cannot find a MiscData[140].msbin file to get custom song titles from.");
						return;
					}
				}

				ResourceNode toBrres_node = NodeFactory.FromFile(null, "MiscData[80].brres");

				if (o.RSBE01 != null) {
					GCT.add(o.RSBE01.FullName, "Codeset.txt", "RSBE01.gct");
					filesCreated.Add("RSBE01.gct");
				}

				if (o.Info != null) {
					string info_filename = o.Info.FullName;
					ResourceNode info = NodeFactory.FromFile(null, info_filename);
					MSBinNode info140 = info.FindChild("MiscData[140]", false) as MSBinNode;
					using (MSBinNode custom140 = NodeFactory.FromFile(null, "MiscData[140].msbin") as MSBinNode) {
						SongTitles.copy(custom140, info140);
					}
					info.Export(o.Info.Name);
					filesCreated.Add(o.Info.Name);

					FileInfo[] other_files = new FileInfo[4];
					string[] append = { "_boss_battle", "_corps", "_homerun", "_training" };
					for (int i = 0; i < 4; i++) {
						other_files[i] = new FileInfo(info_filename.Replace("info.pac", "info" + append[i] + ".pac"));
					}
					DataSource info140source = info140.OriginalSource;
					foreach (FileInfo file in other_files) {
						Console.WriteLine(file.FullName);
						if (file.Exists) {
							using (ResourceNode root = NodeFactory.FromFile(null, file.FullName)) {
								root.FindChild("MiscData[140]", false).ReplaceRaw(info140source.Address, info140source.Length);
								root.Export(file.Name);
								filesCreated.Add(file.Name);
							}
						}
					}
				}

				if (o.Copy_std || o.Copy_exp) {
					if (o.Common5 != null) {
						ResourceNode fromBrres_common5 = NodeFactory.FromFile(null, o.Common5.FullName);
						ResourceNode fromBrres_common5_node = fromBrres_common5.FindChild("sc_selmap_en", false).FindChild("MiscData[80]", false);
						SSS.copy(fromBrres_common5_node, toBrres_node, o);
						fromBrres_common5_node.ReplaceRaw(toBrres_node.WorkingSource.Address, toBrres_node.WorkingSource.Length);
						fromBrres_common5.Merge();
						fromBrres_common5.Export(o.Common5.Name);
						filesCreated.Add(o.Common5.Name);
					}
					if (o.Mu_menumain != null) {
						ResourceNode fromBrres_mu_menumain = NodeFactory.FromFile(null, o.Mu_menumain.FullName);
						ResourceNode fromBrres_mu_menumain_node = fromBrres_mu_menumain.FindChild("MiscData[0]", false);
						if (o.Common5 == null) {
							SSS.copy(fromBrres_mu_menumain_node, toBrres_node, o);
						}
						fromBrres_mu_menumain_node.ReplaceRaw(toBrres_node.WorkingSource.Address, toBrres_node.WorkingSource.Length);
						fromBrres_mu_menumain.Merge();
						fromBrres_mu_menumain.Export(o.Mu_menumain.Name);
						filesCreated.Add(o.Mu_menumain.Name);
					}
				}

				string msg = "Files created:\n";
				foreach (string s in filesCreated) {
					msg += s + "\n";
				}
				MessageBox.Show(msg+"Remember to copy the new files to the correct locations.", "Finished");
			}
		}
	}
}
