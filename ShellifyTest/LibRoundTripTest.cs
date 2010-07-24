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

namespace Shellify.Test
{
    [TestClass]
    public class LibRoundTripTest
    {
        public LibRoundTripTest()
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

        private static void CompareFiles(BinaryReader reoriginal, BinaryReader recompared)
        {
            Assert.AreEqual(reoriginal.BaseStream.Length, recompared.BaseStream.Length, "Size mismatch");
            while (reoriginal.BaseStream.Position < reoriginal.BaseStream.Length)
            {
                byte b1 = reoriginal.ReadByte();
                byte b2 = recompared.ReadByte();
                Assert.AreEqual(b1, b2, "Position: {0}", reoriginal.BaseStream.Position);
            }
        }

        public static void CompareFiles(Stream soriginal, Stream scompared)
        {
            using (BinaryReader reoriginal = new BinaryReader(soriginal))
            {
                using (BinaryReader recompared = new BinaryReader(scompared))
                {
                    CompareFiles(reoriginal, recompared);
                }
            }
        }

        public static void CompareFiles(string original, string compared) {
            using (FileStream fsoriginal = new FileStream(original, FileMode.Open))
            {
                using (FileStream fscompared = new FileStream(compared, FileMode.Open))
                {
                    CompareFiles(fsoriginal, fscompared);
                }
            }
        }

        [TestMethod]
        public void TestRoundTrip()
        {
            foreach (string file in Directory.GetFiles(filesDirectory))
            {
                testContextInstance.WriteLine("Testing {0}", file);
                ShellLinkFile slf = ShellLinkFile.Load(file);
                testContextInstance.WriteLine("{0}", slf);
                string tmpFile = Path.GetTempFileName();

                slf.SaveAs(tmpFile);
                ShellLinkFile slf2 = ShellLinkFile.Load(tmpFile);
                Assert.AreEqual(slf.ToString(), slf2.ToString());
                CompareFiles(file, tmpFile);
            }
        }
    }
}
