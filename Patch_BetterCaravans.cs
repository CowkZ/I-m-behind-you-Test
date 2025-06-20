using HarmonyLib;
using RimWorld;
using Verse;

namespace Im_behind_you
{
    // O alvo do patch agora é a CLASSE-MÃE, onde o método é originalmente definido
    [HarmonyPatch(typeof(IncidentWorker), "TryExecuteWorker")]
    public static class Patch_BetterCaravans
    {
        // Usamos um Prefix para modificar os parâmetros ANTES do método original rodar.
        // '__instance' nos permite ver qual tipo de evento está acontecendo.
        // 'ref IncidentParms parms' nos permite alterar o orçamento do evento.
        public static void Prefix(IncidentWorker __instance, ref IncidentParms parms)
        {
            // Verificamos se a instância atual é a que nos interessa (caravana de comércio)
            if (__instance is IncidentWorker_TraderCaravanArrival)
            {
                if (parms == null || parms.target == null) return;

                var visibilityTracker = Current.Game.GetComponent<VisibilityTracker>();
                if (visibilityTracker == null) return;

                // --- LÓGICA DE BÔNUS AUTO-BALANCEADA ---
                float currentVisibility = visibilityTracker.visibilityScore;
                float normalThreatPoints = StorytellerUtility.DefaultThreatPointsNow(parms.target);
                if (normalThreatPoints <= 0) normalThreatPoints = 1f;

                float visibilityRatio = currentVisibility / normalThreatPoints;

                if (visibilityRatio > 1f)
                {
                    float wealthBonusMultiplier = 1f + ((visibilityRatio - 1f) / 5f);

                    Log.Message($"[I'm behind you] CARAVAN PATCH: Orçamento de pontos original: {parms.points:F0}. Aplicando bônus de Visibilidade de {wealthBonusMultiplier:F2}x.");

                    parms.points *= wealthBonusMultiplier;

                    Log.Message($"[I'm behind you] CARAVAN PATCH: Novo orçamento de pontos: {parms.points:F0}.");
                }
            }
        }
    }
}