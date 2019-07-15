/* Shellify Copyright (c) 2010-2019 Sebastien Lebreton

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */

using System.Linq;
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
            Command command = ProgramContext.Commands.FirstOrDefault(c => c.Tag.Equals(item, System.StringComparison.InvariantCultureIgnoreCase));

	        if (command == null)
                throw new CommandLineParseException(string.Format("Unknown command {0}", item));

            return command;
        }

        public static Option ParseOption(Command command, string item)
        {
            Option option = null;

            foreach (var o in ProgramContext.Options)
            {
                var tokens = item.Split(OptionArgumentSeparator.ToCharArray()[0]);
                var oname = tokens[0];

	            if (!oname.Equals(string.Concat(OptionPrefix, o.Tag), System.StringComparison.InvariantCultureIgnoreCase))
		            continue;

	            if (!o.Applies.Contains(command))
		            throw new CommandLineParseException(string.Format("'{0}' option is not available for '{1}' command", o,
		                                                              command));
	            if (tokens.Length > 1)
		            o.Arguments.Add(tokens[1]);

				option = o;
	            break;
            }

	        if (option == null)
		        throw new CommandLineParseException(string.Format("Unknown option {0}", item));

			CheckItem(option);
	        return option;
        }

        public static Command Parse(string[] args)
        {
            Command command = null;

            foreach (var item in args)
            {
                if (command == null)
                    command = ParseCommand(item);
                else
                {
                    if (item.StartsWith(OptionPrefix))
                    {
                        var option = ParseOption(command, item);
                        if (!command.Options.Contains(option))
                            command.Options.Add(option);
                    }
                    else
                        command.Arguments.Add(item);
                }
            }

            CheckItem(command);
            return command;
        }

    }
}