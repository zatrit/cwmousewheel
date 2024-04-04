using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using static System.Reflection.Emit.OpCodes;

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
            var delta = Math.Sign(Input.GetAxis("Mouse ScrollWheel")) * (config.InvertScroll! ? -1 : 1);
            var slots = inventory.slots;

            if (delta == 0 || slots.All(slot => slot.ItemInSlot.item == null)) {
                return;
            }

            do {
                curSlot = (curSlot + delta + slots.Length) % slots.Length;
            } while (config.SkipEmptySlots && !inventory.TryGetItemInSlot(curSlot, out _));

            ___player.data.selectedItemSlot = curSlot;
        }
    }
}

[HarmonyPatch(typeof(VideoCamera), "Update")]
public class ZoomKeyCheck {
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes) {
        var matcher = new CodeMatcher(codes)
            .MatchForward(true, new CodeMatch(Call, typeof(GlobalInputHandler).GetMethod("CanTakeInput")));
        var noZoomLabel = (Label) matcher.InstructionAt(1).operand;

        return matcher
            .MatchForward(true, new CodeMatch(Call, typeof(GlobalInputHandler).GetMethod("CanTakeInput")))
            .Insert(
                new(Ldsfld, typeof(Plugin).GetField("Config")),
                new(Callvirt, typeof(PluginConfig).GetProperty("IsZoomKeyPressed").GetMethod),
                new(Brfalse, noZoomLabel)
            )
            .InstructionEnumeration();
    }
}