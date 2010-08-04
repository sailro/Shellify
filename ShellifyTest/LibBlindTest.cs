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

using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shellify.ExtraData;

namespace Shellify.Test
{
    [TestClass]
    public class LibBlindTest
    {
        public LibBlindTest()
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
        public void TestBlindWrite()
        {
            BlindWrite();
        }

        private string BlindWrite()
        {
            var tmpFile = Path.GetTempFileName();
            var slf = new ShellLinkFile();

            foreach (ExtraDataBlockSignature signature in System.Enum.GetValues(typeof(ExtraDataBlockSignature)))
            {
                if (signature == ExtraDataBlockSignature.UnknownDataBlock)
                    continue;

                ExtraDataBlock block = ExtraDataBlockFactory.GetInstance(signature);
                slf.ExtraDataBlocks.Add(block);
            }

            slf.SaveAs(tmpFile);
            return tmpFile;
        }

        [TestMethod]
        public void TestBlindRead()
        {
            var slf = ShellLinkFile.Load(BlindWrite());
        }
    }
}
