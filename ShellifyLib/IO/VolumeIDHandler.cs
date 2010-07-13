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

namespace Shellify.IO
{
    public class VolumeIDHandler : IBinaryReadable, IBinaryWriteable, ISizeComputable
	{
		
		private VolumeID Item;
		private int VolumeIDSize { get; set; }
		private int VolumeLabelOffset { get; set; }
		private int? VolumeLabelOffsetUnicode { get; set; } // Optional
		
		public VolumeIDHandler(VolumeID item)
		{
			this.Item = item;
		}
		
		public void ReadFrom(BinaryReader reader)
		{
			long readerOffset = reader.BaseStream.Position;
			
			VolumeIDSize = reader.ReadInt32();
			Item.DriveType = (DriveType) (reader.ReadInt32());
			Item.DriveSerialNumber = reader.ReadInt32();
			VolumeLabelOffset = reader.ReadInt32();
			
			if (VolumeLabelOffset == 0x14)
			{
				VolumeLabelOffsetUnicode = reader.ReadInt32();
			}
			
			Item.VolumeLabel = IOHelper.ReadASCIIZ(reader, readerOffset, VolumeLabelOffset, VolumeLabelOffsetUnicode);
		}
		
		public int ComputedSize
		{
			get
			{
                return Marshal.SizeOf(VolumeIDSize) +
                Marshal.SizeOf(typeof(int)) +
                Marshal.SizeOf(Item.DriveSerialNumber) +
                Marshal.SizeOf(VolumeLabelOffset) +
                IOHelper.GetASCIIZSize(Item.VolumeLabel, Encoding.Default);

                // TODO: Handle unicode strings and offsets
                // VolumeLabelOffsetUnicode = 
                // VolumeLabelOffset = &H14 
			}
		}
		
		public void WriteTo(System.IO.BinaryWriter writer)
		{
			VolumeIDSize = ComputedSize;
			writer.Write(VolumeIDSize);
			writer.Write((int) Item.DriveType);
			writer.Write(Item.DriveSerialNumber);
			
			VolumeLabelOffset = Marshal.SizeOf(VolumeIDSize) +
			Marshal.SizeOf(typeof(int)) +
			Marshal.SizeOf(Item.DriveSerialNumber) +
			Marshal.SizeOf(VolumeLabelOffset);
			writer.Write(VolumeLabelOffset);

            // TODO: Handle unicode strings and offsets
            // VolumeLabelOffsetUnicode = 
            // VolumeLabelOffset = &H14 
			
			IOHelper.WriteASCIIZ(Item.VolumeLabel, writer, Encoding.Default);
		}
	}
}
