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

using System.Linq;
using System.Runtime.InteropServices;
using Shellify.ExtraData;

namespace Shellify.IO
{
	public abstract class BaseFolderIDDataBlockHandler<T> : ExtraDataBlockHandler<T> where T : BaseFolderIDDataBlock
	{
		private int Offset { get; set; }

		protected BaseFolderIDDataBlockHandler(T item, ShellLinkFile context)
			: base(item, context)
		{
		}

		public override int ComputedSize => base.ComputedSize + Marshal.SizeOf(Offset);

		public override void ReadFrom(System.IO.BinaryReader reader)
		{
			base.ReadFrom(reader);
			ReadID(reader);
			Offset = reader.ReadInt32();

			var computedOffset = 0;
			foreach (var shitemid in Context.ShItemIDs)
			{
				if (computedOffset == Offset)
				{
					Item.ShItemID = shitemid;
					break;
				}

				var handler = new ShItemIdHandler(shitemid);
				computedOffset += handler.ComputedSize;
			}
		}

		public override void WriteTo(System.IO.BinaryWriter writer)
		{
			Offset = 0;
			foreach (var handler in Context.ShItemIDs.TakeWhile(shitemid => Item.ShItemID != shitemid).Select(shitemid => new ShItemIdHandler(shitemid)))
				Offset += handler.ComputedSize;

			base.WriteTo(writer);
			WriteID(writer);
			writer.Write(Offset);
		}

		protected abstract void ReadID(System.IO.BinaryReader reader);

		protected abstract void WriteID(System.IO.BinaryWriter writer);
	}
}
