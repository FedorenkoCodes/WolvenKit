using System;
using System.Collections.Generic;

namespace WolvenKit.App.ViewModels.Shell;

public class ContextMenuItem
{
    public string Name { get; set; }
    public Action? Action { get; set; }
    public List<ContextMenuItem> Children { get; } = new();

    public ContextMenuItem(string name)
    {
        Name = name;
    }

    public ContextMenuItem(string name, Action action)
    {
        Name = name;
        Action = action;
    }
}