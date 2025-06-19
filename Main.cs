using HarmonyLib;
using Verse;

namespace Im_behind_you
{
    // Esta anotação faz o RimWorld executar o código abaixo assim que o mod é carregado
    [StaticConstructorOnStartup]
    public static class Main
    {
        // Este é um construtor estático, ele roda apenas uma vez
        static Main()
        {
            // Cria uma instância do Harmony com um ID único para o seu mod
            var harmony = new Harmony("CowkZ.ImBehindYou");

            // Este é o comando que aplica todos os patches do seu mod
            harmony.PatchAll();
        }
    }
}