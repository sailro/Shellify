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
using System.Runtime.InteropServices;

namespace Shellify.IO
{
	public class ConsoleFEDataBlockHandler : ExtraDataBlockHandler<ConsoleFEDataBlock>
	{
        public const int ExactBlockSize = 0xC;

        public ConsoleFEDataBlockHandler(ConsoleFEDataBlock item, ShellLinkFile context)
            : base(item, context)
		{
		}
		
		public override int ComputedSize
		{
			get
			{
				return base.ComputedSize + Marshal.SizeOf(Item.CodePage);
			}
		}
		
		public override void ReadFrom(System.IO.BinaryReader reader)
		{
			base.ReadFrom(reader);
            FormatChecker.CheckExpression(() => BlockSize == ExactBlockSize);
            Item.CodePage = reader.ReadUInt32();
		}
		
		public override void WriteTo(System.IO.BinaryWriter writer)
		{
			base.WriteTo(writer);
            FormatChecker.CheckExpression(() => BlockSize == ExactBlockSize);
            writer.Write(Item.CodePage);
		}
		
	}
}
