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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Shellify.Extensions;
using Shellify.ExtraData;

namespace Shellify.IO
{
	public class ConsoleDataBlockHandler : ExtraDataBlockHandler<ConsoleDataBlock>
	{
		private const int UnusedLength = 8;
		private const int FaceNameLength = 64;
		private const int ColorTableLength = 64;
		private const int ExactBlockSize = 0xCC;

		public ConsoleDataBlockHandler(ConsoleDataBlock item, ShellLinkFile context)
			: base(item, context)
		{
		}

		public override int ComputedSize =>
			base.ComputedSize
			+ Marshal.SizeOf(typeof(short)) * 8
			+ UnusedLength
			+ Marshal.SizeOf(typeof(int)) * 6
			+ Marshal.SizeOf(Item.FontWeight)
			+ Marshal.SizeOf(Item.FontSize)
			+ FaceNameLength
			+ Marshal.SizeOf(Item.CursorSize)
			+ Marshal.SizeOf(Item.HistoryBufferSize)
			+ Marshal.SizeOf(Item.NumberOfHistoryBuffers)
			+ ColorTableLength;

		public override void ReadFrom(System.IO.BinaryReader reader)
		{
			base.ReadFrom(reader);

			FormatChecker.CheckExpression(() => BlockSize == ExactBlockSize);

			Item.FillAttributes = (FillAttributes)reader.ReadUInt16();
			Item.PopupFillAttributes = (FillAttributes)reader.ReadUInt16();

			Item.ScreenBufferSize = new Size(reader.ReadInt16(), reader.ReadInt16());
			Item.WindowSize = new Size(reader.ReadInt16(), reader.ReadInt16());
			Item.WindowOrigin = new Point(reader.ReadInt16(), reader.ReadInt16());

			reader.ReadBytes(UnusedLength);

			//Item.FontSize = reader.ReadInt32();
			reader.ReadInt16();
			Item.FontSize = reader.ReadInt16();

			Item.FontFamily = (FontFamily)reader.ReadUInt32();
			Item.FontWeight = reader.ReadUInt32();

			// Keep unknown data padding to preserve valid file roundtrip
			Item.FaceName = reader.ReadASCIIZF(Encoding.Unicode, FaceNameLength, out var padding);
			Item.FaceNamePadding = padding;

			Item.CursorSize = reader.ReadUInt32();
			Item.FullScreen = reader.ReadUInt32() > 0;
			Item.FastEdit = reader.ReadUInt32() > 0;
			Item.InsertMode = reader.ReadUInt32() > 0;
			Item.AutoPosition = reader.ReadUInt32() > 0;

			Item.HistoryBufferSize = reader.ReadUInt32();
			Item.NumberOfHistoryBuffers = reader.ReadUInt32();
			Item.HistoryDuplicateAllowed = reader.ReadUInt32() > 0;
			Item.ColorTable = reader.ReadBytes(ColorTableLength);
		}

		public override void WriteTo(System.IO.BinaryWriter writer)
		{
			base.WriteTo(writer);

			FormatChecker.CheckExpression(() => Item.FaceName == null || Item.FaceName.Length < FaceNameLength);
			FormatChecker.CheckExpression(() => Item.ColorTable != null && Item.ColorTable.Length == ColorTableLength);
			FormatChecker.CheckExpression(() => BlockSize == ExactBlockSize);

			writer.Write((ushort)Item.FillAttributes);
			writer.Write((ushort)Item.PopupFillAttributes);

			writer.Write((short)Item.ScreenBufferSize.Width);
			writer.Write((short)Item.ScreenBufferSize.Height);
			writer.Write((short)Item.WindowSize.Width);
			writer.Write((short)Item.WindowSize.Height);
			writer.Write((short)Item.WindowOrigin.X);
			writer.Write((short)Item.WindowOrigin.Y);

			writer.Write(new byte[UnusedLength]);

			//writer.Write(Item.FontSize);
			writer.Write((short)0);
			writer.Write((short)Item.FontSize);

			writer.Write((uint)Item.FontFamily);
			writer.Write(Item.FontWeight);

			writer.WriteASCIIZF(Item.FaceName, Encoding.Unicode, FaceNameLength, Item.FaceNamePadding);

			writer.Write(Item.CursorSize);
			writer.Write(Convert.ToInt32(Item.FullScreen));
			writer.Write(Convert.ToInt32(Item.FastEdit));
			writer.Write(Convert.ToInt32(Item.InsertMode));
			writer.Write(Convert.ToInt32(Item.AutoPosition));

			writer.Write(Item.HistoryBufferSize);
			writer.Write(Item.NumberOfHistoryBuffers);
			writer.Write(Convert.ToInt32(Item.HistoryDuplicateAllowed));

			writer.Write(Item.ColorTable);
		}
	}
}
