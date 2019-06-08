using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using UnityEngine;
using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;

using Harmony;
using Harmony.ILCopying;

using Rimhammer40k.Orks;

namespace Rimhammer40k
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        private static readonly Type patchType = typeof(HarmonyPatches);

        static HarmonyPatches()
        {
            HarmonyInstance Rimhammer40k = HarmonyInstance.Create("com.rimhammer40k.rimworld.mod");
            
            // Remove Orkoid ability to get food poisoning.
            Rimhammer40k.Patch(AccessTools.Method(typeof(FoodUtility), "AddFoodPoisoningHediff", null, null), new HarmonyMethod(HarmonyPatches.patchType, "AddFoodPoisoningHediffPrefix"), null, null);

            Rimhammer40k.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static bool AddFoodPoisoningHediffPrefix(Pawn pawn, Thing ingestible, FoodPoisonCause cause)
        {
            if (pawn.IsOrk())
            {
                return false;
            }
            return true;
        }
    }
}
