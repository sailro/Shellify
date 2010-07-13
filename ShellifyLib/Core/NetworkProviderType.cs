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

namespace Shellify.Core
{
	public enum NetworkProviderType : uint
	{
    Avid = 0x1A0000,
    Docuspace = 0x1B0000,
    Mangosoft = 0x1C0000,
    Sernet = 0x1D0000,
    Riverfront1 = 0x1E0000,
    Riverfront2 = 0x1F0000,
    Decorb = 0x200000,
    Protstor = 0x210000,
    FjRedir = 0x220000,
    Distinct = 0x230000,
    Twins = 0x240000,
    Rdr2Sample = 0x250000,
    Csc = 0x260000,
    ThreeInOne = 0x270000,
    ExtendNet = 0x290000,
    Stac = 0x2A0000,
    Foxbat = 0x2B0000,
    Yahoo = 0x2C0000,
    Exifs = 0x2D0000,
    Dav = 0x2E0000,
    Knoware = 0x2F0000,
    ObjectDire = 0x300000,
    Masfax = 0x310000,
    HobNfs = 0x320000,
    Shiva = 0x330000,
    Ibmal = 0x340000,
    Lock = 0x350000,
    Termsrv = 0x360000,
    Srt = 0x370000,
    Quincy = 0x380000,
    Openafs = 0x390000,
    Avid1 = 0x3A0000,
    Dfs = 0x3B0000,
    Kwnp = 0x3C0000,
    Zenworks = 0x3D0000,
    Driveonweb = 0x3E0000,
    Vmware = 0x3F0000,
    Rsfx = 0x40000,
    Mfiles = 0x410000,
    MsNfs = 0x420000,
    Google = 0x43000,
    }
}
