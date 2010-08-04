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
using Shellify.Tool.CommandLine;
using Shellify.Tool.Commands;
using Shellify.Tool.Options;

namespace Shellify.Test
{
    [TestClass]
    public class ToolCommandsTest
    {
        public ToolCommandsTest()
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
        public void TestToolCommands()
        {
            CreateRelativeCommand crc = new CreateRelativeCommand(string.Empty, string.Empty);
            CreateAbsoluteCommand cac = new CreateAbsoluteCommand(string.Empty, string.Empty);
            UpdateCommand uc = new UpdateCommand(string.Empty, string.Empty);
            DisplayInfosCommand dic = new DisplayInfosCommand(string.Empty, string.Empty);

            string tmpFile = Path.GetTempFileName();

            crc.Arguments.Add(tmpFile);
            crc.Arguments.Add("ShellifyTool.exe");
            crc.Execute();
            crc.Arguments[1] = @"c:\foo";
            try
            {
                crc.Execute();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }
            crc.Arguments[1] = @"foo";
            crc.Execute();

            cac.Arguments.Add(tmpFile);
            cac.Arguments.Add("ShellifyTool.exe");
            cac.Execute();
            cac.Arguments[1] = @"foo";
            cac.Execute();
            cac.Arguments[1] = ".";
            cac.Execute();

            Option on = Enumerable.ToList(ProgramContext.Options).Where(o => o.Tag == "name").First();
            on.Arguments.Add(string.Empty);

            uc.Arguments.Add(tmpFile);
            uc.Options.Add(on);
            uc.Execute();
        }
    }
}
