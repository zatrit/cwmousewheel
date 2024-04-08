using System;
using BepInEx.Configuration;
using UnityEngine;

namespace CWMouseWheel;

public class PluginConfig(ConfigFile config) {
    #region Private fields
    static readonly KeyboardShortcut _defaultZoomKey = new(KeyCode.Z);
    const string SECTION = "Input";

    private readonly ConfigEntry<KeyboardShortcut> _zoomKey = Bind(config, "Camera zoom key", _defaultZoomKey);
    private readonly ConfigEntry<bool> _enabled = Bind(config, "Enabled", true);
    private readonly ConfigEntry<bool> _invertScroll = Bind(config, "Invert scroll", true);
    private readonly ConfigEntry<bool> _skipEmptySlots = Bind(config, "Skip empty slots", true);
    private readonly ConfigEntry<bool> _skipArtifactSlot = Bind(config, "Skip artifact slot", false);
    #endregion

    #region Public fields
    public bool IsZoomKeyPressed => _zoomKey.Value.IsPressed();
    public bool InvertScroll => _invertScroll.Value;
    public bool SkipEmptySlots => _skipEmptySlots.Value;
    public bool SkipArtifactSlot => _skipArtifactSlot.Value;

    public string? ZoomKeyName => _zoomKey?.Value.MainKey.ToString();

    public Action<bool> OnPluginToggled = _ => { };
    #endregion

    public void Init() {
        _enabled.SettingChanged += (s, a) => OnPluginToggled(_enabled.Value);
        OnPluginToggled(_enabled.Value);
    }

    private static ConfigEntry<T> Bind<T>(ConfigFile config, string name, T _default)
        => config.Bind(SECTION, name, _default);
}