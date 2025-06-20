using RimWorld;
using Verse;

namespace Im_behind_you
{
    public class HistoryAutoRecorderWorker_Visibility : HistoryAutoRecorderWorker
    {
        public override float PullRecord()
        {
            float valueToRecord = Current.Game.GetComponent<VisibilityTracker>()?.visibilityScore ?? 0f; // Valor de erro
            Log.Message($"[I'm behind you] GRÁFICO PUXOU VALOR: O valor que o gráfico está recebendo é {valueToRecord}");
            return valueToRecord;
        }
    }
}