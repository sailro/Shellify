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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shellify.IO;
using Shellify.Tool.CommandLine;
using Shellify.Tool.Commands;

namespace Shellify.Test
{
    [TestClass]
    public class ToolParserTest
    {
        public ToolParserTest()
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

        [TestMethod]
        public void TestToolParser()
        {
            try
            {
                CommandLineParser.Parse(new string[] { "foo" });
                Assert.Fail("Exception  required");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(CommandLineParseException));
            }

            DisplayInfosCommand dic = (DisplayInfosCommand)(Enumerable.ToList(ProgramContext.Commands).Where(c => c is DisplayInfosCommand).First());
            CommandLineParser.Parse(new string[] { dic.Tag, "filename" });

            try
            {
                CommandLineParser.Parse(new string[] { dic.Tag, "-foo", "filename" });
                Assert.Fail("Exception  required");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(CommandLineParseException));
            }

            try
            {
                dic.Arguments.Clear();
                Command cmd = CommandLineParser.Parse(new string[] { dic.Tag, "filename.foo" });
                cmd.Execute();
                Assert.Fail("Exception  required");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(FileNotFoundException));
            }

            try
            {
                dic.Arguments.Clear();
                Command cmd = CommandLineParser.Parse(new string[] { dic.Tag, @"..\..\..\Shellify.sln" });
                cmd.Execute();
                Assert.Fail("Exception  required");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(MalformedException));
            }
        }
    }
}
