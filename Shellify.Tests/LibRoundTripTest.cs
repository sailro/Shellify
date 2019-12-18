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

using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shellify.Tests
{
	[TestClass]
	public class LibRoundTripTest
	{
		public TestContext TestContext
		{
			get;
			set;
		}

		private static void CompareFiles(BinaryReader reoriginal, BinaryReader recompared)
		{
			Assert.AreEqual(reoriginal.BaseStream.Length, recompared.BaseStream.Length, "Size mismatch");
			while (reoriginal.BaseStream.Position < reoriginal.BaseStream.Length)
			{
				var b1 = reoriginal.ReadByte();
				var b2 = recompared.ReadByte();
				Assert.AreEqual(b1, b2, "Position: {0}", reoriginal.BaseStream.Position);
			}
		}

		private static void CompareFiles(Stream soriginal, Stream scompared)
		{
			using (var reoriginal = new BinaryReader(soriginal))
			{
				using (var recompared = new BinaryReader(scompared))
				{
					CompareFiles(reoriginal, recompared);
				}
			}
		}

		private static void CompareFiles(string original, string compared)
		{
			using (var fsoriginal = new FileStream(original, FileMode.Open))
			{
				using (var fscompared = new FileStream(compared, FileMode.Open))
				{
					CompareFiles(fsoriginal, fscompared);
				}
			}
		}

		[TestMethod]
		public void TestRoundTrip()
		{
			var basePath = GetType().Assembly.Location;
			var filesPath = Path.Combine(Path.GetDirectoryName(basePath), "Files");

			foreach (var file in Directory.GetFiles(filesPath))
			{
				TestContext.WriteLine("Testing {0}", file);
				var slf = ShellLinkFile.Load(file);
				TestContext.WriteLine("{0}", slf);
				var tmpFile = Path.GetTempFileName();

				slf.SaveAs(tmpFile);
				var slf2 = ShellLinkFile.Load(tmpFile);
				Assert.AreEqual(slf.ToString(), slf2.ToString());
				CompareFiles(file, tmpFile);
			}
		}
	}
}
