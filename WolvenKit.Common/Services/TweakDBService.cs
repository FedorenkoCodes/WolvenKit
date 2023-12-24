using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using WolvenKit.Common.Model;
using WolvenKit.RED4.TweakDB;
using WolvenKit.RED4.TweakDB.Helper;
using WolvenKit.RED4.Types;

namespace WolvenKit.Common.Services
{
    public class TweakDBService : ITweakDBService
    {
        private const string s_tweakdbstr = "WolvenKit.Common.Resources.tweakdbstr.kark";
        private const string s_userStrs = "userStrs.kark";

        private static readonly TweakDBStringHelper s_stringHelper = new();
        private static TweakDB s_tweakDb = new();

        private bool _isLoading;

        public bool IsLoaded { get; set; }
        public event EventHandler? Loaded;

        public TweakDBService()
        {
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(s_tweakdbstr);
            ArgumentNullException.ThrowIfNull(resourceStream);

            s_stringHelper.LoadFromStream(resourceStream);

            var userStrsPath = Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory) ?? throw new InvalidOperationException(), s_userStrs);
            if (File.Exists(userStrsPath))
            {
                s_stringHelper.Load(userStrsPath);
            }

            userStrsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "REDModding", "WolvenKit", s_userStrs);
            if (File.Exists(userStrsPath))
            {
                s_stringHelper.Load(userStrsPath);
            }

            TweakDBIDPool.ResolveHashHandler = s_stringHelper.GetString;
        }

        private void OnLoadDB() => Loaded?.Invoke(this, EventArgs.Empty);

        public void LoadDB(string path)
        {
            if (IsLoaded || _isLoading)
            {
                return;
            }

            _isLoading = true;

            using var fh = File.OpenRead(path);
            using var reader = new TweakDBReader(fh);

            if (reader.ReadFile(out var tweakDb) == WolvenKit.RED4.TweakDB.EFileReadErrorCodes.NoError)
            {
                s_tweakDb = tweakDb!;
                OnLoadDB();

                IsLoaded = true;
            }

            _isLoading = false;
        }

        public async Task LoadDBAsync(string path)
        {
            if (IsLoaded || _isLoading)
            {
                return;
            }

            _isLoading = true;

            await Task.Run(() =>
            {
                using var fh = File.OpenRead(path);
                using var reader = new TweakDBReader(fh);

                if (reader.ReadFile(out var tweakDb) == WolvenKit.RED4.TweakDB.EFileReadErrorCodes.NoError)
                {
                    s_tweakDb = tweakDb!;
                    OnLoadDB();

                    IsLoaded = true;
                }

                _isLoading = false;
            });
        }

        public static bool Exists(TweakDBID key) => s_tweakDb.Flats.Exists(key) || s_tweakDb.Records.Exists(key);

        public string? GetString(ulong key) => s_stringHelper.GetString(key);

        public static IRedType? GetFlat(TweakDBID tdb) => s_tweakDb.Flats.GetValue((ulong)tdb);
        public static List<TweakDBID>? GetQuery(TweakDBID tdb) => s_tweakDb.Queries.GetQuery((ulong)tdb);
        public static byte? GetGroupTag(TweakDBID tdb) => s_tweakDb.GroupTags.GetGroupTag((ulong)tdb);

        public static bool TryGetType(TweakDBID tweakDBID, [NotNullWhen(true)] out Type? type)
        {
            var hash = (ulong)tweakDBID;

            var recordType = s_tweakDb.Records.GetRecord(hash);
            if (recordType != null)
            {
                type = recordType;
                return true;
            }

            var flatValue = s_tweakDb.Flats.GetValue(hash);
            if (flatValue != null)
            {
                type = flatValue.GetType();
                return true;
            }

            type = null; 
            return false;
        }

        //public Type GetType(TweakDBID tdb)
        //{
        //    var hash = (ulong)tdb;

        //    var recordType = s_tweakDb.Records.GetRecord(hash);
        //    if (recordType != null)
        //    {
        //        return recordType;
        //    }

        //    var flatValue = s_tweakDb.Flats.GetValue(hash);
        //    if (flatValue != null)
        //    {
        //        return flatValue.GetType();
        //    }

        //    throw new NotImplementedException();
        //}

        public static List<TweakDBID> GetRecords() => s_tweakDb.GetRecords();
        public static List<TweakDBID> GetFlats() => s_tweakDb.GetFlats();
        public static List<TweakDBID> GetQueries() => s_tweakDb.GetQueries();
        public static List<TweakDBID> GetGroupTags() => s_tweakDb.GetGroupTags();

        public static gamedataTweakDBRecord? GetRecord(TweakDBID tdb) => s_tweakDb.GetFullRecord(tdb);
        public static gamedataTweakDBRecord? GetRecord(SAsciiString path) => s_tweakDb.GetFullRecord(path.ToString());

        public static IRedType? GetFlat(SAsciiString path) => s_tweakDb.GetFlatValue(path.ToString());
    }
}
