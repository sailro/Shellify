/* Shellify Copyright (c) 2010-2013 Sebastien LEBRETON

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
        public const int MinimumLinkInfoHeaderSize = 0x1C;
		
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
			Item = item;
		}

        private void EnsurePosition(BinaryReader reader, long baseOffset, int offset)
        {
            var delta = (int)(baseOffset + offset - reader.BaseStream.Position);
            if (delta > 0)
                reader.ReadBytes(delta);
            else if (delta < 0)
                throw new ArgumentException();
		}
		
		public void ReadFrom(BinaryReader reader)
		{
			var readerOffset = reader.BaseStream.Position;
			
			LinkInfoSize = reader.ReadInt32();
			LinkInfoHeaderSize = reader.ReadInt32();
            FormatChecker.CheckExpression(() => LinkInfoHeaderSize < LinkInfoSize);

            Item.LinkInfoFlags = (LinkInfoFlags) (reader.ReadInt32());
			
			VolumeIDOffset = reader.ReadInt32();
            FormatChecker.CheckExpression(() => VolumeIDOffset < LinkInfoSize);
            
            LocalBasePathOffset = reader.ReadInt32();
            FormatChecker.CheckExpression(() => LocalBasePathOffset < LinkInfoSize);
            
            CommonNetworkRelativeLinkOffset = reader.ReadInt32();
            FormatChecker.CheckExpression(() => CommonNetworkRelativeLinkOffset < LinkInfoSize);
            
            CommonPathSuffixOffset = reader.ReadInt32();
            FormatChecker.CheckExpression(() => CommonPathSuffixOffset < LinkInfoSize);

            if (LinkInfoHeaderSize > MinimumLinkInfoHeaderSize)
            {
                LocalBasePathOffsetUnicode = reader.ReadInt32();
                FormatChecker.CheckExpression(() => LocalBasePathOffsetUnicode < LinkInfoSize);

                CommonPathSuffixOffsetUnicode = reader.ReadInt32();
                FormatChecker.CheckExpression(() => CommonPathSuffixOffsetUnicode < LinkInfoSize);
            }
            else
                FormatChecker.CheckExpression(() => LinkInfoHeaderSize == MinimumLinkInfoHeaderSize);
			
			if ((Item.LinkInfoFlags & LinkInfoFlags.VolumeIDAndLocalBasePath) != 0)
			{
                EnsurePosition(reader, readerOffset, VolumeIDOffset);
				Item.VolumeID = new VolumeID();
				var vidReader = new VolumeIDHandler(Item.VolumeID);
				vidReader.ReadFrom(reader);
                Item.LocalBasePath = reader.ReadASCIIZ(readerOffset, LocalBasePathOffset, LocalBasePathOffsetUnicode);
			} 
            
            if ((Item.LinkInfoFlags & LinkInfoFlags.CommonNetworkRelativeLinkAndPathSuffix) != 0)
			{
                EnsurePosition(reader, readerOffset, CommonNetworkRelativeLinkOffset);
				Item.CommonNetworkRelativeLink = new CommonNetworkRelativeLink();
				var cnrlReader = new CommonNetworkRelativeLinkHandler(Item.CommonNetworkRelativeLink);
				cnrlReader.ReadFrom(reader);
			}

            Item.CommonPathSuffix = reader.ReadASCIIZ(readerOffset, CommonPathSuffixOffset, CommonPathSuffixOffsetUnicode);
		}
		
		public void WriteTo(BinaryWriter writer)
		{
            var padding = new byte[]{};

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
                writer.Write(padding);

			if (cnrWriter != null)
				cnrWriter.WriteTo(writer);

			writer.WriteASCIIZ(Item.CommonPathSuffix, Encoding.Default);
			
		}
	}
}
