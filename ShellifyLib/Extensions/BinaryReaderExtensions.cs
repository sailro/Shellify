/*
    Shellify, .NET implementation of Shell Link (.LNK) Binary File Format
    Copyright (C) 2010 Sebastien LEBRETON

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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
            int bytecount = charcount * encoding.GetByteCount(" ");
            return encoding.GetString(reader.ReadBytes(bytecount));
        }

        public static string ReadASCIIZ(this BinaryReader reader, long baseOffset, long defaultOffset, long? unicodeOffset)
        {
            long offset = defaultOffset;
            Encoding encoding = Encoding.Default;
            if (unicodeOffset.HasValue)
            {
                offset = unicodeOffset.Value;
                encoding = Encoding.Unicode;
            }
            return ReadASCIIZ(reader, encoding, reader.BaseStream.Position - baseOffset - offset);
        }

        public static string ReadASCIIZ(this BinaryReader reader, Encoding encoding, long offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Current);
            return ReadASCIIZ(reader, encoding);
        }

        public static string ReadASCIIZ(this BinaryReader reader, Encoding encoding)
        {
            List<byte> bytes = new List<byte>();
            byte[] read;
            int bytecount = encoding.GetByteCount(" ");

            while ( (read = reader.ReadBytes(bytecount)).First() != 0 )
            {
                bytes.AddRange(read);
            }

            return encoding.GetString(bytes.ToArray());
        }

        public static string ReadASCIIZF(this BinaryReader reader, Encoding encoding, int length)
        {
            byte[] padding = null;
            return ReadASCIIZF(reader, encoding, length, out padding);
        }

        public static string ReadASCIIZF(this BinaryReader reader, Encoding encoding, int length, out byte[] padding)
        {
            byte[] bytes = reader.ReadBytes(length);
            int bytecount = encoding.GetByteCount(" ");
            byte[] nullsequence = new byte[bytecount];

            int split = bytes.IndexOf(nullsequence);
            if (split <= 0)
            {
                padding = bytes.ToArray();
                return string.Empty;
            }
            else
            {
                byte[] stringdata = bytes.Take(split -1 + bytecount).ToArray();
                List<byte> temp = bytes.ToList();
                temp.RemoveRange(0, split - 1 + bytecount);
                padding = temp.ToArray();
                return encoding.GetString(stringdata);
            }
        }

    }
}
