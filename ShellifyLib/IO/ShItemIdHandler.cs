/* Shellify Copyright (c) 2010-2013 Sebastien LEBRETON

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
	public class ShItemIdHandler : IBinaryReadable, IBinaryWriteable, ISizeComputable
	{
		
		private ShItemID Item { get; set; }
		private ushort Size { get; set; }
		
		public ShItemIdHandler(ShItemID item)
		{
			Item = item;
		}
		
		public void ReadFrom(BinaryReader reader)
		{
			Size = reader.ReadUInt16();
			if (Size > Marshal.SizeOf(Size))
			{
				Item.Data = reader.ReadBytes(Size - Marshal.SizeOf(Size));
			}
		}
		
		public int ComputedSize
		{
			get
			{
				return ((Item.Data != null) ? Item.Data.Length : 0) + Marshal.SizeOf(Size);
			}
		}
		
		public void WriteTo(BinaryWriter writer)
		{
			Size = Convert.ToUInt16(ComputedSize);
			writer.Write(Size);
			if (Size > 0)
			{
				writer.Write(Item.Data);
			}
		}
	}
}
