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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Shellify.Core;
using Shellify.Tool.Commands;
using Shellify.Tool.Options;

namespace Shellify.Tool.CommandLine
{
    public class ProgramContext
    {

        public static IList<Command> Commands { get; private set; }
        public static IList<Command> WriteCommands { get; private set; }
        public static IList<Option> Options { get; private set; }

        private static string DumpEnum(Type enumType) {
            var builder = new StringBuilder();

            foreach(object item in Enum.GetNames(enumType)) {
                if (builder.Length > 0) 
                    builder.Append(",");
                
                builder.Append(item);
            }

            return builder.ToString();
        }

        static ProgramContext()
        {
            Command anonymize = new AnonimyzeCommand("A", "Anonymize .LNK file (remove all tracking data).");
            Command createAbsolute = new CreateAbsoluteCommand("C", "Create absolute .LNK file.");
            Command displayInfos = new DisplayInfosCommand("D", "Display .LNK file information.");
            Command createRelative = new CreateRelativeCommand("R", "Create relative .LNK file.");
            Command update = new UpdateCommand("U", "Update .LNK file.");

            var allcommands = new List<Command> { anonymize, createAbsolute, displayInfos, createRelative, update };
            var writecommands = new List<Command> { anonymize, createAbsolute, createRelative, update };
            Commands = allcommands.AsReadOnly();
            WriteCommands = writecommands.AsReadOnly();

            var atime = CreateDateTimeOption("atime", "Header", "AccessTime");
            var ctime = CreateDateTimeOption("ctime", "Header", "CreationTime");
            var wtime = CreateDateTimeOption("wtime", "Header", "WriteTime");

            var fsize = CreateOption("fsize", "Header", "FileSize");
            var iidx = CreateOption("iidx", "Header", "IconIndex");
            var iloc = CreateOption("iloc", "IconLocation");
            var name = CreateOption("name", "Name");
            var rpath = CreateOption("rpath", "RelativePath");
            var wdir = CreateOption("wdir", "WorkingDirectory");
            var args = CreateOption("args", "Arguments");

            var fattr = CreateEnumOption("fattr", typeof(FileAttributes), "Header", "FileAttributes");
            var swin = CreateEnumOption("scmd", typeof(ShowCommand), "Header", "ShowCommand");

            var dlpt = CreateHeaderFlagOption("dlpt", LinkFlags.DisableLinkPathTracking);
            var dkft = CreateHeaderFlagOption("dkft", LinkFlags.DisableKnownFolderTracking);
            var dkfa = CreateHeaderFlagOption("dkfa", LinkFlags.DisableKnownFolderAlias);
            var fnli = CreateHeaderFlagOption("fnli", LinkFlags.ForceNoLinkInfo);
            var fnlt = CreateHeaderFlagOption("fnlt", LinkFlags.ForceNoLinkTrack);
            var npa = CreateHeaderFlagOption("npa", LinkFlags.NoPidlAlias);
            var risp = CreateHeaderFlagOption("risp", LinkFlags.RunInSeparateProcess);
            var rau = CreateHeaderFlagOption("rau", LinkFlags.RunAsUser);

            var alloptions = new List<Option> { atime, ctime, wtime, fsize, iidx, iloc, name, rpath, wdir, args, fattr, swin, dlpt, dkft, dkfa, fnli, fnlt, npa, risp, rau};
            Options = alloptions.AsReadOnly();
        }

        private static Option CreateHeaderFlagOption(string tag, LinkFlags flag)
        {
            string description = string.Format("Set/Unset {0} flag (true/false).", flag);
            return new HeaderFlagOption(tag, description, WriteCommands, flag);
        }

        private static Option CreateOption(string tag, params string[] propertyPath)
        {
            string description = string.Format("Set {0}.", propertyPath[propertyPath.Length - 1]);
            return new ReflectionSetterOption(tag, description, WriteCommands, propertyPath);
        }

        private static Option CreateDateTimeOption(string tag, params string[] propertyPath)
        {
            var info = new DateTimeFormatInfo();
            var description = string.Format("Set {0} (\"{1} {2}\").", propertyPath[propertyPath.Length - 1], info.ShortDatePattern, info.ShortTimePattern);
            return new ReflectionSetterOption(tag, description, WriteCommands, propertyPath);
        }

        private static Option CreateEnumOption(string tag, Type enumType, params string[] propertyPath)
        {
            var flags = enumType.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;

            var description = string.Format("Set {0} ({1}).", propertyPath[propertyPath.Length - 1], DumpEnum(enumType));
            if (flags) 
                description = string.Concat(description, " Values can be combined.");

            return new EnumReflectionSetterOption(tag, description, WriteCommands, enumType, propertyPath);
        }

    }
}