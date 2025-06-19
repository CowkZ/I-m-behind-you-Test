using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Im_behind_you
{
    public static class VisibilityCalculator
    {
        public static float CalculateBaseTileVisibility(int tile)
        {
            float locationFactor = 0f;
            foreach (var factionBase in Find.WorldObjects.FactionBases)
            {
                if (factionBase.Faction == Faction.OfPlayer) continue;
                int distance = Find.WorldGrid.ApproxDistanceInTiles(tile, factionBase.Tile);
                if (distance > 0)
                {
                    locationFactor += 200f / distance;
                }
            }
            return locationFactor;
        }
    }
}