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

    public void SuitesMenu(){

        Console.Clear();
        Console.WriteLine("Digite uma opção:");
        Console.WriteLine("");
        Console.WriteLine("1 - Cadastrar suite");
        Console.WriteLine("2 - Listar suites");
        Console.WriteLine("3 - Listar suites sem reserva");
        Console.WriteLine("4 - Deletar suite cadastrada");
        Console.WriteLine("5 - Voltar");
        Console.WriteLine("");

        switch (Console.ReadLine()){
            case "1":
                Console.WriteLine("Digite tipo da suite:");
                string suiteTypeValue = Console.ReadLine();

                if (String.IsNullOrEmpty(suiteTypeValue.Trim())){

                    System.Console.WriteLine("Valor inválido para tipo");
                    return;
                }

                Console.WriteLine("");
                Console.WriteLine("Digite a capacidade:");
                bool checkCapacityValue = int.TryParse(Console.ReadLine(), out int capacityValue);

                if (!checkCapacityValue)
                {
                    System.Console.WriteLine("Valor inválido para capacidade");
                    return;
                }

                if (capacityValue <= 0)
                {

                    System.Console.WriteLine("Capacidade obrigatório");
                    return;
                }

                Console.WriteLine("");
                Console.WriteLine("Digite valor da diária no formato 00.00:");
                bool checkDailyValue = decimal.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal dailyValue) || decimal.TryParse(Console.ReadLine(), NumberStyles.Number, new CultureInfo("pt-BR"), out dailyValue);

                // bool checkDailyValue = int.TryParse(Console.ReadLine(), out int dailyValue);

                if (!checkDailyValue)
                {
                    Console.WriteLine("Valor inválido para diária");
                    return;
                }

                if (dailyValue <= 0)
                {
                    System.Console.WriteLine("Valor da diária obrigatório");
                    return;
                }


                var suite = new Suite(suiteType: suiteTypeValue, capacity: capacityValue, dailyValue: (int)(dailyValue * 100m));

                var insert =  _repository.RegisterSuite(suite: suite);
                if (insert) {

                    Console.Clear();
                    Console.WriteLine("Cadastro da suite realizado com sucesso!");

                } else {
                    Console.Clear();
                    Console.WriteLine("Erro ao realizar cadastro da suite");
                }

                Console.WriteLine();
                Console.WriteLine("Aperte qualquer tecla para voltar");
                Console.ReadLine();
            break;



            case "2":
                 Console.Clear();
                 var suites = _repository.GetAllSuites();

                if (suites == null || suites.Count == 0)
                {
                    Console.WriteLine("Não há suites cadastradas");
                    return;
                }
                foreach ( var  value in suites)
                {
                    Console.WriteLine($"Suite id: {value.Id?.ToString() ??  "(Sem id)"}" );
                    Console.WriteLine($"Tipo: {value.SuiteType}");
                    Console.WriteLine($"Capacidade: {value.Capacity}");
                    Console.WriteLine($"Valor: {value.FormatedDailyValue()}");
                    Console.WriteLine("--------------------------");
                }

                Console.WriteLine();
                Console.WriteLine("Aperte qualquer tecla para voltar");
                Console.ReadLine();
                break;

            case "3":
                break;

            case "4":
                Console.Clear();
                Console.WriteLine("Digite o id da suite que quer remover:");
                bool checkIdValue = int.TryParse(Console.ReadLine(), out var id);

                if (!checkIdValue) {
                    Console.Clear();
                    Console.WriteLine("Formato do id inválido");
                    Console.WriteLine("Aperte qualquer tecla para voltar");
                    Console.ReadLine();
                    return;
                }
                var remove = _repository.RemoveSuite(id:id);
                if (!remove)
                {
                    Console.Clear();
                    Console.WriteLine("Erro aom deletar suite");
                    Console.WriteLine("Aperte qualquer tecla para voltar");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Aperte qualquer tecla para voltar");
                Console.ReadLine();
                break;

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
                break;
            case "2":
                break;
            case "3":
                break;
            case "4":
                break;
            case "5":
                break;

            default:
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
                break;
            case "2":
                break;
            case "3":
                break;
            case "4":
                break;
            case "5":
                break;

            default:
                break;
        }
    }
}
