
namespace DesafioProjetoHospedagem.ConsoleHelpers;

public static class ConsolePause
{
    public static void Wait(
        string message = "Pressione qualquer tecla para voltar ao menu principal.")
    {
        Console.WriteLine();
        Console.WriteLine(message);
        Console.ReadKey(intercept: true);
    }
}
