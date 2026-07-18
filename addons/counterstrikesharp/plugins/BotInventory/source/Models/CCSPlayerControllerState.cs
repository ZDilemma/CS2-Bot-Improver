/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using CounterStrikeSharp.API.Core;

namespace InventorySimulator;

public class CCSPlayerControllerState(
    ulong steamId,
    ulong ownerKey,
    nint controllerHandle,
    PlayerInventory? inventory
)
{
    public ulong SteamID = steamId;
    public ulong OwnerKey = ownerKey;
    public nint ControllerHandle = controllerHandle;
    public PlayerInventory? Inventory = inventory;

    private static readonly ConcurrentDictionary<
        (ulong OwnerKey, int Team, int Slot, ushort DefIndex, bool IsLoadoutView),
        nint
    > _econItemViewManager = [];

    // Agent loadout views deliberately use a separate cache. They are constructed from zero and
    // never copied from the BOT's original custom-player slot, so stale static item data cannot
    // leak from a previous/default agent into a paid-agent identity.
    private static readonly ConcurrentDictionary<
        (ulong OwnerKey, int Team, ushort DefIndex),
        nint
    > _agentItemViewManager = [];

    public nint GetEconItemView(
        int team,
        int slot,
        InventoryItem item,
        nint copyFrom = 0,
        bool isLoadoutView = false
    )
    {
        ushort defIndex = item.Def ?? 0;
        var key = (OwnerKey, team, slot, defIndex, isLoadoutView);
        ulong? accountSteamId = SteamID != 0 ? SteamID : null;

        if (_econItemViewManager.TryGetValue(key, out var ptr))
        {
            var existingItemView = new CEconItemView(ptr);
            existingItemView.ApplyAttributes(
                item,
                (loadout_slot_t)slot,
                accountSteamId,
                assignNewItemId: false
            );
            return ptr;
        }

        var itemView = SchemaHelper.CreateCEconItemView(copyFrom);
        itemView.ApplyAttributes(
            item,
            (loadout_slot_t)slot,
            accountSteamId,
            assignNewItemId: true
        );
        _econItemViewManager[key] = itemView.Handle;
        return itemView.Handle;
    }

    public nint GetAgentEconItemView(int team, ushort defIndex, uint fallbackAccountId)
    {
        var key = (OwnerKey, team, defIndex);
        if (_agentItemViewManager.TryGetValue(key, out var ptr))
        {
            var existing = new CEconItemView(ptr);
            existing.ApplyAgentIdentity(defIndex, fallbackAccountId, assignNewItemId: false);
            return ptr;
        }

        var itemView = SchemaHelper.CreateCEconItemView();
        itemView.ApplyAgentIdentity(defIndex, fallbackAccountId, assignNewItemId: true);
        _agentItemViewManager[key] = itemView.Handle;
        return itemView.Handle;
    }

    public void ClearEconItemView()
    {
        foreach (var key in _econItemViewManager.Keys)
        {
            if (key.OwnerKey != OwnerKey)
                continue;

            if (_econItemViewManager.TryRemove(key, out var ptr) && ptr != nint.Zero)
                Marshal.FreeHGlobal(ptr);
        }

        foreach (var key in _agentItemViewManager.Keys)
        {
            if (key.OwnerKey != OwnerKey)
                continue;

            if (_agentItemViewManager.TryRemove(key, out var ptr) && ptr != nint.Zero)
                Marshal.FreeHGlobal(ptr);
        }
    }

    public static void ClearAllEconItemView()
    {
        foreach (var key in _econItemViewManager.Keys)
        {
            if (_econItemViewManager.TryRemove(key, out var ptr) && ptr != nint.Zero)
                Marshal.FreeHGlobal(ptr);
        }

        foreach (var key in _agentItemViewManager.Keys)
        {
            if (_agentItemViewManager.TryRemove(key, out var ptr) && ptr != nint.Zero)
                Marshal.FreeHGlobal(ptr);
        }
    }
}
