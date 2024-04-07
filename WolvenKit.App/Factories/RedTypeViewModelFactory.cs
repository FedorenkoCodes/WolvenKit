using WolvenKit.App.Controllers;
using WolvenKit.App.Services;
using WolvenKit.App.ViewModels.Shell;
using WolvenKit.App.ViewModels.Shell.RedTypes;
using WolvenKit.Common;
using WolvenKit.Common.Services;
using WolvenKit.Core.Interfaces;

namespace WolvenKit.App.Factories;

public class RedTypeViewModelFactory : IRedTypeViewModelFactory
{
    private readonly ILoggerService _loggerService;
    private readonly ISettingsManager _settingsManager;
    private readonly IGameControllerFactory _gameControllerFactory;
    private readonly ITweakDBService _tweakDbService;
    private readonly IArchiveManager _archiveManager;

    public RedTypeViewModelFactory(ILoggerService loggerService, ISettingsManager settingsManager, IGameControllerFactory gameControllerFactory, ITweakDBService tweakDbService, IArchiveManager archiveManager)
    {
        _loggerService = loggerService;
        _settingsManager = settingsManager;
        _gameControllerFactory = gameControllerFactory;
        _tweakDbService = tweakDbService;
        _archiveManager = archiveManager;
    }

    public RedTypeHelper RedTypeHelper(AppViewModel appViewModel) => new(appViewModel, _loggerService, _settingsManager, _gameControllerFactory, _tweakDbService, _archiveManager);
}