using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Shellify.Test
{
    [TestClass]
    public class RoundTripTest
    {
        public RoundTripTest()
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
        public void TestRoundTripFile()
        {
            foreach (string file in Directory.GetFiles(filesDirectory))
            {
                testContextInstance.WriteLine("Testing {0}", file);
                ShellLinkFile shf = ShellLinkFile.Load(file);
                testContextInstance.WriteLine("{0}", shf);
                string tmpFile = Path.GetTempFileName();

                shf.SaveAs(tmpFile);
                ShellLinkFile shf2 = ShellLinkFile.Load(tmpFile);
                Assert.AreEqual(shf.ToString(), shf2.ToString());
                CompareFiles(file, tmpFile);
            }
        }
    }
}
