using System;
using System.Threading.Tasks;

namespace WolvenKit.Common.Services
{
    public interface ITweakDBService
    {
        event EventHandler? Loaded;

        public string? GetString(ulong hash);
        public void LoadDB(string path);
        public Task LoadDBAsync(string path);
        public bool IsLoaded { get; set; }
    }
}
