using System;
using BepInEx.Configuration;
using UnityEngine;

namespace CWMouseWheel;

public class PluginConfig {
    #region Private fields
    static readonly KeyboardShortcut _defaultZoomKey = new(KeyCode.Z);
    const string SECTION = "Input";

    private ConfigEntry<KeyboardShortcut>? _zoomKey;
    private ConfigEntry<bool>? _invertScroll;
    private ConfigEntry<bool>? _skipEmptySlots;
    private ConfigEntry<bool>? _skipArtifactSlot;
    #endregion

    #region Public fields
    public bool IsZoomKeyPressed => _zoomKey!.Value.IsPressed();
    public bool InvertScroll => _invertScroll!.Value;
    public bool SkipEmptySlots => _skipEmptySlots!.Value;
    public bool SkipArtifactSlot => _skipArtifactSlot!.Value;

    public string? ZoomKeyName => _zoomKey?.Value.MainKey.ToString();

    public Action<bool> OnPluginToggled = _ => { };
    #endregion

    public void Init(ConfigFile config) {
        var enabled = config.Bind(SECTION, "Enabled", true);
        _zoomKey = config.Bind(SECTION, "Camera zoom key", _defaultZoomKey);
        _invertScroll = config.Bind(SECTION, "Invert scroll", true);
        _skipEmptySlots = config.Bind(SECTION, "Skip empty slots", true);
        _skipArtifactSlot = config.Bind(SECTION, "Skip artifact slot", false);

        enabled.SettingChanged += (s, a) => OnPluginToggled(enabled.Value);
        OnPluginToggled(enabled.Value);
    }
}