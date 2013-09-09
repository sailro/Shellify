/* Shellify Copyright (c) 2010-2013 Sebastien LEBRETON

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

using System.Linq;
using Shellify.Core;
using Shellify.ExtraData;

namespace Shellify.Tool.Commands
{
    public class AnonimyzeCommand : Command
    {

        public AnonimyzeCommand(string tag, string description)
            : base(tag, description, 1)
        {
        }

        public override void Execute()
        {
            Context = ShellLinkFile.Load(Filename);

            Context.Header.LinkFlags &= ~LinkFlags.EnableTargetMetaData;
            Context.Header.LinkFlags |= LinkFlags.ForceNoLinkTrack;

            if (Context.LinkInfo != null)
            {
                if (Context.LinkInfo.VolumeID != null)
                {
                    Context.LinkInfo.VolumeID.DriveSerialNumber = 0;
                    Context.LinkInfo.VolumeID.VolumeLabel = null;
                }
                if (Context.LinkInfo.CommonNetworkRelativeLink != null)
                {
                    Context.LinkInfo.CommonNetworkRelativeLink.DeviceName = null;
                    Context.LinkInfo.CommonNetworkRelativeLink.NetworkProviderType = null;
                }
            }

            var blocks = Context.ExtraDataBlocks.Where(block => ! (block is TrackerDataBlock || block is PropertyStoreDataBlock)).ToList();
	        Context.ExtraDataBlocks = blocks;

            foreach (var option in Options) option.Execute(Context);
            Context.SaveAs(Filename);
        }

    }
}