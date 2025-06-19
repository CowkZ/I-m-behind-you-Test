using I_m_behind_you;
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
            float locationFactor = 0f;

            foreach (var settlement in Find.WorldObjects.AllWorldObjects.OfType<Settlement>())
            {
                if (settlement.Faction == Faction.OfPlayer) continue;

                float distance = Find.WorldGrid.ApproxDistanceInTiles(tile, settlement.Tile);
                if (distance > 0)
                {
                    locationFactor += 200f / distance;
                }
            }
            return locationFactor;
        }

        public static float GetCurrentColonyVisibility()
        {
            return Current.Game.GetComponent<VisibilityTracker>()?.visibilityScore ?? 0f;
        }
    }
}