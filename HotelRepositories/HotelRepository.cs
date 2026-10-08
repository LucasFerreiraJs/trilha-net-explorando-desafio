using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Queries;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System;
using DesafioProjetoHospedagem.HotelRepositories;
namespace DesafioProjetoHospedagem.HotelRepositories;

public class HotelRepository
{

    public HotelRepository(SqliteConnection connection, HotelQueries queries, HotelLogRepository log){
        _connection = connection;
        _queries= queries;
        _log= log;
    }
    private HotelLogRepository _log;

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

        _log.RegisterLog(action: "INSERT", $"{id} - {suite.SuiteType} - {suite.Capacity} - {suite.DailyValue}");
        return id > 0;

        // suites.Add(proximaKey, suite);
    }

    public Suite GetSuiteById(int id)
    {
        var command = _connection.CreateCommand();
        command.CommandText = _queries.getSuiteById;
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        string type = reader.GetString(1);
        int capacity = reader.GetInt32(2);
        int value = reader.GetInt32(3);

        Suite suite = new Suite(id: id, suiteType: type, capacity: capacity, dailyValue: value);
        _log.RegisterLog(action: "GET", $"{id} - {suite.SuiteType} - {suite.Capacity} - {suite.DailyValue}");
        return suite;

    }
    public bool RemoveSuite(int id)
    {
        var suite = GetSuiteById(id);
        if (suite == null)
        {
            return false;
        }

        var command = _connection.CreateCommand();
        command.CommandText = _queries.deleteSuite;
        command.Parameters.AddWithValue("$id", id);
        int rows = command.ExecuteNonQuery();
        _log.RegisterLog(action: "DELETE", $"{id} - {suite.SuiteType} - {suite.Capacity} - {suite.DailyValue}");
        return rows > 0;
    }


    public List<Suite> GetAllSuites()
    {
        var command = _connection.CreateCommand();
        command.CommandText = _queries.getAllSuites;
        var suites = new List<Suite>();

        using var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        while (reader.Read())
        {

            int id = reader.GetInt32(0);
            string type = reader.GetString(1);
            int capacity = reader.GetInt32(2);
            int value = reader.GetInt32(3);

            suites.Add(new Suite(id: id, suiteType: type, capacity: capacity, dailyValue: value));
        }
        _log.RegisterLog(action: "GET", $"LISTA DE SUITES");
        return suites;
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
