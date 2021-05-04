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

using System.Text;

namespace Shellify.Core
{
	public class CommonNetworkRelativeLink
	{
		public CommonNetworkRelativeLinkFlags CommonNetworkRelativeLinkFlags { get; set; }
		public string NetName { get; set; }

		private NetworkProviderType? _networkProviderType;

		public NetworkProviderType? NetworkProviderType
		{
			get => _networkProviderType;
			set
			{
				_networkProviderType = value;
				UpdateHeaderFlags(!value.HasValue, CommonNetworkRelativeLinkFlags.ValidNetType);
			}
		}

		private string _deviceName;

		public string DeviceName
		{
			get => _deviceName;
			set
			{
				_deviceName = value;
				UpdateHeaderFlags(value, CommonNetworkRelativeLinkFlags.ValidDevice);
			}
		}

		private void UpdateHeaderFlags(object item, CommonNetworkRelativeLinkFlags flag)
		{
			UpdateHeaderFlags(item is string s && string.IsNullOrEmpty(s) || (item == null), flag);
		}

		private void UpdateHeaderFlags(bool resetcondition, CommonNetworkRelativeLinkFlags flag)
		{
			if (resetcondition)
			{
				CommonNetworkRelativeLinkFlags &= ~flag;
			}
			else
			{
				CommonNetworkRelativeLinkFlags |= flag;
			}
		}


		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine(">> CommonNetworkRelativeLink");
			builder.AppendFormat("Flags: {0}", CommonNetworkRelativeLinkFlags);
			builder.AppendLine();
			builder.AppendFormat("NetworkProviderType: {0}", NetworkProviderType);
			builder.AppendLine();
			builder.AppendFormat("NetName: {0}", NetName);
			builder.AppendLine();
			builder.AppendFormat("DeviceName: {0}", DeviceName);
			return builder.ToString();
		}
	}
}
