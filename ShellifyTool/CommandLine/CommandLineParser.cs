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

using Shellify.Tool.Commands;
using Shellify.Tool.Options;

namespace Shellify.Tool.CommandLine
{
    public class CommandLineParser
    {

        public const string OptionPrefix = "-";
        public const string OptionArgumentSeparator = "=";

        public static void CheckItem(CommandLineItem item)
        {
            if (item != null)
            {
                if (item.ExpectedArguments != item.Arguments.Count)
                {
                    throw new CommandLineParseException(string.Format("'{0}' expects {1} argument(s)", item, item.ExpectedArguments));
                }
            }
        }

        public static Command ParseCommand(string item)
        {
            Command command = null;

            foreach (Command c in ProgramContext.Commands)
            {
                if (c.Tag.Equals(item, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    command = c;
                    break;
                }
            }

            if (command == null)
            {
                throw new CommandLineParseException(string.Format("Unknown command {0}", item));
            }

            return command;
        }

        public static Option ParseOption(Command command, string item)
        {
            Option option = null;

            foreach (Option o in ProgramContext.Options)
            {
                string[] tokens = item.Split(OptionArgumentSeparator.ToCharArray()[0]);
                string oname = tokens[0];

                if (oname.Equals(string.Concat(OptionPrefix, o.Tag), System.StringComparison.InvariantCultureIgnoreCase))
                {
                    if (o.Applies.Contains(command))
                    {
                        if (tokens.Length > 1)
                        {
                            o.Arguments.Add(tokens[1]);
                        }
                        option = o;
                        break;
                    }
                    else
                    {
                        throw new CommandLineParseException(string.Format("'{0}' option is not available for '{1}' command", o, command));
                    }
                }
            }

            if (option == null)
            {
                throw new CommandLineParseException(string.Format("Unknown option {0}", item));
            }

            CheckItem(option);
            return option;
        }

        public static Command Parse(string[] args)
        {
            Command command = null;

            foreach (string item in args)
            {
                if (command == null)
                {
                    command = ParseCommand(item);
                }
                else
                {
                    if (item.StartsWith(OptionPrefix.ToString()))
                    {
                        Option option = ParseOption(command, item);
                        if (!command.Options.Contains(option))
                        {
                            command.Options.Add(option);
                        }
                    }
                    else
                    {
                        command.Arguments.Add(item);
                    }
                }
            }

            CheckItem(command);
            return command;
        }

    }
}