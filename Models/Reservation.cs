using System.Globalization;
namespace DesafioProjetoHospedagem.Models
{
    public class Reservation
    {
        public const string OpenStatus = "AB";
        public const string ClosedStatus = "FE";
        public const string CanceledStatus = "CA";

        public int Id { get; set; }
        public List<Guest> Guests { get; set; }
        public Suite Suite { get; set; }
        public int ReservedDays { get; set; }
        public string Status { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }

        public Reservation() { }

        public Reservation(int reservedDays)
        {
            ReservedDays = reservedDays;
        }

        public Reservation( int reservedDays, Suite suite, List<Guest> guests)
        {
            ReservedDays = reservedDays;
            Suite = suite;
            Guests = guests;
        }
        public Reservation( int reservedDays, Suite suite)
        {
            ReservedDays = reservedDays;
            Suite = suite;
        }
        public Reservation(int id, int reservedDays, Suite suite, List<Guest> guests)
        {
            Id = id;
            ReservedDays = reservedDays;
            Suite = suite;
            Guests = guests;
        }

        public Reservation(
            int id,
            int reservedDays,
            Suite suite,
            List<Guest> guests,
            string status,
            DateTime openingDate,
            DateTime? closingDate)
        {
            Id = id;
            ReservedDays = reservedDays;
            Suite = suite;
            Guests = guests;
            Status = status;
            OpeningDate = openingDate;
            ClosingDate = closingDate;
        }

        public void RegisterGuest(List<Guest> guests)
        {
            if (guests.Count == 0)
            {
                throw new ArgumentException(
                    "Selecione pelo menos um hóspede."
                );
            }

            if (guests.Count > Suite.Capacity)
            {
                throw new ArgumentException(
                    "Quantidade de hóspedes maior que a capacidade da suíte."
                );
            }

            Guests = guests;

            // if (Suite != null && guests.Count <= Suite.Capacity )
            // {
            //     Guests = guests;
            // }
            // else
            // {
            //     // TODO: Retornar uma exception caso a capacidade seja menor que o número de hóspedes recebido
            //     // *IMPLEMENTE AQUI*
            //     throw new Exception("Quantidade de hospedes maior que capacidade da suite");
            // }
        }

        public void RegisterSuite(Suite suite)
        {
            Suite = suite;
        }

        public int GetGuestQuantity()
        {
            return Guests.Count;
        }

        public decimal CalcDailyValue()
        {

            decimal valor = ReservedDays * Suite.DailyValue;


            if (ReservedDays >= 10)
            {
                valor *= 0.90m;
            }

            return valor;
        }

        public decimal DailyValueToDecimal()
        {

            return CalcDailyValue() / 100m;
        }
        public string FormatedDailyValue()
        {
            return DailyValueToDecimal().ToString("0.00", CultureInfo.InvariantCulture);

        }
    }
}
