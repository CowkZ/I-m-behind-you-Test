using HarmonyLib;
using RimWorld;
using Verse;
using System;

namespace Im_behind_you
{
    // A correção está aqui, no nome do método
    [HarmonyPatch(typeof(IncidentWorker), nameof(IncidentWorker.ChanceFactorNow))]
    public static class Patch_PositiveIncidents_Chance
    {
        // A assinatura do Postfix precisa ser ajustada para corresponder ao método ChanceFactorNow
        public static void Postfix(IncidentWorker __instance, IIncidentTarget target, ref float __result)
        {
            if (__result <= 0) return;

            var visibilityTracker = Current.Game.GetComponent<VisibilityTracker>();
            if (visibilityTracker == null) return;

            // Lista de eventos positivos que queremos influenciar
            bool isPositiveEvent = __instance is IncidentWorker_TraderCaravanArrival ||
                                   __instance is IncidentWorker_VisitorGroup ||
                                   __instance is IncidentWorker_ResourcePodCrash;

            if (isPositiveEvent)
            {
                // --- LÓGICA DE BÔNUS AUTO-BALANCEADA ---
                float currentVisibility = visibilityTracker.visibilityScore;
                float normalThreatPoints = StorytellerUtility.DefaultThreatPointsNow(target);
                if (normalThreatPoints <= 0) normalThreatPoints = 1f;

                float visibilityRatio = currentVisibility / normalThreatPoints;

                if (visibilityRatio > 1f)
                {
                    float bonusMultiplier = 1f + ((visibilityRatio - 1f) / 5f);

                    Log.Message($"[I'm behind you] Evento Positivo '{__instance.def.defName}': Visibilidade ({currentVisibility:F0}) é maior que a Ameaça Normal ({normalThreatPoints:F0}). " +
                                $"Bônus de chance: {bonusMultiplier:F2}x. Chance Final: {(__result * bonusMultiplier):P0}");

                    __result *= bonusMultiplier;
                }
            }
        }
    }
}