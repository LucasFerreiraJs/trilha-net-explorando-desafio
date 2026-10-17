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

        using var command = _connection.CreateCommand();
        command.CommandText = _queries.deleteSuite;
        command.Parameters.AddWithValue("$id", id);
        int rows = command.ExecuteNonQuery();
        _log.RegisterLog(action: "DELETE", $"{id} - {suite.SuiteType} - {suite.Capacity} - {suite.DailyValue}");
        return rows > 0;
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

            suites.Add(new Suite(id: suiteId, suiteType: type, capacity: capacity, dailyValue: value));
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

            suites.Add(new Suite(id: suiteId, suiteType: type, capacity: capacity, dailyValue: value));
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

        return id > 0;
    }

    public List<Guest> GetAllGuest()
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getAllGuests;
        var guests = new List<Guest>();

        using var reader = command.ExecuteReader();

        while (reader.Read()){

            int guestId = reader.GetInt32(0);
            string name = reader.GetString(1);
            string lastName = reader.IsDBNull(2) ?  null : reader.GetString(2);
            guests.Add(new Guest(id: guestId, name: name, lastName: lastName));
        }

        return guests;
    }

    public List<Guest> GetAllGuestAvailable()
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getAllGuestAvailable;
        var guests = new List<Guest>();

        using var reader = command.ExecuteReader();

        while (reader.Read()){

            int guestId = reader.GetInt32(0);
            string name = reader.GetString(1);
            string lastName = reader.IsDBNull(2) ?  null : reader.GetString(2);
            guests.Add(new Guest(id: guestId, name: name, lastName: lastName));
        }

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

        return new Guest(id: guestId, name: name, lastName: lastName);
    }
    public bool RemoveGuestById(int id)
    {
        var guest = GetGuestById(id);

        if (guest == null){
            return false;

        }
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.deleteGuest;
        command.Parameters.AddWithValue("$id", id);

        int rows = command.ExecuteNonQuery();
        return rows > 0;
    }




     public long RegisterReservation(Reservation reservation)
    {

        if (reservation.Suite?.Id is not int suiteId){
            return 0;

        }

        if (reservation.Guests is null ||
            reservation.Guests.Count() == 0 ||
            reservation.Guests.Any(guest => !guest.Id.HasValue)
        ){

            return 0;
        }

        using var transaction = _connection.BeginTransaction();
        using var reservationCommand = _connection.CreateCommand();
        reservationCommand.Transaction = transaction;
        reservationCommand.CommandText = _queries.insertReservation;

        reservationCommand.Parameters.AddWithValue("$reservedDays", reservation.ReservedDays);
        reservationCommand.Parameters.AddWithValue("$suiteId", reservation.Suite.Id);

        var reservationId = (long)reservationCommand.ExecuteScalar();

        if (reservationId <= 0){
            transaction.Rollback();
            return 0;
        }


        var  guestCommand = _connection.CreateCommand();
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
                break;
            }
        }

        transaction.Commit();
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

        // ID, DIAS_RESERVADOS, ID_RESERVA
        int reservationId = reader.GetInt32(0);
        int reservedDays = reader.GetInt32(1);
        // int idSuite = reader.GetInt32(2);

        int suiteId = reader.GetInt32(3);
        string type = reader.GetString(4);
        int capacity = reader.GetInt32(5);
        int value = reader.GetInt32(6);

        Suite suite = new Suite(
            id: suiteId,
            suiteType: type,
            capacity: capacity,
            dailyValue: value
        );

        var ReservationGuest = GetGuestsByReservationId(id: reservationId);

        return new Reservation(
            id: reservationId,
            reservedDays: reservedDays,
            suite: suite,
            guests: ReservationGuest
        );
    }

    public bool RemoveReservationById(int id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.deleteReservation;
        command.Parameters.AddWithValue("$id", id);
        var rows = command.ExecuteNonQuery();
        return rows > 0;
    }

    public List<Reservation> GetAllReservation()
    {
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getAllReservationsWithSuite;
        using var reader = command.ExecuteReader();

        List<Reservation> reservations = new List<Reservation>();

        while (reader.Read()){

            int reservationId = reader.GetInt32(0);
            int reservedDays = reader.GetInt32(1);

            int suiteId = reader.GetInt32(3);
            string type = reader.GetString(4);
            int capacity = reader.GetInt32(5);
            int value = reader.GetInt32(6);

            Suite suite = new Suite(
                id: suiteId,
                suiteType: type,
                capacity: capacity,
                dailyValue: value
             );

            var reservationGuests = GetGuestsByReservationId(id: reservationId);
            Console.WriteLine($"reservationGuests.Count(): {reservationGuests.Count()}");
            reservations.Add(new Reservation(
                id: reservationId,
                reservedDays:reservedDays,
                suite: suite,
                guests: reservationGuests
                )
            );
        }

        return reservations;
    }

    public bool RegisterReservationGuest(List<int> guestIds, int  reservationId){

        var ids = guestIds.Distinct().ToList();
        if (ids.Count() == 0){
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

        foreach (int id in ids){
            // command.Parameters.AddWithValue("pessoaId", id);
            // command.Parameters.AddWithValue("reservaId", reservationId);


            guestParameter.Value = id;
            var affectedRows = command.ExecuteNonQuery();

            if (affectedRows != 1){
                return false;
            }
        }
        transaction.Commit();
        return true;
    }
    public ReservationGuest ReservationGuestById(int id){
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
    public ReservationGuest GetReservationGuestByGuestId(int id){
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

    public List<ReservationGuest> GetReservationGuestByReservationId(int id){
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getReservationGuestsByReservationId;
        command.Parameters.AddWithValue("$reservaId", id);
        using var reader = command.ExecuteReader();
        List<ReservationGuest> reservationGuests = new List<ReservationGuest>();

        while (reader.Read()){

            int reservationGuestById = reader.GetInt32(0);
            int guestId = reader.GetInt32(1);
            int reservationId = reader.GetInt32(2);

            reservationGuests.Add(new ReservationGuest(id: reservationGuestById, guestId: guestId, reservationId: reservationId));

        }

        return reservationGuests;
    }


    public List<Guest> GetGuestsByReservationId(int id){
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.getReservationGuestsFullByReservationId;
        command.Parameters.AddWithValue("$reservaId", id);
        using var reader = command.ExecuteReader();

        var guests = new List<Guest>();

        System.Console.WriteLine("cai cai");
        while (reader.Read()){
            System.Console.WriteLine("cai nesse while?");
            int reservationGuestById = reader.GetInt32(0);
            int guestId = reader.GetInt32(1);
            int reservationId = reader.GetInt32(2);

            string guestName = reader.GetString(4);
            string guestLastName = reader.GetString(5);


            int reservedDays = reader.GetInt32(7);
            int suiteId = reader.GetInt32(8);

            guests.Add(new Guest(
                id: guestId,
                name: guestName,
                lastName: guestLastName
            ));
        }

        return guests;
    }


    public List<ReservationGuest> GetAllReservationGuest(){
      using var command = _connection.CreateCommand();
        command.CommandText = _queries.getAllReservationGuests;
        using var reader = command.ExecuteReader();
        List<ReservationGuest> reservationGuests = new List<ReservationGuest>();

        while (reader.Read()){

            int reservationGuestById = reader.GetInt32(0);
            int guestId = reader.GetInt32(1);
            int reservationId = reader.GetInt32(2);

            reservationGuests.Add(new ReservationGuest(id: reservationGuestById, guestId: guestId, reservationId: reservationId));

        }

        return reservationGuests;
    }

    public bool RemoveReservationGuestById(int id){
        using var command = _connection.CreateCommand();
        command.CommandText = _queries.deleteReservationGuests;
        command.Parameters.AddWithValue("$id", id);
        var rows = command.ExecuteNonQuery();
        return rows > 0;
    }

}
