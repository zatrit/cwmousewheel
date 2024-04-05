using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace CWMouseWheel.Patches;

[HarmonyPatch(typeof(PlayerItems), "Update")]
public class ChangeSlot {
    [HarmonyPrefix]
    public static void Prefix(Player ___player) {
        Plugin.print(___player.TryGetInventory(out var inv1) + ": " + inv1);
        if (___player.data.isLocal && ___player.data.physicsAreReady &&
            !___player.HasLockedInput() && ___player.TryGetInventory(out var inventory)) {
            var config = Plugin.Config!;
            if (config.IsZoomKeyPressed) {
                return;
            }

            var curSlot = ___player.data.selectedItemSlot;
            var delta = Math.Sign(Input.GetAxis("Mouse ScrollWheel")) * (config.InvertScroll ? -1 : 1);
            var slots = inventory.slots;

            if (delta == 0 || config.SkipEmptySlots && slots.All(slot => slot.ItemInSlot.item == null)) {
                return;
            }

            do {
                curSlot = (curSlot + delta + slots.Length) % slots.Length;
            } while (config.SkipEmptySlots && !inventory.TryGetItemInSlot(curSlot, out _));

            ___player.data.selectedItemSlot = curSlot;
        }
    }
}

[HarmonyPatch(typeof(ItemKeyTooltip), "GetString")]
public class ZoomKeyInfo {
    private const string SCROLL_PREFIX = "[Scroll]";
    private static string ZoomPrefix => $"[{Plugin.Config.ZoomKeyName} + Scroll]";

    static void Prefix(ref string ___m_key) {
        if (___m_key.StartsWith(SCROLL_PREFIX)) {
            ___m_key = ZoomPrefix + ___m_key[SCROLL_PREFIX.Length..];
        }
    }
}