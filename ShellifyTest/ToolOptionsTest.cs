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
	    private TestContext _testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }

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
            string[] testargs = { "0", "string", new DateTime().ToString(string.Format("{0} {1}",info.ShortDatePattern, info.ShortTimePattern), CultureInfo.InvariantCulture), "true" };

            var slf = new ShellLinkFile();
            foreach (Option option in ProgramContext.Options)
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

	                    _testContextInstance.WriteLine("Option '{0}' fail for argument '{1}', testing '{2}'", option,
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
