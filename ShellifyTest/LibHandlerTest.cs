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
using System.IO;
using Shellify.ExtraData;
using Shellify.IO;

namespace Shellify.Test
{
    [TestClass]
    public class LibHandlerTest
    {
        public LibHandlerTest()
        {
        }

        private TestContext testContextInstance;
        private string filesDirectory;

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
                filesDirectory = Path.Combine(testContextInstance.TestDir, @"..\..");
                filesDirectory = Path.Combine(filesDirectory, @"ShellifyTest\Files");
                filesDirectory = Path.GetFullPath(filesDirectory);
            }
        }

        [TestMethod]
        public void TestHandler()
        {
            foreach (ExtraDataBlockSignature signature in System.Enum.GetValues(typeof(ExtraDataBlockSignature)))
            {
                ExtraDataBlock block = ExtraDataBlockFactory.GetInstance(signature);
                ExtraDataBlockHandler handler = ExtraDataBlockHandlerFactory.GetInstance(block, null);
            }
        }
    }
}
