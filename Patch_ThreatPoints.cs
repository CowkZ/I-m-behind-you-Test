using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Im_behind_you
{
    [HarmonyPatch(typeof(StorytellerUtility), nameof(StorytellerUtility.DefaultThreatPointsNow))]
    public static class Patch_ThreatPoints
    {
        public static void Postfix(ref float __result)
        {
            var visibilityTracker = Current.Game.GetComponent<VisibilityTracker>();
            if (visibilityTracker == null || visibilityTracker.visibilityScore <= 0) return;

            // Guarda o valor original para mostrar no log
            float originalPoints = __result;

            // Pega o nosso valor de visibilidade já calculado
            float finalVisibilityScore = visibilityTracker.visibilityScore;

            // Log para vermos a substituição acontecendo
            Log.Message($"[I'm behind you] RAID PATCH: Pontos vanilla eram {originalPoints:F0}. Substituído pela Visibilidade de {finalVisibilityScore:F0}.");

            // Substituímos o resultado final pelo nosso valor
            __result = finalVisibilityScore;
        }
    }
}