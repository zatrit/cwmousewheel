using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace CWMouseWheel.Patches;

[HarmonyPatch(typeof(PlayerItems), "Update")]
public class ChangeSlot {
    [HarmonyPrefix]
    public static void Prefix(Player ___player) {
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

            var itemCount = slots.Length;
            if (config.SkipArtifactSlot) {
                itemCount -= 1;
            }

            do {
                curSlot = (curSlot + delta + itemCount) % itemCount;
            } while (config.SkipEmptySlots && !inventory.TryGetItemInSlot(curSlot, out _));

            ___player.data.selectedItemSlot = curSlot;
        }
    }
}

[HarmonyPatch(typeof(ItemKeyTooltip), "GetString")]
public class ZoomKeyInfo {
    private const string SCROLL_PREFIX = "[Scroll]";
    private static string ZoomPrefix => $"[{Plugin.Config.ZoomKeyName} + Scroll]";

    static void Postfix(ref string __result, ref string ___m_key) {
        if (__result.StartsWith(SCROLL_PREFIX)) {
            __result = ZoomPrefix + ___m_key[SCROLL_PREFIX.Length..];
        }
    }
}