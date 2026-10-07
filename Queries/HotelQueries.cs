namespace DesafioProjetoHospedagem.Queries;

public class HotelQueries
{
    public string createSuitesTable = @"
        CREATE TABLE IF NOT EXISTS SUITES(
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            TIPO_SUITE TEXT NOT NULL,
            CAPACIDADE INTEGER NOT NULL,
            VALOR_DIARIA INTEGER NOT NULL
        );
    ";

    public string createGuestsTable = @"
        CREATE TABLE IF NOT EXISTS PESSOAS (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            NOME TEXT NOT NULL,
            SOBRENOME TEXT NULL
        );
    ";

    public string createReservationsTable = @"
        CREATE TABLE IF NOT EXISTS RESERVAS(
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            DIAS_RESERVADOS INTEGER NOT NULL,
            ID_SUITE INTEGER NOT NULL,
            FOREIGN KEY (ID_SUITE) REFERENCES SUITES(ID)
        );
    ";

    public string createReservationGuestsTable = @"
        CREATE TABLE IF NOT EXISTS RESERVA_HOSPEDES(
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            ID_PESSOA INTEGER NOT NULL,
            ID_RESERVA INTEGER NOT NULL,
            FOREIGN KEY (ID_PESSOA) REFERENCES PESSOAS(ID),
            FOREIGN KEY (ID_RESERVA) REFERENCES RESERVAS(ID)
        );
    ";

    public string getAllSuites = @"
        SELECT ID, TIPO_SUITE, CAPACIDADE, VALOR_DIARIA FROM SUITES;
    ";

    public string getSuiteById = @"
        SELECT ID, TIPO_SUITE, CAPACIDADE, VALOR_DIARIA FROM SUITES
        WHERE ID = $id;
    ";

    // SUITES
    public string insertSuite = @"
        INSERT INTO SUITES (
            TIPO_SUITE, CAPACIDADE, VALOR_DIARIA
        ) VALUES (
            $type, $capacity, $dailyValue
        ) RETURNING ID;
    ";

    public string updateSuite = @"
        UPDATE SUITE
            SET TIPO_SUITE  = $type,
                CAPACIDADE = $capacity,
                VALOR_DIARIA = $dailyValue
        WHERE ID = $id;
    ";

    public string deleteSuite = @"
        DELETE FROM SUITES
        WHERE ID = $id;
    ";

    public string getAllGuests = @"
        SELECT ID, NOME, SOBRENOME FROM PESSOAS;
    ";
    public string getGuestById = @"
        SELECT ID, NOME, SOBRENOME FROM PESSOAS
        WHERE ID = $id;
    ";
    public string insertGuest = @"
        INSERT INTO PESSOAS (
            NOME, SOBRENOME
        ) VALUES (
            $name, $lastName
        ) RETURNING ID;
    ";
    public string updateGuest = @"
        UPDATE PESSOAS
            SET NOME = $name,
                SOBRENOME = $lastName
        WHERE ID = $id;
    ";
    public string deleteGuest = @"
        DELETE FROM PESSOAS
        WHERE ID = $id;
    ";

    public string getAllReservations = @"
        SELECT ID, DIAS_RESERVADOS, ID_SUITE FROM RESERVAS;
    ";
    public string getReservationById = @"
        SELECT ID, DIAS_RESERVADOS, ID_RESERVA FROM RESERVAS
        WHERE ID = $id;
    ";
    public string insertReservation = @"
        INSERT INTO RESERVAS (
            DIAS_RESERVADOS, ID_SUITE
        ) VALUES (
            $reservedDays, $idSuite
        ) RETURNING ID;
    ";
    public string updateReservation = @"
        UPDATE RESERVAS
            SET
            DIAS_RESERVADOS  = $reservedDays
            ID_SUITE = $idSuite
        WHERE ID = $id;
    ";
    public string deleteReservation = @"
        DELETE FROM  RESERVAS
        WHERE ID = $id;
    ";


}
