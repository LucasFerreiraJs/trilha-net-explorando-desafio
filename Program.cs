using System.Text;
using DesafioProjetoHospedagem.Models;
using Microsoft.Data.Sqlite;
using DesafioProjetoHospedagem.Queries;
using DesafioProjetoHospedagem.HotelRepositories;
using DesafioProjetoHospedagem.Menus;

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

void ExecuteIfColumnIsMissing(
    string tableName,
    string columnName,
    string migrationSql)
{
    using var checkColumnCommand = conn.CreateCommand();
    checkColumnCommand.CommandText = $"PRAGMA table_info({tableName});";

    using var reader = checkColumnCommand.ExecuteReader();
    bool columnExists = false;

    while (reader.Read())
    {
        if (reader.GetString(1).Equals(columnName, StringComparison.OrdinalIgnoreCase))
        {
            columnExists = true;
            break;
        }
    }

    reader.Close();

    if (!columnExists)
    {
        using var migrationCommand = conn.CreateCommand();
        migrationCommand.CommandText = migrationSql;
        migrationCommand.ExecuteNonQuery();
    }
}

ExecuteIfColumnIsMissing("RESERVAS", "STATUS", hotelQueries.addReservationStatusColumn);
ExecuteIfColumnIsMissing("RESERVAS", "DATA_ABERTURA", hotelQueries.addReservationOpeningDateColumn);
ExecuteIfColumnIsMissing("RESERVAS", "DATA_FECHAMENTO", hotelQueries.addReservationClosingDateColumn);
ExecuteIfColumnIsMissing("SUITES", "ATIVO", hotelQueries.addSuiteActiveColumn);
ExecuteIfColumnIsMissing("PESSOAS", "ATIVO", hotelQueries.addGuestActiveColumn);

using (var fillOpeningDateCommand = conn.CreateCommand())
{
    fillOpeningDateCommand.CommandText = hotelQueries.fillReservationOpeningDate;
    fillOpeningDateCommand.ExecuteNonQuery();
}

string filepath = "Hotel_log.txt";

HotelLogRepository logRepository = new HotelLogRepository(path: filepath);
logRepository.StartFile();

HotelRepository repository = new HotelRepository(connection: conn, queries: hotelQueries, log: logRepository);

// menus
var guestMenu = new GuestMenu(
    logRepository: logRepository,
    repository: repository
);

var reservationMenu = new ReservationMenu(
    logRepository: logRepository,
    repository: repository
);

var suiteMenu = new SuiteMenu(
    logRepository: logRepository,
    repository: repository
);

var mainMenu = new MainMenu(
    logRepository: logRepository,
    repository: repository,
    guestMenu: guestMenu,
    reservationMenu: reservationMenu,
    suiteMenu: suiteMenu
);


mainMenu.Run();

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
