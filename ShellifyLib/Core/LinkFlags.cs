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

using System;

namespace Shellify.Core
{
    [Flags()]
    public enum LinkFlags : uint
    {
        None = 0,
        HasLinkTargetIDList = 1,
        HasLinkInfo = 2,
        HasName = 4,
        HasRelativePath = 8,
        HasWorkingDir = 16,
        HasArguments = 32,
        HasIconLocation = 64,
        IsUnicode = 128,
        ForceNoLinkInfo = 256,
        HasExpString = 512,
        RunInSeparateProcess = 1024,
        Unused1 = 2048,
        HasDarwinID = 4096,
        RunAsUser = 8192,
        HasExpIcon = 16384,
        NoPidlAlias = 32768,
        Unused2 = 65536,
        RunWithShimLayer = 131072,
        ForceNoLinkTrack = 262144,
        EnableTargetMetaData = 524288,
        DisableLinkPathTracking = 1048576,
        DisableKnownFolderTracking = 2097152,
        DisableKnownFolderAlias = 4194304,
        AllowLinkToLink = 8388608,
        UnAliasOnSave = 16777216,
        PreferEnvironmentPath = 33554432,
        KeepLocalIDListForUNCTarget = 67108864,
    }

}
