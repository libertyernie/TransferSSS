using System;
using System.Drawing;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Textures;
using BrawlLib.IO;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace TransferSSS {
	class Program {
		static void Main(string[] args) {
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream toBrres_common5_stream = assembly.GetManifestResourceStream("TransferSSS.common5.pac");
			byte[] toBrres_common5_data = new byte[toBrres_common5_stream.Length];
			toBrres_common5_stream.Read(toBrres_common5_data, 0, toBrres_common5_data.Length);
			toBrres_common5_stream.Close();
			string toBrres_common5_file = Path.GetTempFileName();
			FileStream toBrres_common5_outstream = new FileStream(toBrres_common5_file, FileMode.Create);
			toBrres_common5_outstream.Write(toBrres_common5_data, 0, toBrres_common5_data.Length);
			toBrres_common5_outstream.Close();
			ResourceNode toBrres_common5 = NodeFactory.FromFile(null, toBrres_common5_file);

			const int prevtoBrres_width = 88;
			const int prevtoBrres_height = 88;
			const int frontstname_width = 104;
			const int frontstname_height = 56;
			// Icons will be copied directly. MenSelchrMark will have special logic. MenSelmapMark will be ignored.
			int[] seriesicon_mappings = { -1, 20, 20, 1, 1, 1, 1, 2, 2, 3, 3, 4, 4, 5, 6, 7, 8, 8, 9, 11, 18, 15, 12, 14, 13, 1, 10, 21, 17, 19, 22, 23,
												-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
												3, 5, 2, 10, 6, 1, 7, 9, 4, 8};

			ResourceNode toBrres_file = toBrres_common5.FindChild("sc_selmap_en", false).FindChild("MiscData[80]", false);
			ResourceNode fromBrres_file = NodeFactory.FromFile(null, "custom.brres");
			ResourceNode toBrres_tex = toBrres_file.FindChild("Textures(NW4R)", false);
			ResourceNode fromBrres_tex = fromBrres_file.FindChild("Textures(NW4R)", false);
			ResourceNode toBrres_pal = toBrres_file.FindChild("Palettes(NW4R)", false);
			ResourceNode fromBrres_pal = fromBrres_file.FindChild("Palettes(NW4R)", false);

			//TODO check both source and dest for menselchrmark vs seriesicon (4 cases)
			bool toBrres_usesSeriesIcon = (toBrres_tex.FindChild("SeriesIcon.01", false) != null);
			bool fromBrres_usesSeriesIcon = (fromBrres_tex.FindChild("SeriesIcon.01", false) != null);

			CMPR cmpr_converter = TextureConverter.Get(WiiPixelFormat.CMPR) as CMPR;
			TextureConverter i4_converter = TextureConverter.Get(WiiPixelFormat.I4);
			for (int i = 1; i < 60; i++) if (i <= 31 || i >= 50) { // Copy 1-31 and 50-59, the original Brawl stages.
					string num = i.ToString("00");

					#region MenSelmapPrevbase (resize to 88x88)
					TEX0Node fromBrres_prevbase = fromBrres_tex.FindChild("MenSelmapPrevbase." + num, false) as TEX0Node;
					TEX0Node toBrres_prevbase = toBrres_tex.FindChild("MenSelmapPrevbase." + num, false) as TEX0Node;
					if (fromBrres_prevbase.Width <= prevtoBrres_width && fromBrres_prevbase.Height <= prevtoBrres_height) {
						//					Console.WriteLine("C--> " + num);
						toBrres_prevbase.ReplaceRaw(fromBrres_prevbase.OriginalSource.Address, fromBrres_prevbase.OriginalSource.Length);
					} else {
						//					Console.WriteLine("R--> " + num);
						using (Bitmap source = fromBrres_prevbase.GetImage(0)) {
							using (Bitmap thumbnail = new Bitmap(prevtoBrres_width, prevtoBrres_height)) {
								using (Graphics g = Graphics.FromImage(thumbnail)) {
									g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
									g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
									g.DrawImage(source, 0, 0, prevtoBrres_width, prevtoBrres_height);
								}
								//							if (i == 31) thumbnail.Save("31.png", System.Drawing.Imaging.ImageFormat.Png);
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
						//					Console.WriteLine("C--> " + num);
						toBrres_frontstname.ReplaceRaw(fromBrres_frontstname.OriginalSource.Address, fromBrres_frontstname.OriginalSource.Length);
					} else {
						//					Console.WriteLine("R--> " + num);
						using (Bitmap source = fromBrres_frontstname.GetImage(0)) {
							using (Bitmap thumbnail = new Bitmap(frontstname_width, frontstname_height)) {
								using (Graphics g = Graphics.FromImage(thumbnail)) {
									g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
									g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
									g.DrawImage(source, 0, 0, frontstname_width, frontstname_height);
								}
								//							if (i == 31) thumbnail.Save("31.png", System.Drawing.Imaging.ImageFormat.Png);
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
				}

			#region MenSelchrMark (original mapping) --> SeriesIcon (my new common5)
			if (toBrres_usesSeriesIcon) {
				// The common5 we're copying *to* uses SeriesIcon. Copy 1-31 and 50-59, the original Brawl stages.
				for (int i = 1; i < 60; i++) if (i <= 31 || i >= 50) {
						if (fromBrres_usesSeriesIcon) {
							// Direct copy
							copyTexture(fromBrres_tex, "SeriesIcon." + i.ToString("00"),
								toBrres_tex, "SeriesIcon." + i.ToString("00"));
						} else {
							// The common5 we're copying *from* uses MenSelchrMark
							copyTexture(fromBrres_tex, "MenSelchrMark." + seriesicon_mappings[i].ToString("00"),
								toBrres_tex, "SeriesIcon." + i.ToString("00"));
						}
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
							Console.WriteLine("4,i=" + i);
							copyTexture(fromBrres_tex, "MenSelchrMark." + i.ToString("00"),
								toBrres_tex, "MenSelchrMark." + i.ToString("00"));
						}
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
			toBrres_file.Export("out.brres");
			toBrres_common5.Export("out.pac");
		}

		private static void copyTexture(ResourceNode parent_from, string child_from, ResourceNode parent_to, string child_to) {
			ResourceNode from = parent_from.FindChild(child_from, false);
/*			TEX0Node to = fromBrres_tex.FindChild((
				fromBrres_usesSeriesIcon ? name_SeriesIcon : name_MenSelchrMark
				), false) as TEX0Node;*/
			ResourceNode to = parent_to.FindChild(child_to, false);
			if (from == null) {
				MessageBox.Show("No " + child_from + " in source");
			} else if (to == null) {
				MessageBox.Show("No " + child_to + " in destination");
			} else {
				copyTexture(from, to);
			}
		}

		private static void copyTexture(ResourceNode from, ResourceNode to) {
			DataSource from_source = from.OriginalSource;
			to.ReplaceRaw(from_source.Address, from_source.Length);
			Console.WriteLine(from.Name + " --> " + to.Name);
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
