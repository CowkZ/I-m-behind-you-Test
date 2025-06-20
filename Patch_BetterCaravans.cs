using HarmonyLib;
using RimWorld;
using Verse;

namespace Im_behind_you
{
    // O alvo do nosso patch é o método que calcula a quantidade de prata de uma caravana
    [HarmonyPatch(typeof(TraderCaravanUtility), "GetTraderCaravanSilver")]
    public static class Patch_BetterCaravans
    {
        // Usamos um Postfix para modificar o valor DEPOIS que o jogo já o calculou
        public static void Postfix(Map map, ref float __result)
        {
            // Se o valor original for zero, não fazemos nada
            if (__result <= 0) return;

            var visibilityTracker = Current.Game.GetComponent<VisibilityTracker>();
            if (visibilityTracker == null) return;

            // --- LÓGICA DE BÔNUS AUTO-BALANCEADA ---

            // 1. Pega a Visibilidade atual do nosso mod
            float currentVisibility = visibilityTracker.visibilityScore;

            // 2. Calcula os pontos de ameaça "normais" para a colônia. Esta é nossa base.
            float normalThreatPoints = StorytellerUtility.DefaultThreatPointsNow(map);
            if (normalThreatPoints <= 0) normalThreatPoints = 1f;

            // 3. Compara a Visibilidade com o "normal" para obter uma proporção
            float visibilityRatio = currentVisibility / normalThreatPoints;

            // Se a nossa visibilidade for maior que o normal, calculamos um bônus.
            if (visibilityRatio > 1f)
            {
                // O bônus é a diferença da proporção, dividido por um fator para não ser forte demais.
                // Ex: Se a visibilidade for o dobro (ratio=2.0), o bônus será de 20% (1 + (1 / 5))
                float wealthBonusMultiplier = 1f + ((visibilityRatio - 1f) / 5f);

                // Log para depuração
                Log.Message($"[I'm behind you] CARAVAN PATCH: Prata original: {__result:F0}. Visibilidade ({currentVisibility:F0}) > Ameaça ({normalThreatPoints:F0}). " +
                            $"Bônus de Riqueza: {wealthBonusMultiplier:F2}x. Prata Final: {(__result * wealthBonusMultiplier):F0}");

                // Aplica o bônus à quantidade de prata final
                __result *= wealthBonusMultiplier;
            }
        }
    }
}