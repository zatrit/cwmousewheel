﻿extern alias RealUnity;

using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using RealUnity::UnityEngine;
using static CWMouseWheel.MyPluginInfo;

namespace CWMouseWheel;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    private static KeyboardShortcut? Zoom = null;

    public static bool IsZoomKeyPressed => Zoom!.Value.IsDown();

    public static bool InvertScroll;

    void Awake() {
        var enabled = Config.Bind("Input", "Enabled", true).Value;
        Zoom = Config.Bind<KeyboardShortcut>("Input", "Camera zoom key", new(KeyCode.Z)).Value;
        InvertScroll = Config.Bind("Input", "Invert scrool", true).Value;

        if (enabled) {
            new Harmony(PLUGIN_GUID).PatchAll();
        }
    }
}