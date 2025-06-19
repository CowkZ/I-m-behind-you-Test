using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using System;

namespace Im_behind_you
{
    // Usando a forma mais explícita para garantir que o Harmony encontre o método
    [HarmonyPatch(typeof(Page_SelectStartingSite))]
    [HarmonyPatch("DoWindowContents")]
    [HarmonyPatch(new Type[] { typeof(Rect) })]
    public static class Patch_Page_SelectStartingSite_DoWindowContents
    {
        public static void Postfix(Rect rect)
        {
            // Log para confirmar que o patch está rodando
            Log.Message("[I'm behind you] DEBUG: Patch da tela de mundo foi EXECUTADO.");

            int selectedTile = Find.World.UI.SelectedTile;
            if (selectedTile < 0) return;

            float visibility = VisibilityCalculator.CalculateBaseTileVisibility(selectedTile);
            string visibilityText = $"Visibilidade Inicial: {visibility:F2}";

            Rect testRect = new Rect(15f, 215f, 300f, 30f);

            Text.Font = GameFont.Small;
            GUI.color = Color.white;
            Widgets.Label(testRect, visibilityText);
            GUI.color = Color.white;
        }
    }
}