/* Shellify Copyright (c) 2010-2021 Sebastien Lebreton

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
			get => _linkInfo;
			set
			{
				_linkInfo = value;
				UpdateHeaderFlags(value, LinkFlags.HasLinkInfo);
			}
		}

		private string _name;

		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				UpdateHeaderFlags(value, LinkFlags.HasName);
			}
		}

		private string _relativePath;

		public string RelativePath
		{
			get => _relativePath;
			set
			{
				_relativePath = value;
				UpdateHeaderFlags(value, LinkFlags.HasRelativePath);
			}
		}

		private string _workingDirectory;

		public string WorkingDirectory
		{
			get => _workingDirectory;
			set
			{
				_workingDirectory = value;
				UpdateHeaderFlags(value, LinkFlags.HasWorkingDir);
			}
		}

		private string _arguments;

		public string Arguments
		{
			get => _arguments;
			set
			{
				_arguments = value;
				UpdateHeaderFlags(value, LinkFlags.HasArguments);
			}
		}

		private string _iconLocation;

		public string IconLocation
		{
			get => _iconLocation;
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
			if (item is string s && string.IsNullOrEmpty(s) || item == null)
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
			var builder = new StringBuilder();
			if (Header != null) builder.AppendLine(Header.ToString());
			if (LinkInfo != null) builder.AppendLine(LinkInfo.ToString());

			if (ExtraDataBlocks != null)
			{
				foreach (var block in ExtraDataBlocks)
					builder.AppendLine(block.ToString());
			}

			if (ShItemIDs != null)
			{
				foreach (var shitem in ShItemIDs)
					builder.AppendLine(shitem.ToString());
			}

			builder.AppendLine(">>File");
			builder.AppendFormat("Name: {0}", Name);
			builder.AppendLine();
			builder.AppendFormat("RelativePath: {0}", RelativePath);
			builder.AppendLine();
			builder.AppendFormat("WorkingDirectory: {0}", WorkingDirectory);
			builder.AppendLine();
			builder.AppendFormat("Arguments: {0}", Arguments);
			builder.AppendLine();
			builder.AppendFormat("IconLocation: {0}", IconLocation);
			builder.AppendLine();
			return builder.ToString();
		}

		public static ShellLinkFile Load(string filename)
		{
			var result = new ShellLinkFile();
			
			using var stream = File.OpenRead(filename);
			using var binaryReader = new BinaryReader(stream);
			
			var reader = new ShellLinkFileHandler(result);
			reader.ReadFrom(binaryReader);
			return result;
		}

		private static FileSystemInfo SetFileSystemInfo(ShellLinkFile slf, string target)
		{
			var targetInfo = Directory.Exists(target) ? (FileSystemInfo)new DirectoryInfo(target) : new FileInfo(target);

			if (!targetInfo.Exists)
				return targetInfo;

			slf.Header.FileAttributes = targetInfo.Attributes;
			slf.Header.AccessTime = targetInfo.LastAccessTime;
			slf.Header.CreationTime = targetInfo.CreationTime;
			slf.Header.WriteTime = targetInfo.LastWriteTime;

			if (targetInfo is FileInfo info)
				slf.Header.FileSize = Convert.ToInt32(info.Length);

			return targetInfo;
		}

		public static ShellLinkFile CreateRelative(string baseDirectory, string relativeTarget)
		{
			if (Path.IsPathRooted(relativeTarget))
				throw new ArgumentException("Target must be relative to base directory !!!");

			var result = new ShellLinkFile();

			SetFileSystemInfo(result, Path.Combine(baseDirectory, relativeTarget));
			result.Header.ShowCommand = ShowCommand.Normal;

			result.RelativePath = relativeTarget;
			result.WorkingDirectory = ".";

			return result;
		}

		public static ShellLinkFile CreateAbsolute(string target)
		{
			var result = new ShellLinkFile();

			var targetInfo = SetFileSystemInfo(result, target);
			result.Header.ShowCommand = ShowCommand.Normal;

			result.RelativePath = targetInfo.FullName;
			result.WorkingDirectory = targetInfo is FileInfo info ? info.DirectoryName : targetInfo.FullName;

			return result;
		}

		public void SaveAs(string filename)
		{
			using var stream = new FileStream(filename, FileMode.Create);
			using var binaryWriter = new BinaryWriter(stream);

			var writer = new ShellLinkFileHandler(this);
			writer.WriteTo(binaryWriter);
		}
	}
}
