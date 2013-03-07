using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TransferSSS {
	class GCT {
		public static void gct_add(string gct_file, string txt_file) {
			FileStream gct = new FileStream(gct_file, FileMode.Open, FileAccess.Read);
			byte[] gct_data = new byte[gct.Length - 16];
			gct.Seek(8, SeekOrigin.Begin); // Skip eight-byte GCT header
			gct.Read(gct_data, 0, gct_data.Length); // Also skip eight-byte GCT footer
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
			Console.WriteLine("b,a " + byte_array_contains(b, a));
			Console.WriteLine("c,a " + byte_array_contains(c, a));
			Console.WriteLine("d,a " + byte_array_contains(d, a));
			Console.WriteLine("b,c " + byte_array_contains(b, c));
			Console.WriteLine("b,d " + byte_array_contains(b, d));
			Console.WriteLine("gct_data,txt_data " + byte_array_contains(gct_data, txt_data));

			FileStream gct_out = new FileStream("RSBE01.gct", FileMode.Create, FileAccess.Write);
			gct_out.Write(txt_data, 0, txt_data.Length);
			byte[] footer = { 0xf0, 0, 0, 0, 0, 0, 0, 0 };
			gct_out.Write(footer, 0, footer.Length);
			gct_out.Close();
		}

		#region Utility methods
		public static int byte_array_contains(byte[] large, byte[] small) {
			int index_in_large = 0;
			int index_in_small = 0;
			int potential_position_of_small_in_large = -1; // will be overwritten during search
			while (index_in_large < large.Length) { // Could be improved - could stop once there is too little space left for result to be true
				if (large[index_in_large] == small[index_in_small]) {
					if (potential_position_of_small_in_large < 0) {
						potential_position_of_small_in_large = index_in_large;
					}
					index_in_small++;
					if (index_in_small == small.Length) {
						return potential_position_of_small_in_large;
					}
				} else {
					index_in_small = 0;
					if (potential_position_of_small_in_large >= 0) {
						index_in_large = potential_position_of_small_in_large; // go back and look again from start
						potential_position_of_small_in_large = -1;
					}
				}
				index_in_large++;
			}
			return -1; // not found
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
