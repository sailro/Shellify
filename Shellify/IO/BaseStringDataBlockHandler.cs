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

using System.IO;
using Shellify.ExtraData;
using Shellify.Extensions;
using System.Text;

namespace Shellify.IO
{
	public abstract class BaseStringDataBlockHandler<T> : ExtraDataBlockHandler<T> where T : BaseStringDataBlock
	{
		private const int ValueSize = 260;
		private const int ValueSizeUnicode = 520;
		private const int ExactBlockSize = 0x314;

		protected BaseStringDataBlockHandler(T item, ShellLinkFile context)
			: base(item, context)
		{
		}

		public override int ComputedSize => base.ComputedSize + ValueSize + ValueSizeUnicode;

		public override void ReadFrom(BinaryReader reader)
		{
			base.ReadFrom(reader);

			FormatChecker.CheckExpression(() => BlockSize == ExactBlockSize);

			Item.Value = reader.ReadASCIIZF(Encoding.Default, ValueSize, out var padding);
			Item.ValuePadding = padding;

			Item.ValueUnicode = reader.ReadASCIIZF(Encoding.Unicode, ValueSizeUnicode, out padding);
			Item.ValueUnicodePadding = padding;
		}

		public override void WriteTo(BinaryWriter writer)
		{
			base.WriteTo(writer);

			FormatChecker.CheckExpression(() => Item.Value == null || Item.Value.Length < ValueSize);
			FormatChecker.CheckExpression(() => Item.ValueUnicode == null || Item.ValueUnicode.Length < ValueSizeUnicode);
			FormatChecker.CheckExpression(() => BlockSize == ExactBlockSize);

			writer.WriteASCIIZF(Item.Value, Encoding.Default, ValueSize, Item.ValuePadding);
			writer.WriteASCIIZF(Item.ValueUnicode, Encoding.Unicode, ValueSizeUnicode, Item.ValueUnicodePadding);
		}
	}
}
