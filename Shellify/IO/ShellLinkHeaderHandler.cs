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
using System.IO;
using System.Runtime.InteropServices;
using Shellify.Core;

namespace Shellify.IO
{
	public class ShellLinkHeaderHandler : IBinaryReadable, IBinaryWriteable
	{
		private const int ExactHeaderSize = 0x4C;

		private ShellLinkHeader Item { get; set; }
		private int HeaderSize { get; set; }
		private const int ReservedSize = 10;

		public ShellLinkHeaderHandler(ShellLinkHeader item)
		{
			Item = item;
		}

		public void ReadFrom(BinaryReader reader)
		{
			HeaderSize = reader.ReadInt32();
			FormatChecker.CheckExpression(() => HeaderSize == ExactHeaderSize);

			Item.Guid = new Guid(reader.ReadBytes(16));
			FormatChecker.CheckExpression(() => Item.Guid.ToString().Equals(ShellLinkHeader.LNKGuid));

			Item.LinkFlags = (LinkFlags)(reader.ReadInt32());
			Item.FileAttributes = (FileAttributes)(reader.ReadInt32());

			Item.CreationTime = DateTime.FromFileTime(reader.ReadInt64());
			Item.AccessTime = DateTime.FromFileTime(reader.ReadInt64());
			Item.WriteTime = DateTime.FromFileTime(reader.ReadInt64());
			Item.FileSize = reader.ReadInt32();
			Item.IconIndex = reader.ReadInt32();

			Item.ShowCommand = (ShowCommand)reader.ReadInt32();
			Item.HotKey = reader.ReadInt16();

			reader.ReadBytes(ReservedSize); // Reserved
		}

		public void WriteTo(BinaryWriter writer)
		{
			HeaderSize = Marshal.SizeOf(HeaderSize) +
			             Marshal.SizeOf(Item.Guid) +
			             Marshal.SizeOf(Item.FileSize) +
			             Marshal.SizeOf(Item.IconIndex) +
			             1 * Marshal.SizeOf(typeof(short)) +
			             3 * Marshal.SizeOf(typeof(int)) +
			             3 * Marshal.SizeOf(typeof(long)) +
			             ReservedSize;

			FormatChecker.CheckExpression(() => HeaderSize == ExactHeaderSize);
			FormatChecker.CheckExpression(() => Item.Guid.ToString().Equals(ShellLinkHeader.LNKGuid));

			writer.Write(HeaderSize);
			writer.Write(Item.Guid.ToByteArray());
			writer.Write((int)Item.LinkFlags);
			writer.Write((int)Item.FileAttributes);

			writer.Write(Item.CreationTime.ToFileTime());
			writer.Write(Item.AccessTime.ToFileTime());
			writer.Write(Item.WriteTime.ToFileTime());

			writer.Write(Item.FileSize);
			writer.Write(Item.IconIndex);

			writer.Write((int)Item.ShowCommand);
			writer.Write((short)Item.HotKey);

			var reserved = new byte[ReservedSize];
			writer.Write(reserved); // Reserved
		}
	}
}
