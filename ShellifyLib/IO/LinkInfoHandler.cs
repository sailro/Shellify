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
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Shellify.Core;
using Shellify.Extensions;

namespace Shellify.IO
{
	public class LinkInfoHandler : IBinaryReadable, IBinaryWriteable
	{
		
		private LinkInfo Item { get; set; }
		private int LinkInfoSize { get; set; }
		private int LinkInfoHeaderSize { get; set; }
		private int VolumeIDOffset { get; set; }
		private int LocalBasePathOffset { get; set; }
		private int CommonNetworkRelativeLinkOffset { get; set; }
		private int CommonPathSuffixOffset { get; set; }
		
		private int? LocalBasePathOffsetUnicode { get; set; } // Optional
		private int? CommonPathSuffixOffsetUnicode { get; set; } // Optional
		
		public LinkInfoHandler(LinkInfo item)
		{
			this.Item = item;
		}

        private void EnsurePosition(BinaryReader reader, long baseOffset, int offset)
        {
            int delta = (int)(baseOffset + offset - reader.BaseStream.Position);
            if (delta > 0)
            {
                byte[] padding = reader.ReadBytes(delta);
            }
            else if (delta < 0)
            {
                throw new ArgumentException();
            }
        }
		
		public void ReadFrom(BinaryReader reader)
		{
			long readerOffset = reader.BaseStream.Position;
			
			LinkInfoSize = reader.ReadInt32();
			LinkInfoHeaderSize = reader.ReadInt32();
			Item.LinkInfoFlags = (LinkInfoFlags) (reader.ReadInt32());
			
			VolumeIDOffset = reader.ReadInt32();
			LocalBasePathOffset = reader.ReadInt32();
			CommonNetworkRelativeLinkOffset = reader.ReadInt32();
			CommonPathSuffixOffset = reader.ReadInt32();
			
			if (LinkInfoHeaderSize >= 0x24)
			{
				LocalBasePathOffsetUnicode = reader.ReadInt32();
				CommonPathSuffixOffsetUnicode = reader.ReadInt32();
			}
			
			if ((Item.LinkInfoFlags & LinkInfoFlags.VolumeIDAndLocalBasePath) != 0)
			{
                EnsurePosition(reader, readerOffset, VolumeIDOffset);
				Item.VolumeID = new VolumeID();
				VolumeIDHandler vidReader = new VolumeIDHandler(Item.VolumeID);
				vidReader.ReadFrom(reader);
                Item.LocalBasePath = reader.ReadASCIIZ(readerOffset, LocalBasePathOffset, LocalBasePathOffsetUnicode);
			} 
            
            if ((Item.LinkInfoFlags & LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix) != 0)
			{
                EnsurePosition(reader, readerOffset, CommonNetworkRelativeLinkOffset);
				Item.CommonNetworkRelativeLink = new CommonNetworkRelativeLink();
				CommonNetworkRelativeLinkHandler cnrlReader = new CommonNetworkRelativeLinkHandler(Item.CommonNetworkRelativeLink);
				cnrlReader.ReadFrom(reader);
			}

            Item.CommonPathSuffix = reader.ReadASCIIZ(readerOffset, CommonPathSuffixOffset, CommonPathSuffixOffsetUnicode);
		}
		
		public void WriteTo(System.IO.BinaryWriter writer)
		{
            byte[] padding = new byte[]{};

            LinkInfoHeaderSize = Marshal.SizeOf(LinkInfoSize) +
			Marshal.SizeOf(LinkInfoHeaderSize) +
			Marshal.SizeOf(typeof(int)) +
			Marshal.SizeOf(VolumeIDOffset) +
			Marshal.SizeOf(LocalBasePathOffset) +
			Marshal.SizeOf(CommonNetworkRelativeLinkOffset) +
			Marshal.SizeOf(CommonPathSuffixOffset);

            LinkInfoSize = LinkInfoHeaderSize + Encoding.Default.GetASCIIZSize(Item.CommonPathSuffix);
			
			VolumeIDHandler vidWriter = null;
            CommonNetworkRelativeLinkHandler cnrWriter = null;

            VolumeIDOffset = 0;
            LocalBasePathOffset = 0;
            CommonNetworkRelativeLinkOffset = 0;
            int nextBlockOffset = LinkInfoHeaderSize;

            if ((Item.LinkInfoFlags & (LinkInfoFlags.VolumeIDAndLocalBasePath | LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix)) == (LinkInfoFlags.VolumeIDAndLocalBasePath | LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix))
            {
                padding = new byte[] {0};
                LinkInfoSize += padding.Length;
            }

            if ((Item.LinkInfoFlags & LinkInfoFlags.VolumeIDAndLocalBasePath) != 0)
			{
				vidWriter = new VolumeIDHandler(Item.VolumeID);
                LinkInfoSize += vidWriter.ComputedSize + Encoding.Default.GetASCIIZSize(Item.LocalBasePath);
                VolumeIDOffset = nextBlockOffset;
				LocalBasePathOffset = VolumeIDOffset + vidWriter.ComputedSize;
                CommonPathSuffixOffset = LocalBasePathOffset + Encoding.Default.GetASCIIZSize(Item.LocalBasePath);
                nextBlockOffset = CommonPathSuffixOffset + padding.Length;
			} 
            
            if ((Item.LinkInfoFlags & LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix) != 0)
			{
				cnrWriter = new CommonNetworkRelativeLinkHandler(Item.CommonNetworkRelativeLink);
                LinkInfoSize += cnrWriter.ComputedSize;
                CommonNetworkRelativeLinkOffset = nextBlockOffset;
				CommonPathSuffixOffset = CommonNetworkRelativeLinkOffset + cnrWriter.ComputedSize;
			}

            writer.Write(LinkInfoSize);
            writer.Write(LinkInfoHeaderSize);
            writer.Write((int)Item.LinkInfoFlags);

			writer.Write(VolumeIDOffset);
			writer.Write(LocalBasePathOffset);
			writer.Write(CommonNetworkRelativeLinkOffset);
			writer.Write(CommonPathSuffixOffset);
			
            // TODO: Handle unicode strings and offsets
			// LocalBasePathOffsetUnicode = 
			// CommonPathSuffixOffsetUnicode = 
            // LinkInfoHeaderSize >= &H24 
			
			if (vidWriter != null)
			{
				vidWriter.WriteTo(writer);
                writer.WriteASCIIZ(Item.LocalBasePath, Encoding.Default);
			}
            if (padding.Length > 0)
            {
                writer.Write(padding);
            }
            if (cnrWriter != null)
			{
				cnrWriter.WriteTo(writer);
			}
            writer.WriteASCIIZ(Item.CommonPathSuffix, Encoding.Default);
			
		}
	}
}
