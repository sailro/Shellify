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
using System.Reflection;

namespace Shellify.Tool
{
    public class Program
    {
        private static void ForEachEnumCLA<T>(Action<CommandLineAttribute, T> action)
        {
            foreach (FieldInfo fi in typeof(T).GetFields())
            {
                foreach (CommandLineAttribute cla in fi.GetCustomAttributes(typeof(CommandLineAttribute), false))
                {
                    action(cla, (T) fi.GetValue(null));
                }
            }
        }

        private static Command GetCommand(string p)
        {
            Command result = Command.None;
            ForEachEnumCLA<Command>((cla, cmd) => result = (cla.Tag.Equals(p, StringComparison.InvariantCultureIgnoreCase)) ? cmd : result);
            return result;
        }

        public static void Usage()
        {
            Console.WriteLine("Usage: ShellifyTool <Command> filename [target]");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            ForEachEnumCLA<Command>((cla, cmd) => Console.WriteLine(String.Format("{0}\t{1}", cla.Tag, cla.Description)));
        }

        public static void Main(string[] args)
        {
            try
            {
                if (args.Length >= 2)
                {
                    Command command = GetCommand(args[0]);
                    string filename = args[1];
                    string target = args.Length >= 3 ? args[2] : null;

                    ProcessCommand(command, target, filename);
                }
                else
                {
                    Usage();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Concat("ERROR: ", e.Message));
            }
        }

        private static void CheckExists(string target, string filename)
        {
            if (!File.Exists(target))
            {
                Console.WriteLine(string.Format("WARN: {0} doesn't exist", target));
            }
            else
            {
                Console.WriteLine(string.Format("{0} => {1}", filename, target));
            }
        }

        private static void ProcessCommand(Command command, string target, string filename)
        {
            switch (command)
            {
                case Command.CreateAbsolute:
                    var slfabs = ShellLinkFile.CreateAbsolute(target);
                    slfabs.SaveAs(filename);
                    CheckExists(target, filename);
                    break;

                case Command.CreateRelative:
                    var baseDirectory = Path.GetDirectoryName(filename);
                    if (string.IsNullOrEmpty(baseDirectory)) {
                        baseDirectory = ".";
                    }
                    Console.WriteLine(String.Format("Using {0} as base directory", baseDirectory));

                    var slfrel = ShellLinkFile.CreateRelative(baseDirectory, target);
                    slfrel.SaveAs(filename);
                    CheckExists(Path.Combine(baseDirectory, target), filename);
                    break;

                case Command.DisplayInfo:
                    var slf = ShellLinkFile.Load(filename);
                    Console.WriteLine(slf);
                    break;

                default:
                    Usage();
                    break;
            }
        }

    }
	
}
