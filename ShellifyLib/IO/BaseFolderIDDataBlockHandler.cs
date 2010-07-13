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

using System.Runtime.InteropServices;
using Shellify.Core;
using Shellify.ExtraData;

namespace Shellify.IO
{
    public abstract class BaseFolderIDDataBlockHandler<T> : ExtraDataBlockHandler<T> where T:BaseFolderIDDataBlock 
    {

        private int Offset { get; set; }

        public BaseFolderIDDataBlockHandler(T item, ShellLinkFile context)
            : base(item, context)
        {
        }

        public override int ComputedSize
        {
            get
            {
                return base.ComputedSize + Marshal.SizeOf(Offset);
            }
        }

        public override void ReadFrom(System.IO.BinaryReader reader)
        {
            base.ReadFrom(reader);
            ReadID(reader);
            Offset = reader.ReadInt32();

            int computedOffset = 0;
            foreach (ShItemID shitemid in Context.ShItemIDs)
            {
                if (computedOffset == Offset)
                {
                    Item.ShItemID = shitemid;
                    break;
                }
                ShItemIdHandler handler = new ShItemIdHandler(shitemid);
                computedOffset += handler.ComputedSize;
            }
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            Offset = 0;
            foreach (ShItemID shitemid in Context.ShItemIDs)
            {
                if (Item.ShItemID == shitemid)
                {
                    break;
                }
                ShItemIdHandler handler = new ShItemIdHandler(shitemid);
                Offset += handler.ComputedSize;
            }

            base.WriteTo(writer);
            WriteID(writer);
            writer.Write(Offset);
        }

        protected abstract void ReadID(System.IO.BinaryReader reader);

        protected abstract void WriteID(System.IO.BinaryWriter writer);

    }
}
