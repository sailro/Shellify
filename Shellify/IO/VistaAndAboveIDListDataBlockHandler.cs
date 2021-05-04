/* Shellify Copyright (c) 2010-2021 Sebastien Lebreton

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

using System.IO;
using System.Runtime.InteropServices;
using Shellify.ExtraData;

namespace Shellify.IO
{
	public class VistaAndAboveIDListDataBlockHandler : ExtraDataBlockHandler<VistaAndAboveIDListDataBlock>
	{
		private const int MinimumBlockSize = 0xA;

		public VistaAndAboveIDListDataBlockHandler(VistaAndAboveIDListDataBlock item, ShellLinkFile context) : base(item, context)
		{
		}

		public override int ComputedSize
		{
			get
			{
				var result = base.ComputedSize + Marshal.SizeOf(typeof(short));
				foreach (var item in Item.ShItemIDs)
				{
					result += Marshal.SizeOf(typeof(short));
					result += item.Data?.Length ?? 0;
				}

				return result;
			}
		}

		public override void ReadFrom(BinaryReader reader)
		{
			base.ReadFrom(reader);

			FormatChecker.CheckExpression(() => BlockSize >= MinimumBlockSize);

			var handler = new IDListHandler(Item, false);
			handler.ReadFrom(reader);
		}

		public override void WriteTo(BinaryWriter writer)
		{
			base.WriteTo(writer);

			FormatChecker.CheckExpression(() => BlockSize >= MinimumBlockSize);
			var handler = new IDListHandler(Item, false);
			handler.WriteTo(writer);
		}
	}
}
