using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Queries;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Globalization;
using System;
using DesafioProjetoHospedagem.HotelRepositories;
using System.Runtime.InteropServices;
namespace DesafioProjetoHospedagem.HotelRepositories;

public class HotelRepository
{

    public HotelRepository(SqliteConnection connection, HotelQueries queries, HotelLogRepository log)
    {
        _connection = connection;
        _queries = queries;
        _log = log;
    }
    private HotelLogRepository _log;
    private SqliteConnection _connection;
    private HotelQueries _queries;


    public bool RegisterSuite(Suite suite)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.insertSuite;

        command.Parameters.AddWithValue("$type", suite.SuiteType);
        command.Parameters.AddWithValue("$capacity", suite.Capacity);
        command.Parameters.AddWithValue("$dailyValue", suite.DailyValue);
        long id = (long)command.ExecuteScalar();

        _log.RegisterLog(action: "INSERT", $"{id} - {suite.SuiteType} - {suite.Capacity} - {suite.DailyValue}");
        return id > 0;
    }

    public Suite GetSuiteById(int id)
    {
        using var command = _connection.CreateCommand();
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
        string status = reader.GetString(4);

        Suite suite = new Suite(
            id: id,
            suiteType: type,
            capacity: capacity,
            dailyValue: value,
            status: status
        );
        _log.RegisterLog(action: "GET", $"{id} - {suite.SuiteType} - {suite.Capacity} - {suite.DailyValue}");
        return suite;

    }
    public bool DeactivateSuite(int id)
    {
        var suite = GetSuiteById(id);
        if (suite == null)
        {
            return false;
        }

        using var command = _connection.CreateCommand();
        command.CommandText = _queries.deactivateSuite;
        command.Parameters.AddWithValue("$id", id);
        int rows = command.ExecuteNonQuery();
        bool deactivated = rows == 1;

        if (deactivated)
        {
            _log.RegisterLog(
                action: "DEACTIVATE:SUITE",
                message: $"ID={id}; Tipo={suite.SuiteType}"
            );
        }

        return deactivated;
    }


    public List<Suite> GetAllSuites()
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getAllSuites;
        var suites = new List<Suite>();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {

            int suiteId = reader.GetInt32(0);
            string type = reader.GetString(1);
            int capacity = reader.GetInt32(2);
            int value = reader.GetInt32(3);
            string status = reader.GetString(4);

            suites.Add(new Suite(
                id: suiteId,
                suiteType: type,
                capacity: capacity,
                dailyValue: value,
                status: status
            ));
        }
        _log.RegisterLog(action: "GET", $"LISTA DE SUITES");
        return suites;
    }


    public List<Suite> GetAllSuitesAvailable()
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getAllSuitesAvailable;
        var suites = new List<Suite>();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {

            int suiteId = reader.GetInt32(0);
            string type = reader.GetString(1);
            int capacity = reader.GetInt32(2);
            int value = reader.GetInt32(3);
            string status = reader.GetString(4);

            suites.Add(new Suite(
                id: suiteId,
                suiteType: type,
                capacity: capacity,
                dailyValue: value,
                status: status
            ));
        }
        _log.RegisterLog(action: "GET", $"LISTA DE SUITES");
        return suites;
    }



    public bool RegisterGuest(Guest guest)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.insertGuest;
        command.Parameters.AddWithValue("$name", guest.Name);
        object lastName = string.IsNullOrWhiteSpace(guest.LastName)
            ? DBNull.Value
            : guest.LastName;
        command.Parameters.AddWithValue("$lastName", lastName);

        var id = (long)command.ExecuteScalar();

        if (id > 0)
        {
            _log.RegisterLog(
                action: "INSERT:GUEST",
                message: $"ID={id}; Nome={guest.FullName}"
                );

        }

        return id > 0;
    }

    public List<Guest> GetAllGuest()
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getAllGuests;
        var guests = new List<Guest>();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {

            int guestId = reader.GetInt32(0);
            string name = reader.GetString(1);
            string lastName = reader.IsDBNull(2) ? null : reader.GetString(2);
            string status = reader.GetString(3);
            guests.Add(new Guest(
                id: guestId,
                name: name,
                lastName: lastName,
                status: status
            ));
        }


        _log.RegisterLog(
            action: "GET:GUESTS",
            message: $"Quantidade={guests.Count}"
        );
        return guests;
    }

    public List<Guest> GetAllGuestAvailable()
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getAllGuestAvailable;
        var guests = new List<Guest>();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {

            int guestId = reader.GetInt32(0);
            string name = reader.GetString(1);
            string lastName = reader.IsDBNull(2) ? null : reader.GetString(2);
            string status = reader.GetString(3);
            guests.Add(new Guest(
                id: guestId,
                name: name,
                lastName: lastName,
                status: status
            ));
        }

        _log.RegisterLog(
            action: "GET:AVAILABLE_GUESTS",
            message: $"Quantidade={guests.Count}"
        );

        return guests;
    }

    public Guest GetGuestById(int id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getGuestById;
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }
        int guestId = reader.GetInt32(0);
        string name = reader.GetString(1);
        string lastName = reader.IsDBNull(2)
            ? null
            : reader.GetString(2);
        string status = reader.GetString(3);

        _log.RegisterLog(
            action: "GET:GUEST",
            message: $"ID={guestId}"
        );

        return new Guest(
            id: guestId,
            name: name,
            lastName: lastName,
            status: status
        );
    }
    public bool DeactivateGuest(int id)
    {
        var guest = GetGuestById(id);

        if (guest == null)
        {
            return false;

        }
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.deactivateGuest;
        command.Parameters.AddWithValue("$id", id);

        int rows = command.ExecuteNonQuery();
        var deactivated = rows == 1;

        if (deactivated)
        {
            _log.RegisterLog(
                action: "DEACTIVATE:GUEST",
                message: $"ID={id}; Nome={guest.FullName}"
            );
        }



        return deactivated;
    }




    public long RegisterReservation(Reservation reservation)
    {

        if (reservation.Suite?.Id is not int suiteId)
        {
            return 0;

        }

        if (reservation.Guests is null ||
            reservation.Guests.Count() == 0 ||
            reservation.Guests.Any(guest => !guest.Id.HasValue)
        )
        {

            return 0;
        }

        using var transaction = _connection.BeginTransaction();
        using var reservationCommand = _connection.CreateCommand();
        reservationCommand.Transaction = transaction;
        reservationCommand.CommandText = _queries.insertReservation;

        reservationCommand.Parameters.AddWithValue("$reservedDays", reservation.ReservedDays);
        reservationCommand.Parameters.AddWithValue("$suiteId", reservation.Suite.Id);

        var reservationId = (long)reservationCommand.ExecuteScalar();

        if (reservationId <= 0)
        {
            transaction.Rollback();
            return 0;
        }


        var guestCommand = _connection.CreateCommand();
        guestCommand.Transaction = transaction;
        guestCommand.CommandText = _queries.insertReservationGuests;

        var reservationParameter = guestCommand.Parameters.Add(
            "$reservaId",
            SqliteType.Integer
        );
        var guestParameter = guestCommand.Parameters.Add(
            "$pessoaId",
            SqliteType.Integer
        );

        reservationParameter.Value = reservationId;


        var guestIdList = reservation.Guests
            .Select(guest => guest.Id.Value)
            .Distinct();

        foreach (var guestId in guestIdList)
        {

            guestParameter.Value = guestId;
            var affectedRows = guestCommand.ExecuteNonQuery();

            if (affectedRows != 1)
            {
                transaction.Rollback();
                _log.RegisterLog(
                    action: "ERROR:RESERVATION",
                    message:
                        $"Falha ao associar hóspede; " +
                        $"ReservationId={reservationId}; GuestId={guestId}"
                );
                break;
            }
        }

        transaction.Commit();

        _log.RegisterLog(
            action: "INSERT:RESERVATION",
            message:
                $"ID={reservationId}; " +
                $"SuiteId={suiteId}; " +
                $"Hospedes={reservation.Guests.Count}; " +
                $"Dias={reservation.ReservedDays}"
        );

        return reservationId;
    }

    public Reservation GetReservationById(int id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getReservationWithSuiteById;
        command.Parameters.AddWithValue("$id", id);
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        int reservationId = reader.GetInt32(0);
        int reservedDays = reader.GetInt32(1);
        int suiteId = reader.GetInt32(3);
        string type = reader.GetString(4);
        int capacity = reader.GetInt32(5);
        int value = reader.GetInt32(6);
        string status = reader.GetString(7);
        DateTime openingDate = DateTime.Parse(
            reader.GetString(8),
            CultureInfo.InvariantCulture
        );
        DateTime? closingDate = reader.IsDBNull(9)
            ? null
            : DateTime.Parse(reader.GetString(9), CultureInfo.InvariantCulture);
        string suiteStatus = reader.GetString(10);

        Suite suite = new Suite(
            id: suiteId,
            suiteType: type,
            capacity: capacity,
            dailyValue: value,
            status: suiteStatus
        );

        reader.Close();

        var reservationGuests = GetGuestsByReservationId(id: reservationId);

        _log.RegisterLog(
            action: "GET:RESERVATION",
            message: $"ID={reservationId}"
        );

        return new Reservation(
            id: reservationId,
            reservedDays: reservedDays,
            suite: suite,
            guests: reservationGuests,
            status: status,
            openingDate: openingDate,
            closingDate: closingDate
        );
    }

    public bool RemoveReservationById(int id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.deleteReservation;
        command.Parameters.AddWithValue("$id", id);
        var rows = command.ExecuteNonQuery();


        bool removed = rows > 0;

        if (removed)
        {
            _log.RegisterLog(
                action: "DELETE:RESERVATION",
                message: $"ID={id}"
            );
        }

        return removed;
    }

    public List<Reservation> GetAllReservation()
    {
        return GetReservations();
    }

    public List<Reservation> GetOpenReservations()
    {
        return GetReservationsByStatus(Reservation.OpenStatus);
    }

    public List<Reservation> GetClosedReservations()
    {
        return GetReservationsByStatus(Reservation.ClosedStatus);
    }

    public List<Reservation> GetCanceledReservations()
    {
        return GetReservationsByStatus(Reservation.CanceledStatus);
    }

    public List<Reservation> GetReservationsByStatus(string status)
    {
        bool validStatus = status == Reservation.OpenStatus
            || status == Reservation.ClosedStatus
            || status == Reservation.CanceledStatus;

        if (!validStatus)
        {
            throw new ArgumentException("Status de reserva inválido.", nameof(status));
        }

        return GetReservations(status);
    }

    private List<Reservation> GetReservations(string status = null)
    {
        bool filterByStatus = status is not null;

        using var command = _connection.CreateCommand();
        command.CommandText = filterByStatus
            ? _queries.getReservationsWithSuiteByStatus
            : _queries.getAllReservationsWithSuite;

        if (filterByStatus)
        {
            command.Parameters.AddWithValue("$status", status);
        }

        using var reader = command.ExecuteReader();

        var reservationRows = new List<(
            int Id,
            int ReservedDays,
            Suite Suite,
            string Status,
            DateTime OpeningDate,
            DateTime? ClosingDate
        )>();

        while (reader.Read())
        {
            int reservationId = reader.GetInt32(0);
            int reservedDays = reader.GetInt32(1);
            int suiteId = reader.GetInt32(3);
            string type = reader.GetString(4);
            int capacity = reader.GetInt32(5);
            int value = reader.GetInt32(6);
            string reservationStatus = reader.GetString(7);
            DateTime openingDate = DateTime.Parse(
                reader.GetString(8),
                CultureInfo.InvariantCulture
            );
            DateTime? closingDate = reader.IsDBNull(9)
                ? null
                : DateTime.Parse(reader.GetString(9), CultureInfo.InvariantCulture);
            string suiteStatus = reader.GetString(10);

            Suite suite = new Suite(
                id: suiteId,
                suiteType: type,
                capacity: capacity,
                dailyValue: value,
                status: suiteStatus
             );

            reservationRows.Add((
                reservationId,
                reservedDays,
                suite,
                reservationStatus,
                openingDate,
                closingDate
            ));
        }

        reader.Close();

        var reservations = reservationRows.Select(item => new Reservation(
            id: item.Id,
            reservedDays: item.ReservedDays,
            suite: item.Suite,
            guests: GetGuestsByReservationId(item.Id),
            status: item.Status,
            openingDate: item.OpeningDate,
            closingDate: item.ClosingDate
        )).ToList();

        _log.RegisterLog(
            action: "GET:RESERVATIONS",
            message: $"Status={status ?? "TODOS"}; Quantidade={reservations.Count}"
        );

        return reservations;
    }

    public bool UpdateReservationStatus(int id, string status)
    {
        if (status != Reservation.ClosedStatus
            && status != Reservation.CanceledStatus)
        {
            return false;
        }

        using var command = _connection.CreateCommand();
        command.CommandText = _queries.updateReservationStatus;
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$status", status);

        bool updated = command.ExecuteNonQuery() == 1;

        if (updated)
        {
            _log.RegisterLog(
                action: "UPDATE:RESERVATION_STATUS",
                message: $"ID={id}; Status={status}"
            );
        }

        return updated;
    }

    public bool CloseReservation(int id)
    {
        return UpdateReservationStatus(id, Reservation.ClosedStatus);
    }

    public bool CancelReservation(int id)
    {
        return UpdateReservationStatus(id, Reservation.CanceledStatus);
    }

    public bool RegisterReservationGuest(List<int> guestIds, int reservationId)
    {

        var ids = guestIds.Distinct().ToList();
        if (ids.Count() == 0)
        {
            return false;
        }


        using var transaction = _connection.BeginTransaction();
        using var command = _connection.CreateCommand();

        command.Transaction = transaction;
        command.CommandText = _queries.insertReservationGuests;

        var guestParameter = command.Parameters.Add(
            "$pessoaId",
            SqliteType.Integer
        );

        var reservationParameter = command.Parameters.Add(
            "$reservaId",
            SqliteType.Integer
        );

        reservationParameter.Value = reservationId;

        foreach (int id in ids)
        {
            // command.Parameters.AddWithValue("pessoaId", id);
            // command.Parameters.AddWithValue("reservaId", reservationId);


            guestParameter.Value = id;
            var affectedRows = command.ExecuteNonQuery();

            if (affectedRows != 1)
            {
                return false;
            }
        }
        transaction.Commit();

        _log.RegisterLog(
            action: "INSERT:RESERVATION_GUESTS",
            message:
                $"ReservationId={reservationId}; " +
                $"Quantidade={ids.Count}"
        );

        return true;
    }
    public ReservationGuest ReservationGuestById(int id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getReservationGuestsById;
        command.Parameters.AddWithValue("$id", id);
        using var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        int reservationGuestById = reader.GetInt32(0);
        int guestId = reader.GetInt32(1);
        int reservationId = reader.GetInt32(2);

        return new ReservationGuest(id: reservationGuestById, guestId: guestId, reservationId: reservationId);
    }
    public ReservationGuest GetReservationGuestByGuestId(int id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getReservationGuestsByGuestId;
        command.Parameters.AddWithValue("$pessoaId", id);
        using var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        int reservationGuestById = reader.GetInt32(0);
        int guestId = reader.GetInt32(1);
        int reservationId = reader.GetInt32(2);
        return new ReservationGuest(id: reservationGuestById, guestId: guestId, reservationId: reservationId);

    }

    public List<ReservationGuest> GetReservationGuestByReservationId(int id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getReservationGuestsByReservationId;
        command.Parameters.AddWithValue("$reservaId", id);
        using var reader = command.ExecuteReader();
        List<ReservationGuest> reservationGuests = new List<ReservationGuest>();

        while (reader.Read())
        {

            int reservationGuestById = reader.GetInt32(0);
            int guestId = reader.GetInt32(1);
            int reservationId = reader.GetInt32(2);

            reservationGuests.Add(new ReservationGuest(id: reservationGuestById, guestId: guestId, reservationId: reservationId));

        }

        return reservationGuests;
    }


    public List<Guest> GetGuestsByReservationId(int id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getReservationGuestsFullByReservationId;
        command.Parameters.AddWithValue("$reservaId", id);
        using var reader = command.ExecuteReader();

        var guests = new List<Guest>();

        while (reader.Read())
        {
            int reservationGuestById = reader.GetInt32(0);
            int guestId = reader.GetInt32(1);
            int reservationId = reader.GetInt32(2);

            string guestName = reader.GetString(4);
            string guestLastName = reader.IsDBNull(5)
            ? null
            : reader.GetString(5);


            int reservedDays = reader.GetInt32(7);
            int suiteId = reader.GetInt32(8);
            string guestStatus = reader.GetString(9);

            guests.Add(new Guest(
                id: guestId,
                name: guestName,
                lastName: guestLastName,
                status: guestStatus
            ));
        }

        return guests;
    }


    public List<ReservationGuest> GetAllReservationGuest()
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getAllReservationGuests;
        using var reader = command.ExecuteReader();
        List<ReservationGuest> reservationGuests = new List<ReservationGuest>();

        while (reader.Read())
        {

            int reservationGuestById = reader.GetInt32(0);
            int guestId = reader.GetInt32(1);
            int reservationId = reader.GetInt32(2);

            reservationGuests.Add(new ReservationGuest(id: reservationGuestById, guestId: guestId, reservationId: reservationId));

        }

        return reservationGuests;
    }

    public bool RemoveReservationGuestById(int id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.deleteReservationGuests;
        command.Parameters.AddWithValue("$id", id);
        var rows = command.ExecuteNonQuery();

        bool removed = rows > 0;

        if (removed)
        {
            _log.RegisterLog(
                action: "DELETE:RESERVATION_GUEST",
                message: $"AssociationId={id}"
            );
        }

        return removed;
    }



    public bool RemoveReservationGuestByReservationId(int id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.deleteReservationGuestsByReservationId;
        command.Parameters.AddWithValue("$reservaId", id);
        var rows = command.ExecuteNonQuery();
        bool removed = rows > 0;

        if (removed)
        {
            _log.RegisterLog(
                action: "DELETE:RESERVATION_GUESTS",
                message: $"ReservationId={id}; Quantidade={rows}"
            );
        }

        return removed;
    }
}
