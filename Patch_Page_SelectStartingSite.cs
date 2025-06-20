using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using System;

namespace Im_behind_you
{
    [HarmonyPatch(typeof(Page_SelectStartingSite), "DoWindowContents")]
    public static class Patch_Page_SelectStartingSite_DoWindowContents
    {
        public static void Postfix(Rect rect)
        {
            int selectedTile = Find.World.UI.SelectedTile;
            if (selectedTile < 0) return;

            float visibility = VisibilityCalculator.CalculateBaseTileVisibility(selectedTile);
            string visibilityText = $"Visibilidade Inicial: {visibility:F0}";

            float yPosition = 238f;

            Rect textRect = new Rect(15f, yPosition, 200f, 30f);

            Text.Font = GameFont.Small;
            GUI.color = Color.white;
            TooltipHandler.TipRegion(textRect, "A visibilidade inicial da sua colônia. Afeta a frequência de eventos bons e ruins. Influenciada pela proximidade de outras facções.");

            Widgets.Label(textRect, visibilityText);
            GUI.color = Color.white;
        }
    }
}