using RimWorld;
using Verse;

namespace Im_behind_you
{
    public class HistoryAutoRecorderWorker_Visibility : HistoryAutoRecorderWorker
    {
        public override float PullRecord()
        {
            Log.Message("[I'm behind you] DEBUG: Sistema do gráfico pediu o valor da visibilidade.");
            return Current.Game.GetComponent<VisibilityTracker>()?.visibilityScore ?? 0f;
        }
    }
}