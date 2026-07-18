/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Collections.Concurrent;
using CounterStrikeSharp.API.Core;

namespace InventorySimulator;

public static class CCSPlayerControllerExtensions
{
    private static readonly ConcurrentDictionary<uint, CCSPlayerControllerState> _controllerStateManager = [];

    public static CCSPlayerControllerState GetState(this CCSPlayerController self)
    {
        if (_controllerStateManager.TryGetValue(self.Index, out var existing))
        {
            if (existing.ControllerHandle == self.Handle && existing.SteamID == self.SteamID)
                return existing;

            if (_controllerStateManager.TryRemove(self.Index, out var stale))
            {
                stale.ClearEconItemView();
                Inventories.ReleaseAgentAssignment(stale.OwnerKey);
            }
        }

        return _controllerStateManager.GetOrAdd(self.Index, _ =>
        {
            PlayerInventory? inventory = null;
            if (self.IsBot && Inventories.TryGet(self.SteamID, out var generated, self))
            {
                generated.InitializeWearOverrides();
                inventory = generated;
            }

            return new CCSPlayerControllerState(
                self.SteamID,
                BuildOwnerKey(self),
                self.Handle,
                inventory
            );
        });
    }

    private static ulong BuildOwnerKey(CCSPlayerController self)
    {
        // BotInventory is BOT-only. Keep BOT state isolated by controller index so it does not
        // depend on SteamID, GC inventory state, or BotHider-provided identities.
        return 0xB070000000000000UL | self.Index;
    }

    public static void RemoveState(this CCSPlayerController self)
    {
        RemoveState(self.Index);
    }

    public static void RemoveState(uint controllerIndex)
    {
        if (!_controllerStateManager.TryRemove(controllerIndex, out var controllerState))
            return;

        controllerState.ClearEconItemView();
        Inventories.ReleaseAgentAssignment(controllerState.OwnerKey);
    }

    public static void ClearAllStates()
    {
        foreach (var controllerIndex in _controllerStateManager.Keys)
            RemoveState(controllerIndex);

        CCSPlayerControllerState.ClearAllEconItemView();
    }
}
