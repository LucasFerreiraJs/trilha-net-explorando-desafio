namespace DesafioProjetoHospedagem.Models;

public class Guest
{
    public const string ActiveStatus = "S";
    public const string InactiveStatus = "N";

    public Guest() { }

    public Guest(string name)
    {
        Name = name;
    }

    public Guest(string name, string lastName)
    {
        Name = name;
        LastName = lastName;
    }
    public Guest(int id, string name, string lastName, string status = ActiveStatus)
    {
        Id = id;
        Name = name;
        LastName = lastName;
        Status = status;
    }

    public int? Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Status { get; set; } = ActiveStatus;
    public string FullName => $"{Name} {LastName}".ToUpper();
}
