using System;
using BepInEx.Configuration;
using UnityEngine;

namespace CWMouseWheel;

public class PluginConfig {
    static readonly KeyboardShortcut _defaultZoomKey = new(KeyCode.Z);

    private ConfigEntry<KeyboardShortcut>? _zoomKey;
    private ConfigEntry<bool>? _invertScroll;
    private ConfigEntry<bool>? _skipEmptySlots;
    private ConfigEntry<bool>? _skipArtifactSlot;

    public bool IsZoomKeyPressed => _zoomKey!.Value.IsPressed();
    public bool InvertScroll => _invertScroll!.Value;
    public bool SkipEmptySlots => _skipEmptySlots!.Value;
    public bool SkipArtifactSlot => _skipArtifactSlot!.Value;

    public string? ZoomKeyName => _zoomKey?.Value.MainKey.ToString();

    public Action<bool> OnPluginToggled = _ => {};

    public void Init(ConfigFile config) {
        var enabled = config.Bind("Input", "Enabled", true);
        _zoomKey = config.Bind("Input", "Camera zoom key", _defaultZoomKey);
        _invertScroll = config.Bind("Input", "Invert scroll", true);
        _skipEmptySlots = config.Bind("Input", "Skip empty slots", true);
        _skipArtifactSlot = config.Bind("Input", "Skip artifact slot", false);

        enabled.SettingChanged += (s, a) => {
            OnPluginToggled(((ConfigEntry<bool>)((SettingChangedEventArgs)a).ChangedSetting).Value);
        };

        OnPluginToggled(enabled.Value);
    }
}