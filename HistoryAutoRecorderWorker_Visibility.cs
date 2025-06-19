using RimWorld;
using Verse;

namespace Im_behind_you
{
    public class HistoryAutoRecorderWorker_Visibility : HistoryAutoRecorderWorker
    {
        public override float PullRecord()
        {
            // VAMOS IGNORAR O CÁLCULO REAL E RETORNAR UM NÚMERO GIGANTE APENAS PARA O TESTE
            return 50000f; // Retorna um valor fixo de 50,000
        }
    }
}