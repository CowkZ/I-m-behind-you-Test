using RimWorld;
using RimWorld.Planet;
using System.Linq;
using Verse;

namespace I_m_behind_you
{
    public class VisibilityTracker : GameComponent
    {
        public float visibilityScore = 0f;

        // Construtor necessário
        public VisibilityTracker(Game game) { }

        public override void GameComponentTick()
        {
            // A cada 2000 ticks, recalcula a visibilidade
            if (Find.TickManager.TicksGame % 2000 == 0)
            {
                RecalculateVisibility();
            }

            // A CADA 3600 TICKS (APROX. 1 MINUTO REAL), MOSTRA A MENSAGEM NO LOG
            if (Find.TickManager.TicksGame % 3600 == 0)
            {
                // A linha abaixo envia a mensagem para o log do jogo
                Log.Message($"[Mod Visibilidade] Visibilidade Atual: {visibilityScore}");
            }
        }

        private void RecalculateVisibility()
        {
            if (Current.Game.CurrentMap == null) return;

            float wealthFactor = Current.Game.CurrentMap.wealthWatcher.WealthTotal / 1000f;
            float populationFactor = PawnsFinder.AllMaps_FreeColonists.Count() * 10;

            float locationFactor = 0f;
            int playerTile = Current.Game.CurrentMap.Tile;

            // A mesma troca aqui: FactionBase por Settlement
            foreach (var settlement in Find.WorldObjects.AllWorldObjects.OfType<Settlement>())
            {
                if (settlement.Faction == Faction.OfPlayer) continue;

                float distance = Find.WorldGrid.ApproxDistanceInTiles(playerTile, settlement.Tile);

                if (distance > 0)
                {
                    locationFactor += 200f / distance;
                }
            }

            this.visibilityScore = wealthFactor + populationFactor + locationFactor;

            Log.Message($"[I'm behind you] Visibilidade Recalculada: {this.visibilityScore} (Riqueza: {wealthFactor}, Pop: {populationFactor}, Local: {locationFactor})");
        }

        public override void ExposeData()
        {
            base.ExposeData(); // É uma boa prática chamar o método base
            Scribe_Values.Look(ref visibilityScore, "visibilityScore", 0f);
        }
    }
}