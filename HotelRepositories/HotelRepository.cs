using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Queries;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System;
namespace DesafioProjetoHospedagem.HotelRepositories;

public class HotelRepository
{

    public HotelRepository(SqliteConnection connection, HotelQueries queries){
        _connection = connection;
        _queries= queries;
    }


    private SqliteConnection _connection;
    private HotelQueries _queries;


    // private Dictionary<int, Suite> suites = new();
    // private Dictionary<int, Guest> hospedes = new();
    // private Dictionary<int, Reservation> reservas = new();

    public bool RegisterSuite(Suite suite)
    {
        var command = _connection.CreateCommand();
        command.CommandText = _queries.insertSuite;

        command.Parameters.AddWithValue("$type", suite.SuiteType);
        command.Parameters.AddWithValue("$capacity", suite.Capacity);
        command.Parameters.AddWithValue("$dailyValue", suite.DailyValue);
        long id = (long)command.ExecuteScalar();

        return id > 0;
        // suites.Add(proximaKey, suite);
    }

    // public void RemoverSuite(int id)
    // {

    //     // ! validar se term reserva

    //     if (id < 0 || id >= suites.Count)
    //     {
    //         Console.WriteLine("Suite não encontrada");
    //         return;
    //     }

    //     suites.Remove(id);
    // }


    public void GetAllSuites()
    {
        var command = _connection.CreateCommand();
        command.CommandText = _queries.getAllSuites;
        var suites = new List<Suite>();

        using var reader = command.ExecuteReader();
            while (reader.Read()){

                int id = reader.GetInt32(0);
                string type = reader.GetString(1);
                int capacity = reader.GetInt32(2);
                int value = reader.GetInt32(3);

                suites.Add(new Suite(id:id, suiteType: type, capacity: capacity, dailyValue: value));
            }

        if (suites.Count() > 0)
        {
            foreach ( var  suite in suites)
            {
                Console.WriteLine($"Suite id: {suite.Id?.ToString() ??  "(Sem id)"}" );
                Console.WriteLine($"Tipo: {suite.SuiteType}");
                Console.WriteLine($"Capacidade: {suite.Capacity}");
                Console.WriteLine($"Valor: {suite.FormatedDailyValue()}");
                Console.WriteLine("--------------------------");
            }
            return;
        }

        Console.WriteLine("Não há suites cadastradas");
        return;
    }

    // public void ListarSuitesLivres()
    // {





    // }

    // public void CadastrarHospedes(string nome)
    // {
    //     string value = nome.Trim();

    //     if (value == "")
    //     {
    //         Console.WriteLine();
    //         return;
    //     }

    //     string[] fullname = value.Split(" ");
    //     int proximoValor = hospedes.Keys.Max();

    //     if (fullname.Count() < 1)
    //     {
    //         hospedes.Add(proximoValor, new Guest(nome: fullname[0], sobrenome: string.Join(" ", fullname)[1..]));
    //         return;
    //     }
    //     hospedes.Add(proximoValor, new Guest(nome: value));
    //     return;
    // }


    // public void ListarHospedes()
    // {
    //     if (!hospedes.Any())
    //     {
    //         System.Console.WriteLine("Não há hospedes cadastrados");
    //         return;
    //     }

    //     foreach (KeyValuePair<int, Guest> hospede in hospedes)
    //     {
    //         string sobrenome = hospede.Value.Sobrenome != null ? hospede.Value.Sobrenome.Trim():"";

    //         System.Console.WriteLine($"{hospede.Key} - {hospede.Value.Nome} {(hospede.Value.Sobrenome != null ? $"- {hospede.Value.Sobrenome}" : "")}");
    //     }

    // }

    // void ListarHospedesSemReserva()
    // {
    //     if (!hospedes.Any())
    //     {
    //         System.Console.WriteLine("Não há hospedes cadastrados");
    //         return;
    //     }


    //     foreach (KeyValuePair<int, Guest> hospede in hospedes)
    //     {
    //         string sobrenome = hospede.Value.Sobrenome != null ? hospede.Value.Sobrenome.Trim():"";

    //         System.Console.WriteLine($"{hospede.Key} - {hospede.Value.Nome} {(hospede.Value.Sobrenome != null ? $"- {hospede.Value.Sobrenome}" : "")}");
    //     }
    // }

    // void RemoverHospede(int id)
    // {
    //     if (id <= 0)
    //     {
    //         System.Console.WriteLine("id inválido");
    //         return;
    //     }


    //     // ! validar se tem reserva

    //     // if(hospedes.TryGetValue(id, out Pessoa? pessoa))
    //     if (hospedes.ContainsKey(id))
    //     {
    //         System.Console.WriteLine("Hospede não encontrado, verifique a lista de hospedes");
    //         return;
    //     }

    //     hospedes.Remove(id);
    // }






    // public void CadastrarReserva(int diasReservados, int idSuite, List<int> listaIdsHospedes)
    // {



    // }

    // public void ListarReservas()
    // {



    // }


    // public void FinalizarReserva()
    // {



    // }
}
