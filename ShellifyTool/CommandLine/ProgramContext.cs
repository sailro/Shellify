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
            StringBuilder builder = new StringBuilder();

            foreach(object item in System.Enum.GetNames(enumType)) {
                if (builder.Length > 0) {
                    builder.Append(",");
                }
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

            List<Command> allcommands = new List<Command>() { anonymize, createAbsolute, displayInfos, createRelative, update };
            List<Command> writecommands = new List<Command>() { anonymize, createAbsolute, createRelative, update };
            Commands = allcommands.AsReadOnly();
            WriteCommands = writecommands.AsReadOnly();

            Option atime = CreateDateTimeOption("atime", "Header", "AccessTime");
            Option ctime = CreateDateTimeOption("ctime", "Header", "CreationTime");
            Option wtime = CreateDateTimeOption("wtime", "Header", "WriteTime");

            Option fsize = CreateOption("fsize", "Header", "FileSize");
            Option iidx = CreateOption("iidx", "Header", "IconIndex");
            Option iloc = CreateOption("iloc", "IconLocation");
            Option name = CreateOption("name", "Name");
            Option rpath = CreateOption("rpath", "RelativePath");
            Option wdir = CreateOption("wdir", "WorkingDirectory");
            Option args = CreateOption("args", "Arguments");

            Option fattr = CreateEnumOption("fattr", typeof(FileAttributes), "Header", "FileAttributes");
            Option swin = CreateEnumOption("scmd", typeof(ShowCommand), "Header", "ShowCommand");

            Option dlpt = CreateHeaderFlagOption("dlpt", LinkFlags.DisableLinkPathTracking);
            Option dkft = CreateHeaderFlagOption("dkft", LinkFlags.DisableKnownFolderTracking);
            Option dkfa = CreateHeaderFlagOption("dkfa", LinkFlags.DisableKnownFolderAlias);
            Option fnli = CreateHeaderFlagOption("fnli", LinkFlags.ForceNoLinkInfo);
            Option fnlt = CreateHeaderFlagOption("fnlt", LinkFlags.ForceNoLinkTrack);
            Option npa = CreateHeaderFlagOption("npa", LinkFlags.NoPidlAlias);
            Option risp = CreateHeaderFlagOption("risp", LinkFlags.RunInSeparateProcess);
            Option rau = CreateHeaderFlagOption("rau", LinkFlags.RunAsUser);

            List<Option> alloptions = new List<Option>() { atime, ctime, wtime, fsize, iidx, iloc, name, rpath, wdir, args, fattr, swin, dlpt, dkft, dkfa, fnli, fnlt, npa, risp, rau};
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
            DateTimeFormatInfo info = new DateTimeFormatInfo();
            string description = string.Format("Set {0} (\"{1} {2}\").", propertyPath[propertyPath.Length - 1], info.ShortDatePattern, info.ShortTimePattern);
            return new ReflectionSetterOption(tag, description, WriteCommands, propertyPath);
        }

        private static Option CreateEnumOption(string tag, Type enumType, params string[] propertyPath)
        {
            bool flags = enumType.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;

            string description = string.Format("Set {0} ({1}).", propertyPath[propertyPath.Length - 1], DumpEnum(enumType));
            if (flags) {
                description = string.Concat(description, " Values can be combined.");
            }

            return new EnumReflectionSetterOption(tag, description, WriteCommands, enumType, propertyPath);
        }

    }
}