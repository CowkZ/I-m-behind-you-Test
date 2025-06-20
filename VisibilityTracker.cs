using RimWorld;
using RimWorld.Planet;
using System.Linq;
using UnityEngine;
using Verse;

namespace Im_behind_you
{
    public class VisibilityTracker : GameComponent
    {
        public float visibilityScore = 0f;
        private float lastCheckedWealth = -1f;
        private float startingLocationFactor = -1f;

        public VisibilityTracker(Game game) { }

        public override void GameComponentTick()
        {
            if (Find.TickManager.TicksGame > 1 && Find.TickManager.TicksGame % 2000 == 0)
            {
                // O nome do método está correto agora
                RecalculateVisibility();
            }
        }

        // A nova lógica de balanceamento está aqui dentro
        private void RecalculateVisibility()
        {
            if (Current.Game.CurrentMap == null) return;

            float currentWealth = Current.Game.CurrentMap.wealthWatcher.WealthTotal;

            if (lastCheckedWealth < 0f)
            {
                InitializeVisibility();
                lastCheckedWealth = currentWealth;
                return;
            }

            // --- LÓGICA DE BALANCEAMENTO CORRIGIDA ---

            float wealthChange = currentWealth - lastCheckedWealth;
            float wealthChangeThreshold = 200f;
            float increaseFactor = 1000f;
            float decreaseFactor = 500;

            if (wealthChange > wealthChangeThreshold)
            {
                float visibilityIncrease = wealthChange / increaseFactor;
                visibilityScore += visibilityIncrease;
            }
            else if (wealthChange < -wealthChangeThreshold)
            {
                float visibilityDecrease = wealthChange / decreaseFactor;
                visibilityScore += visibilityDecrease;
                if (visibilityScore < 10) visibilityScore = 10;
            }

            lastCheckedWealth = currentWealth;
        }

        private void InitializeVisibility()
        {
            int playerTile = Current.Game.CurrentMap.Tile;
            startingLocationFactor = 10f;
            var nearbySettlements = Find.WorldObjects.AllWorldObjects.OfType<Settlement>()
                .Where(s => s.Faction != Faction.OfPlayer && Find.WorldGrid.ApproxDistanceInTiles(playerTile, s.Tile) < 30);
            startingLocationFactor += nearbySettlements.Count() * 10f;

            visibilityScore = startingLocationFactor;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref visibilityScore, "visibilityScore", 0f);
            Scribe_Values.Look(ref lastCheckedWealth, "lastCheckedWealth", -1f);
            Scribe_Values.Look(ref startingLocationFactor, "startingLocationFactor", -1f);
        }
    }
}