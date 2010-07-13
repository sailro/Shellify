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

namespace Shellify.IO
{
	public abstract class BaseRawDataBlockHandler<T> : ExtraDataBlockHandler<T> where T: BaseRawDataBlock
	{
		
		public BaseRawDataBlockHandler(T item, ShellLinkFile context) : base(item, context)
		{
		}
		
		public override int ComputedSize
		{
			get
			{
				return base.ComputedSize + ((Item.Raw != null) ? Item.Raw.Length : 0);
			}
		}
		
		public override void ReadFrom(System.IO.BinaryReader reader)
		{
			base.ReadFrom(reader);
			Item.Raw = reader.ReadBytes(BlockSize - base.ComputedSize);
		}
		
		public override void WriteTo(System.IO.BinaryWriter writer)
		{
			base.WriteTo(writer);
			if (ComputedSize > 0)
			{
				writer.Write(Item.Raw);
			}
		}
		
	}
}
