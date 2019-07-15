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
using System.Collections.Generic;
using Shellify.Tool.Commands;
using System.Globalization;
using System.Reflection;

namespace Shellify.Tool.Options
{
	internal class ReflectionSetterOption : Option
	{
		protected string[] PropertyPath { get; set; }

		public ReflectionSetterOption(string tag, string description, IList<Command> applies, params string[] propertyPath)
			: base(tag, description, 1, applies)
		{
			PropertyPath = propertyPath;
		}

		protected virtual object ChangeType(object source, Type targetType)
		{
			return Convert.ChangeType(source, targetType, CultureInfo.InvariantCulture);
		}

		public override void Execute(ShellLinkFile context)
		{
			PropertyInfo pinfo = null;
			var ptype = context.GetType();
			object obj = context;
			foreach (var item in PropertyPath)
			{
				if (pinfo != null)
					obj = pinfo.GetValue(obj, null);

				pinfo = ptype.GetProperty(item);
				if (pinfo != null)
					ptype = pinfo.PropertyType;
			}

			var value = ChangeType(Argument, ptype);

			pinfo?.SetValue(obj, value, null);
		}
	}
}
