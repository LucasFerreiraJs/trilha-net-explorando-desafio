using System.Collections.Generic;
using System.Globalization;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.HotelRepositories;

namespace DesafioProjetoHospedagem.HotelServices;

public class HotelService
{
    private HotelRepository _repository;

    public HotelService(HotelRepository repository) {
        _repository = repository;
    }

    public void PressAnyButtonMessage()
    {
        Console.WriteLine();
        Console.WriteLine("Aperte qualquer tecla para voltar");
        Console.ReadLine();
    }

    public void SuitesMenu()
    {

        Console.Clear();
        Console.WriteLine("Digite uma opção:");
        Console.WriteLine("");
        Console.WriteLine("1 - Cadastrar suite");
        Console.WriteLine("2 - Listar suites");
        Console.WriteLine("3 - Listar suites sem reserva");
        Console.WriteLine("4 - Deletar suite cadastrada");
        Console.WriteLine("5 - Voltar");
        Console.WriteLine("");

        switch (Console.ReadLine())
        {
            case "1":
                {
                    Console.WriteLine("Digite tipo da suite:");
                    string suiteTypeValue = Console.ReadLine();

                    if (String.IsNullOrEmpty(suiteTypeValue.Trim()))
                    {

                        System.Console.WriteLine("Valor inválido para tipo");
                        PressAnyButtonMessage();
                        return;

                    }

                    Console.WriteLine();
                    Console.WriteLine("Digite a capacidade:");
                    bool checkCapacityValue = int.TryParse(Console.ReadLine(), out int capacityValue);

                    if (!checkCapacityValue)
                    {
                        System.Console.WriteLine("Valor inválido para capacidade");
                        PressAnyButtonMessage();
                        return;

                    }

                    if (capacityValue <= 0)
                    {

                        System.Console.WriteLine("Capacidade obrigatório");
                        PressAnyButtonMessage();
                        return;

                    }

                    Console.WriteLine("");
                    Console.WriteLine("Digite valor da diária no formato 00.00:");
                    bool checkDailyValue = decimal.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal dailyValue) || decimal.TryParse(Console.ReadLine(), NumberStyles.Number, new CultureInfo("pt-BR"), out dailyValue);

                    // bool checkDailyValue = int.TryParse(Console.ReadLine(), out int dailyValue);

                    if (!checkDailyValue)
                    {
                        Console.WriteLine("Valor inválido para diária");
                        PressAnyButtonMessage();
                        return;
                    }

                    if (dailyValue <= 0)
                    {
                        System.Console.WriteLine("Valor da diária obrigatório");
                        PressAnyButtonMessage();
                        return;
                    }


                    var suite = new Suite(suiteType: suiteTypeValue, capacity: capacityValue, dailyValue: (int)(dailyValue * 100m));

                    var insert = _repository.RegisterSuite(suite: suite);
                    if (!insert)
                    {
                        Console.Clear();
                        Console.WriteLine("Erro ao realizar cadastro da suite");
                        PressAnyButtonMessage();
                        return;
                    }

                    Console.Clear();
                    Console.WriteLine("Cadastro da suite realizado com sucesso!");
                    PressAnyButtonMessage();
                    break;
                }



            case "2":
                {
                    Console.Clear();
                    var suites = _repository.GetAllSuites();

                    if (suites == null || suites.Count == 0)
                    {

                        Console.Clear();
                        Console.WriteLine("Não há suites cadastradas");
                        PressAnyButtonMessage();
                        return;
                    }

                    foreach (var value in suites)
                    {
                        Console.WriteLine($"Suite id: {value.Id?.ToString() ?? "(Sem id)"}");
                        Console.WriteLine($"Tipo: {value.SuiteType}");
                        Console.WriteLine($"Capacidade: {value.Capacity}");
                        Console.WriteLine($"Valor: {value.FormatedDailyValue()}");
                        Console.WriteLine("--------------------------");
                    }
                    PressAnyButtonMessage();
                    break;

                }

            case "3":
                {
                    var suites = _repository.GetAllSuitesAvailable();

                    if (suites.Count() == 0){
                        Console.Clear();
                        Console.WriteLine($"Não há suites disponíveis");
                        Console.WriteLine();
                        PressAnyButtonMessage();
                        return;
                    }

                    Console.Clear();

                    foreach (var value in suites)
                    {
                        Console.WriteLine($"Suite id: {value.Id?.ToString() ?? "(Sem id)"}");
                        Console.WriteLine($"Tipo: {value.SuiteType}");
                        Console.WriteLine($"Capacidade: {value.Capacity}");
                        Console.WriteLine($"Valor: {value.FormatedDailyValue()}");
                        Console.WriteLine("--------------------------");
                    }

                    Console.WriteLine();


                    PressAnyButtonMessage();
                    break;
                }

            case "4":
               {
                Console.Clear();
                Console.WriteLine("Digite o id da suite que quer remover:");
                bool checkIdValue = int.TryParse(Console.ReadLine(), out var id);

                if (!checkIdValue)
                {
                    Console.Clear();
                    Console.WriteLine("Formato do id inválido");
                    PressAnyButtonMessage();
                    return;
                }
                var remove = _repository.RemoveSuite(id: id);
                if (!remove)
                {
                    Console.Clear();
                    Console.WriteLine("Erro ao deletar suite");
                    PressAnyButtonMessage();
                    return;
                }

                Console.WriteLine();
                PressAnyButtonMessage();
                break;

               }
            default:
                break;
        }
    }

    public void GuestMenu(){

        Console.Clear();
        Console.WriteLine("Digite uma opção:");
        Console.WriteLine("");
        Console.WriteLine("1 - Cadastrar hospedes");
        Console.WriteLine("2 - Listar hospedes");
        Console.WriteLine("3 - Listar hospedes sem reserva");
        Console.WriteLine("4 - Remover hospede");
        Console.WriteLine("5 - Voltar");
        Console.WriteLine("");


        switch (Console.ReadLine()){
            case "1":
               {
                Console.WriteLine("Nome:");
                string name = Console.ReadLine();

                Console.WriteLine("Sobrenome:");
                string lastName = Console.ReadLine();

                var guest = new Guest(name: name, lastName: lastName);
                _repository.RegisterGuest(guest);

                PressAnyButtonMessage();
                break;
               }

            case "2":
                {
                    var guests = _repository.GetAllGuest();
                    Console.Clear();
                    foreach (var item in guests){
                        Console.WriteLine($"{item.Id} - {item.FullName}");
                    }
                    PressAnyButtonMessage();
                    break;
                }

            case "3":
                PressAnyButtonMessage();
                break;

            case "4":
                PressAnyButtonMessage();
                break;

            case "5":
                PressAnyButtonMessage();
                break;


            default:
                PressAnyButtonMessage();
                break;

        }
    }

    public void ReservationMenu(){

        Console.Clear();
        Console.WriteLine("Digite uma opção:");
        Console.WriteLine("");
        Console.WriteLine("1 - Cadastrar reserva");
        Console.WriteLine("2 - Listar reserva");
        Console.WriteLine("3 - Finalizar reserva");
        Console.WriteLine("4 - Cancelar reserva");
        Console.WriteLine("5 - Voltar");
        Console.WriteLine("");


        switch (Console.ReadLine()){
            case "1":
                {
                    var suites = _repository.GetAllSuitesAvailable();

                    if (suites.Count() == 0){
                        Console.Clear();
                        Console.WriteLine($"Não há suites disponíveis");
                        Console.WriteLine();
                        PressAnyButtonMessage();
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
                    // var suiteIdCheck = int.TryParse(Console.ReadLine(), out var suiteId);
                    int suiteId;

                    while (true){

                        Console.WriteLine($"Digite o id da suite: ");
                        var input = Console.ReadLine();
                        bool isNumber= int.TryParse(input, out  suiteId);

                        bool suiteExists = isNumber && suites.Any( suite => suite.Id == suiteId);
                        if (suiteExists){
                            break;
                        }
                        // suiteId = inputSuiteId;
                        Console.WriteLine("Suite inválida");

                    }

                    var suiteSelected = suites.Where(suite => suite.Id == suiteId)
                    .ToList()[0];

                    var selectedIds = new HashSet<int>();
                    // List<int> selectedIds = new List<int>();
                    string search = "";
                    int focusedIndex = 0;
                    bool selectionCancelled = false;
                    var guests = _repository.GetAllGuestAvailable();

                    if (guests.Count() == 0){
                        Console.WriteLine("não há hospesdes cadastrados ou sem reservas");
                        PressAnyButtonMessage();
                        return;
                    }


                    while (true){

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
                        if (visibleGuests.Count() > 0){
                            for (int index = 0; index < visibleGuests.Count(); index++){

                                Guest guest = visibleGuests[index];
                                bool focused = index == focusedIndex;
                                bool selected = selectedIds.Contains(guest.Id!.Value);

                                string pointer = focused ? ">" : " ";
                                string check = selected ? "[X]" : "[ ]";

                                Console.WriteLine($"{pointer} {check} {guest.FullName}");
                            }
                        }else {
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

                        if (key.Key == ConsoleKey.UpArrow && focusedIndex > 0){

                            focusedIndex--;
                        }else if (
                            key.Key == ConsoleKey.DownArrow
                            && focusedIndex < visibleGuests.Count() - 1
                        ){
                            focusedIndex++;
                        }

                        if (
                            (key.Key == ConsoleKey.D1
                            || key.Key == ConsoleKey.NumPad1) && visibleGuests.Count() > 0
                        ) {
                            Console.WriteLine("selected press");
                            Guest focusedGuest = visibleGuests[focusedIndex];

                            if (focusedGuest.Id.HasValue){
                                int guestId = focusedGuest.Id.Value;

                                if (!selectedIds.Add(guestId)){
                                    selectedIds.Remove(guestId);
                                }
                            }

                        }
                        if (key.Key == ConsoleKey.Backspace && search.Length > 0){
                            search  = search[..^1];
                            focusedIndex =0;

                        }else if (!char.IsControl(key.KeyChar) && !char.IsDigit(key.KeyChar)){
                            search += key.KeyChar;
                            focusedIndex =0;
                        }

                        if (key.Key == ConsoleKey.Enter)
                        {
                            break;
                        }
                    }

                    // !
                    // var reservation = _repository.RegisterReservation();


                    break;
                }
            case "2":
                PressAnyButtonMessage();
                break;
            case "3":
                PressAnyButtonMessage();
                break;
            case "4":
                PressAnyButtonMessage();
                break;
            case "5":
                PressAnyButtonMessage();
                break;

            default:
                PressAnyButtonMessage();
                break;
        }
    }
}
