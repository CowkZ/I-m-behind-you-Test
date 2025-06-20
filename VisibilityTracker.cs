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
                RecalculateVisibility();
            }
        }

        private void RecalculateVisibility()
        {
            if (Current.Game.CurrentMap == null) return;

            if (startingLocationFactor < 0f)
            {
                CalculateStartingLocationFactor();
            }

            // --- NOVA FÓRMULA HÍBRIDA ---

            // 1. Pega os pontos de ameaça vanilla como base
            float vanillaThreatPoints = StorytellerUtility.DefaultThreatPointsNow(Current.Game.CurrentMap);

            // 2. Calcula o "Momentum" da riqueza (bônus/penalidade por mudança recente)
            float wealthMomentumFactor = 0f;
            float currentWealth = Current.Game.CurrentMap.wealthWatcher.WealthTotal;
            if (lastCheckedWealth > 0)
            {
                float wealthChange = currentWealth - lastCheckedWealth;
                // A mudança de riqueza agora tem um impacto muito menor, servindo como um ajuste fino.
                wealthMomentumFactor = wealthChange / 2000f;
            }

            // 3. O score final é a Base + Localização + Momentum
            this.visibilityScore = vanillaThreatPoints + startingLocationFactor + wealthMomentumFactor;

            // Garante que a visibilidade não seja negativa
            if (this.visibilityScore < 0) this.visibilityScore = 0;

            // Atualiza a última riqueza checada para o próximo ciclo
            lastCheckedWealth = currentWealth;
        }

        private void CalculateStartingLocationFactor()
        {
            int playerTile = Current.Game.CurrentMap.Tile;
            startingLocationFactor = 10f;
            var nearbySettlements = Find.WorldObjects.AllWorldObjects.OfType<Settlement>()
                .Where(s => s.Faction != Faction.OfPlayer && Find.WorldGrid.ApproxDistanceInTiles(playerTile, s.Tile) < 30);
            startingLocationFactor += nearbySettlements.Count() * 10f;
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