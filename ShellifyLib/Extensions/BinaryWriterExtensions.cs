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

namespace Shellify.Extensions
{
    public static class BinaryWriterExtensions
    {

        static public void WriteSTDATA(this BinaryWriter writer, string value, Encoding encoding)
        {
            short charcount = Convert.ToInt16(value.Length);
            writer.Write(charcount);
            writer.Write(encoding.GetBytes(value));
        }

        public static void WriteASCIIZ(this BinaryWriter writer, string value, Encoding encoding)
        {
            if (value != null)
            {
                writer.Write(encoding.GetBytes(value));
            }
            writer.Write((byte)0);
        }

        public static void WriteASCIIZF(this BinaryWriter writer, string value, Encoding encoding, int length)
        {
            List<byte> bytes = new List<byte>(encoding.GetBytes(value));

            while (bytes.Count < length)
            {
                bytes.Add(0);
            }

            writer.Write(bytes.Take(length).ToArray());
        }

        public static void WriteASCIIZF(this BinaryWriter writer, string value, Encoding encoding, int length, byte[] padding)
        {
            List<byte> bytes = new List<byte>(encoding.GetBytes(value));

            if (padding != null)
            {
                int padindex = 0;
                while ((bytes.Count < length) && (padindex < padding.Length))
                {
                    bytes.Add(padding[padindex++]);
                }
            }

            while (bytes.Count < length)
            {
                bytes.Add(0);
            }

            writer.Write(bytes.Take(length).ToArray());
        }


    }
}
