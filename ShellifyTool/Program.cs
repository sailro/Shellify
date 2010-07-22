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
using Shellify.Tool.CommandLine;
using Shellify.Tool.Commands;
using Shellify.Tool.Options;
using System.Text;

namespace Shellify.Tool
{
    public class Program
    {
        public const string DemoValue = "value";
        public const string OptionTagDescriptionSeparator = " - ";

        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(string.Format("ShellifyTool v{0} by Sebastien LEBRETON", typeof(Program).Assembly.GetName().Version.ToString(2)));
                Console.WriteLine();
                Command command = CommandLineParser.Parse(args);
                if (command != null)
                {
                    command.Execute();
                }
                else
                {
                    DisplayUsage();
                }
            }
            catch (CommandLineParseException e)
            {
                DisplayUsage();
                Console.WriteLine();
                Console.WriteLine(string.Concat("ERROR: ", e.Message));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Concat("ERROR: ", e.Message));
            }
            Console.ReadKey();
        }

        private static int ComputeOptionWidth()
        {
            int result = 0;
            int value = 0;

            foreach (Option option in ProgramContext.Options)
            {
                value = option.Tag.Length + CommandLineParser.OptionPrefix.Length;
                if (option.ExpectedArguments > 0)
                {
                    value += CommandLineParser.OptionArgumentSeparator.Length + DemoValue.Length;
                }
                result = Math.Max(result, value);
            }

            return result;
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("Usage: ShellifyTool <Command> [Options...] filename [target]");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            foreach (Command command in ProgramContext.Commands)
            {
                Console.WriteLine(string.Format("{0}: {1}", command.Tag, command.Description));
            }

            int optionWidth = ComputeOptionWidth();
            Console.WriteLine();
            Console.WriteLine("Options:");
            foreach (Option option in ProgramContext.Options)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("{0}{1}", CommandLineParser.OptionPrefix, option.Tag);
                if (option.ExpectedArguments > 0)
                {
                    builder.Append(CommandLineParser.OptionArgumentSeparator);
                    builder.Append("value");
                }
                while (builder.Length < optionWidth)
                {
                    builder.Append(" ");
                }
                builder.Append(OptionTagDescriptionSeparator);
                builder.Append(option.Description);
                Console.WriteLine(NormalizeBuilder(builder, optionWidth));
            }
        }

        private static string NormalizeBuilder(StringBuilder builder, int optionWidth)
        {
            string result = builder.ToString();
            char separator = ',';
            int currentWidth = 0;
            const int windowWidth = 80; // Console.WindowWidth no implemented in mono...

            if (builder.Length > windowWidth)
            {
                string[] splits = result.Split(separator);
                builder.Length = 0;
                foreach (String split in splits)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(separator);
                        currentWidth++;
                    }
                    if (currentWidth + split.Length >= windowWidth)
                    {
                        builder.AppendLine();
                        currentWidth = 0;
                        while (currentWidth < optionWidth + OptionTagDescriptionSeparator.Length)
                        {
                            builder.Append(" ");
                            currentWidth++;
                        }
                    }
                    builder.Append(split);
                    currentWidth += split.Length;
                }
                result = builder.ToString();
            }

            return result;
        }

    }
	
}
