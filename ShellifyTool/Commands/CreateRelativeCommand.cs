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
using System.IO;
using Shellify.Tool.Options;

namespace Shellify.Tool.Commands
{
    class CreateRelativeCommand : CreateAbsoluteCommand
    {

        public CreateRelativeCommand(string tag, string description)
            : base(tag, description)
        {
        }

        public override void Execute()
        {
            var baseDirectory = Path.GetDirectoryName(Filename);
            if (string.IsNullOrEmpty(baseDirectory))
            {
                baseDirectory = ".";
            }
            Console.WriteLine(String.Format("Using {0} as base directory", baseDirectory));

            Context = ShellLinkFile.CreateRelative(baseDirectory, Target);
            foreach (Option option in Options) option.Execute(Context);
            Context.SaveAs(Filename);
            CheckExists(Path.Combine(baseDirectory, Target), Filename);
        }

    }
}