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

namespace Shellify.ExtraData
{
    public enum ExtraDataBlockSignature : uint
    {
        UnknownDataBlock = 0,
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
