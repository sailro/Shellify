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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shellify.Tool.CommandLine;
using Shellify.Tool.Options;
using System;
using System.Globalization;

namespace Shellify.Test
{
    [TestClass]
    public class ToolOptionsTest
    {
        public ToolOptionsTest()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        private void TestOption(ShellLinkFile context, Option option, string argument)
        {
            option.Arguments.Clear();
            for (int i = 0; i < option.ExpectedArguments; i++)
            {
                option.Arguments.Add(argument);
            }
            // As we use Reflection to alter properties, just check if properties names are correct.
            option.Execute(context);
        }

        [TestMethod]
        public void TestToolOptions()
        {
            var info = new DateTimeFormatInfo();
            string[] testargs = { "0", "string", new DateTime().ToString(string.Format("{0} {1}",info.ShortDatePattern, info.ShortTimePattern), CultureInfo.InvariantCulture), "true" };

            ShellLinkFile slf = new ShellLinkFile();
            foreach (Option option in ProgramContext.Options)
            {
                int argindex = 0;
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
                        if (argindex < testargs.Length - 1)
                        {
                            testContextInstance.WriteLine("Option '{0}' fail for argument '{1}', testing '{2}'", option, testargs[argindex], testargs[argindex + 1]);
                            retry = true;
                            argindex++;
                        }
                        else
                        {
                            throw;
                        }
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
