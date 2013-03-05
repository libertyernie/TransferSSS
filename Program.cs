using System;
using System.Drawing;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Textures;
using BrawlLib.IO;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

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

				gct_add("codes/RSBE01.gct", "Codeset.txt");
				Console.ReadLine();
				return;

				if (o.Common5 != null) {
					ResourceNode fromBrres_common5 = NodeFactory.FromFile(null, o.Common5.FullName);
					ResourceNode fromBrres_common5_node = fromBrres_common5.FindChild("sc_selmap_en", false).FindChild("MiscData[80]", false);
					sss_copy(fromBrres_common5_node, toBrres_node, o);
					toBrres_node.Rebuild();
					fromBrres_common5_node.ReplaceRaw(toBrres_node.WorkingSource.Address, toBrres_node.WorkingSource.Length);
					fromBrres_common5.Merge();
					fromBrres_common5.Export("common5.pac");
				}
				if (o.Mu_menumain != null) {
					ResourceNode fromBrres_mu_menumain = NodeFactory.FromFile(null, o.Mu_menumain.FullName);
					ResourceNode fromBrres_mu_menumain_node = fromBrres_mu_menumain.FindChild("MiscData[0]", false);
					if (o.Common5 == null) {
						sss_copy(fromBrres_mu_menumain_node, toBrres_node, o);
						toBrres_node.Rebuild();
					}
					fromBrres_mu_menumain_node.ReplaceRaw(toBrres_node.WorkingSource.Address, toBrres_node.WorkingSource.Length);
					fromBrres_mu_menumain.Merge();
					fromBrres_mu_menumain.Export("mu_menumain.pac");
				}

				MessageBox.Show("Remember to copy the new files to the correct locations.", "Finished");
			}
		}

		static void gct_add(string gct_file, string txt_file) {
			FileStream gct = new FileStream(gct_file, FileMode.Open, FileAccess.Read);
			byte[] gct_data = new byte[gct.Length - 12];
			gct.Seek(4, SeekOrigin.Begin);
			gct.Read(gct_data, 0, gct_data.Length);
			gct.Close();

			FileStream txt = new FileStream(txt_file, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(txt);
			List<byte> txt_data = new List<byte>();
			string line;
			while ((line = reader.ReadLine()) != null) {
				if (is_enabled_code(line)) {
					string raw = line.Replace(" ", "").Replace("*", "");
					for (int i = 0; i < 16; i += 2) {
						txt_data.Add(Convert.ToByte(raw.Substring(i, 2), 16));
					}
				}
			}

			// testing
			reader.Close(); txt.Close();
			for (int i = 8; i > 0; i--) {
				Console.WriteLine(gct_data[gct_data.Length - i]);
			}
			Console.WriteLine();
			for (int i = 8; i > 0; i--) {
				Console.WriteLine(txt_data[txt_data.Count - i]);
			}

			// check if these codes are already in the GCT (in the same order)
			//TODO

			FileStream gct_out = new FileStream("RSBE01.gct", FileMode.Create, FileAccess.Write);
			gct_out.Write(txt_data.ToArray(), 0, txt_data.Count);
			byte[] footer = { 0xf0, 0, 0, 0, 0, 0, 0, 0 };
			gct_out.Write(footer, 0, footer.Length);
			gct_out.Close();
		}

		#region Functions for txt->gct
		static bool is_enabled_code(string line) {
			if (line.Length < 2 || line[0] != '*' || line[1] != ' ') {
				return false;
			} else {
				return is_code(line.Substring(2));
			}
		}

		static bool is_code(string line) {
			if (line[0] == '*' && line[1] == ' ') {
				return is_code(line.Substring(2));
			}
			int i;
			for (i = 0; i < 17; i++) {
				if (i == 8) {
					if (line[i] != ' ') return false;
				} else {
					if (!isxdigit(line[i])) return false;
				}
			}
			return true;
		}

		static bool isxdigit(char c) {
			return (c >= '0' && c <= '9') ||
					   (c >= 'a' && c <= 'f') ||
					   (c >= 'A' && c <= 'F');
		}
		#endregion

		static void sss_copy(ResourceNode fromBrres, ResourceNode toBrres, Options o) {
			int prevbase_width_std = o.Prevbase_width_std; // Stages 1-31, 50-59
			int prevbase_height_std = o.Prevbase_height_std; // Stages 1-31, 50-59
			int prevbase_width_exp = o.Prevbase_width_exp; // Stages 32-49, 60+
			int prevbase_height_exp = o.Prevbase_height_exp; // Stages 32-49, 60+
			int frontstname_width = o.Frontstname_width;
			int frontstname_height = o.Frontstname_height;
			bool copy_std = o.Copy_std;
			bool copy_exp = o.Copy_exp;

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
				int prevbase_width, prevbase_height;
				bool copy;
				if ((i < 32) || ((i >= 50) && (i < 60))) {
					prevbase_width = prevbase_width_std;
					prevbase_height = prevbase_height_std;
					copy = copy_std;
				} else {
					prevbase_width = prevbase_width_exp;
					prevbase_height = prevbase_height_exp;
					copy = copy_exp;
				}
				if (copy &&
				(fromBrres_tex.FindChild("MenSelmapFrontStname." + num, false) != null) &&
				(toBrres_tex.FindChild("MenSelmapFrontStname." + num, false) != null)) {
					Console.WriteLine("  Copying picture, name and icon for " + num);
					#region MenSelmapPrevbase (resize)
					TEX0Node fromBrres_prevbase = fromBrres_tex.FindChild("MenSelmapPrevbase." + num, false) as TEX0Node;
					TEX0Node toBrres_prevbase = toBrres_tex.FindChild("MenSelmapPrevbase." + num, false) as TEX0Node;
					if (fromBrres_prevbase.Width <= prevbase_width && fromBrres_prevbase.Height <= prevbase_height) {
						toBrres_prevbase.ReplaceRaw(fromBrres_prevbase.OriginalSource.Address, fromBrres_prevbase.OriginalSource.Length);
					} else {
						using (Bitmap source = fromBrres_prevbase.GetImage(0)) {
							using (Bitmap thumbnail = new Bitmap(prevbase_width, prevbase_height)) {
								using (Graphics g = Graphics.FromImage(thumbnail)) {
									g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
									g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
									g.DrawImage(source, 0, 0, prevbase_width, prevbase_height);
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
