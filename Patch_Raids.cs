using HarmonyLib;
using I_m_behind_you;
using RimWorld;
using Verse;

[HarmonyPatch(typeof(StorytellerUtility), nameof(StorytellerUtility.DefaultThreatPointsNow))]
public static class Patch_Raids
{
    // __result é o valor de retorno original do método
    public static void Postfix(ref float __result)
    {
        // Pega o seu componente de visibilidade
        var visibilityTracker = Current.Game.GetComponent<VisibilityTracker>();
        if (visibilityTracker != null)
        {
            // Modifica os pontos de ameaça com base na visibilidade
            // Ex: visibilidade alta aumenta, visibilidade baixa diminui
            float visibilityModifier = visibilityTracker.visibilityScore / 500f; // Exemplo
            __result *= visibilityModifier;
        }
    }
}