using RimWorld;
using Verse;

namespace Im_behind_you
{
    public class HistoryAutoRecorderWorker_Visibility : HistoryAutoRecorderWorker
    {
        public override float PullRecord()
        {
            // Voltamos a pegar o valor real do nosso GameComponent
            return Current.Game.GetComponent<VisibilityTracker>()?.visibilityScore ?? 0f;
        }
    }
}