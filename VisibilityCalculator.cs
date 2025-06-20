using RimWorld;
using RimWorld.Planet;
using System.Linq;
using Verse;

namespace Im_behind_you
{
    public static class VisibilityCalculator
    {
        public static float CalculateBaseTileVisibility(int tile)
        {
            // Valor inicial base
            float baseVisibility = 10f;

            // Encontra assentamentos em um raio de 30 células
            var nearbySettlements = Find.WorldObjects.AllWorldObjects.OfType<Settlement>()
                .Where(s => s.Faction != Faction.OfPlayer && Find.WorldGrid.ApproxDistanceInTiles(tile, s.Tile) < 30);

            // Adiciona +10 para cada um
            baseVisibility += nearbySettlements.Count() * 10f;

            return baseVisibility;
        }

        // ... seu outro método GetCurrentColonyVisibility() continua aqui ...
        public static float GetCurrentColonyVisibility()
        {
            return Current.Game.GetComponent<VisibilityTracker>()?.visibilityScore ?? 0f;
        }
    }
}