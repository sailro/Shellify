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
    class CreateAbsoluteCommand : Command
    {

        public CreateAbsoluteCommand(string tag, string description)
            : base(tag, description, 2)
        {
        }

        protected static void CheckExists(string target, string filename)
        {
            if (!File.Exists(target) && !Directory.Exists(target))
            {
                Console.WriteLine(string.Format("WARN: {0} doesn't exist", target));
            }
            else
            {
                Console.WriteLine(string.Format("{0} => {1}", filename, target));
            }
        }

        public override void Execute()
        {
            Context = ShellLinkFile.CreateAbsolute(Target);
            foreach (Option option in Options) option.Execute(Context);
            Context.SaveAs(Filename);
            CheckExists(Target, Filename);
        }

    }
}