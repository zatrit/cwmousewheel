﻿using BepInEx;
using HarmonyLib;
using static CWMouseWheel.MyPluginInfo;

#pragma warning disable IDE0051 // Remove unused private members

namespace CWMouseWheel;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    public static new PluginConfig? Config;

    void Awake() {
        var harmony = new Harmony(PLUGIN_GUID);
        Config = new(base.Config);

        Config.OnPluginToggled += enabled => {
            if (enabled)
                harmony.PatchAll();
            else
                harmony.UnpatchSelf();
        };

        Config.Reload();
    }
}