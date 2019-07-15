/* Shellify Copyright (c) 2010-2019 Sebastien Lebreton

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shellify.Extensions
{
	public static class BinaryReaderExtensions
	{
		public static string ReadSTDATA(this BinaryReader reader, Encoding encoding)
		{
			int charcount = reader.ReadUInt16();
			var bytecount = charcount * encoding.GetByteCount(" ");
			return encoding.GetString(reader.ReadBytes(bytecount));
		}

		public static string ReadASCIIZ(this BinaryReader reader, long baseOffset, long defaultOffset, long? unicodeOffset)
		{
			var offset = defaultOffset;
			var encoding = Encoding.Default;
			if (unicodeOffset.HasValue)
			{
				offset = unicodeOffset.Value;
				encoding = Encoding.Unicode;
			}

			return ReadASCIIZ(reader, encoding, reader.BaseStream.Position - baseOffset - offset);
		}

		private static string ReadASCIIZ(this BinaryReader reader, Encoding encoding, long offset)
		{
			reader.BaseStream.Seek(offset, SeekOrigin.Current);
			return ReadASCIIZ(reader, encoding);
		}

		private static string ReadASCIIZ(this BinaryReader reader, Encoding encoding)
		{
			var bytes = new List<byte>();
			byte[] read;
			var bytecount = encoding.GetByteCount(" ");

			while ((read = reader.ReadBytes(bytecount)).First() != 0)
			{
				bytes.AddRange(read);
			}

			return encoding.GetString(bytes.ToArray());
		}

		public static string ReadASCIIZF(this BinaryReader reader, Encoding encoding, int length)
		{
			return ReadASCIIZF(reader, encoding, length, out _);
		}

		public static string ReadASCIIZF(this BinaryReader reader, Encoding encoding, int length, out byte[] padding)
		{
			var bytes = reader.ReadBytes(length);
			var bytecount = encoding.GetByteCount(" ");
			var nullsequence = new byte[bytecount];

			var split = bytes.IndexOf(nullsequence);
			if (split <= 0)
			{
				padding = bytes.ToArray();
				return string.Empty;
			}

			var stringdata = bytes.Take(split - 1 + bytecount).ToArray();
			var temp = bytes.ToList();
			temp.RemoveRange(0, split - 1 + bytecount);
			padding = temp.ToArray();
			return encoding.GetString(stringdata);
		}
	}
}
