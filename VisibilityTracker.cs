using RimWorld;
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

            // Fatores que você já tinha
            float wealthFactor = Current.Game.CurrentMap.wealthWatcher.WealthTotal / 1000f;
            float populationFactor = PawnsFinder.AllMaps_FreeColonists.Count() * 10;

            // NOVO: Fator de Localização
            float locationFactor = 0f;
            int playerTile = Current.Game.CurrentMap.Tile;

            // Itera por todas as bases de facções no mapa
            foreach (var factionBase in Find.WorldObjects.FactionBases)
            {
                // Ignora sua própria facção
                if (factionBase.Faction == Faction.OfPlayer) continue;

                // Calcula a distância
                int distance = Find.WorldGrid.ApproxDistanceInTiles(playerTile, factionBase.Tile);

                // Adiciona um score: quanto menor a distância, maior o bônus de visibilidade
                if (distance > 0)
                {
                    locationFactor += 200f / distance; // Fórmula de exemplo, ajuste como quiser
                }
            }

            // Soma tudo para o score final
            this.visibilityScore = wealthFactor + populationFactor + locationFactor;

            // Use o log que criamos para ver o resultado do cálculo
            Log.Message($"[I'm behind you] Visibilidade Recalculada: {this.visibilityScore} (Riqueza: {wealthFactor}, Pop: {populationFactor}, Local: {locationFactor})");
        }

        public override void ExposeData()
        {
            base.ExposeData(); // É uma boa prática chamar o método base
            Scribe_Values.Look(ref visibilityScore, "visibilityScore", 0f);
        }
    }
}