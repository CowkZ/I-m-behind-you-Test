using RimWorld;
using RimWorld.Planet;
using System.Linq;
using Verse;

namespace Im_behind_you
{
    public class VisibilityTracker : GameComponent
    {
        public float visibilityScore = 0f;

        // Adicionamos um log no construtor para saber se ele foi criado
        public VisibilityTracker(Game game)
        {
            Log.Message("[I'm behind you] DEBUG: VisibilityTracker foi CONSTRUÍDO!");
        }

        public override void GameComponentTick()
        {
            if (Find.TickManager.TicksGame % 2000 == 0)
            {
                RecalculateVisibility();
            }
        }

        private void RecalculateVisibility()
        {
            Log.Message($"[I'm behind you] DEBUG: Recalculando visibilidade...");

            if (Current.Game.CurrentMap == null) return;

            // --- NOVA FÓRMULA ---

            // 1. Riqueza: Cada 1000 de riqueza vale 10 pontos de visibilidade.
            float wealthFactor = Current.Game.CurrentMap.wealthWatcher.WealthTotal / 100f;

            // 2. População: Cada colono vale 50 pontos.
            float populationFactor = PawnsFinder.AllMaps_FreeColonists.Count * 50f;

            // 3. Localização (como antes)
            float locationFactor = 0f;
            int playerTile = Current.Game.CurrentMap.Tile;
            foreach (var settlement in Find.WorldObjects.AllWorldObjects.OfType<Settlement>())
            {
                if (settlement.Faction == Faction.OfPlayer) continue;
                float distance = Find.WorldGrid.ApproxDistanceInTiles(playerTile, settlement.Tile);
                if (distance > 0)
                {
                    locationFactor += 200f / distance;
                }
            }

            // 4. Valor Base: Garantimos que a visibilidade comece com um valor mínimo.
            float baseVisibility = 50f;

            // Soma tudo
            this.visibilityScore = baseVisibility + wealthFactor + populationFactor + locationFactor;

            Log.Message($"[I'm behind you] DEBUG: Visibilidade agora é {this.visibilityScore} (Base: {baseVisibility}, Riqueza: {wealthFactor}, Pop: {populationFactor}, Local: {locationFactor})");
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref visibilityScore, "visibilityScore", 0f);
        }
    }
}