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

				#region Add to GCT
				if (o.RSBE01 != null) {
					GCT.add(o.RSBE01.FullName, "Codeset.txt", "RSBE01.gct");
					filesCreated.Add("RSBE01.gct");
				}
				#endregion

				#region Edit info.pac and its siblings
				if (o.Info != null) {
					string info_filename = o.Info.FullName;
					ResourceNode info = NodeFactory.FromFile(null, info_filename);
					MSBinNode info140 = info.FindChild("MiscData[140]", false) as MSBinNode;
					bool changed;
					using (MSBinNode custom140 = NodeFactory.FromFile(null, "MiscData[140].msbin") as MSBinNode) {
						changed = SongTitles.copy(custom140, info140);
					}
					if (changed) {
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
				}
				#endregion

				#region Replace MiscData[80], insert custom icons if any, resize icons if needed
				try {
					if (o.Copy_std || o.Copy_exp) {
						ResourceNode toBrres_node = NodeFactory.FromFile(null, "MiscData[80].brres");

						if (o.Common5 != null) {
							ResourceNode fromBrres_common5 = NodeFactory.FromFile(null, o.Common5.FullName);
							ResourceNode fromBrres_common5_node = fromBrres_common5.FindChild("sc_selmap_en", false).FindChild("MiscData[80]", false);
							SSS.copy(fromBrres_common5_node, toBrres_node, o);
							fromBrres_common5_node.ReplaceRaw(toBrres_node.WorkingSource.Address, toBrres_node.WorkingSource.Length);
							fromBrres_common5.Merge();
							fromBrres_common5.Export("common5.pac");
							filesCreated.Add("common5.pac");
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
							filesCreated.Add("mu_menumain.pac");
						}
					}
				} catch (KeyNotFoundException) {
					MessageBox.Show("BrawlLib could not put the file back together. The MiscData[80] may be corrupted, or you may be using an older version of BrawlLib.");
				}
				#endregion

				string msg = "Files created:\n";
				foreach (string s in filesCreated) {
					msg += s + "\n";
				}
				msg += "Remember to copy the new files to the correct locations.";
				if (filesCreated.Count == 0) {
					msg = "No files copied.";
				}
				MessageBox.Show(msg, "Finished");
			}
		}
	}
}
