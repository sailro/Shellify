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

using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Shellify.Core;
using Shellify.Extensions;

namespace Shellify.IO
{
    public class CommonNetworkRelativeLinkHandler : IBinaryReadable, IBinaryWriteable, ISizeComputable
	{
		
		private CommonNetworkRelativeLink Item { get; set; }
		private int CommonNetworkRelativeLinkSize { get; set; }
		private int NetNameOffset { get; set; }
		private int DeviceNameOffset { get; set; }

        private int? NetNameOffsetUnicode { get; set; } // Optional
		private int? DeviceNameOffsetUnicode { get; set; } // Optional
		
		public CommonNetworkRelativeLinkHandler(CommonNetworkRelativeLink item)
		{
			this.Item = item;
		}
		
		public void ReadFrom(BinaryReader reader)
		{
			long readerOffset = reader.BaseStream.Position;
			
			CommonNetworkRelativeLinkSize = reader.ReadInt32();
			Item.CommonNetworkRelativeLinkFlags = (CommonNetworkRelativeLinkFlags) (reader.ReadInt32());
			
			NetNameOffset = reader.ReadInt32();
			DeviceNameOffset = reader.ReadInt32();
            int nptvalue = reader.ReadInt32();
            if ((Item.CommonNetworkRelativeLinkFlags & CommonNetworkRelativeLinkFlags.ValidNetType) != 0)
            {
                Item.NetworkProviderType = (NetworkProviderType)(nptvalue);
            }
            else
            {
                Item.NetworkProviderType = null;
            }
			
			if (NetNameOffset > 0x14)
			{
				NetNameOffsetUnicode = reader.ReadInt32();
				DeviceNameOffsetUnicode = reader.ReadInt32();
			}

            Item.NetName = reader.ReadASCIIZ(readerOffset, NetNameOffset, NetNameOffsetUnicode);
			if ((Item.CommonNetworkRelativeLinkFlags & CommonNetworkRelativeLinkFlags.ValidDevice) != 0)
			{
                Item.DeviceName = reader.ReadASCIIZ(readerOffset, DeviceNameOffset, DeviceNameOffsetUnicode);
			}
		}
		
		public int ComputedSize
		{
			get
			{
				int result = Marshal.SizeOf(CommonNetworkRelativeLinkSize) +
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
		
		public void WriteTo(System.IO.BinaryWriter writer)
		{
			CommonNetworkRelativeLinkSize = ComputedSize;
			writer.Write(CommonNetworkRelativeLinkSize);
			writer.Write((int) Item.CommonNetworkRelativeLinkFlags);
			
			NetNameOffset = Marshal.SizeOf(ComputedSize) +
			Marshal.SizeOf(CommonNetworkRelativeLinkSize) +
			Marshal.SizeOf(typeof(int)) +
			Marshal.SizeOf(NetNameOffset) +
			Marshal.SizeOf(DeviceNameOffset);
			writer.Write(NetNameOffset);
			
			if ((Item.CommonNetworkRelativeLinkFlags & CommonNetworkRelativeLinkFlags.ValidDevice) != 0)
			{
                DeviceNameOffset = NetNameOffset + Encoding.Default.GetASCIIZSize(Item.NetName);
			}
			else
			{
				DeviceNameOffset = 0;
			}
			writer.Write(DeviceNameOffset);

            int nptvalue = 0;
			if ((Item.CommonNetworkRelativeLinkFlags & CommonNetworkRelativeLinkFlags.ValidNetType) != 0)
			{
                nptvalue = (int)Item.NetworkProviderType;
			}
            writer.Write(nptvalue);

            // TODO: Handle unicode strings and offsets
            // NetNameOffsetUnicode = 
            // DeviceNameOffsetUnicode = 
            // NetNameOffset > &H14

            writer.WriteASCIIZ(Item.NetName, Encoding.Default);
			if (DeviceNameOffset > 0)
			{
                writer.WriteASCIIZ(Item.DeviceName, Encoding.Default);
			}
		}
	}
}
