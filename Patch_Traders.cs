using HarmonyLib;
using RimWorld;
using Verse;
using System;

namespace Im_behind_you
{
    // O patch agora mira na classe MÃE, onde o método é definido
    [HarmonyPatch(typeof(IncidentWorker), "CanFireNow")]
    public static class Patch_Traders
    {
        // Adicionamos '__instance' para pegar o objeto que está executando o método
        public static bool Prefix(IncidentWorker __instance, IncidentParms parms, ref bool __result)
        {
            // Verificamos se a instância atual é do tipo que nos interessa
            if (__instance is IncidentWorker_TraderCaravanArrival)
            {
                // Se for, executamos nossa lógica de visibilidade
                var visibilityTracker = Current.Game.GetComponent<VisibilityTracker>();

                if (visibilityTracker != null && visibilityTracker.visibilityScore < 100)
                {
                    if (Rand.Range(0f, 100f) > visibilityTracker.visibilityScore)
                    {
                        __result = false;
                        return false;
                    }
                }
            }

            // Se não for uma caravana de mercadores, ou se passou no teste,
            // o método original continua normalmente.
            return true;
        }
    }
}