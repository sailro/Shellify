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

using Shellify.Tool.CommandLine;
using Shellify.Tool.Options;
using Shellify.Core;
using Shellify.ExtraData;
using System.Collections.Generic;

namespace Shellify.Tool.Commands
{
    class AnonimyzeCommand : Command
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

            List<ExtraDataBlock> blocks = new List<ExtraDataBlock>(); 
            foreach (ExtraDataBlock block in Context.ExtraDataBlocks)
            {
                if (! (block is TrackerDataBlock || block is PropertyStoreDataBlock))
                {
                    blocks.Add(block);                    
                }
            }
            Context.ExtraDataBlocks = blocks;

            foreach (Option option in Options) option.Execute(Context);
            Context.SaveAs(Filename);
        }

    }
}