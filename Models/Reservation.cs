namespace DesafioProjetoHospedagem.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public List<Guest> Guests { get; set; }
        public Suite Suite { get; set; }
        public int ReservedDays { get; set; }

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
        public Reservation(int id, int reservedDays, Suite suite, List<Guest> guests)
        {
            Id = id;
            ReservedDays = reservedDays;
            Suite = suite;
            Guests = guests;
        }

        public void CadastrarHospedes(List<Guest> guests)
        {
            // TODO: Verificar se a capacidade é maior ou igual ao número de hóspedes sendo recebido
            // *IMPLEMENTE AQUI*
            if (true)
            {
                Guests = guests;
            }
            else
            {
                // TODO: Retornar uma exception caso a capacidade seja menor que o número de hóspedes recebido
                // *IMPLEMENTE AQUI*
            }
        }

        public void CadastrarSuite(Suite suite)
        {
            Suite = suite;
        }

        public int ObterQuantidadeHospedes()
        {
            // TODO: Retorna a quantidade de hóspedes (propriedade Hospedes)
            // *IMPLEMENTE AQUI*
            return 0;
        }

        public decimal CalcularValorDiaria()
        {
            // TODO: Retorna o valor da diária
            // Cálculo: DiasReservados X Suite.ValorDiaria
            // *IMPLEMENTE AQUI*
            decimal valor = 0;

            // Regra: Caso os dias reservados forem maior ou igual a 10, conceder um desconto de 10%
            // *IMPLEMENTE AQUI*
            if (true)
            {
                valor = 0;
            }

            return valor;
        }
    }
}
