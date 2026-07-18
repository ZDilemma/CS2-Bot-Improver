/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace InventorySimulator;

// Lightweight wrapper used only to compare the embedded inventory handle received by
// CCSPlayerInventory::GetItemInLoadout with a controller's InventoryServices object.
public class CCSPlayerInventory(nint handle)
{
    public nint Handle { get; set; } = handle;
}
