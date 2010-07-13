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

using System.Text;

namespace Shellify.Core
{
	public class LinkInfo
	{
		
		public LinkInfoFlags LinkInfoFlags { get; set; }
		public VolumeID VolumeID { get; set; }
		public string LocalBasePath { get; set; }
		public CommonNetworkRelativeLink CommonNetworkRelativeLink { get; set; }
		public string CommonPathSuffix { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(">> LinkInfo");
            builder.AppendFormat("Flags: {0}", LinkInfoFlags); builder.AppendLine();
            builder.AppendFormat("LocalBasePath: {0}", LocalBasePath); builder.AppendLine();
            builder.AppendFormat("CommonPathSuffix: {0}", CommonPathSuffix); builder.AppendLine();
            if (VolumeID != null) builder.Append(VolumeID.ToString());
            if (CommonNetworkRelativeLink != null)
            {
                if (VolumeID != null) builder.AppendLine();
                builder.Append(CommonNetworkRelativeLink.ToString());
            }
            return builder.ToString();
        }
		
	}
	
	
}
