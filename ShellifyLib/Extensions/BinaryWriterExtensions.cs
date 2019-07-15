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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shellify.Extensions
{
    public static class BinaryWriterExtensions
    {

        public static void WriteSTDATA(this BinaryWriter writer, string value, Encoding encoding)
        {
            var charcount = Convert.ToInt16(value.Length);
            writer.Write(charcount);
            writer.Write(encoding.GetBytes(value));
        }

        public static void WriteASCIIZ(this BinaryWriter writer, string value, Encoding encoding)
        {
            if (value != null)
                writer.Write(encoding.GetBytes(value));

			writer.Write((byte)0);
        }

        public static void WriteASCIIZF(this BinaryWriter writer, string value, Encoding encoding, int length)
        {
            var bytes = new List<byte>(encoding.GetBytes(value ?? string.Empty));

            while (bytes.Count < length)
                bytes.Add(0);

            writer.Write(bytes.Take(length).ToArray());
        }

        public static void WriteASCIIZF(this BinaryWriter writer, string value, Encoding encoding, int length, byte[] padding)
        {
            var bytes = new List<byte>(encoding.GetBytes(value ?? string.Empty));

            if (padding != null)
            {
                var padindex = 0;
                while ((bytes.Count < length) && (padindex < padding.Length))
                    bytes.Add(padding[padindex++]);
            }

            while (bytes.Count < length)
                bytes.Add(0);

            writer.Write(bytes.Take(length).ToArray());
        }


    }
}
