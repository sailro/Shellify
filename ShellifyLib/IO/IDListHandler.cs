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

using System.IO;
using System.Linq;
using Shellify.Core;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace Shellify.IO
{
    public class IDListHandler : IBinaryReadable, IBinaryWriteable
	{
		private readonly IHasIDList _item;
        private readonly bool _useIDListSize;

        public IDListHandler(IHasIDList item, bool useIDListSize)
		{
			_item = item;
            _useIDListSize = useIDListSize;
		}

        public void ReadFrom(BinaryReader reader)
        {
            _item.ShItemIDs = new List<ShItemID>();
            short idListSize = 0;
            if (_useIDListSize)
                idListSize = reader.ReadInt16();

			if (_useIDListSize && idListSize <= 0)
				return;
	        
			do
	        {
		        var shitem = new ShItemID();
		        var shitemReader = new ShItemIdHandler(shitem);
		        shitemReader.ReadFrom(reader);
		        if (shitem.Data != null)
			        _item.ShItemIDs.Add(shitem);
		        else
			        break;
	        } while (true);
        }
	
		public void WriteTo(BinaryWriter writer)
		{
            ushort idListSize = 0;

            idListSize = Convert.ToUInt16(Marshal.SizeOf(idListSize));
            var writers = new List<ShItemIdHandler>();
            foreach (var shitemWriter in _item.ShItemIDs.Select(shitem => new ShItemIdHandler(shitem)))
            {
	            idListSize += Convert.ToUInt16(shitemWriter.ComputedSize);
	            writers.Add(shitemWriter);
            }

            if (_useIDListSize)
                writer.Write(idListSize);

			writers.ForEach(handler => handler.WriteTo(writer));
            writer.Write((short)0);
        }
	}
}
