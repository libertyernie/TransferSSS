﻿using System;
using System.Drawing;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Textures;
using BrawlLib.IO;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace TransferSSS {
	class Program {
		[STAThread]
		static void Main(string[] args) {
			Options o = new Options();
			if (o.ShowDialog() == DialogResult.OK) {
				MessageBox.Show(
					o.Prevbase_width_std + "," +
					o.Prevbase_height_std + "," +
					o.Prevbase_width_exp + "," +
					o.Prevbase_height_exp + "," +
					o.Frontstname_width + "," +
					o.Frontstname_height);

				Assembly assembly = Assembly.GetExecutingAssembly();
				Stream toBrres_embedded_stream = assembly.GetManifestResourceStream("TransferSSS.MiscData[80].brres");
				byte[] toBrres_embedded_data = new byte[toBrres_embedded_stream.Length];
				toBrres_embedded_stream.Read(toBrres_embedded_data, 0, toBrres_embedded_data.Length);
				toBrres_embedded_stream.Close();
				string toBrres_embedded_file = Path.GetTempFileName();
				FileStream toBrres_embedded_outstream = new FileStream(toBrres_embedded_file, FileMode.Create);
				toBrres_embedded_outstream.Write(toBrres_embedded_data, 0, toBrres_embedded_data.Length);
				toBrres_embedded_outstream.Close();

				ResourceNode toBrres_node = NodeFactory.FromFile(null, toBrres_embedded_file);

				if (o.Common5 != null) {
					ResourceNode fromBrres_common5 = NodeFactory.FromFile(null, o.Common5.FullName);
					ResourceNode fromBrres_common5_node = fromBrres_common5.FindChild("sc_selmap_en", false).FindChild("MiscData[80]", false);
					realmain(fromBrres_common5_node, toBrres_node);
					toBrres_node.Rebuild();
					fromBrres_common5_node.ReplaceRaw(toBrres_node.WorkingSource.Address, toBrres_node.WorkingSource.Length);
					fromBrres_common5.Merge();
					fromBrres_common5.Export("common5.pac");
				}
				if (o.Mu_menumain != null) {
					ResourceNode fromBrres_mu_menumain = NodeFactory.FromFile(null, o.Mu_menumain.FullName);
					ResourceNode fromBrres_mu_menumain_node = fromBrres_mu_menumain.FindChild("MiscData[0]", false);
					if (o.Common5 == null) {
						realmain(fromBrres_mu_menumain_node, toBrres_node);
						toBrres_node.Rebuild();
					}
					fromBrres_mu_menumain_node.ReplaceRaw(toBrres_node.WorkingSource.Address, toBrres_node.WorkingSource.Length);
					fromBrres_mu_menumain.Merge();
					fromBrres_mu_menumain.Export("mu_menumain.pac");
				}
			}
		}
		static void realmain(ResourceNode fromBrres, ResourceNode toBrres) {
			const int prevbase_width_std = 88; // Stages 1-31, 50-59
			const int prevbase_height_std = 88; // Stages 1-31, 50-59
			const int prevbase_width_exp = 88; // Stages 32-49, 60+
			const int prevbase_height_exp = 88; // Stages 32-49, 60+
			const int frontstname_width = 104;
			const int frontstname_height = 56;

			ProgressWindow progress = new ProgressWindow();
			progress.Begin(0, 341, 0);

			// Icons will be copied directly. MenSelchrMark will have special logic. MenSelmapMark will be ignored.
			int[] seriesicon_mappings = { -1, 20, 20, 1, 1, 1, 1, 2, 2, 3, 3, 4, 4, 5, 6, 7, 8, 8, 9, 11, 18, 15, 12, 14, 13, 1, 10, 21, 17, 19, 22, 23,
												-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
												3, 5, 2, 10, 6, 1, 7, 9, 4, 8};

			ResourceNode toBrres_tex = toBrres.FindChild("Textures(NW4R)", false);
			ResourceNode fromBrres_tex = fromBrres.FindChild("Textures(NW4R)", false);
			ResourceNode toBrres_pal = toBrres.FindChild("Palettes(NW4R)", false);
			ResourceNode fromBrres_pal = fromBrres.FindChild("Palettes(NW4R)", false);

			//TODO check both source and dest for menselchrmark vs seriesicon (4 cases)
			bool toBrres_usesSeriesIcon = (toBrres_tex.FindChild("SeriesIcon.01", false) != null);
			bool fromBrres_usesSeriesIcon = (fromBrres_tex.FindChild("SeriesIcon.01", false) != null);

			CMPR cmpr_converter = TextureConverter.Get(WiiPixelFormat.CMPR) as CMPR;
			TextureConverter i4_converter = TextureConverter.Get(WiiPixelFormat.I4);
			for (int i = 1; i < 80; i++) {
				string num = i.ToString("00");
				if ((fromBrres_tex.FindChild("MenSelmapFrontStname." + num, false) != null) &&
				(toBrres_tex.FindChild("MenSelmapFrontStname." + num, false) != null)) {
					Console.WriteLine("  Copying picture, name and icon for " + num);
					#region MenSelmapPrevbase (resize)
					int width, height;
					if ((i < 32) || ((i >= 50) && (i < 60))) {
						width = prevbase_width_std;
						height = prevbase_height_std;
					} else {
						width = prevbase_width_exp;
						height = prevbase_height_exp;
					}
					TEX0Node fromBrres_prevbase = fromBrres_tex.FindChild("MenSelmapPrevbase." + num, false) as TEX0Node;
					TEX0Node toBrres_prevbase = toBrres_tex.FindChild("MenSelmapPrevbase." + num, false) as TEX0Node;
					if (fromBrres_prevbase.Width <= width && fromBrres_prevbase.Height <= height) {
						toBrres_prevbase.ReplaceRaw(fromBrres_prevbase.OriginalSource.Address, fromBrres_prevbase.OriginalSource.Length);
					} else {
						using (Bitmap source = fromBrres_prevbase.GetImage(0)) {
							using (Bitmap thumbnail = new Bitmap(width, height)) {
								using (Graphics g = Graphics.FromImage(thumbnail)) {
									g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
									g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
									g.DrawImage(source, 0, 0, width, height);
								}
								using (UnsafeBuffer cmprPreview = TextureConverter.CMPR.GeneratePreview(thumbnail)) {
									FileMap map = cmpr_converter.EncodeTextureCached(thumbnail, 1, cmprPreview);
									toBrres_prevbase.ReplaceRaw(map);
								}
							}
						}
					}
					#endregion
					
					#region MenSelmapFrontStname (resize to 104x56)
					TEX0Node fromBrres_frontstname = fromBrres_tex.FindChild("MenSelmapFrontStname." + num, false) as TEX0Node;
					TEX0Node toBrres_frontstname = toBrres_tex.FindChild("MenSelmapFrontStname." + num, false) as TEX0Node;
					if (fromBrres_frontstname.Width <= frontstname_width && fromBrres_frontstname.Height <= frontstname_height) {
						toBrres_frontstname.ReplaceRaw(fromBrres_frontstname.OriginalSource.Address, fromBrres_frontstname.OriginalSource.Length);
					} else {
						using (Bitmap source = fromBrres_frontstname.GetImage(0)) {
							using (Bitmap thumbnail = new Bitmap(frontstname_width, frontstname_height)) {
								using (Graphics g = Graphics.FromImage(thumbnail)) {
									g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
									g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
									g.DrawImage(source, 0, 0, frontstname_width, frontstname_height);
								}
								FileMap map = i4_converter.EncodeTEX0Texture(thumbnail, 1);
								toBrres_frontstname.ReplaceRaw(map);
							}
						}
					}
					#endregion

					#region MenSelmapIcon (direct copy, texture and palette)
					copyTexture(fromBrres_pal, "MenSelmapIcon." + num, toBrres_pal, "MenSelmapIcon." + num);
					copyTexture(fromBrres_tex, "MenSelmapIcon." + num, toBrres_tex, "MenSelmapIcon." + num);
					#endregion
					progress.Update(3*i);
				} else {
					Console.WriteLine("Skipped " + num);
				}
			}

			#region MenSelchrMark / SeriesIcon
			if (toBrres_usesSeriesIcon) {
				// The common5 we're copying *to* uses SeriesIcon.
				for (int i = 1; i < 81; i++) {
					string num = i.ToString("00");
					if (fromBrres_usesSeriesIcon) {
						// Direct copy
						copyTexture(fromBrres_tex, "SeriesIcon." + num,
							toBrres_tex, "SeriesIcon." + num);
					} else {
						// The common5 we're copying *from* uses MenSelchrMark
						if (i < seriesicon_mappings.Length) {
							copyTexture(fromBrres_tex, "MenSelchrMark." + seriesicon_mappings[i].ToString("00"),
								toBrres_tex, "SeriesIcon." + num);
						}
					}
					progress.Update(237 + i);
				}
			} else {
				// The common5 we're copying *to* uses MenSelchrMark. Skip #16 (not present in the SSS).
				for (int i = 1; i < 24; i++) if (i != 16) {
						if (fromBrres_usesSeriesIcon) {
							// The common5 we're copying *from* uses SeriesIcon - we can't copy all the icons, because there aren't as many
							copyTexture(fromBrres_tex, "SeriesIcon." + i.ToString("00"),
								toBrres_tex, "MenSelchrMark." + firstIndexOf(seriesicon_mappings, i).ToString("00"));
						} else {
							// Direct copy
							copyTexture(fromBrres_tex, "MenSelchrMark." + i.ToString("00"),
								toBrres_tex, "MenSelchrMark." + i.ToString("00"));
						}
						progress.Update(247 + 3*i);
					}
			}
			#endregion

			#region Copy 3D models
			/*ResourceNode toBrres_mdl = toBrres_file.FindChild("3DModels(NW4R)", false);
			ResourceNode fromBrres_mdl = fromBrres_file.FindChild("3DModels(NW4R)", false);
			foreach (ResourceNode from in fromBrres_mdl.Children) {
				if (from.Name != "MenSelmapPos") {
					ResourceNode to = toBrres_mdl.FindChild(from.Name, false);
					if (to == null) {
						MessageBox.Show("No " + from.Name + " model in destination");
					} else {
						copyTexture(from, to);
					}
				}
			}*/
			#endregion
//			Console.ReadLine();
			progress.Dispose();
		}

		private static void copyTexture(ResourceNode parent_from, string child_from, ResourceNode parent_to, string child_to) {
			ResourceNode from = parent_from.FindChild(child_from, false);
/*			TEX0Node to = fromBrres_tex.FindChild((
				fromBrres_usesSeriesIcon ? name_SeriesIcon : name_MenSelchrMark
				), false) as TEX0Node;*/
			ResourceNode to = parent_to.FindChild(child_to, false);
			if (from == null) {
				Console.WriteLine("  E: No " + child_from + " in source");
			} else if (to == null) {
				Console.WriteLine("  E: No " + child_to + " in destination");
			} else {
				copyTexture(from, to);
			}
		}

		private static void copyTexture(ResourceNode from, ResourceNode to) {
			DataSource from_source = from.OriginalSource;
			to.ReplaceRaw(from_source.Address, from_source.Length);
			Console.WriteLine("  Direct copy: " + from.Name + " --> " + to.Name);
		}

		private static int firstIndexOf(int[] array, int search) {
			for (int i = 0; i < array.Length; i++) {
				if (array[i] == search) {
					return i;
				}
			}
			return -1;
		}
	}
}
