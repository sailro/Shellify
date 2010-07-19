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
using Shellify.Tool.CommandLine;
using Shellify.Tool.Options;

namespace Shellify.Tool.Commands
{
    public abstract class Command : CommandLineItem
    {

        public IList<Option> Options { get; set; }
        public ShellLinkFile Context { get; set; }

        public abstract void Execute();

        public Command(string tag, string description, int expectedArguments)
            : base(tag, description, expectedArguments)
        {
            Options = new List<Option>();
        }

        public string Filename
        {
            get { return Arguments.Count > 0 ? Arguments[0] : null; }
        }

        public string Target
        {
            get { return Arguments.Count > 1 ? Arguments[1] : null; }
        }

    }
}