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

using System;
using System.Text;
using System.Collections;

namespace Shellify.ExtraData
{
	public class TrackerDataBlock : ExtraDataBlock
	{
		
		public int Version { get; set; }
		public string MachineID;
		public Guid[] Droid { get; set; }
		public Guid[] DroidBirth { get; set; }
		
		public TrackerDataBlock()
		{
			Signature = ExtraDataBlockSignature.TrackerDataBlock;
            Droid = new Guid[2];
            DroidBirth = new Guid[2];
        }

        private static string ToString(IEnumerable collection)
        {
            var builder = new StringBuilder();
            builder.Append("{");
            foreach (var item in collection)
            {
                if (builder.Length > 1)
                {
                    builder.Append(",");
                }
                builder.Append(item);
            }
            builder.Append("}");
            return builder.ToString();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(base.ToString());
            builder.AppendFormat("Version: {0}", Version); builder.AppendLine();
            builder.AppendFormat("MachineID: {0}", MachineID); builder.AppendLine();
            builder.AppendFormat("Droid:\n{0}", ToString(Droid)); builder.AppendLine();
            builder.AppendFormat("DroidBirth:\n{0}", ToString(DroidBirth));
            return builder.ToString();
        }
		
	}
}
