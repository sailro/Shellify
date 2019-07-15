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

using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Shellify.Core;
using Shellify.Extensions;

namespace Shellify.IO
{
    public class CommonNetworkRelativeLinkHandler : IBinaryReadable, IBinaryWriteable, ISizeComputable
	{
        public const int MinimumCommonNetworkRelativeLinkSize = 0x14;
		
		private CommonNetworkRelativeLink Item { get; set; }
		private int CommonNetworkRelativeLinkSize { get; set; }
		private int NetNameOffset { get; set; }
		private int DeviceNameOffset { get; set; }

        private int? NetNameOffsetUnicode { get; set; } // Optional
		private int? DeviceNameOffsetUnicode { get; set; } // Optional
		
		public CommonNetworkRelativeLinkHandler(CommonNetworkRelativeLink item)
		{
			Item = item;
		}
		
		public void ReadFrom(BinaryReader reader)
		{
			var readerOffset = reader.BaseStream.Position;
			
			CommonNetworkRelativeLinkSize = reader.ReadInt32();
            FormatChecker.CheckExpression(() => CommonNetworkRelativeLinkSize >= MinimumCommonNetworkRelativeLinkSize);

			Item.CommonNetworkRelativeLinkFlags = (CommonNetworkRelativeLinkFlags) (reader.ReadInt32());
			
			NetNameOffset = reader.ReadInt32();
            FormatChecker.CheckExpression(() => NetNameOffset < CommonNetworkRelativeLinkSize);

			DeviceNameOffset = reader.ReadInt32();
            FormatChecker.CheckExpression(() => DeviceNameOffset < CommonNetworkRelativeLinkSize);

            var nptvalue = reader.ReadInt32();
            if ((Item.CommonNetworkRelativeLinkFlags & CommonNetworkRelativeLinkFlags.ValidNetType) != 0)
                Item.NetworkProviderType = (NetworkProviderType)(nptvalue);
            else
                Item.NetworkProviderType = null;

            if (NetNameOffset > MinimumCommonNetworkRelativeLinkSize)
            {
                NetNameOffsetUnicode = reader.ReadInt32();
                FormatChecker.CheckExpression(() => NetNameOffsetUnicode < CommonNetworkRelativeLinkSize);

                DeviceNameOffsetUnicode = reader.ReadInt32();
                FormatChecker.CheckExpression(() => DeviceNameOffsetUnicode < CommonNetworkRelativeLinkSize);
            }
            else
                FormatChecker.CheckExpression(() => NetNameOffset == MinimumCommonNetworkRelativeLinkSize);

            Item.NetName = reader.ReadASCIIZ(readerOffset, NetNameOffset, NetNameOffsetUnicode);
			if ((Item.CommonNetworkRelativeLinkFlags & CommonNetworkRelativeLinkFlags.ValidDevice) != 0)
                Item.DeviceName = reader.ReadASCIIZ(readerOffset, DeviceNameOffset, DeviceNameOffsetUnicode);
		}
		
		public int ComputedSize
		{
			get
			{
				var result = Marshal.SizeOf(CommonNetworkRelativeLinkSize) +
				Marshal.SizeOf(typeof(int))*2 +
				Marshal.SizeOf(NetNameOffset) +
				Marshal.SizeOf(DeviceNameOffset) +
                Encoding.Default.GetASCIIZSize(Item.NetName);
				
                // TODO: Handle unicode strings and offsets
                // NetNameOffsetUnicode = 
                // DeviceNameOffsetUnicode = 
                // NetNameOffset > &H14
				
				if ((Item.CommonNetworkRelativeLinkFlags & CommonNetworkRelativeLinkFlags.ValidDevice) != 0)
				{
                    result += Encoding.Default.GetASCIIZSize(Item.DeviceName);
				}
				
				return result;
			}
		}
		
		public void WriteTo(BinaryWriter writer)
		{
			CommonNetworkRelativeLinkSize = ComputedSize;
            FormatChecker.CheckExpression(() => CommonNetworkRelativeLinkSize >= MinimumCommonNetworkRelativeLinkSize);

            writer.Write(CommonNetworkRelativeLinkSize);
			writer.Write((int) Item.CommonNetworkRelativeLinkFlags);
			
			NetNameOffset = Marshal.SizeOf(ComputedSize) +
			Marshal.SizeOf(CommonNetworkRelativeLinkSize) +
			Marshal.SizeOf(typeof(int)) +
			Marshal.SizeOf(NetNameOffset) +
			Marshal.SizeOf(DeviceNameOffset);
			writer.Write(NetNameOffset);

			DeviceNameOffset = (Item.CommonNetworkRelativeLinkFlags & CommonNetworkRelativeLinkFlags.ValidDevice) != 0
				                   ? NetNameOffset + Encoding.Default.GetASCIIZSize(Item.NetName)
				                   : 0;
			writer.Write(DeviceNameOffset);

            var nptvalue = 0;
			if ((Item.CommonNetworkRelativeLinkFlags & CommonNetworkRelativeLinkFlags.ValidNetType) != 0 &&
			    Item.NetworkProviderType != null)
				nptvalue = (int) Item.NetworkProviderType;
			writer.Write(nptvalue);

            // TODO: Handle unicode strings and offsets
            // NetNameOffsetUnicode = 
            // DeviceNameOffsetUnicode = 
            // NetNameOffset > &H14

            writer.WriteASCIIZ(Item.NetName, Encoding.Default);
			if (DeviceNameOffset > 0)
                writer.WriteASCIIZ(Item.DeviceName, Encoding.Default);
		}
	}
}
