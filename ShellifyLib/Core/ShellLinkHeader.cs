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
using System.Text;
using System.Windows.Forms;

namespace Shellify.Core
{
	public class ShellLinkHeader
	{
        public const string LNKGuid = "00021401-0000-0000-c000-000000000046";
		
		public Guid Guid { get; set; }
		public LinkFlags LinkFlags { get; set; }
		public FileAttributes FileAttributes { get; set; }
		public DateTime CreationTime { get; set; }
		public DateTime AccessTime { get; set; }
		public DateTime WriteTime { get; set; }
		public int FileSize { get; set; }
		public int IconIndex { get; set; }
		public ShowCommand ShowCommand { get; set; }
		public Keys HotKey { get; set; }

        public ShellLinkHeader()
        {
            Guid = new Guid(LNKGuid);
            CreationTime = DateTime.Now;
            AccessTime = DateTime.Now;
            WriteTime = DateTime.Now;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(">> Header");
            builder.AppendFormat("Guid: {0}", Guid); builder.AppendLine();
            builder.AppendFormat("LinkFlags: {0}", LinkFlags); builder.AppendLine();
            builder.AppendFormat("FileAttributes: {0}", FileAttributes); builder.AppendLine();
            builder.AppendFormat("CreationTime: {0}", CreationTime); builder.AppendLine();
            builder.AppendFormat("AccessTime: {0}", AccessTime); builder.AppendLine();
            builder.AppendFormat("WriteTime: {0}", WriteTime); builder.AppendLine();
            builder.AppendFormat("FileSize: {0}", FileSize); builder.AppendLine();
            builder.AppendFormat("IconIndex: {0}", IconIndex); builder.AppendLine();
            builder.AppendFormat("ShowCommand: {0}", ShowCommand); builder.AppendLine();
            builder.AppendFormat("HotKey: {0}", HotKey);
            return builder.ToString();
        }
		
	}
}
