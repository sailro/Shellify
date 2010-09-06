/* Shellify Copyright (c) 2010 Sebastien LEBRETON

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
using System.Runtime.InteropServices;
using System.Text;
using Shellify.Core;
using Shellify.Extensions;

namespace Shellify.IO
{
    public class VolumeIDHandler : IBinaryReadable, IBinaryWriteable, ISizeComputable
	{
        public const int MinimumVolumeIDSize = 0x10;
		
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
            FormatChecker.CheckExpression(() => VolumeIDSize > MinimumVolumeIDSize);

			Item.DriveType = (DriveType) (reader.ReadInt32());
			Item.DriveSerialNumber = reader.ReadInt32();
			VolumeLabelOffset = reader.ReadInt32();

            if (VolumeLabelOffset > MinimumVolumeIDSize)
            {
                VolumeLabelOffsetUnicode = reader.ReadInt32();
            }
            else
            {
                FormatChecker.CheckExpression(() => VolumeLabelOffset == MinimumVolumeIDSize);
            }

            Item.VolumeLabel = reader.ReadASCIIZ(readerOffset, VolumeLabelOffset, VolumeLabelOffsetUnicode);
		}
		
		public int ComputedSize
		{
			get
			{
                return Marshal.SizeOf(VolumeIDSize) +
                Marshal.SizeOf(typeof(int)) +
                Marshal.SizeOf(Item.DriveSerialNumber) +
                Marshal.SizeOf(VolumeLabelOffset) +
                Encoding.Default.GetASCIIZSize(Item.VolumeLabel);

                // TODO: Handle unicode strings and offsets
                // VolumeLabelOffsetUnicode = 
                // VolumeLabelOffset = &H14 
			}
		}
		
		public void WriteTo(System.IO.BinaryWriter writer)
		{
			VolumeIDSize = ComputedSize;

            FormatChecker.CheckExpression(() => VolumeIDSize > MinimumVolumeIDSize);

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

            writer.WriteASCIIZ(Item.VolumeLabel, Encoding.Default);
		}
	}
}
