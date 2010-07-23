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
	public class ShellLinkFile : IHasIDList
	{

        public ShellLinkHeader Header { get; set; }
		public IList<ExtraDataBlock> ExtraDataBlocks { get; set; }
		public IList<ShItemID> ShItemIDs { get; set; }

        private LinkInfo _linkInfo;
        public LinkInfo LinkInfo
        {
            get
            {
                return _linkInfo;
            }
            set
            {
                _linkInfo = value;
                UpdateHeaderFlags(value, LinkFlags.HasLinkInfo);
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                UpdateHeaderFlags(value, LinkFlags.HasName);
            }
        }

        private string _relativePath;
        public string RelativePath
        {
            get
            {
                return _relativePath;
            }
            set
            {
                _relativePath = value;
                UpdateHeaderFlags(value, LinkFlags.HasRelativePath);
            }
        }

        private string _workingDirectory;
        public string WorkingDirectory
        {
            get
            {
                return _workingDirectory;
            }
            set
            {
                _workingDirectory = value;
                UpdateHeaderFlags(value, LinkFlags.HasWorkingDir);
            }
        }

        private string _arguments;
        public string Arguments
        {
            get
            {
                return _arguments;
            }
            set
            {
                _arguments = value;
                UpdateHeaderFlags(value, LinkFlags.HasArguments);
            }
        }

        private string _iconLocation;
        public string IconLocation 
        {
            get
            {
                return _iconLocation;
            }
            set
            {
                _iconLocation = value;
                UpdateHeaderFlags(value, LinkFlags.HasIconLocation);
            }
        }

        public ShellLinkFile()
        {
            Header = new ShellLinkHeader();
            ExtraDataBlocks = new List<ExtraDataBlock>();
            ShItemIDs = new List<ShItemID>();
        }

        private void UpdateHeaderFlags(object item, LinkFlags flag)
        {
            if (((item is string) && string.IsNullOrEmpty(item as string)) || (item == null))
            {
                Header.LinkFlags &= ~flag;
            }
            else
            {
                Header.LinkFlags |= flag;
            }
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

        public static FileSystemInfo SetFileSystemInfo(ShellLinkFile slf, string target)
        {
            FileSystemInfo targetInfo;
            if (Directory.Exists(target))
            {
                targetInfo = new DirectoryInfo(target);
            }
            else
            {
                targetInfo = new FileInfo(target);
            }

            if (targetInfo.Exists)
            {
                slf.Header.FileAttributes = targetInfo.Attributes;
                slf.Header.AccessTime = targetInfo.LastAccessTime;
                slf.Header.CreationTime = targetInfo.CreationTime;
                slf.Header.WriteTime = targetInfo.LastWriteTime;
                if (targetInfo is FileInfo)
                {
                    slf.Header.FileSize = Convert.ToInt32((targetInfo as FileInfo).Length);
                }
            }
            return targetInfo;
        }

        public static ShellLinkFile CreateRelative(string baseDirectory, string relativeTarget)
        {
            if (Path.IsPathRooted(relativeTarget))
            {
                throw new ArgumentException("Target must be relative to base directory !!!");
            }

            ShellLinkFile result = new ShellLinkFile();

            SetFileSystemInfo(result, Path.Combine(baseDirectory, relativeTarget));
            result.Header.ShowCommand = ShowCommand.Normal;

            result.RelativePath = relativeTarget;
            result.WorkingDirectory = ".";

            return result;
        }

        public static ShellLinkFile CreateAbsolute(string target)
        {
            ShellLinkFile result = new ShellLinkFile();

            FileSystemInfo targetInfo = SetFileSystemInfo(result, target);
            result.Header.ShowCommand = ShowCommand.Normal;

            result.RelativePath = targetInfo.FullName;
            if (targetInfo is FileInfo)
            {
                result.WorkingDirectory = (targetInfo as FileInfo).DirectoryName;
            }
            else
            {
                result.WorkingDirectory = targetInfo.FullName;
            }

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
