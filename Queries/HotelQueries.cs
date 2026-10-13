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
    public string getAllSuitesAvailable = @"
        SELECT
            S.ID,
            S.TIPO_SUITE,
            S.CAPACIDADE,
            S.VALOR_DIARIA
        FROM SUITES S
        WHERE NOT EXISTS (
            SELECT 1 FROM RESERVAS R
            WHERE R.ID_SUITE = S.ID
        );
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
        UPDATE SUITES
            SET TIPO_SUITE  = $type,
                CAPACIDADE = $capacity,
                VALOR_DIARIA = $dailyValue
        WHERE ID = $id;
    ";

    public string deleteSuite = @"
        DELETE FROM SUITES
        WHERE ID = $id;
    ";




    // GUEST
    public string getAllGuests = @"
        SELECT ID, NOME, SOBRENOME FROM PESSOAS;
    ";

    public string getAllGuestAvailable = @"
        SELECT
            P.ID,
            P.NOME,
            P.SOBRENOME
        FROM PESSOAS P
        WHERE NOT EXISTS (
            SELECT 1 FROM RESERVA_HOSPEDES RH
            WHERE RH.ID_PESSOA = P.ID
        );
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

    // RESERVATION
    public string getAllReservations = @"
        SELECT ID, DIAS_RESERVADOS, ID_SUITE FROM RESERVAS;
    ";

    public string getAllReservationsWithSuite = @"
            SELECT
            R.ID,
            R.DIAS_RESERVADOS,
            R.ID_SUITE,
            S.ID,
            S.TIPO_SUITE,
            S.CAPACIDADE,
            S.VALOR_DIARIA
         FROM RESERVAS R
            JOIN SUITES S
            ON R.ID_SUITE = S.ID;
    ";

    public string getReservationById = @"
        SELECT ID, DIAS_RESERVADOS, ID_SUITE FROM RESERVAS
        WHERE ID = $id;
    ";
    public string getReservationWithSuiteById = @"
        SELECT
            R.ID,
            R.DIAS_RESERVADOS,
            R.ID_SUITE,
            S.ID,
            S.TIPO_SUITE,
            S.CAPACIDADE,
            S.VALOR_DIARIA
         FROM RESERVAS R
            JOIN SUITES S
            ON R.ID_SUITE = S.ID
        WHERE R.ID = $id;
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
                DIAS_RESERVADOS = $reservedDays,
                ID_SUITE = $idSuite
        WHERE ID = $id;
    ";
    public string deleteReservation = @"
        DELETE FROM  RESERVAS
        WHERE ID = $id;
    ";



    //
    public string insertReservationGuests = @"
        INSERT INTO RESERVA_HOSPEDES (
            ID_PESSOA,
            ID_RESERVA
        ) VALUES (
            $pessoaId,
            $reservaId
        );
    ";

    public string getAllReservationGuests = @"
        SELECT
            RH.ID,
            RH.ID_PESSOA,
            RH.ID_RESERVA
        FROM RESERVA_HOSPEDES RH
        JOIN PESSOAS P
            ON P.ID = RH.ID_PESSOA;
    ";

    public string getReservationGuestsById = @"
        SELECT
            RH.ID,
            RH.ID_PESSOA,
            RH.ID_RESERVA,
        FROM RESERVA_HOSPEDES RH
        JOIN PESSOAS P
            ON P.ID = RH.ID_PESSOA
        WHERE RH.ID = $id;
    ";
    public string getReservationGuestsByReservationId = @"
        SELECT
            RH.ID,
            RH.ID_PESSOA,
            RH.ID_RESERVA,
        FROM RESERVA_HOSPEDES RH
        JOIN PESSOAS P
            ON P.ID = RH.ID_PESSOA
        WHERE RH.ID_RESERVA = $reservaId;
    ";
    public string getReservationGuestsByGuestId = @"
        SELECT
            RH.ID,
            RH.ID_PESSOA,
            RH.ID_RESERVA,
        FROM RESERVA_HOSPEDES RH
        JOIN PESSOAS P
            ON P.ID = RH.ID_PESSOA
        WHERE RH.ID_PESSOA = $pessoaId;
    ";

    public string getReservationGuestsFullByReservationId = @"
        SELECT
            RH.ID,
            RH.ID_PESSOA,
            RH.ID_RESERVA,
            P.ID,
            P.NOME,
            P.SOBRENOME,
            R.ID,
            R.DIAS_RESERVADOS,
            R.ID_SUITE
        FROM RESERVA_HOSPEDES RH
        JOIN PESSOAS P
            ON P.ID = RH.ID_PESSOA
        JOIN RESERVAS R
            ON R.ID = RH.ID_RESERVA
        WHERE RH.ID_RESERVA = $reservaId;
    ";



    public string updateReservationGuests = @"
        UPDATE RESERVA_HOSPEDES
        SET
            ID_PESSOA = $pessoaId,
            ID_RESERVA = $reservaId
        WHERE
            ID = $id;
    ";
    public string deleteReservationGuests = @"
        DELETE FROM RESERVA_HOSPEDES
        WHERE ID = $id;
    ";



}
