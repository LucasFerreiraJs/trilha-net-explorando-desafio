using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.HotelRepositories;
using DesafioProjetoHospedagem.ConsoleHelpers;
namespace DesafioProjetoHospedagem.Menus;

public class ReservationMenu
{


    private readonly HotelLogRepository _logRepository;
    private readonly HotelRepository _repository;
    public ReservationMenu( HotelLogRepository logRepository, HotelRepository repository)
    {

        _logRepository = logRepository;
        _repository = repository;
    }

    public void ShowOptions()
    {
        Console.Clear();
        Console.WriteLine("MENU RESERVAS");
        Console.WriteLine("Digite uma opção:");
        Console.WriteLine("");
        Console.WriteLine("1 - Cadastrar reserva");
        Console.WriteLine("2 - Listar reservas abertas");
        Console.WriteLine("3 - Finalizar reserva");
        Console.WriteLine("4 - Cancelar reserva");
        Console.WriteLine("5 - Histórico de reservas fechadas");
        Console.WriteLine("6 - Histórico de reservas canceladas");
        Console.WriteLine("7 - Voltar");
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
                    var suites = _repository.GetAllSuitesAvailable();

                    if (!suites.Any())
                    {
                        Console.Clear();
                        Console.WriteLine($"Não há suites disponíveis");
                        ConsolePause.Wait();
                        return;
                    }

                    Console.Clear();
                    Console.WriteLine($"Selecione uma suite disponível: ");
                    Console.WriteLine();


                    foreach (var value in suites)
                    {
                        Console.WriteLine($"Suite id: {value.Id?.ToString() ?? "(Sem id)"}");
                        Console.WriteLine($"Tipo: {value.SuiteType}");
                        Console.WriteLine($"Capacidade: {value.Capacity}");
                        Console.WriteLine($"Valor: {value.FormatedDailyValue()}");
                        Console.WriteLine("--------------------------");
                    }

                    Console.WriteLine();
                    int suiteId = 0;
                    bool checkSuite = false;
                    while (!checkSuite)
                    {

                        Console.WriteLine($"Digite o id de uma suite disponível: ");
                        bool isNumber = int.TryParse(Console.ReadLine(), out var id);
                        checkSuite = isNumber && suites.Any(suite => suite.Id == id);

                        if (checkSuite)
                        {
                            suiteId = id;
                        }
                        // Console.WriteLine("Suite inválida");

                    }
                    if (suiteId <= 0)
                    {
                        Console.WriteLine($"Erro ao selecionar suite ");
                        ConsolePause.Wait();
                        break;
                    }

                    // var suiteSelected = suites.Where(suite => suite.Id == suiteId)
                    // .ToList()[0];
                    var suiteSelected = suites.First(suite => suite.Id == suiteId);

                    var selectedIds = new HashSet<int>();
                    string search = "";
                    int focusedIndex = 0;
                    bool selectionCancelled = false;
                    var guests = _repository.GetAllGuestAvailable();

                    if (!guests.Any())
                    {
                        Console.WriteLine("não há hospesdes cadastrados ou sem reservas");
                        ConsolePause.Wait();
                        return;
                    }

                    bool checkForm = false;
                    while (!checkForm)
                    {

                        Console.Clear();
                        Console.WriteLine($"Suite {suiteSelected.SuiteType}, permite {suiteSelected.Capacity} hospedes");
                        Console.WriteLine();


                        Console.WriteLine($"Quantidade hospedes selecionados: {selectedIds.Count()}");
                        Console.WriteLine($"ids hospedes selecionados: {string.Join("-", selectedIds)}");

                        var visibleGuests = guests.Where(item => item.FullName.Contains(
                                search,
                                StringComparison.OrdinalIgnoreCase
                            )
                        ).ToList();

                        Console.WriteLine();
                        Console.WriteLine("lista de hospedes: ");
                        if (visibleGuests.Count() > 0)
                        {
                            for (int index = 0; index < visibleGuests.Count(); index++)
                            {

                                Guest guest = visibleGuests[index];
                                bool focused = index == focusedIndex;
                                bool selected = selectedIds.Contains(guest.Id!.Value);

                                string pointer = focused ? ">" : " ";
                                string check = selected ? "[X]" : "[ ]";

                                Console.WriteLine($"{pointer} {check} {guest.FullName}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nenhum hospede encontrado");
                        }

                        Console.WriteLine();
                        Console.WriteLine("↑/↓ Navegar");
                        Console.WriteLine("1 Marcar/desmarcar");
                        Console.WriteLine("Enter finalizar seleção");

                        Console.WriteLine();
                        Console.WriteLine($"Pesquisa: {search}");
                        Console.WriteLine();

                        ConsoleKeyInfo key = Console.ReadKey(intercept: true);

                        if (key.Key == ConsoleKey.UpArrow && focusedIndex > 0)
                        {

                            focusedIndex--;
                        }
                        else if (
                            key.Key == ConsoleKey.DownArrow
                            && focusedIndex < visibleGuests.Count() - 1
                        )
                        {
                            focusedIndex++;
                        }

                        if (
                            (key.Key == ConsoleKey.D1
                            || key.Key == ConsoleKey.NumPad1) && visibleGuests.Count() > 0
                        )
                        {
                            Guest focusedGuest = visibleGuests[focusedIndex];

                            if (focusedGuest.Id.HasValue)
                            {
                                int guestId = focusedGuest.Id.Value;

                                if (!selectedIds.Add(guestId))
                                {
                                    selectedIds.Remove(guestId);
                                }
                            }

                        }
                        if (key.Key == ConsoleKey.Backspace && search.Length > 0)
                        {
                            search = search[..^1];
                            focusedIndex = 0;

                        }
                        else if (!char.IsControl(key.KeyChar) && !char.IsDigit(key.KeyChar))
                        {
                            search += key.KeyChar;
                            focusedIndex = 0;
                        }

                        if (key.Key == ConsoleKey.Enter)
                        {
                            checkForm = true;
                            break;
                        }
                    }

                    int reservedDays = 0;
                    bool checkReservedDaysValue = false;

                    while (!checkReservedDaysValue)
                    {
                        Console.WriteLine("Digite quantidade de dias: ");
                        checkReservedDaysValue = int.TryParse(Console.ReadLine(), out var qtdDays);

                        if (checkReservedDaysValue && qtdDays > 0)
                        {
                            reservedDays = qtdDays;
                            break;
                        }

                        Console.WriteLine("Digite uma quantidade maior que zero.");
                    }

                    var selectedGuests = guests.Where(item => selectedIds.Contains(item.Id.Value)).ToList();

                    var reservation = new Reservation(
                        // suite: suiteSelected,
                        // guests: selectedGuests,
                        reservedDays: reservedDays);

                    try
                    {

                        reservation.RegisterSuite(suite: suiteSelected);
                        reservation.RegisterGuest(guests: selectedGuests);

                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                        ConsolePause.Wait();
                        break;
                    }


                    var registerReservation = _repository.RegisterReservation(reservation: reservation);


                    if (registerReservation <= 0)
                    {

                        Console.WriteLine("Erro ao salvar reserva");
                        ConsolePause.Wait();
                        break;

                    }

                    Console.WriteLine("Reserva cadastrada com sucesso");
                    ConsolePause.Wait();

                    break;
                }
            case "2":
                {
                    ShowReservationList(
                        _repository.GetOpenReservations(),
                        "RESERVAS ABERTAS",
                        "Não há reservas abertas."
                    );
                    break;
                }
            case "3":
                {
                    Console.Clear();
                    var checkId = false;
                    int reservationId = 0;

                    if (!checkId)
                    {
                        Console.WriteLine("Digite o id da reserva:");
                        checkId = int.TryParse(Console.ReadLine(), out int id);
                        if (checkId)
                        {
                            reservationId = id;
                        }
                    }

                    if (reservationId <= 0)
                    {
                        Console.WriteLine("Valor da reserva inválido");
                        ConsolePause.Wait();
                        break;
                    }

                    var reservation = _repository.GetReservationById(id: reservationId);
                    if (reservation == null)
                    {

                        Console.WriteLine("Reserva não encontrada");
                        ConsolePause.Wait();
                        break;
                    }


                    var checkCloseReservation = _repository.CloseReservation(id: reservationId);

                    if (!checkCloseReservation)
                    {
                        Console.WriteLine("Reserva não encontrada ou não está aberta.");
                        ConsolePause.Wait();
                        break;
                    }

                    Console.WriteLine("Reserva finalizada com sucesso.");
                    Console.WriteLine($"Reserva id: {reservation.Id.ToString() ?? "(Sem id)"}");
                    Console.WriteLine($"Tipo suite: {reservation.Suite.SuiteType}");
                    Console.WriteLine($"Capacidade: {reservation.Suite.Capacity}");
                    Console.WriteLine($"Quantidade hospedes: {reservation.Guests.Count}");
                    Console.WriteLine($"Dias reservados: {reservation.ReservedDays}");
                    Console.WriteLine($"Total: R${reservation.FormatedDailyValue()}");
                    Console.WriteLine("--------------------------");



                    ConsolePause.Wait();
                    break;
                }
            case "4":
                {
                    Console.Clear();
                    bool checkReservationId = false;
                    int reservationId = 0;


                    while (!checkReservationId)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Digite o ID da reserva: ");
                        checkReservationId = int.TryParse(Console.ReadLine(), out int id);

                        if (checkReservationId)
                        {

                            reservationId = id;

                        }
                    }

                    if (reservationId == 0)
                    {
                        System.Console.WriteLine("Id da reserva inválido");
                        ConsolePause.Wait();
                        break;
                    }

                    var reservation = _repository.GetReservationById(id: reservationId);
                    if (reservation == null)
                    {

                        Console.WriteLine("Reserva não encontrada");
                        ConsolePause.Wait();
                        break;
                    }

                    var reservationCanceled = _repository.CancelReservation(id: reservationId);
                    if (!reservationCanceled)
                    {
                        Console.WriteLine("Reserva não encontrada ou não está aberta.");
                        ConsolePause.Wait();
                        break;
                    }

                    Console.WriteLine("Reserva cancelada com sucesso");
                    ConsolePause.Wait();
                    break;
                }

