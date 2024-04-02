﻿using BepInEx;
using HarmonyLib;
using UnityEngine;
using static CWMouseWheel.MyPluginInfo;

namespace CWMouseWheel;

#pragma warning disable IDE0051

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    private static string? ZoomKey = null;

    public static bool IsZoomKeyPressed => Input.GetKey(ZoomKey);

    public static bool InvertScroll;

    void Awake() {
        ZoomKey = Config.Bind("Input", "Camera zoom key", "z").Value;
        InvertScroll = Config.Bind("Input", "Invert scrool", true).Value;

        new Harmony(PLUGIN_GUID).PatchAll();
    }
}