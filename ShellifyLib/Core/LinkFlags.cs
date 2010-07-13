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
