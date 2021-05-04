/* Shellify Copyright (c) 2010-2021 Sebastien Lebreton

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
using System.Collections.Generic;
using Shellify.Tool.Commands;

namespace Shellify.Tool.Options
{
	internal class EnumReflectionSetterOption : ReflectionSetterOption
	{
// ReSharper disable UnusedAutoPropertyAccessor.Local
		Type EnumType { get; set; }
// ReSharper restore UnusedAutoPropertyAccessor.Local

		public EnumReflectionSetterOption(string tag, string description, IList<Command> applies, Type enumType, params string[] propertyPath)
			: base(tag, description, applies, propertyPath)
		{
			PropertyPath = propertyPath;
			EnumType = enumType;
		}

		protected override object ChangeType(object source, Type targetType)
		{
			return Enum.Parse(targetType, source.ToString());
		}

		public override void Execute(ShellLinkFile context)
		{
			Argument = Argument.Replace(" ", string.Empty);
			base.Execute(context);
		}
	}
}
