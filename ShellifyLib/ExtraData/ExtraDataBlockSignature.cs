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

namespace Shellify.ExtraData
{
    public enum ExtraDataBlockSignature : uint
    {
        Unknown = 0,
        ConsoleDataBlock = 0xA0000002,
        ConsoleFEDataBlock = 0xA0000004,
        DarwinDataBlock = 0xA0000006,
        EnvironmentVariableDataBlock = 0xA0000001,
        IconEnvironmentDataBlock = 0xA0000007,
        KnownFolderDataBlock = 0xA000000B,
        PropertyStoreDataBlock = 0xA0000009,
        ShimDataBlock = 0xA0000008,
        SpecialFolderDataBlock = 0xA0000005,
        TrackerDataBlock = 0xA0000003,
        VistaAndAboveIDListDataBlock = 0xA000000C,
    }

}
