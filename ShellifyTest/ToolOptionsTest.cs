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

using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shellify.Tool.CommandLine;
using Shellify.Tool.Options;

namespace Shellify.Test
{
	[TestClass]
	public class ToolOptionsTest
	{
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		// ReSharper disable once MemberCanBePrivate.Global
		public TestContext TestContext { get; set; }

		private static void TestOption(ShellLinkFile context, Option option, string argument)
		{
			option.Arguments.Clear();
			for (var i = 0; i < option.ExpectedArguments; i++)
				option.Arguments.Add(argument);

			// As we use Reflection to alter properties, just check if properties names are correct.
			option.Execute(context);
		}

		[TestMethod]
		public void TestToolOptions()
		{
			var info = new DateTimeFormatInfo();
			string[] testargs = {"0", "string", new DateTime().ToString($"{info.ShortDatePattern} {info.ShortTimePattern}", CultureInfo.InvariantCulture), "true"};

			var slf = new ShellLinkFile();
			foreach (var option in ProgramContext.Options)
			{
				var argindex = 0;
				bool retry;
				do
				{
					retry = false;
					try
					{
						TestOption(slf, option, testargs[argindex]);
					}
					catch (FormatException)
					{
						if (argindex >= testargs.Length - 1)
							throw;

						TestContext.WriteLine("Option '{0}' fail for argument '{1}', testing '{2}'", option,
							testargs[argindex], testargs[argindex + 1]);
						retry = true;
						argindex++;
					}
					catch (Exception)
					{
						Assert.Fail("Check option '{0}', type {1}", option, option.GetType().Name);
					}
				} while (retry);
			}
		}
	}
}