            case "5":
                {
                    ShowReservationList(
                        _repository.GetClosedReservations(),
                        "HISTÓRICO DE RESERVAS FECHADAS",
                        "Não há reservas fechadas."
                    );
                    break;
                }
            case "6":
                {
                    ShowReservationList(
                        _repository.GetCanceledReservations(),
                        "HISTÓRICO DE RESERVAS CANCELADAS",
                        "Não há reservas canceladas."
                    );
                    break;
                }
            case "7":
                break;
            default:
                Console.WriteLine("Opção inválida.");
                ConsolePause.Wait();
                break;
        }

    }

    private void ShowReservationList(
        List<Reservation> reservations,
        string title,
        string emptyMessage)
    {
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();

        if (!reservations.Any())
        {
            Console.WriteLine(emptyMessage);
            ConsolePause.Wait();
            return;
        }

        foreach (var item in reservations)
        {
            Console.WriteLine($"Reserva id: {item.Id}");
            Console.WriteLine($"Status: {item.Status}");
            Console.WriteLine($"Data abertura: {item.OpeningDate:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Data fechamento: {item.ClosingDate:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Tipo suite: {item.Suite.SuiteType}");
            Console.WriteLine($"Capacidade: {item.Suite.Capacity}");
            Console.WriteLine($"Quantidade hospedes: {item.Guests?.Count}");
            Console.WriteLine($"Dias reservados: {item.ReservedDays}");
            Console.WriteLine($"Valor diária: {item.FormatedDailyValue()}");
            Console.WriteLine("--------------------------");
        }

        ConsolePause.Wait();
    }
}
