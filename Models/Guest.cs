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

    public string Name { get; set; }
    public string LastName { get; set; }
    public string NameCompleto => $"{Name} {LastName}".ToUpper();
}
