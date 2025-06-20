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
            // Se os pontos originais forem muito baixos, não fazemos nada.
            if (__result <= 0) return;

            var visibilityTracker = Current.Game.GetComponent<VisibilityTracker>();
            if (visibilityTracker == null) return;

            // --- NOVA LÓGICA AUTO-BALANCEADA ---

            // 1. Pegamos os pontos de ameaça que o jogo calculou. Este é nosso "normal".
            float vanillaThreatPoints = __result;

            // 2. Pegamos a nossa pontuação de Visibilidade.
            float currentVisibility = visibilityTracker.visibilityScore;

            // 3. Calculamos o multiplicador com base na proporção entre os dois.
            // Se a visibilidade for igual aos pontos de ameaça, o multiplicador será 1.0x.
            float visibilityMultiplier = currentVisibility / vanillaThreatPoints;

            // Garante que o multiplicador não seja baixo demais (ex: mínimo de 10% de dificuldade)
            visibilityMultiplier = Mathf.Max(0.10f, visibilityMultiplier);

            // Log de DEBUG para vermos a nova mágica acontecer
            Log.Message($"[I'm behind you] RAID POINTS: Pontos Vanilla: {vanillaThreatPoints:F0}, " +
                        $"Visibilidade Atual: {currentVisibility:F0}, " +
                        $"Multiplicador Final: {visibilityMultiplier:F2}x, " +
                        $"Pontos Finais: {(__result * visibilityMultiplier):F0}");

            // 4. Aplica nosso novo multiplicador ao resultado final
            __result *= visibilityMultiplier;
        }
    }
}