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
		public TestContext TestContext { get; set; }

		[TestMethod]
		public void TestToolParser()
		{
			try
			{
				CommandLineParser.Parse(new[] {"foo"});
				Assert.Fail("Exception required");
			}
			catch (Exception e)
			{
				Assert.IsInstanceOfType(e, typeof(CommandLineParseException));
			}

			var dic = (DisplayInfosCommand)(ProgramContext.Commands.ToList().First(c => c is DisplayInfosCommand));
			CommandLineParser.Parse(new[] {dic.Tag, "filename"});

			try
			{
				CommandLineParser.Parse(new[] {dic.Tag, "-foo", "filename"});
				Assert.Fail("Exception required");
			}
			catch (Exception e)
			{
				Assert.IsInstanceOfType(e, typeof(CommandLineParseException));
			}

			try
			{
				dic.Arguments.Clear();
				var cmd = CommandLineParser.Parse(new[] {dic.Tag, "filename.foo"});
				cmd.Execute();
				Assert.Fail("Exception required");
			}
			catch (Exception e)
			{
				Assert.IsInstanceOfType(e, typeof(FileNotFoundException));
			}

			try
			{
				dic.Arguments.Clear();
				var cmd = CommandLineParser.Parse(new[] {dic.Tag, @"..\..\..\Shellify.sln"});
				cmd.Execute();
				Assert.Fail("Exception required");
			}
			catch (Exception e)
			{
				Assert.IsInstanceOfType(e, typeof(MalformedException));
			}
		}
	}
}
