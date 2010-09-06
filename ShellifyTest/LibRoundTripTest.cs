/* Shellify Copyright (c) 2010 Sebastien LEBRETON

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

using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
