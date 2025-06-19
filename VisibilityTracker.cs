using RimWorld;
using Verse;

namespace SeuModDeVisibilidade
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
            // Sua lógica de cálculo de visibilidade vai aqui
            // Exemplo simples:
            if (Current.Game.CurrentMap != null) // Garante que há um mapa ativo
            {
                float wealthFactor = Current.Game.CurrentMap.wealthWatcher.WealthTotal / 1000f;
                float populationFactor = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count() * 10;
                visibilityScore = wealthFactor + populationFactor;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData(); // É uma boa prática chamar o método base
            Scribe_Values.Look(ref visibilityScore, "visibilityScore", 0f);
        }
    }
}