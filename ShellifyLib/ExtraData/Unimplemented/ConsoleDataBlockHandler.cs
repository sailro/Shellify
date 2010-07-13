//using Shellify.ExtraData;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Drawing;

//namespace Shellify.IO
//{
//    public class ConsoleDataBlockHandler : ExtraDataBlockHandler<ConsoleDataBlock>
//    {

//        public const int UnusedLength = 8;
//        public const int FaceNameLength = 64;
//        public const int ColorTableLength = 64;

//        public ConsoleDataBlockHandler(ConsoleDataBlock item, ShellLinkFile context)
//            : base(item, context)
//        {
//        }

//        public override int ComputedSize
//        {
//            get
//            {
//                //return Marshal.SizeOf(typeof(int)) * 7 +
//                //             Marshal.SizeOf(Item.ScreenBufferSizeX) +
//                //             Marshal.SizeOf(Item.ScreenBufferSizeY) +
//                //             Marshal.SizeOf(Item.WindowSizeX) +
//                //             Marshal.SizeOf(Item.WindowSizeY) +
//                //             Marshal.SizeOf(Item.WindowOriginX) +
//                //             Marshal.SizeOf(Item.WindowOriginY) +
//                //             UnusedLength +
//                //             Marshal.SizeOf(Item.FontSize) +
//                //             Marshal.SizeOf(Item.FontWeight) +
//                //             FaceNameLength +
//                //             Marshal.SizeOf(Item.CursorSize) +
//                //             Marshal.SizeOf(Item.HistoryBufferSize) +
//                //             Marshal.SizeOf(Item.NumberOfHistoryBuffers) +
//                //             ColorTableLength;
//                return base.ComputedSize;
//            }
//        }

//        public override void ReadFrom(System.IO.BinaryReader reader)
//        {
//            base.ReadFrom(reader);
//            Item.FillAttributes = (FillAttributes) reader.ReadUInt16();
//            Item.PopupFillAttributes = (FillAttributes) reader.ReadUInt16();
            
//            Item.ScreenBufferSize = new Size(reader.ReadInt16(), reader.ReadInt16());
//            Item.WindowSize = new Size(reader.ReadInt16(), reader.ReadInt16());
//            Item.WindowOrigin = new Point(reader.ReadInt16(), reader.ReadInt16());

//            reader.ReadBytes(UnusedLength);

//            Item.FontSize = new Size(reader.ReadInt16(), reader.ReadInt16());
//            Item.FontFamily = (Shellify.ExtraData.FontFamily) reader.ReadUInt32();
//            Item.FontWeight = reader.ReadUInt32();
//            Item.FaceName = IOHelper.ReadASCIIZ(reader, Encoding.Unicode);
//            Item.CursorSize = reader.ReadUInt32();

//            Item.FullScreen = reader.ReadUInt32() > 0;
//            Item.InsertMode = reader.ReadUInt32() > 0;
//            Item.AutoPosition = reader.ReadUInt32() > 0;

//            Item.HistoryBufferSize = reader.ReadUInt32();
//            Item.NumberOfHistoryBuffers = reader.ReadUInt32();
//            Item.HistoryDuplicateAllowed = reader.ReadUInt32() > 0;
//            Item.ColorTable = reader.ReadBytes(ColorTableLength);
//        }

//        public override void WriteTo(System.IO.BinaryWriter writer)
//        {
//            base.WriteTo(writer);
//        }

//    }
//}
