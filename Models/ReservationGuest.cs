namespace DesafioProjetoHospedagem.Models;

public class ReservationGuest
{
    public int? Id { get; set; }
    public int GuestId { get; set; }
    public int ReservationId { get; set; }

    public ReservationGuest(int guestId, int reservationId)
    {
        GuestId= guestId;
        ReservationId= reservationId;
    }

    public ReservationGuest(int id, int guestId, int reservationId)
    {


        Id= id;
        GuestId= guestId;
        ReservationId= reservationId;
    }
}
