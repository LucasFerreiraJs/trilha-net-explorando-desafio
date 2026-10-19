using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.HotelRepositories;
using DesafioProjetoHospedagem.ConsoleHelpers;
namespace DesafioProjetoHospedagem.Menus;

public class GuestMenu
{
    private readonly HotelLogRepository _logRepository;
    private readonly HotelRepository _repository;
    public GuestMenu(HotelLogRepository logRepository, HotelRepository repository)
    {
        _logRepository = logRepository;
        _repository = repository;
    }

    public void ShowOptions()
    {

        Console.Clear();
        Console.WriteLine("Digite uma opção:");
        Console.WriteLine("MENU HOSPEDES");
        Console.WriteLine("");
        Console.WriteLine("1 - Cadastrar hospedes");
        Console.WriteLine("2 - Listar hospedes");
        Console.WriteLine("3 - Listar hospedes sem reserva");
        Console.WriteLine("4 - Desativar hospede");
        Console.WriteLine("5 - Voltar");
        Console.WriteLine("");


    }

    public void Run()
    {
        ShowOptions();

        switch (Console.ReadLine())
        {
            case "1":
                {
                    Console.Clear();
                    Console.WriteLine("Nome:");
                    string name = Console.ReadLine();

                    Console.WriteLine("Sobrenome:");
                    string lastName = Console.ReadLine();

                    var guest = new Guest(name: name, lastName: lastName);
                    _repository.RegisterGuest(guest);

                    ConsolePause.Wait();
                    break;
                }

            case "2":
                {
                    var guests = _repository.GetAllGuest();
                    Console.Clear();
                    foreach (var item in guests)
                    {
                        Console.WriteLine($"{item.Id} - {item.FullName}");
                    }
                    ConsolePause.Wait();
                    break;
                }

            case "3":
                {
                    var guests = _repository.GetAllGuestAvailable();
                    Console.Clear();
                    foreach (var item in guests)
                    {
                        Console.WriteLine($"{item.Id} - {item.FullName}");
                    }

                    ConsolePause.Wait();
                    break;
                }

            case "4":
                {
                    Console.Clear();

                    var checkId = false;
                    int guestId = 0;
                    while (!checkId)
                    {
                        Console.WriteLine("Digite o id do hospede: ");
                        checkId = int.TryParse(Console.ReadLine(), out var id);
                        guestId = id;

                    }

                    if (guestId <= 0)
                    {
                        Console.WriteLine("Valor inválido");
                        ConsolePause.Wait();
                        break;
                    }

                    var getGuest = _repository.GetGuestById(id: guestId);
                    if (getGuest == null){
                        Console.WriteLine("Usuário não encontrado");
                        ConsolePause.Wait();
                        break;
                    }

                    var deactivated = _repository.DeactivateGuest(id: guestId);
                    if (!deactivated)
                    {
                        Console.WriteLine("Erro ao desativar usuário");
                        ConsolePause.Wait();
                        break;
                    }

                    Console.WriteLine("Hóspede desativado com sucesso.");
                    ConsolePause.Wait();
                    break;
                }

            default:
                ConsolePause.Wait();
                break;

        }

    }
}
