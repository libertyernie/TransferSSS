using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using BrawlLib.Wii.Textures;
using BrawlLib.IO;

namespace TransferSSS {
	class Program {
		static void Main(string[] args) {
			const int prevbaseBrres_width = 88;
			const int prevbaseBrres_height = 88;
			const int frontstname_width = 104;
			const int frontstname_height = 56;
			// Icons will be copied directly. MenSelchrMark will have special logic. MenSelmapMark will be ignored.
			int[] seriesicon_mappings = { -1, 20, 20, 1, 1, 1, 1, 2, 2, 3, 3, 4, 4, 5, 6, 7, 8, 8, 9, 11, 18, 15, 12, 14, 13, 1, 10, 21, 17, 19, 22, 23,
											  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
											  3, 5, 2, 10, 6, 1, 7, 9, 4, 8};

			ResourceNode baseBrres = NodeFactory.FromFile(null, "MiscData[80].brres");
			ResourceNode fromBrres = NodeFactory.FromFile(null, "custom.brres");
			ResourceNode baseBrres_tex = baseBrres.FindChild("Textures(NW4R)", false);
			ResourceNode fromBrres_tex = fromBrres.FindChild("Textures(NW4R)", false);

			//TODO check both source and dest for menselchrmark vs seriesicon (4 cases)
			

			CMPR cmpr_converter = TextureConverter.Get(WiiPixelFormat.CMPR) as CMPR;
			TextureConverter i4_converter = TextureConverter.Get(WiiPixelFormat.I4);
			for (int i = 1; i < 60; i++ ) if (i <= 31 || i >= 50) {
				string num = i.ToString("00");

				#region MenSelmapPrevbase (resize to 88x88)
				TEX0Node fromBrres_prevbase = fromBrres_tex.FindChild("MenSelmapPrevbase." + num, false) as TEX0Node;
				TEX0Node baseBrres_prevbase = baseBrres_tex.FindChild("MenSelmapPrevbase." + num, false) as TEX0Node;
				if (fromBrres_prevbase.Width <= 88 && fromBrres_prevbase.Height <= 88) {
					Console.WriteLine("C--> " + num);
					baseBrres_prevbase.ReplaceRaw(fromBrres_prevbase.OriginalSource.Address, fromBrres_prevbase.OriginalSource.Length);
				} else {
					Console.WriteLine("R--> " + num);
					using (Bitmap source = fromBrres_prevbase.GetImage(0)) {
						using (Bitmap thumbnail = new Bitmap(88, 88)) {
							using (Graphics g = Graphics.FromImage(thumbnail)) {
								g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
								g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
								g.DrawImage(source, 0, 0, 88, 88);
							}
							if (i == 31) thumbnail.Save("31.png", System.Drawing.Imaging.ImageFormat.Png);
							using (UnsafeBuffer cmprPreview = TextureConverter.CMPR.GeneratePreview(thumbnail)) {
								FileMap map = cmpr_converter.EncodeTextureCached(thumbnail, 1, cmprPreview);
								baseBrres_prevbase.ReplaceRaw(map);
							}
						}
					}
				}
				#endregion

				#region MenSelmapFrontStname (resize to 104x56)
				TEX0Node fromBrres_frontstname = fromBrres_tex.FindChild("MenSelmapFrontStname." + num, false) as TEX0Node;
				TEX0Node baseBrres_frontstname = baseBrres_tex.FindChild("MenSelmapFrontStname." + num, false) as TEX0Node;
				if (fromBrres_frontstname.Width <= 104 && fromBrres_frontstname.Height <= 56) {
					Console.WriteLine("C--> " + num);
					baseBrres_frontstname.ReplaceRaw(fromBrres_frontstname.OriginalSource.Address, fromBrres_frontstname.OriginalSource.Length);
				} else {
					Console.WriteLine("R--> " + num);
					using (Bitmap source = fromBrres_frontstname.GetImage(0)) {
						using (Bitmap thumbnail = new Bitmap(88, 88)) {
							using (Graphics g = Graphics.FromImage(thumbnail)) {
								g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
								g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
								g.DrawImage(source, 0, 0, 104, 56);
							}
							if (i == 31) thumbnail.Save("31.png", System.Drawing.Imaging.ImageFormat.Png);
							FileMap map = i4_converter.EncodeTEX0Texture(thumbnail, 1);
							baseBrres_frontstname.ReplaceRaw(map);
						}
					}
				}
				#endregion

				#region MenSelchrMark (original mapping) --> SeriesIcon (my new common5)
				TEX0Node fromBrres_selchrmark = fromBrres_tex.FindChild("MenSelchrMark." + seriesicon_mappings[i].ToString("00"), false) as TEX0Node;
				TEX0Node baseBrres_seriesicon = baseBrres_tex.FindChild("SeriesIcon." + num, false) as TEX0Node;
				Console.WriteLine("SeriesIcon ("+seriesicon_mappings[i].ToString("00")+"-->"+num+") --> " + num);
				baseBrres_frontstname.ReplaceRaw(fromBrres_frontstname.OriginalSource.Address, fromBrres_frontstname.OriginalSource.Length);
				#endregion
			}
			baseBrres.Export("out.brres");
		}
	}
}
