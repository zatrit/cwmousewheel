extern alias RealUnity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RealUnity::UnityEngine;

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
            } while (!inventory.TryGetItemInSlot(curSlot, out _));

            ___player.data.selectedItemSlot = curSlot;
        }
    }
}

[HarmonyPatch(typeof(VideoCamera), "Update")]
public class ZoomKeyCheck {
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes) {
        Label? noZoomLabel = null;
        bool wasHoldCheck = false;

        foreach (var code in codes) {
            yield return code;

            if (wasHoldCheck) {
                noZoomLabel = (Label)code.operand;
            }

            // Checks is ZoomKey pressed
            if (code.Calls(typeof(GlobalInputHandler).GetMethod("CanTakeInput"))) {
                yield return new(Brfalse, noZoomLabel);
                yield return new(Ldsfld, typeof(Plugin).GetField("Config"));
                yield return new(Callvirt, typeof(PluginConfig).GetProperty("IsZoomKeyPressed").GetMethod);
            }

            wasHoldCheck = code.LoadsField(typeof(ItemInstanceBehaviour).GetField("isHeldByMe"));
        }
    }
}