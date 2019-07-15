/* Shellify Copyright (c) 2010-2019 Sebastien Lebreton

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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Shellify.Core;
using Shellify.Extensions;
using Shellify.ExtraData;

namespace Shellify.IO
{
    public class ShellLinkFileHandler : IBinaryReadable, IBinaryWriteable
    {

        private ShellLinkFile Item { get; }

        public ShellLinkFileHandler(ShellLinkFile item)
        {
            Item = item;
        }

        private string ReadStringData(BinaryReader reader, LinkFlags mask)
        {
            var enc = (Item.Header.LinkFlags & LinkFlags.IsUnicode) != 0 ? Encoding.Unicode : Encoding.Default;
            return (Item.Header.LinkFlags & mask) != 0 ? reader.ReadSTDATA(enc) : null;
        }

        public void ReadFrom(BinaryReader reader)
        {
            ReadHeaderSection(reader);
            ReadIDListSection(reader);
            ReadLinkInfoSection(reader);
            ReadStringDataSection(reader);
            ReadExtraDataSection(reader);
        }

        public void WriteTo(BinaryWriter writer)
        {
            WriteHeaderSection(writer);
            WriteIDListSection(writer);
            WriteLinkInfoSection(writer);
            WriteStringDataSection(writer);
            WriteExtraDataSection(writer);
        }

        private void ReadHeaderSection(BinaryReader reader)
        {
            Item.Header = new ShellLinkHeader();
            var slhReader = new ShellLinkHeaderHandler(Item.Header);
            slhReader.ReadFrom(reader);
        }

        private void ReadIDListSection(BinaryReader reader)
        {
            Item.ShItemIDs = new List<ShItemID>();
            if ((Item.Header.LinkFlags & LinkFlags.HasLinkTargetIDList) == 0)
	            return;

            var idlhandler = new IDListHandler(Item, true);
            idlhandler.ReadFrom(reader);
        }

        private void ReadLinkInfoSection(BinaryReader reader)
        {
	        if ((Item.Header.LinkFlags & LinkFlags.HasLinkInfo) == 0)
		        return;

	        Item.LinkInfo = new LinkInfo();
            var liReader = new LinkInfoHandler(Item.LinkInfo);
            liReader.ReadFrom(reader);
        }

        private void ReadStringDataSection(BinaryReader reader)
        {
            Item.Name = ReadStringData(reader, LinkFlags.HasName);
            Item.RelativePath = ReadStringData(reader, LinkFlags.HasRelativePath);
            Item.WorkingDirectory = ReadStringData(reader, LinkFlags.HasWorkingDir);
            Item.Arguments = ReadStringData(reader, LinkFlags.HasArguments);
            Item.IconLocation = ReadStringData(reader, LinkFlags.HasIconLocation);
        }

        private void ReadExtraDataSection(BinaryReader reader)
        {
            Item.ExtraDataBlocks = new List<ExtraDataBlock>();
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var blocksize = reader.ReadInt32();
                if (blocksize < 0x4) // Terminal Block
                    break;

                var signature = (ExtraDataBlockSignature)(reader.ReadInt32());
                var block = ExtraDataBlockFactory.GetInstance(signature);
                Item.ExtraDataBlocks.Add(block);

                var blockReader = ExtraDataBlockHandlerFactory.GetInstance(block, Item);
                reader.BaseStream.Seek(- Marshal.SizeOf(blocksize) - Marshal.SizeOf(typeof(int)), SeekOrigin.Current);
                blockReader.ReadFrom(reader);
            }
        }

        private void WriteHeaderSection(BinaryWriter writer)
        {
            var slhWriter = new ShellLinkHeaderHandler(Item.Header);
            slhWriter.WriteTo(writer);
        }

        private void WriteIDListSection(BinaryWriter writer)
        {
	        if ((Item.Header.LinkFlags & LinkFlags.HasLinkTargetIDList) == 0)
		        return;

	        var idlhandler = new IDListHandler(Item, true);
            idlhandler.WriteTo(writer);
        }

        private void WriteLinkInfoSection(BinaryWriter writer)
        {
	        if ((Item.Header.LinkFlags & LinkFlags.HasLinkInfo) == 0)
		        return;

	        var liWriter = new LinkInfoHandler(Item.LinkInfo);
            liWriter.WriteTo(writer);
        }

        private void WriteStringDataSection(BinaryWriter writer)
        {
            WriteStringData(Item.Name, writer, LinkFlags.HasName);
            WriteStringData(Item.RelativePath, writer, LinkFlags.HasRelativePath);
            WriteStringData(Item.WorkingDirectory, writer, LinkFlags.HasWorkingDir);
            WriteStringData(Item.Arguments, writer, LinkFlags.HasArguments);
            WriteStringData(Item.IconLocation, writer, LinkFlags.HasIconLocation);
        }

        private void WriteExtraDataSection(BinaryWriter writer)
        {
	        foreach (var blockWriter in Item.ExtraDataBlocks.Select(block => ExtraDataBlockHandlerFactory.GetInstance(block, Item)))
		        blockWriter.WriteTo(writer);

			writer.Write(0);
        }

	    private void WriteStringData(string value, BinaryWriter writer, LinkFlags mask)
        {
            var enc = ((Item.Header.LinkFlags & LinkFlags.IsUnicode) != 0) ? Encoding.Unicode : Encoding.Default;
            if ((Item.Header.LinkFlags & mask) != 0)
                writer.WriteSTDATA(value, enc);
        }

    }
}
