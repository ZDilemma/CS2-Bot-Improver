/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;

namespace InventorySimulator;

public static class ConVars
{
    public static readonly FakeConVar<bool> IsFallbackTeam = new(
        "invsim_fallback_team",
        "Allow using skins from any team (prioritizes current team first).",
        false
    );

    public static readonly FakeConVar<int> MinModels = new(
        "invsim_minmodels",
        "Enable player agents (0 = enabled, 1 = use map models per team, 2 = SAS & Phoenix).",
        0
    );

    public static void Initialize(BasePlugin plugin)
    {
        plugin.RegisterFakeConVars(typeof(ConVars));
    }
}
