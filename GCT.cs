using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace TransferSSS {
	class GCT {
		public static void add(string gct_file, string txt_file, string output_file) {
			Stopwatch s = Stopwatch.StartNew();
			FileStream gct = new FileStream(gct_file, FileMode.Open, FileAccess.Read);
			byte[] gct_data = new byte[gct.Length - 16];
			gct.Seek(8, SeekOrigin.Begin); // DON'T skip eight-byte GCT header
			gct.Read(gct_data, 0, gct_data.Length); // DO skip eight-byte GCT footer
			gct.Close();

			FileStream txt = new FileStream(txt_file, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(txt);
			List<byte> txt_data_list = new List<byte>();
			string line;
			while ((line = reader.ReadLine()) != null) {
				if (is_enabled_code(line)) {
					string raw = line.Replace(" ", "").Replace("*", "");
					for (int i = 0; i < 16; i += 2) {
						txt_data_list.Add(Convert.ToByte(raw.Substring(i, 2), 16));
					}
				}
			}
			byte[] txt_data = txt_data_list.ToArray();

			// testing
			reader.Close(); txt.Close();
			for (int i = 0; i < 16; i++) {
				Console.Write(Convert.ToString(gct_data[i], 16) + " ");
			}
			Console.WriteLine();
			for (int i = 8; i > 0; i--) {
				Console.Write(Convert.ToString(txt_data[i], 16) + " ");
			}
			Console.WriteLine();

			// check if these codes are already in the GCT (in the same order)
			byte[] a = { 4, 13 };
			byte[] b = { 0, 0, 4, 13, 11 };
			byte[] c = { 4, 13, 55 };
			byte[] d = { 0, 0, 4, 13 };
			Console.WriteLine("b,a " + first_index_of_subsequence(b, a));
			Console.WriteLine("c,a " + first_index_of_subsequence(c, a));
			Console.WriteLine("d,a " + first_index_of_subsequence(d, a));
			Console.WriteLine("b,c " + first_index_of_subsequence(b, c));
			Console.WriteLine("b,d " + first_index_of_subsequence(b, d));
			Console.WriteLine("gct_data,txt_data " + first_index_of_subsequence(gct_data, txt_data));

			if (first_index_of_subsequence(gct_data, txt_data) > 0) {
				MessageBox.Show("The codes from Codeset.txt are already present in the GCT file, in the same order. No new GCT file will be created.");
			} else {
				FileStream gct_out = new FileStream(output_file, FileMode.Create, FileAccess.Write);
				gct_out.Write(gct_data, 0, gct_data.Length);
				gct_out.Write(txt_data, 0, txt_data.Length);
				byte[] footer = { 0xf0, 0, 0, 0, 0, 0, 0, 0 };
				gct_out.Write(footer, 0, footer.Length);
				gct_out.Close();
				MessageBox.Show("GCT file written to " + output_file);
			}
		}

		#region Utility methods
		public static int first_index_of_subsequence(byte[] large, byte[] small) {
			for (int i = 0; i < (large.Length - small.Length + 1); i++) {
				if (subsequence_equal(large, i, small)) {
					return i;
				}
			}
			return -1;
		}

		public static bool subsequence_equal(byte[] large, int offset, byte[] small) {
			for (int i = 0; i < small.Length; i++) {
				if (small[i] != large[i + offset]) {
					return false;
				}
			}
			return true;
		}

		public static bool isxdigit(char c) {
			return (c >= '0' && c <= '9') ||
					   (c >= 'a' && c <= 'f') ||
					   (c >= 'A' && c <= 'F');
		}
		#endregion

		#region Functions for txt->gct
		public static bool is_enabled_code(string line) {
			if (line.Length < 2 || line[0] != '*' || line[1] != ' ') {
				return false;
			} else {
				return is_code(line.Substring(2));
			}
		}

		public static bool is_code(string line) {
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
		#endregion
	}
}
