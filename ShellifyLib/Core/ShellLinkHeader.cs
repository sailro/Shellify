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
            var builder = new StringBuilder();
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
