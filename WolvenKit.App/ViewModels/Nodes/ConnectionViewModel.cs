﻿namespace WolvenKit.App.ViewModels.Nodes;

public class ConnectionViewModel
{
    public ConnectionViewModel(OutputConnectorViewModel source, InputConnectorViewModel target)
    {
        Source = source;
        Target = target;

        Source.IsConnected = true;
        Target.IsConnected = true;
    }

    public OutputConnectorViewModel Source { get; }
    public InputConnectorViewModel Target { get; }
}