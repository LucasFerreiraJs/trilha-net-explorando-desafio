namespace DesafioProjetoHospedagem.Models;

public class Guest
{
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
    public Guest(int id, string name, string lastName)
    {
        Id = id;
        Name = name;
        LastName = lastName;
    }

    public int? Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{Name} {LastName}".ToUpper();
}
