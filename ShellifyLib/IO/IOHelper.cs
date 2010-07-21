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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Diagnostics;

namespace Shellify.IO
{
    public class IOHelper
    {

        public static string ToString(IEnumerable collection)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            foreach (object item in collection)
            {
                if (builder.Length > 1)
                {
                    builder.Append(",");
                }
                builder.Append(item);
            }
            builder.Append("}"); 
            return builder.ToString();
        }

        public static string ToHexString(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.AppendFormat("{0:x2}", b);
            }
            return builder.ToString();
        }

        public static string ComputeHash(byte[] bytes)
        {
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                return ToHexString(md5.ComputeHash(bytes));
            }
        }

        public static int GetASCIIZSize(string value, Encoding encoding)
        {
            if (value == null)
            {
                return 1;
            }
            else
            {
                return encoding.GetByteCount(value) + 1;
            }
        }

        public static string ReadSTDATA(BinaryReader reader, Encoding encoding)
        {
            int charcount = reader.ReadUInt16();
            int bytecount = charcount * encoding.GetByteCount(" ");
            return encoding.GetString(reader.ReadBytes(bytecount));
        }

        static public void WriteSTDATA(string value, BinaryWriter writer, Encoding encoding)
        {
            short charcount = Convert.ToInt16(value.Length);
            writer.Write(charcount);
            writer.Write(encoding.GetBytes(value));
        }

        public static void WriteASCIIZ(string value, BinaryWriter writer, Encoding encoding)
        {
            if (value != null)
            {
                writer.Write(encoding.GetBytes(value));
            }
            writer.Write((byte)0);
        }

        public static string ReadASCIIZ(BinaryReader reader, long baseOffset, long defaultOffset, long? unicodeOffset)
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

        public static string ReadASCIIZ(BinaryReader reader, Encoding encoding, long offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Current);
            return ReadASCIIZ(reader, encoding);
        }

        public static string ReadASCIIZ(BinaryReader reader, Encoding encoding)
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

        public static string ReadASCIIZF(BinaryReader reader, Encoding encoding, int length)
        {
            List<byte> bytes = new List<byte>(reader.ReadBytes(length));
            int bytecount = encoding.GetByteCount(" ");
            int zerocounter = 0;

            List<byte> filtered = bytes.TakeWhile(b => (zerocounter = (b == 0) ? zerocounter + 1 : 0) < bytecount).ToList();
            Debug.Assert(bytes.Skip(filtered.Count).ToList().TrueForAll(b => b==0), "Warning, non zero padding detected");
            return encoding.GetString(filtered.ToArray());
        }

        public static void WriteASCIIZF(string value, BinaryWriter writer, Encoding encoding, int length)
        {
            List<byte> bytes = new List<byte>(encoding.GetBytes(value));

            while (bytes.Count < length)
            {
                bytes.Add(0);
            }

            writer.Write(bytes.Take(length).ToArray());
        }

    }
}
