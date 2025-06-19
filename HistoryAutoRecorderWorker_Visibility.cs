using RimWorld;
using I_m_behind_you;
using Verse;

namespace Im_behind_you
{
    public class HistoryRecorder_Visibility : HistoryRecorder
    {
        // Esta propriedade é chamada pelo jogo para obter o valor a ser plotado no gráfico
        public override float CurrentValue => Current.Game.GetComponent<VisibilityTracker>()?.visibilityScore ?? 0f;
    }
}