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
        // Guarda a riqueza da última vez que o cálculo foi feito
        private float lastCheckedWealth = -1f;

        public VisibilityTracker(Game game) { }

        public override void GameComponentTick()
        {
            // A atualização periódica a cada 2000 ticks continua
            if (Find.TickManager.TicksGame > 1 && Find.TickManager.TicksGame % 2000 == 0)
            {
                UpdateVisibilityBasedOnWealthChange();
            }
        }

        private void UpdateVisibilityBasedOnWealthChange()
        {
            if (Current.Game.CurrentMap == null) return;

            float currentWealth = Current.Game.CurrentMap.wealthWatcher.WealthTotal;

            // Se for a primeira vez que rodamos o cálculo, definimos os valores iniciais
            if (lastCheckedWealth < 0f)
            {
                InitializeVisibility();
                lastCheckedWealth = currentWealth;
                return; // Pula a primeira atualização para começar no próximo ciclo
            }

            // --- LÓGICA DE MUDANÇA DE RIQUEZA ---

            // 1. Calcula a diferença de riqueza desde a última verificação
            float wealthChange = currentWealth - lastCheckedWealth;

            // 2. Define um "limiar". Só fazemos algo se a mudança for maior que 200.
            float significanceThreshold = 200f;

            // 3. Verifica se a mudança foi significativa
            if (Mathf.Abs(wealthChange) > significanceThreshold)
            {
                float visibilityChange = 0f;
                // Se a riqueza AUMENTOU...
                if (wealthChange > 0)
                {
                    // A visibilidade aumenta 1 ponto para cada 1000 de riqueza nova.
                    visibilityChange = wealthChange / 1000f;
                }
                // Se a riqueza DIMINUIU...
                else
                {
                    // A visibilidade diminui 2 pontos para cada 1000 de riqueza perdida (penalidade maior).
                    visibilityChange = (wealthChange / 500f);
                }

                visibilityScore += visibilityChange;
                Log.Message($"[I'm behind you] Mudança de Riqueza Relevante: {wealthChange:F0}. Mudança de Visibilidade: {visibilityChange:F2}. Nova Visibilidade: {visibilityScore:F2}");
            }

            // 4. No final, atualiza a "última riqueza verificada" para a próxima comparação.
            lastCheckedWealth = currentWealth;
        }

        // Método que calcula o valor inicial apenas uma vez
        private void InitializeVisibility()
        {
            int playerTile = Current.Game.CurrentMap.Tile;
            float startingLocationFactor = 10f;
            var nearbySettlements = Find.WorldObjects.AllWorldObjects.OfType<Settlement>()
                .Where(s => s.Faction != Faction.OfPlayer && Find.WorldGrid.ApproxDistanceInTiles(playerTile, s.Tile) < 30);
            startingLocationFactor += nearbySettlements.Count() * 10f;

            visibilityScore = startingLocationFactor;
            Log.Message($"[I'm behind you] Visibilidade Inicial Definida: {visibilityScore}");
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref visibilityScore, "visibilityScore", 0f);
            Scribe_Values.Look(ref lastCheckedWealth, "lastCheckedWealth", -1f);
        }
    }
}