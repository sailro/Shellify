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
using Shellify.IO;
using Shellify.Extensions;

namespace Shellify.ExtraData
{
	public abstract class BaseStringDataBlock: ExtraDataBlock
	{

        public byte[] ValuePadding { get; set; }
        public byte[] ValueUnicodePadding { get; set; }
		public string Value { get; set; }
        public string ValueUnicode { get; set; }
		
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(base.ToString());
            builder.AppendFormat("Value: {0}", Value); builder.AppendLine();
            if (ValuePadding != null)
            {
                builder.AppendFormat("Value padding length: {0}", ValuePadding.Length); builder.AppendLine();
                builder.AppendFormat("Value padding Hash: {0}", ValuePadding.ComputeHash()); builder.AppendLine();
            }
            builder.AppendFormat("ValueUnicode: {0}", ValueUnicode);
            if (ValueUnicodePadding != null)
            {
                builder.AppendLine();
                builder.AppendFormat("ValueUnicode padding length: {0}", ValueUnicodePadding.Length); builder.AppendLine();
                builder.AppendFormat("ValueUnicode padding Hash: {0}", ValueUnicodePadding.ComputeHash());
            }
            return builder.ToString();
        }
		
	}
}
