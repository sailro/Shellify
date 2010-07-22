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

using System;
using System.Runtime.InteropServices;
using System.Text;
using Shellify.IO;
using Shellify.ExtraData;
using Shellify.Extensions;

namespace Shellify.IO
{
    public class TrackerDataBlockHandler : ExtraDataBlockHandler<TrackerDataBlock>
    {

        public int Length { get; set; }
        public const int MachineIDLength = 16;

        public TrackerDataBlockHandler(TrackerDataBlock item, ShellLinkFile context)
            : base(item, context)
        {
        }

        public override int ComputedSize
        {
            get
            {
                int result = base.ComputedSize +
                             Marshal.SizeOf(Length) +
                             Marshal.SizeOf(Item.Version) +
                             MachineIDLength;
                if (Item.Droid != null)
                {
                    result += Marshal.SizeOf(typeof(Guid)) * Item.Droid.Length; 
                }
                if (Item.DroidBirth != null)
                {
                    result += Marshal.SizeOf(typeof(Guid)) * Item.DroidBirth.Length;
                }
                return result;
            }
        }

        public override void ReadFrom(System.IO.BinaryReader reader)
        {
            base.ReadFrom(reader);
            Length = reader.ReadInt32();
            Item.Version = reader.ReadInt32();
            Item.MachineID = reader.ReadASCIIZF(Encoding.Default, MachineIDLength); // 16 bytes, 0 fill
            Item.Droid = new Guid[] { new Guid(reader.ReadBytes(16)), new Guid(reader.ReadBytes(16)) };
            Item.DroidBirth = new Guid[] { new Guid(reader.ReadBytes(16)), new Guid(reader.ReadBytes(16)) };
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            base.WriteTo(writer);
            Length = ComputedSize - base.ComputedSize; 
            writer.Write(Length);
            writer.Write(Item.Version);
            writer.WriteASCIIZF(Item.MachineID, Encoding.Default, MachineIDLength);
            foreach (Guid guid in Item.Droid) writer.Write(guid.ToByteArray());
            foreach (Guid guid in Item.DroidBirth) writer.Write(guid.ToByteArray());
        }

    }
}
