using HarmonyLib;
using RimWorld;
using Verse;
using System; // Adicionado para usar a classe Type

namespace Im_behind_you
{
    // Tornamos o patch explícito, especificando os parâmetros do método alvo
    [HarmonyPatch(typeof(IncidentWorker_TraderCaravanArrival))]
    [HarmonyPatch("CanFireNow")]
    [HarmonyPatch(new Type[] { typeof(IncidentParms) })]
    public static class Patch_Traders
    {
        // Adicionamos o parâmetro 'IncidentParms parms' para corresponder ao método original
        public static bool Prefix(IncidentParms parms, ref bool __result)
        {
            var visibilityTracker = Current.Game.GetComponent<VisibilityTracker>();

            // Exemplo de limite: se a visibilidade for menor que 100, há uma chance de o evento não ocorrer
            if (visibilityTracker != null && visibilityTracker.visibilityScore < 100)
            {
                // Gera um número aleatório. Se for maior que a visibilidade, o evento falha.
                // Ex: Visibilidade 20 -> 80% de chance de falhar. Visibilidade 80 -> 20% de chance de falhar.
                if (Rand.Range(0f, 100f) > visibilityTracker.visibilityScore)
                {
                    // Define o resultado do método original como 'false' e impede que ele rode
                    __result = false;
                    return false;
                }
            }

            // Se a visibilidade for alta ou se o teste aleatório passar, permite que o evento ocorra normalmente
            return true;
        }
    }
}