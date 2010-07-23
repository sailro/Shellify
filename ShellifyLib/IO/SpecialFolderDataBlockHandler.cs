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
using System;

namespace Shellify.IO
{
    public class SpecialFolderDataBlockHandler : BaseFolderIDDataBlockHandler<SpecialFolderDataBlock>
    {
        public const int ExactBlockSize = 0x10;

        public override int ComputedSize
        {
            get
            {
                return base.ComputedSize + Marshal.SizeOf(typeof(int));
            }
        }

        public SpecialFolderDataBlockHandler(SpecialFolderDataBlock item, ShellLinkFile context)
            : base(item, context)
        {
        }

        protected override void ReadID(System.IO.BinaryReader reader)
        {
            FormatChecker.CheckExpression(() => BlockSize == ExactBlockSize);
            Item.SpecialFolder = (Environment.SpecialFolder)reader.ReadInt32();
        }

        protected override void WriteID(System.IO.BinaryWriter writer)
        {
            FormatChecker.CheckExpression(() => BlockSize == ExactBlockSize );
            writer.Write((int)Item.SpecialFolder);
        }

    }
}
