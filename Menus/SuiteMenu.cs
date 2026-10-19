using DesafioProjetoHospedagem.HotelRepositories;
using DesafioProjetoHospedagem.ConsoleHelpers;
using DesafioProjetoHospedagem.Models;
using System.Collections.Generic;
using System.Globalization;
namespace DesafioProjetoHospedagem.Menus;

public class SuiteMenu
{


    private readonly HotelLogRepository _logRepository;
    private readonly HotelRepository _repository;
    public SuiteMenu( HotelLogRepository logRepository, HotelRepository repository)
    {
        _logRepository = logRepository;
        _repository = repository;
    }
    public void ShowOptions()
    {
        Console.Clear();
        Console.WriteLine("MENU SUITE");
        Console.WriteLine("Digite uma opção:");
        Console.WriteLine("");
        Console.WriteLine("1 - Cadastrar suite");
        Console.WriteLine("2 - Listar suites");
        Console.WriteLine("3 - Listar suites sem reserva");
        Console.WriteLine("4 - Desativar suite cadastrada");
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
                    Console.WriteLine("Digite tipo da suite:");
                    string suiteTypeValue = Console.ReadLine();

                    if (String.IsNullOrEmpty(suiteTypeValue.Trim()))
                    {

                        System.Console.WriteLine("Valor inválido para tipo");
                        ConsolePause.Wait();
                        return;

                    }

                    Console.WriteLine();
                    Console.WriteLine("Digite a capacidade:");
                    bool checkCapacityValue = int.TryParse(Console.ReadLine(), out int capacityValue);

                    if (!checkCapacityValue)
                    {
                        System.Console.WriteLine("Valor inválido para capacidade");
                        ConsolePause.Wait();
                        return;

                    }

                    if (capacityValue <= 0)
                    {

                        System.Console.WriteLine("Capacidade obrigatório");
                        ConsolePause.Wait();
                        return;

                    }

                    Console.WriteLine("");
                    Console.WriteLine("Digite valor da diária no formato 00.00:");
                    bool checkDailyValue = decimal.TryParse(Console.ReadLine(), System.Globalization.NumberStyles.Number, CultureInfo.InvariantCulture, out decimal dailyValue) || decimal.TryParse(Console.ReadLine(), NumberStyles.Number, new CultureInfo("pt-BR"), out dailyValue);



                    if (!checkDailyValue)
                    {
                        Console.WriteLine("Valor inválido para diária");
                        ConsolePause.Wait();
                        return;
                    }

                    if (dailyValue <= 0)
                    {
                        System.Console.WriteLine("Valor da diária obrigatório");
                        ConsolePause.Wait();
                        return;
                    }


                    var suite = new Suite(suiteType: suiteTypeValue, capacity: capacityValue, dailyValue: (int)(dailyValue * 100m));

                    var insert = _repository.RegisterSuite(suite: suite);
                    if (!insert)
                    {
                        Console.Clear();
                        Console.WriteLine("Erro ao realizar cadastro da suite");
                        ConsolePause.Wait();
                        return;
                    }

                    Console.Clear();
                    Console.WriteLine("Cadastro da suite realizado com sucesso!");
                    ConsolePause.Wait();
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
                        ConsolePause.Wait();
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
                    ConsolePause.Wait();
                    break;

                }

            case "3":
                {
                    var suites = _repository.GetAllSuitesAvailable();

                    if (suites.Count() == 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"Não há suites disponíveis");
                        Console.WriteLine();
                        ConsolePause.Wait();
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
                    ConsolePause.Wait();
                    break;
                }

            case "4":
                {
                    Console.Clear();
                    bool checkIdValue = false;
                    int suiteId = 0;
                    if (!checkIdValue)
                    {
                        Console.WriteLine("Digite o id da suite que quer desativar:");
                        checkIdValue = int.TryParse(Console.ReadLine(), out var id);
                        if (checkIdValue)
                        {
                            suiteId = id;
                        }
                    }

                    if (suiteId == 0)
                    {
                        Console.WriteLine("Id da suite inválido");
                        ConsolePause.Wait();
                        break;
                    }
                    var deactivated = _repository.DeactivateSuite(id: suiteId);
                    if (!deactivated)
                    {
                        Console.Clear();
                        Console.WriteLine("Suite não encontrada ou já está desativada");
                        ConsolePause.Wait();
                        return;
                    }

                    Console.WriteLine("Suite desativada com sucesso");
                    ConsolePause.Wait();
                    break;

                }
            default:
                break;
        }

    }
}
