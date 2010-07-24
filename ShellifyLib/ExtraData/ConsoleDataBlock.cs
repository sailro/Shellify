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

using System.Drawing;
using System.Text;
using Shellify.Extensions;

namespace Shellify.ExtraData
{
    public class ConsoleDataBlock : ExtraDataBlock
    {

        public FillAttributes FillAttributes { get; set; }
        public FillAttributes PopupFillAttributes { get; set; }
        public Size ScreenBufferSize { get; set; }
        public Size WindowSize { get; set; }
        public Point WindowOrigin { get; set; }
        public int FontSize { get; set; }
        public FontFamily FontFamily { get; set; }
        public uint FontWeight { get; set; }

        private string _facename;
        public string FaceName
        {
            get
            { return _facename; }
            set
            {
                _facename = value;
                FaceNamePadding = null;
            }
        }

        public byte[] FaceNamePadding { get; set; }
        public uint CursorSize { get; set; }
        public bool FullScreen { get; set; }
        public bool FastEdit { get; set; }
        public bool InsertMode { get; set; }
        public bool AutoPosition { get; set; }
        public uint HistoryBufferSize { get; set; }
        public uint NumberOfHistoryBuffers { get; set; }
        public bool HistoryDuplicateAllowed { get; set; }
        public byte[] ColorTable { get; set; }

        public ConsoleDataBlock()
        {
            Signature = ExtraDataBlockSignature.ConsoleDataBlock;
            ColorTable = new byte[64];
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(base.ToString());
            builder.AppendFormat("FillAttributes: {0}", FillAttributes); builder.AppendLine();
            builder.AppendFormat("PopupFillAttributes: {0}", PopupFillAttributes); builder.AppendLine();
            builder.AppendFormat("ScreenBufferSize: {0}", ScreenBufferSize); builder.AppendLine();
            builder.AppendFormat("WindowSize: {0}", WindowSize); builder.AppendLine();
            builder.AppendFormat("WindowOrigin: {0}", WindowOrigin); builder.AppendLine();
            builder.AppendFormat("FontSize: {0}", FontSize); builder.AppendLine();
            builder.AppendFormat("FontWeight: {0}", FontWeight); builder.AppendLine();
            builder.AppendFormat("FaceName: {0}", FaceName); builder.AppendLine();
            if (FaceNamePadding != null)
            {
                builder.AppendFormat("FaceName padding length: {0}", FaceNamePadding.Length); builder.AppendLine();
                builder.AppendFormat("FaceName padding Hash: {0}", FaceNamePadding.ComputeHash()); builder.AppendLine();
            }
            builder.AppendFormat("CursorSize: {0}", CursorSize); builder.AppendLine();
            builder.AppendFormat("FullScreen: {0}", FullScreen); builder.AppendLine();
            builder.AppendFormat("FastEdit: {0}", FastEdit); builder.AppendLine();
            builder.AppendFormat("InsertMode: {0}", InsertMode); builder.AppendLine();
            builder.AppendFormat("AutoPosition: {0}", AutoPosition); builder.AppendLine();
            builder.AppendFormat("HistoryBufferSize: {0}", HistoryBufferSize); builder.AppendLine();
            builder.AppendFormat("NumberOfHistoryBuffers: {0}", NumberOfHistoryBuffers); builder.AppendLine();
            builder.AppendFormat("HistoryDuplicateAllowed: {0}", HistoryDuplicateAllowed); builder.AppendLine();
            if (ColorTable != null)
            {
                builder.AppendFormat("Color Table length: {0}", ColorTable.Length); builder.AppendLine();
                builder.AppendFormat("Color Table Hash: {0}", ColorTable.ComputeHash());
            }
            else
            {
                builder.Append("No data");
            }
            return builder.ToString();
        }

    }
}
