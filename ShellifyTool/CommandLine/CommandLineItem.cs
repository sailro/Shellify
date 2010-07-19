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

using System.Collections.Generic;

namespace Shellify.Tool.CommandLine
{
    public class CommandLineItem
    {

        public string Description { get; set; }
        public string Tag { get; set; }
        public int ExpectedArguments { get; set; }
        public IList<string> Arguments { get; set; }

        public CommandLineItem(string tag, string description, int expectedArguments)
        {
            Arguments = new List<string>();
            Tag = tag;
            Description = description;
            ExpectedArguments = expectedArguments;
        }

        public override string ToString()
        {
            return Tag.ToString();
        }

    }
}
