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
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Shellify.Core;
using Shellify.Extensions;
using Shellify.ExtraData;

namespace Shellify.IO
{
    public class ShellLinkFileHandler : IBinaryReadable, IBinaryWriteable
    {

        private ShellLinkFile Item { get; set; }

        public ShellLinkFileHandler(ShellLinkFile item)
        {
            this.Item = item;
        }

        private string ReadStringData(BinaryReader reader, LinkFlags mask)
        {
            Encoding enc = ((Item.Header.LinkFlags & LinkFlags.IsUnicode) != 0) ? Encoding.Unicode : Encoding.Default;
            if ((Item.Header.LinkFlags & mask) != 0)
            {
                return reader.ReadSTDATA(enc);
            }
            return null;
        }

        public void ReadFrom(System.IO.BinaryReader reader)
        {
            ReadHeaderSection(reader);
            ReadIDListSection(reader);
            ReadLinkInfoSection(reader);
            ReadStringDataSection(reader);
            ReadExtraDataSection(reader);
        }

        public void WriteTo(System.IO.BinaryWriter writer)
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
            ShellLinkHeaderHandler slhReader = new ShellLinkHeaderHandler(Item.Header);
            slhReader.ReadFrom(reader);
        }

        private void ReadIDListSection(BinaryReader reader)
        {
            Item.ShItemIDs = new List<ShItemID>();
            if ((Item.Header.LinkFlags & LinkFlags.HasLinkTargetIDList) != 0)
            {
                IDListHandler idlhandler = new IDListHandler(Item, true);
                idlhandler.ReadFrom(reader);
            }
        }

        private void ReadLinkInfoSection(BinaryReader reader)
        {
            if ((Item.Header.LinkFlags & LinkFlags.HasLinkInfo) != 0)
            {
                Item.LinkInfo = new LinkInfo();
                LinkInfoHandler liReader = new LinkInfoHandler(Item.LinkInfo);
                liReader.ReadFrom(reader);
            }
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
                int blocksize = reader.ReadInt32();
                if (blocksize < 0x4) // Terminal Block
                {
                    break;
                }

                ExtraDataBlockSignature signature = (ExtraDataBlockSignature)(reader.ReadInt32());
                ExtraDataBlock block = ExtraDataBlockFactory.GetInstance(signature);
                Item.ExtraDataBlocks.Add(block);

                ExtraDataBlockHandler blockReader = ExtraDataBlockHandlerFactory.GetInstance(block, Item);
                reader.BaseStream.Seek(- Marshal.SizeOf(blocksize) - Marshal.SizeOf(typeof(int)), SeekOrigin.Current);
                blockReader.ReadFrom(reader);
            }
        }

        private void WriteHeaderSection(BinaryWriter writer)
        {
            ShellLinkHeaderHandler slhWriter = new ShellLinkHeaderHandler(Item.Header);
            slhWriter.WriteTo(writer);
        }

        private void WriteIDListSection(BinaryWriter writer)
        {
            if ((Item.Header.LinkFlags & LinkFlags.HasLinkTargetIDList) != 0)
            {
                IDListHandler idlhandler = new IDListHandler(Item, true);
                idlhandler.WriteTo(writer);
            }
        }

        private void WriteLinkInfoSection(BinaryWriter writer)
        {
            if ((Item.Header.LinkFlags & LinkFlags.HasLinkInfo) != 0)
            {
                LinkInfoHandler liWriter = new LinkInfoHandler(Item.LinkInfo);
                liWriter.WriteTo(writer);
            }
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
            foreach (ExtraDataBlock block in Item.ExtraDataBlocks)
            {
                ExtraDataBlockHandler blockWriter = ExtraDataBlockHandlerFactory.GetInstance(block, Item);
                blockWriter.WriteTo(writer);
            }
            writer.Write((int) 0);
        }

        private void WriteStringData(string value, BinaryWriter writer, LinkFlags mask)
        {
            Encoding enc = ((Item.Header.LinkFlags & LinkFlags.IsUnicode) != 0) ? Encoding.Unicode : Encoding.Default;
            if ((Item.Header.LinkFlags & mask) != 0)
            {
                writer.WriteSTDATA(value, enc);
            }
        }

    }
}
