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
using Shellify.Core;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace Shellify.IO
{
    public class IDListHandler : IBinaryReadable, IBinaryWriteable
	{
		private IHasIDList Item;
        private bool UseIDListSize;

        public IDListHandler(IHasIDList item, bool useIDListSize)
		{
			this.Item = item;
            UseIDListSize = useIDListSize;
		}

        public void ReadFrom(BinaryReader reader)
        {
            Item.ShItemIDs = new List<ShItemID>();
            short idListSize = 0;
            if (UseIDListSize)
            {
                idListSize = reader.ReadInt16();
            }
            if (!UseIDListSize || idListSize > 0)
            {
                do
                {
                    ShItemID shitem = new ShItemID();
                    ShItemIdHandler shitemReader = new ShItemIdHandler(shitem);
                    shitemReader.ReadFrom(reader);
                    if (shitem.Data != null)
                    {
                        Item.ShItemIDs.Add(shitem);
                    }
                    else
                    {
                        break;
                    }
                } while (true);
            }
        }
	
		public void WriteTo(System.IO.BinaryWriter writer)
		{
            ushort idListSize = 0;

            idListSize = Convert.ToUInt16(Marshal.SizeOf(idListSize));
            List<ShItemIdHandler> writers = new List<ShItemIdHandler>();
            foreach (ShItemID shitem in Item.ShItemIDs)
            {
                ShItemIdHandler shitemWriter = new ShItemIdHandler(shitem);
                idListSize += Convert.ToUInt16(shitemWriter.ComputedSize);
                writers.Add(shitemWriter);
            }
            if (UseIDListSize)
            {
                writer.Write(idListSize);
            }
            writers.ForEach(handler => handler.WriteTo(writer));
            writer.Write((short)0);
        }
	}
}
