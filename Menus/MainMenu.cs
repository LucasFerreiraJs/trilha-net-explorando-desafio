using System.Collections.Generic;
using System.Globalization;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.HotelRepositories;
namespace DesafioProjetoHospedagem.Menus;

public class MainMenu
{
    private readonly GuestMenu _guestMenu;
    private readonly ReservationMenu _reservationMenu;
    private readonly SuiteMenu _suiteMenu;
    private readonly HotelLogRepository _logRepository;
    private readonly HotelRepository _repository;
    public MainMenu(
            HotelLogRepository logRepository,
            HotelRepository repository,
            GuestMenu guestMenu,
            ReservationMenu reservationMenu,
            SuiteMenu suiteMenu

            )
    {
        _logRepository = logRepository;
        _repository = repository;
        _guestMenu = guestMenu;
        _reservationMenu = reservationMenu;
        _suiteMenu = suiteMenu;
    }


    public void ShowOptions()
    {
        Console.Clear();
        Console.WriteLine("Digite uma opção:");
        Console.WriteLine("");
        Console.WriteLine("1 - Menu suite");
        Console.WriteLine("2 - Menu hospedes");
        Console.WriteLine("3 - Menu reservas");
        Console.WriteLine("4 - Finalizar reserva");
        Console.WriteLine("");
    }

    public void Run()
    {

        while (true)
        {
            ShowOptions();
            switch (Console.ReadLine())
            {
                case "1":
                    _suiteMenu.Run();
                    break;

                case "2":
                    _guestMenu.Run();
                    break;

                case "3":
                    _reservationMenu.Run();
                    break;

                case "4":
                    return;

                default:
                    Console.WriteLine("Opção inválida");
                    break;
            }

        }
    }

}
