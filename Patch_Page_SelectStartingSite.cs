using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Im_behind_you
{
    [HarmonyPatch(typeof(Page_SelectStartingSite), "DoWindowContents")]
    public static class Patch_Page_SelectStartingSite_DoWindowContents
    {
        public static void Postfix(Rect rect)
        {
            // Pega a célula do mapa que está selecionada
            // No método Postfix
            // int selectedTile = Find.WorldInterface.selectedTile; // LINHA ANTIGA
            int selectedTile = Find.World.UI.SelectedTile; // NOVA LINHA CORRETA
            if (selectedTile < 0) return;

            // Calcula a visibilidade base para aquela célula
            float visibility = VisibilityCalculator.CalculateBaseTileVisibility(selectedTile);

            // Desenha a informação na tela
            // Este trecho pode precisar de ajustes finos de posição
            var listing = new Listing_Standard();
            var textRect = new Rect(rect.width - 280f, rect.height - 60f, 260f, 30f);
            listing.Begin(textRect);
            listing.Label($"Visibilidade Inicial: {visibility:F2}");
            listing.End();
        }
    }
}