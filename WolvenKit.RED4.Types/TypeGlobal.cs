using System;
using Serilog.Core;
using WolvenKit.Core.Interfaces;

namespace WolvenKit.RED4.Types;

public static class TypeGlobal
{
    public static ILoggerService Logger;

    public static bool OnParsingError(ParsingErrorEventArgs e)
    {
        if (e is InvalidDefaultValueEventArgs)
        {
            return true;
        }

        if (e is InvalidRTTIEventArgs e1)
        {
            if (e1.ExpectedType == typeof(redResourceReferenceScriptToken) && e1.ActualType == typeof(CString))
            {
                var orgStr = (string)(CString)e1.Value;

                e1.Value = new redResourceReferenceScriptToken
                {
                    Resource = new CResourceAsyncReference<CResource>
                    {
                        DepotPath = orgStr
                    }
                };

                Logger?.Warning($"Invalid in wolven rtti: [Expected: \"{e1.ExpectedType.Name}\" | Got: \"{e1.ActualType.Name}\"]");

                return true;
            }

            if (e1.ExpectedType == typeof(CResourceAsyncReference<inkWidgetLibraryResource>) &&
                e1.ActualType == typeof(CResourceReference<inkWidgetLibraryResource>))
            {
                var orgVal = (CResourceReference<inkWidgetLibraryResource>)e1.Value;

                e1.Value = new CResourceAsyncReference<inkWidgetLibraryResource>
                {
                    DepotPath = orgVal.DepotPath,
                    Flags = orgVal.Flags,
                };

                Logger?.Warning($"Invalid in wolven rtti: [Expected: \"{e1.ExpectedType.Name}\" | Got: \"{e1.ActualType.Name}\"]");

                return true;
            }
        }

        if (e is InvalidEnumValueEventArgs e2)
        {
            e2.Value = (Enum)System.Activator.CreateInstance(e2.EnumType);

            Logger?.Warning($"Unknown enum value found for type \"{e2.EnumType.Name}\": {e2.StringValue}");

            return true;
        }

        return false;
    }
}
