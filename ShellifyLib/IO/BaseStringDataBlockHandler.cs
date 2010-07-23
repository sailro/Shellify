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

using Shellify.ExtraData;
using Shellify.Extensions;
using System.Text;

namespace Shellify.IO
{
	public abstract class BaseStringDataBlockHandler<T> : ExtraDataBlockHandler<T> where T: BaseStringDataBlock
	{

        public const int ValueSize = 260;
        public const int ValueSizeUnicode = 520;
        public const int ExactBlockSize = 0x314;

        public BaseStringDataBlockHandler(T item, ShellLinkFile context)
            : base(item, context)
		{
		}
		
		public override int ComputedSize
		{
			get
			{
                return base.ComputedSize + ValueSize + ValueSizeUnicode;
			}
		}
		
		public override void ReadFrom(System.IO.BinaryReader reader)
		{
			base.ReadFrom(reader);

            FormatChecker.CheckExpression(() => BlockSize == ExactBlockSize); 
            byte[] padding = null;
            
            Item.Value = reader.ReadASCIIZF(Encoding.Default, ValueSize, out padding);
            Item.ValuePadding = padding;

            Item.ValueUnicode = reader.ReadASCIIZF(Encoding.Unicode, ValueSizeUnicode, out padding);
            Item.ValueUnicodePadding = padding;
        }

		public override void WriteTo(System.IO.BinaryWriter writer)
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
