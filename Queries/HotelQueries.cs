namespace DesafioProjetoHospedagem.Queries;

public class HotelQueries
{
    public string createSuitesTable = @"
        CREATE TABLE IF NOT EXISTS SUITES(
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            TIPO_SUITE TEXT NOT NULL,
            CAPACIDADE INTEGER NOT NULL,
            VALOR_DIARIA INTEGER NOT NULL,
            ATIVO TEXT NOT NULL DEFAULT 'S'
                CHECK (ATIVO IN ('S', 'N'))
        );
    ";

    public string createGuestsTable = @"
        CREATE TABLE IF NOT EXISTS PESSOAS (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            NOME TEXT NOT NULL,
            SOBRENOME TEXT NULL,
            ATIVO TEXT NOT NULL DEFAULT 'S'
                CHECK (ATIVO IN ('S', 'N'))
        );
    ";

    public string createReservationsTable = @"
        CREATE TABLE IF NOT EXISTS RESERVAS(
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            DIAS_RESERVADOS INTEGER NOT NULL,
            ID_SUITE INTEGER NOT NULL,
            STATUS TEXT NOT NULL DEFAULT 'AB'
                CHECK (STATUS IN ('AB', 'FE', 'CA')),
            DATA_ABERTURA TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
            DATA_FECHAMENTO TEXT NULL,
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
        SELECT ID, TIPO_SUITE, CAPACIDADE, VALOR_DIARIA, ATIVO
        FROM SUITES
        WHERE ATIVO = 'S';
    ";
    public string getAllSuitesAvailable = @"
        SELECT
            S.ID,
            S.TIPO_SUITE,
            S.CAPACIDADE,
            S.VALOR_DIARIA,
            S.ATIVO
        FROM SUITES S
        WHERE S.ATIVO = 'S'
          AND NOT EXISTS (
            SELECT 1 FROM RESERVAS R
            WHERE R.ID_SUITE = S.ID
              AND R.STATUS = 'AB'
        );
    ";

    public string getSuiteById = @"
        SELECT ID, TIPO_SUITE, CAPACIDADE, VALOR_DIARIA, ATIVO
        FROM SUITES
        WHERE ID = $id
          AND ATIVO = 'S';
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

    public string deactivateSuite = @"
        UPDATE SUITES
        SET ATIVO = 'N'
        WHERE ID = $id
          AND ATIVO = 'S';
    ";




    // GUEST
    public string getAllGuests = @"
        SELECT ID, NOME, SOBRENOME, ATIVO
        FROM PESSOAS
        WHERE ATIVO = 'S';
    ";

    public string getAllGuestAvailable = @"
        SELECT
            P.ID,
            P.NOME,
            P.SOBRENOME,
            P.ATIVO
        FROM PESSOAS P
        WHERE P.ATIVO = 'S'
          AND NOT EXISTS (
            SELECT 1
            FROM RESERVA_HOSPEDES RH
            JOIN RESERVAS R
                ON R.ID = RH.ID_RESERVA
            WHERE RH.ID_PESSOA = P.ID
              AND R.STATUS = 'AB'
        );
    ";


    public string getGuestById = @"
        SELECT ID, NOME, SOBRENOME, ATIVO
        FROM PESSOAS
        WHERE ID = $id
          AND ATIVO = 'S';
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
    public string deactivateGuest = @"
        UPDATE PESSOAS
        SET ATIVO = 'N'
        WHERE ID = $id
          AND ATIVO = 'S';
    ";

    // RESERVATION
    public string getAllReservationsWithSuite = @"
        SELECT
            R.ID,
            R.DIAS_RESERVADOS,
            R.ID_SUITE,
            S.ID,
            S.TIPO_SUITE,
            S.CAPACIDADE,
            S.VALOR_DIARIA,
            R.STATUS,
            R.DATA_ABERTURA,
            R.DATA_FECHAMENTO,
            S.ATIVO
        FROM RESERVAS R
        JOIN SUITES S
            ON R.ID_SUITE = S.ID;
    ";

    public string getReservationsWithSuiteByStatus = @"
        SELECT
            R.ID,
            R.DIAS_RESERVADOS,
            R.ID_SUITE,
            S.ID,
            S.TIPO_SUITE,
            S.CAPACIDADE,
            S.VALOR_DIARIA,
            R.STATUS,
            R.DATA_ABERTURA,
            R.DATA_FECHAMENTO,
            S.ATIVO
        FROM RESERVAS R
        JOIN SUITES S
            ON R.ID_SUITE = S.ID
        WHERE R.STATUS = $status
        ORDER BY
            CASE WHEN R.DATA_FECHAMENTO IS NULL THEN 0 ELSE 1 END,
            R.DATA_FECHAMENTO DESC,
            R.DATA_ABERTURA DESC;
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
            S.VALOR_DIARIA,
            R.STATUS,
            R.DATA_ABERTURA,
            R.DATA_FECHAMENTO,
            S.ATIVO
        FROM RESERVAS R
        JOIN SUITES S
            ON R.ID_SUITE = S.ID
        WHERE R.ID = $id;
    ";
    public string insertReservation = @"
        INSERT INTO RESERVAS (
            DIAS_RESERVADOS, ID_SUITE
        ) VALUES (
            $reservedDays, $suiteId
        ) RETURNING ID;
    ";
    public string updateReservation = @"
        UPDATE RESERVAS
            SET
                DIAS_RESERVADOS = $reservedDays,
                ID_SUITE = $suiteId
        WHERE ID = $id;
    ";

    public string updateReservationStatus = @"
        UPDATE RESERVAS
        SET
            STATUS = $status,
            DATA_FECHAMENTO = CURRENT_TIMESTAMP
        WHERE ID = $id
          AND STATUS = 'AB';
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
            R.ID_SUITE,
            P.ATIVO
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
    public string deleteReservationGuestsByReservationId = @"
        DELETE FROM RESERVA_HOSPEDES
        WHERE ID_RESERVA = $reservaId;
    ";



}
