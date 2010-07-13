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
using System.Text;
using Shellify.IO;

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
		}

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(base.ToString());
            builder.AppendFormat("Version: {0}", Version); builder.AppendLine();
            builder.AppendFormat("MachineID: {0}", MachineID); builder.AppendLine();
            builder.AppendFormat("Droid:\n{0}", IOHelper.ToString(Droid)); builder.AppendLine();
            builder.AppendFormat("DroidBirth:\n{0}", IOHelper.ToString(DroidBirth));
            return builder.ToString();
        }
		
	}
}
