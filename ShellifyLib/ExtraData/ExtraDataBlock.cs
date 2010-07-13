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

namespace Shellify.ExtraData
{
	public abstract class ExtraDataBlock
	{
	
		public ExtraDataBlockSignature Signature { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(">> {0}", this.GetType().Name);
            builder.AppendLine();
            builder.AppendFormat("Signature: {0:x}", Signature);
            return builder.ToString();
        }
    }
}
