using System;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.IO;
using System.Threading.Tasks;
using CP77Tools.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WolvenKit.Common;

namespace CP77Tools.Commands
{
    [Obsolete("Use ConvertCommand")]
    internal class CR2WCommand : CommandBase
    {
        private const string s_description = "[DEPRECATED] cr2w file conversion command.";
        private const string s_name = "cr2w";

        public CR2WCommand() : base(s_name, s_description)
        {
            AddArgument(new Argument<FileSystemInfo[]>("path", "Input path to a CR2W file or folder."));

            // deprecated. keep for backwards compatibility
            AddOption(new Option<FileSystemInfo[]>(new[] { "--path", "-p" }, "[Deprecated] Input path to a CR2W file or folder."));

            AddOption(new Option<DirectoryInfo>(new[] { "--outpath", "-o" }, "Output directory path."));
            AddOption(new Option<bool>(new[] { "--deserialize", "-d" }, "Create a CR2W file from json."));
            AddOption(new Option<bool>(new[] { "--serialize", "-s" }, "Serialize the CR2W file to json."));

            AddOption(new Option<string>(new[] { "--pattern", "-w" }, "Search pattern (e.g. *.ink), if both regex and pattern is defined, pattern will be prioritized"));
            AddOption(new Option<string>(new[] { "--regex", "-r" }, "Regex search pattern."));

            SetHandler(CommandHandler.Create<FileSystemInfo[], DirectoryInfo, bool, bool, string, string, IHost>(Action));
        }

        private async Task Action(FileSystemInfo[] path, DirectoryInfo outpath, bool deserialize, bool serialize, string pattern,
            string regex, IHost host)
        {
            var serviceProvider = host.Services;
            var consoleFunctions = serviceProvider.GetRequiredService<ConsoleFunctions>();
            await consoleFunctions.Cr2wTask(path, outpath, deserialize, serialize, pattern, regex, ETextConvertFormat.json);
        }
    }
}
