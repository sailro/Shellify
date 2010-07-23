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

using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Shellify.Core;
using Shellify.Extensions;
using Shellify.ExtraData;

namespace Shellify.IO
{
    public class VistaAndAboveIDListDataBlockHandler : ExtraDataBlockHandler<VistaAndAboveIDListDataBlock>
	{
        public const int MinimumBlockSize = 0xA;

        public VistaAndAboveIDListDataBlockHandler(VistaAndAboveIDListDataBlock item, ShellLinkFile context)
            : base(item, context)
        {
        }

        public override int ComputedSize
        {
            get
            {
                int result = base.ComputedSize + Marshal.SizeOf(typeof(short));
                foreach (ShItemID item in Item.ShItemIDs)
                {
                    result += Marshal.SizeOf(typeof(short));
                    result += item.Data == null ? 0 : item.Data.Length;
                }
                return result;
            }
        }

        public override void ReadFrom(BinaryReader reader)
        {
            base.ReadFrom(reader);

            FormatChecker.CheckExpression(() => BlockSize >= MinimumBlockSize);

            IDListHandler handler = new IDListHandler(Item, false);
            handler.ReadFrom(reader);
        }

        public override void WriteTo(BinaryWriter writer)
        {
            base.WriteTo(writer);

            FormatChecker.CheckExpression(() => BlockSize >= MinimumBlockSize);
            IDListHandler handler = new IDListHandler(Item, false);
            handler.WriteTo(writer);
        }
    }
}
