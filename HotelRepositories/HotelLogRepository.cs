namespace DesafioProjetoHospedagem.HotelRepositories;

public class HotelLogRepository
{

    public HotelLogRepository(string path)
    {
        _path = path;
    }
    private readonly string _path;

    public void RegisterLog(string action, string? message)
    {
        string row = $"[{action}]: {(!String.IsNullOrEmpty(message) ? message + " - " : "")}{DateTime.Now:dd/MM/yyyy} - {DateTime.Now:HH:mm}";

        File.AppendAllText(_path, row + Environment.NewLine);

        // using (var writer = new StreamWriter(_path, append: true))
        // {

        // } ;
    }

    public void StartFile()
    {
        if (!File.Exists(_path))
        {
            File.WriteAllText(_path, "=== HOTEL LOG ===" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
        }
    }

}
