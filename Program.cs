using System.Text;
using DesafioProjetoHospedagem.HotelServices;
using DesafioProjetoHospedagem.Models;
using Microsoft.Data.Sqlite;
using DesafioProjetoHospedagem.Queries;
using DesafioProjetoHospedagem.HotelRepositories;

Console.OutputEncoding = Encoding.UTF8;


// CONFIG
using var conn = new SqliteConnection("Data Source=hotel.db");
conn.Open();

using (var pragma = conn.CreateCommand())
{
    pragma.CommandText = "PRAGMA foreign_keys = ON";
    pragma.ExecuteNonQuery();

}
using var command = conn.CreateCommand();



var hotelQueries = new HotelQueries();
string[] scripts = {

    hotelQueries.createSuitesTable,
    hotelQueries.createGuestsTable,
    hotelQueries.createReservationsTable,
    hotelQueries.createReservationGuestsTable,
};

foreach (string sql in scripts)
{
    command.CommandText = sql;
    command.ExecuteNonQuery();
}


// Cria os modelos de hóspedes e cadastra na lista de hóspedes
// List<Pessoa> hospedes = new List<Pessoa>();

// Pessoa p1 = new Pessoa(nome: "Hóspede 1");
// Pessoa p2 = new Pessoa(nome: "Hóspede 2");

// hospedes.Add(p1);
// hospedes.Add(p2);

// Cria a suíte
// Suite suite = new Suite(tipoSuite: "Premium", capacidade: 2, valorDiaria: 30);

// Cria uma nova reserva, passando a suíte e os hóspedes
// Reserva reserva = new Reserva(diasReservados: 5);
// reserva.CadastrarSuite(suite);
// reserva.CadastrarHospedes(hospedes);

// Exibe a quantidade de hóspedes e o valor da diária
// Console.WriteLine($"Hóspedes: {reserva.ObterQuantidadeHospedes()}");
// Console.WriteLine($"Valor diária: {reserva.CalcularValorDiaria()}");

bool exibirMenu = true;

HotelRepository repository = new HotelRepository(connection: conn, queries: hotelQueries);

HotelService service = new HotelService(repository: repository);

menu();

Console.WriteLine("Programa finalizado");


void menu()
{
    while (exibirMenu)
{
    // Console.Clear();
    // Console.WriteLine("Digite uma opção");
    // Console.WriteLine("1 - Cadastrar suite");
    // Console.WriteLine("2 - Listar suites");
    // Console.WriteLine("3 - Listar suites livres");
    // Console.WriteLine("4 - Cadastrar hospedes");
    // Console.WriteLine("5 - Listar hospedes");
    // Console.WriteLine("6 - Listar hospedes sem reserva");
    // Console.WriteLine("7 - Remover hospedes");
    // Console.WriteLine("8 - Realizar reserva");
    // Console.WriteLine("9 - Listar reservas");
    // Console.WriteLine("10 - Finalizar reserva");

    Console.Clear();
    Console.WriteLine("Digite uma opção:");
    Console.WriteLine("");
    Console.WriteLine("1 - Menu suite");
    Console.WriteLine("2 - Menu hospedes");
    Console.WriteLine("3 - Menu reservas");
    Console.WriteLine("4 - Finalizar reserva");
    Console.WriteLine("");


    switch (Console.ReadLine())
    {
        case "1":
            service.SuitesMenu();

            break;

        case "2":
            service.GuestMenu();
            break;

        case "3":
            service.ReservationMenu();
            break;

        case "4":
            Console.Clear();
            break;

        default:
            Console.WriteLine("Opção inválida");
            break;
    }


    // Console.ReadLine();

}


}
