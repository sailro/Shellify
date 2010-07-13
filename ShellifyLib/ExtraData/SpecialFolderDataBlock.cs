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
using Shellify.Core;
using System;

namespace Shellify.ExtraData
{
	public class SpecialFolderDataBlock : BaseFolderIDDataBlock
	{

        public Environment.SpecialFolder SpecialFolder { get; set; }
		
		public SpecialFolderDataBlock()
		{
			Signature = ExtraDataBlockSignature.SpecialFolderDataBlock;
		}

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(base.ToString());
            builder.AppendFormat("SpecialFolder: {0}", SpecialFolder);
            return builder.ToString();
        }
		
	}
}
