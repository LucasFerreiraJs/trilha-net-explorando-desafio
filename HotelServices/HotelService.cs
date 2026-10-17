using System.Collections.Generic;
using System.Globalization;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.HotelRepositories;

namespace DesafioProjetoHospedagem.HotelServices;

public class HotelService
{
    private HotelRepository _repository;

    public HotelService(HotelRepository repository) {
        _repository = repository;
    }

    public void PressAnyButtonMessage()
    {
        Console.WriteLine();
        Console.WriteLine("Aperte qualquer tecla para voltar");
        Console.ReadLine();
    }

    public void SuitesMenu()
    {


    }

    public void GuestMenu(){


    }

    public void ReservationMenu(){


    }
}
