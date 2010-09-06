/* Shellify Copyright (c) 2010 Sebastien LEBRETON

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
using System.Runtime.InteropServices;
using Shellify.ExtraData;

namespace Shellify.IO
{
    public abstract class ExtraDataBlockHandler : IBinaryReadable, IBinaryWriteable
	{
		public abstract void ReadFrom(System.IO.BinaryReader reader);
		public abstract void WriteTo(System.IO.BinaryWriter writer);
	}

    public abstract class ExtraDataBlockHandler<T> : ExtraDataBlockHandler, ISizeComputable where T : ExtraDataBlock
	{
		protected T Item { get; set; }
		protected int BlockSize { get; set; }
        protected ShellLinkFile Context { get; set; }

		public virtual int ComputedSize
		{
			get
			{
				return Marshal.SizeOf(BlockSize) + Marshal.SizeOf(typeof(int));
			}
		}
		
		public ExtraDataBlockHandler(T item, ShellLinkFile context)
		{
			this.Item = item;
            this.Context = context;
		}
		
		public override void ReadFrom(System.IO.BinaryReader reader)
		{
			BlockSize = reader.ReadInt32();
            Item.Signature = (ExtraDataBlockSignature) (reader.ReadInt32());
		}
		
		public override void WriteTo(System.IO.BinaryWriter writer)
		{
            FormatChecker.CheckExpression(() => Item.Signature != ExtraDataBlockSignature.UnknownDataBlock);
            BlockSize = ComputedSize;
            writer.Write(BlockSize);
			writer.Write((int) Item.Signature);
		}
	
	}
}
