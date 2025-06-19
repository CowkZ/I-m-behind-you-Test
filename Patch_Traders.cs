using HarmonyLib;
using I_m_behind_you;
using RimWorld;
using Verse;

[HarmonyPatch(typeof(IncidentWorker_TraderCaravanArrival), "CanFireNow")]
public static class Patch_Traders
{
    // Se este método retornar 'false', o evento é cancelado
    public static bool Prefix(ref bool __result)
    {
        var visibilityTracker = Current.Game.GetComponent<VisibilityTracker>();
        if (visibilityTracker != null && visibilityTracker.visibilityScore < 100) // Exemplo de limite
        {
            __result = false; // Define o resultado como falso
            return false; // Impede o método original de rodar
        }
        return true; // Permite que o método original continue
    }
}