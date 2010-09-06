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
using System.Runtime.InteropServices;
using System.Text;
using Shellify.IO;
using Shellify.ExtraData;
using Shellify.Extensions;

namespace Shellify.IO
{
    public class TrackerDataBlockHandler : ExtraDataBlockHandler<TrackerDataBlock>
    {
        public const int ExactBlockSize = 0x60;
        public const int MinimumLength = 0x58;

        public int Length { get; set; }
        public const int MachineIDLength = 16;

        public TrackerDataBlockHandler(TrackerDataBlock item, ShellLinkFile context)
            : base(item, context)
        {
        }

        public override int ComputedSize
        {
            get
            {
                int result = base.ComputedSize +
                             Marshal.SizeOf(Length) +
                             Marshal.SizeOf(Item.Version) +
                             MachineIDLength +
                             Marshal.SizeOf(typeof(Guid)) * 4; 
                return result;
            }
        }

        public override void ReadFrom(System.IO.BinaryReader reader)
        {
            base.ReadFrom(reader);

            FormatChecker.CheckExpression(() => BlockSize == ExactBlockSize);
            
            Length = reader.ReadInt32();
            FormatChecker.CheckExpression(() => Length >= MinimumLength);

            Item.Version = reader.ReadInt32();
            Item.MachineID = reader.ReadASCIIZF(Encoding.Default, MachineIDLength); // 16 bytes, 0 fill
            Item.Droid = new Guid[] { new Guid(reader.ReadBytes(16)), new Guid(reader.ReadBytes(16)) };
            Item.DroidBirth = new Guid[] { new Guid(reader.ReadBytes(16)), new Guid(reader.ReadBytes(16)) };
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            base.WriteTo(writer);

            FormatChecker.CheckExpression(() => Item.MachineID == null || Item.MachineID.Length <= MachineIDLength);
            FormatChecker.CheckExpression(() => BlockSize == ExactBlockSize);
            FormatChecker.CheckExpression(() => Item.Droid != null && Item.Droid.Length == 2);
            FormatChecker.CheckExpression(() => Item.DroidBirth != null && Item.DroidBirth.Length == 2);

            Length = ComputedSize - base.ComputedSize;
            FormatChecker.CheckExpression(() => Length >= MinimumLength);

            writer.Write(Length);
            writer.Write(Item.Version);
            writer.WriteASCIIZF(Item.MachineID, Encoding.Default, MachineIDLength);
            foreach (Guid guid in Item.Droid) writer.Write(guid.ToByteArray());
            foreach (Guid guid in Item.DroidBirth) writer.Write(guid.ToByteArray());
        }

    }
}
