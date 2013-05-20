using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace TransferSSS {
	class Program {
		[STAThread]
		static void Main(string[] args) {
			try {
				NodeFactory.FromFile(null, "");
			} catch (ArgumentException) {
				// expected behavior
			} catch (TypeInitializationException) {
				MessageBox.Show("TypeInitializationException thrown! Your version of BrawlLib may require OpenTK.dll.");
				return;
			}

			Options o = new Options();
			if (o.ShowDialog() == DialogResult.OK) {
				List<string> filesCreated = new List<string>();
				Dictionary<string, string> destinations = new Dictionary<string, string>();
				List<string> filesSkipped = new List<string>();

				#region Add to GCT
				if (o.RSBE01 != null) {
					if (GCT.add(o.RSBE01.FullName, "Codeset.txt", "RSBE01.gct")) {
						filesCreated.Add("RSBE01.gct");
						destinations.Add("RSBE01.gct", o.RSBE01.FullName);
					}
				}
				#endregion

				#region Edit info.pac and its siblings
				if (o.Info != null) {
					string info_filename = o.Info.FullName;
					using (ResourceNode info = NodeFactory.FromFile(null, info_filename)) {
						MSBinNode info140 = info.FindChild("MiscData[140]", false) as MSBinNode;
						bool changed = true;
						using (MSBinNode custom140 = NodeFactory.FromFile(null, "MiscData[140].msbin") as MSBinNode) {
//							changed = 
							SongTitles.copy(custom140, info140);
						}
						if (!changed) {
							filesSkipped.Add(o.Info.Name);
						} else {
							info.Export(o.Info.Name);
							filesCreated.Add(o.Info.Name);
							destinations.Add(o.Info.Name, o.Info.FullName);

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
										destinations.Add(file.Name, file.FullName);
									}
								}
							}
						}
					}
				}
				#endregion

				#region Replace MiscData[80], insert custom icons if any, resize icons if needed
				try {
					if (o.Common5 != null || o.Mu_menumain != null) {
						ResourceNode toBrres_node = NodeFactory.FromFile(null, "MiscData[80].brres");

						if (o.Common5 != null) {
							ResourceNode fromBrres_common5 = NodeFactory.FromFile(null, o.Common5.FullName);
							ResourceNode fromBrres_common5_node = fromBrres_common5.FindChild("sc_selmap_en", false).FindChild("MiscData[80]", false);
							SSS.copy(fromBrres_common5_node, toBrres_node, o);
							fromBrres_common5_node.ReplaceRaw(toBrres_node.WorkingSource.Address, toBrres_node.WorkingSource.Length);
							fromBrres_common5.Merge();
							fromBrres_common5.Export("common5.pac");
							filesCreated.Add("common5.pac");
							destinations.Add("common5.pac", o.Common5.FullName);
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
							destinations.Add("mu_menumain.pac", o.Mu_menumain.FullName);
						}
					}
				} catch (KeyNotFoundException) {
					MessageBox.Show("BrawlLib could not put the file back together. The MiscData[80] may be corrupted, or you may be using an older version of BrawlLib.");
				}
				#endregion

				MessageBoxButtons buttons = MessageBoxButtons.YesNo;

				string msg = "Files created:\n";
				foreach (string s in filesCreated) {
					msg += s + "\n";
				}
				foreach (string s in filesSkipped) {
					msg += "(Skipped " + s + ")\n";
				}
				if (filesCreated.Count == 0) {
					msg = "No files copied.";
					buttons = MessageBoxButtons.OK;
				} else {
					msg += "\nMove the files to their correct locations now?";
				}
				DialogResult result = MessageBox.Show(msg, "", buttons);
				if (result == DialogResult.Yes) {
					string filesCopied = "";
					foreach (string s in filesCreated) {
						try {
							string dest = destinations[s];
							filesCopied += s + " --> " + dest;
							if (new FileInfo(dest).Exists) {
								if (new FileInfo(dest + ".bak").Exists) {
									File.Delete(dest);
									filesCopied += " (did not back up - older backup already exists)";
								} else {
									File.Move(dest, dest + ".bak");
									filesCopied += " (backed up original first)";
								}
							}
							File.Move(s, dest);
							filesCopied += "\n";
						} catch (KeyNotFoundException) {
							MessageBox.Show("Error: did not remember where to move " + s + " to.");
						}
					}
					MessageBox.Show(filesCopied);
				}
			}
		}
	}
}
