//using Shellify.IO;
//using System.Text;
//using System.Drawing;

//namespace Shellify.ExtraData
//{
//    public class ConsoleDataBlock : ExtraDataBlock
//    {

//        public FillAttributes FillAttributes { get; set; }
//        public FillAttributes PopupFillAttributes { get; set; }
//        public Size ScreenBufferSize { get; set; }
//        public Size WindowSize { get; set; }
//        public Point WindowOrigin { get; set; }
//        public Size FontSize { get; set; }
//        public FontFamily FontFamily { get; set; }
//        public uint FontWeight { get; set; }
//        public string FaceName { get; set; }
//        public uint CursorSize { get; set; }
//        public bool FullScreen { get; set; }
//        public bool InsertMode { get; set; }
//        public bool AutoPosition { get; set; }
//        public uint HistoryBufferSize { get; set; }
//        public uint NumberOfHistoryBuffers { get; set; }
//        public bool HistoryDuplicateAllowed { get; set; }
//        public byte[] ColorTable { get; set; }
		
//        public ConsoleDataBlock()
//        {
//            Signature = ExtraDataBlockSignature.ConsoleDataBlock;
//        }

//        public override string ToString() {
//            StringBuilder builder = new StringBuilder();
//            builder.AppendLine(base.ToString());
//            builder.AppendFormat("FillAttributes: {0}", FillAttributes); builder.AppendLine();
//            builder.AppendFormat("PopupFillAttributes: {0}", PopupFillAttributes); builder.AppendLine();
//            builder.AppendFormat("ScreenBufferSize: {0}", ScreenBufferSize); builder.AppendLine();
//            builder.AppendFormat("WindowSize: {0}", WindowSize); builder.AppendLine();
//            builder.AppendFormat("WindowOrigin: {0}", WindowOrigin); builder.AppendLine();
//            builder.AppendFormat("FontSize: {0}", FontSize); builder.AppendLine();
//            builder.AppendFormat("FontWeight: {0}", FontWeight); builder.AppendLine();
//            builder.AppendFormat("FaceName: {0}", FaceName); builder.AppendLine();
//            builder.AppendFormat("CursorSize: {0}", CursorSize); builder.AppendLine();
//            builder.AppendFormat("FullScreen: {0}", FullScreen); builder.AppendLine();
//            builder.AppendFormat("InsertMode: {0}", InsertMode); builder.AppendLine();
//            builder.AppendFormat("AutoPosition: {0}", AutoPosition); builder.AppendLine();
//            builder.AppendFormat("HistoryBufferSize: {0}", HistoryBufferSize); builder.AppendLine();
//            builder.AppendFormat("NumberOfHistoryBuffers: {0}", NumberOfHistoryBuffers); builder.AppendLine();
//            builder.AppendFormat("HistoryDuplicateAllowed: {0}", HistoryDuplicateAllowed); builder.AppendLine();
//            if (ColorTable != null)
//            {
//                builder.AppendFormat("Color Table lenth: {0}", ColorTable.Length); builder.AppendLine();
//                builder.AppendFormat("Hash: {0}", IOHelper.ComputeHash(ColorTable));
//            }
//            else
//            {
//                builder.Append("No data");
//            }
//            return builder.ToString();
//        }

//    }
//}
