using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Utf8Json;

namespace NGo.Commands
{

    internal abstract class BaseCommand
    {
        protected FileInfo DefaultConfig { get; }

        public abstract FileInfo ConfigFile { get; set; }

        protected BaseCommand()
        {
            string baseFolder;
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                    baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    break;
                default:
                    baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    break;
            }
            DefaultConfig = new FileInfo(Path.Join(
                    EnsureDirectoryExists(
                        Path.Join(
                            baseFolder,
                            ".NGo"
                        )
                    ),
                    "NGo.json"
                )
            );
        }

        public abstract Task ExecuteAsync();

        protected async Task<Dictionary<string, string>> ReadConfig(FileInfo configFile = null)
        {
            var configTuple = await TryReadConfigFile(configFile);
            if (!configTuple.DidSucceed)
            {
                configTuple = await TryReadConfigFile(DefaultConfig);
            }

            return configTuple.DidSucceed && !string.IsNullOrWhiteSpace(configTuple.ConfigJson)
                ? JsonSerializer.Deserialize<Dictionary<string, string>>(configTuple.ConfigJson)
                : new Dictionary<string, string>();
        }

        protected async Task WriteConfig(Dictionary<string, string> config)
        {
            using var stream = (ConfigFile ?? DefaultConfig).Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            await JsonSerializer.SerializeAsync(stream, config);
        }

        private static string EnsureDirectoryExists(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch
            {
                // Ignored
            }

            return path;
        }

        private static async Task<(bool DidSucceed, string ConfigJson)> TryReadConfigFile(FileInfo configFile)
        {
            if (configFile != null)
            {
                try
                {
                    using var stream = configFile.OpenText();
                    var configJson = await stream.ReadToEndAsync();
                    return (true, configJson);
                }
                catch
                {
                    // ignored
                }
            }

            return (false, string.Empty);
        }
    }
}