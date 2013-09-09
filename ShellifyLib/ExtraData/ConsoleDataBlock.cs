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
            var builder = new StringBuilder();
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
