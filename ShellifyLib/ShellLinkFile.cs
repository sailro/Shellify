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
using System.Text;
using Shellify.Core;
using Shellify.ExtraData;
using Shellify.IO;

namespace Shellify
{
	public class ShellLinkFile
	{

        public ShellLinkHeader Header { get; set; }
		public LinkInfo LinkInfo { get; set; }
		public IList<ExtraDataBlock> ExtraDataBlocks { get; set; }
		public IList<ShItemID> ShItemIDs { get; set; }
		public string Name { get; set; }
		public string RelativePath { get; set; }
		public string WorkingDirectory { get; set; }
		public string Arguments { get; set; }
		public string IconLocation { get; set; }

        public ShellLinkFile()
        {
            Header = new ShellLinkHeader();
            ExtraDataBlocks = new List<ExtraDataBlock>();
            ShItemIDs = new List<ShItemID>();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (Header != null) builder.AppendLine(Header.ToString());
            if (LinkInfo != null) builder.AppendLine(LinkInfo.ToString());
            if (ExtraDataBlocks != null)
            {
                foreach (ExtraDataBlock block in ExtraDataBlocks)
                {
                    builder.AppendLine(block.ToString());
                }
            }
            if (ShItemIDs != null)
            {
                foreach (ShItemID shitem in ShItemIDs)
                {
                    builder.AppendLine(shitem.ToString());
                }
            }
            builder.AppendLine(">>File");
            builder.AppendFormat("Name: {0}", Name); builder.AppendLine();
            builder.AppendFormat("RelativePath: {0}", RelativePath); builder.AppendLine();
            builder.AppendFormat("WorkingDirectory: {0}", WorkingDirectory); builder.AppendLine();
            builder.AppendFormat("Arguments: {0}", Arguments); builder.AppendLine();
            builder.AppendFormat("IconLocation: {0}", IconLocation); builder.AppendLine();
            return builder.ToString();
        }

		public static ShellLinkFile Load(string filename)
		{
			ShellLinkFile result = new ShellLinkFile();
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                using (BinaryReader binaryReader = new BinaryReader(stream))
                {
                    ShellLinkFileHandler reader = new ShellLinkFileHandler(result);
                    reader.ReadFrom(binaryReader);
                    return result;
                }
            }
		}
		
		public static ShellLinkFile CreateRelative(string baseDirectory, string relativeTarget)
		{
            if (Path.IsPathRooted(relativeTarget))
            {
                throw new ArgumentException("Target must be relative to base directory !!!");
            }
            
            ShellLinkFile result = new ShellLinkFile();
			
			FileInfo targetInfo = new FileInfo(Path.Combine(baseDirectory, relativeTarget));
            if (targetInfo.Exists)
            {
                result.Header.FileAttributes = targetInfo.Attributes;
                result.Header.AccessTime = targetInfo.LastAccessTime;
                result.Header.CreationTime = targetInfo.CreationTime;
                result.Header.WriteTime = targetInfo.LastWriteTime;
                result.Header.FileSize = Convert.ToInt32(targetInfo.Length);
            }
			result.Header.ShowCommand = ShowCommand.Normal;
			
			result.Header.LinkFlags = LinkFlags.HasWorkingDir | LinkFlags.HasRelativePath;
			result.RelativePath = relativeTarget;
			result.WorkingDirectory = ".";
			
			return result;
		}
		
		public static ShellLinkFile CreateAbsolute(string target)
		{
			ShellLinkFile result = new ShellLinkFile();
			
			FileInfo targetInfo = new FileInfo(target);
            if (targetInfo.Exists)
            {
                result.Header.FileAttributes = targetInfo.Attributes;
                result.Header.AccessTime = targetInfo.LastAccessTime;
                result.Header.CreationTime = targetInfo.CreationTime;
                result.Header.WriteTime = targetInfo.LastWriteTime;
                result.Header.FileSize = Convert.ToInt32(targetInfo.Length);
            }
			result.Header.ShowCommand = ShowCommand.Normal;
			
			result.Header.LinkFlags = LinkFlags.HasWorkingDir | LinkFlags.HasRelativePath;
			result.RelativePath = targetInfo.FullName;
			result.WorkingDirectory = targetInfo.DirectoryName;
			
			return result;
		}

        public void SaveAs(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(stream))
                {
                    ShellLinkFileHandler writer = new ShellLinkFileHandler(this);
                    writer.WriteTo(binaryWriter);
                }
            }
        }
		
	}
}
