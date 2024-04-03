extern alias RealUnity;
using BepInEx.Configuration;
using RealUnity::UnityEngine;

namespace CWMouseWheel;

public delegate void ModToggleHandler(bool enabled);

public class PluginConfig {
    static readonly KeyboardShortcut _defaultZoomKey = new(KeyCode.Z);

    readonly ConfigEntry<KeyboardShortcut> _zoomKey;
    readonly ConfigEntry<bool> _invertScroll;
    readonly ConfigEntry<bool> _enabled;

    public bool IsZoomKeyPressed => _zoomKey.Value.IsPressed();
    public bool InvertScroll => _invertScroll!.Value;

    public ModToggleHandler OnPluginToggled = new(_ => { });

    public PluginConfig(ConfigFile config) {
        _zoomKey = config.Bind("Input", "Camera zoom key", _defaultZoomKey);
        _invertScroll = config.Bind("Input", "Invert scroll", true);
        _enabled = config.Bind("Input", "Enabled", true);

        _enabled.SettingChanged += (s, a) => {
            OnPluginToggled(((ConfigEntry<bool>)((SettingChangedEventArgs)a).ChangedSetting).Value);
        };
    }

    public void Reload() => OnPluginToggled(_enabled.Value);
}