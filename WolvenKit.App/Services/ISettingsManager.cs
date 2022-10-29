using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using WolvenKit.Common.DDS;

namespace WolvenKit.Functionality.Services
{

    public interface ISettingsManager : ISettingsDto, INotifyPropertyChanged
    {
        #region lifecyclestuff

        void Save();
        void Bounce();

        bool IsHealthy();


        #endregion lifecyclestuff

        #region settingspropertystuff

        public bool IsUpdateAvailable { get; set; }

        string GetRED4OodleDll();

        string GetRED4GameRootDir();

        string GetRED4GameExecutablePath();

        string GetRED4GameLaunchCommand();

        string GetRED4GameLaunchOptions();

        public string GetRED4GameLegacyModDir();
        public string GetRED4GameModDir();


        public static string GetAppData()
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "REDModding", "WolvenKit");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static string GetManagerCacheDir()
        {
            var dir = Path.Combine(GetAppData(), "Config");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static string GetWorkDir()
        {
            var dir = Path.Combine(GetAppData(), "tmp_workdir");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static string GetTemp_AudioPath()
        {
            var dir = Path.Combine(GetAppData(), "Temp_Audio");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static string GetTemp_OBJPath()
        {
            var dir = Path.Combine(GetAppData(), "Temp_OBJ");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static string GetTemp_Audio_importPath()
        {
            var dir = Path.Combine(GetAppData(), "Temp_Audio_import");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static string GetTemp_Video_PreviewPath()
        {
            var dir = Path.Combine(GetAppData(), "Temp_Video_Preview");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static string GetWebViewDataPath()
        {
            var dir = Path.Combine(GetAppData(), "WebViewData");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static string GetSaveGameDir()
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Saved Games", "CD Projekt Red", "Cyberpunk 2077");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }


        Color GetThemeAccent();

        void SetThemeAccent(Color color);

        string GetVersionNumber();

        #endregion settingspropertystuff
    }
}
