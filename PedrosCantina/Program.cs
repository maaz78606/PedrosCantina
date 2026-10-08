using PedrosCantina;
using Microsoft.Extensions.Configuration;
using System.Reflection;

// Connection string hentes fra User Secrets (nøgle "Pcstring"), ligesom i PedroForbindelse
var config = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
    .Build();
string? connectionString = config["Pcstring"];

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("Mangler connection string. Kør:");
    Console.WriteLine("  dotnet user-secrets set \"Pcstring\" \"Server=...;Database=PedrosCantina;...\"");
    return;
}

Iarbejder repository = new MedarbejderRepository(connectionString);
Ivagt vagtRepository = new VagtRepository(connectionString);

Console.WriteLine("Pedros Cantina");
Console.WriteLine("==============");

bool kører = true;
while (kører)
{
    Console.WriteLine();
    Console.WriteLine("1. Vis alle medarbejdere");
    Console.WriteLine("2. Find medarbejder");
    Console.WriteLine("3. Opret medarbejder");
    Console.WriteLine("4. Opdater medarbejder");
    Console.WriteLine("5. Slet medarbejder");
    Console.WriteLine("6. Vis alle vagter");
    Console.WriteLine("7. Find vagt");
    Console.WriteLine("8. Opret vagt");
    Console.WriteLine("9. Opdater vagt");
    Console.WriteLine("10. Slet vagt");
    Console.WriteLine("0. Afslut");
    Console.Write("Vælg: ");

    try
    {
        switch (Console.ReadLine())
        {
            case "1":
                var alle = repository.GetAllMedarbejdere();
                if (alle.Count == 0)
                    Console.WriteLine("Ingen medarbejdere fundet.");
                foreach (var m in alle)
                    Console.WriteLine(m);
                break;

            case "2":
                {
                    int id = LæsTal("Id: ");
                    var m = repository.GetMedarbejderById(id);
                    Console.WriteLine(m != null ? m.ToString() : "Medarbejder ikke fundet.");
                    break;
                }

            case "3":
                {
                    var ny = new Medarbejder
                    {
                        Navn = LæsTekst("Navn: "),
                        TelefonNummer = LæsTekst("Telefonnummer: ")
                    };
                    repository.CreateMedarbejder(ny);
                    Console.WriteLine("Oprettet: " + ny);
                    break;
                }

            case "4":
                {
                    int id = LæsTal("Id på medarbejder der skal opdateres: ");
                    var m = repository.GetMedarbejderById(id);
                    if (m == null)
                    {
                        Console.WriteLine("Medarbejder ikke fundet.");
                        break;
                    }
                    Console.WriteLine("Nuværende: " + m);
                    m.Navn = LæsTekst("Nyt navn: ");
                    m.TelefonNummer = LæsTekst("Nyt telefonnummer: ");
                    Console.WriteLine(repository.UpdateMedarbejder(m)
                        ? "Opdateret: " + m
                        : "Medarbejder blev ikke opdateret.");
                    break;
                }

            case "5":
                {
                    int id = LæsTal("Id på medarbejder der skal slettes: ");
                    Console.WriteLine(repository.DeleteMedarbejder(id)
                        ? "Medarbejder slettet."
                        : "Medarbejder ikke fundet.");
                    break;
                }

            case "6":
                var alleVagter = vagtRepository.GetAllVagter();
                if (alleVagter.Count == 0)
                    Console.WriteLine("Ingen vagter fundet.");
                foreach (var v in alleVagter)
                    Console.WriteLine(v);
                break;

            case "7":
                {
                    int id = LæsTal("Id: ");
                    var v = vagtRepository.GetVagtById(id);
                    Console.WriteLine(v != null ? v.ToString() : "Vagt ikke fundet.");
                    break;
                }

            case "8":
                {
                    var ny = new Vagt(0,
                        LæsDato("Dato (åååå-mm-dd): "),
                        LæsTid("Starttid (tt:mm): "),
                        LæsTid("Sluttid (tt:mm): "));
                    vagtRepository.CreateVagt(ny);
                    Console.WriteLine("Oprettet: " + ny);
                    break;
                }

            case "9":
                {
                    int id = LæsTal("Id på vagt der skal opdateres: ");
                    var v = vagtRepository.GetVagtById(id);
                    if (v == null)
                    {
                        Console.WriteLine("Vagt ikke fundet.");
                        break;
                    }
                    Console.WriteLine("Nuværende: " + v);
                    v.Dato = LæsDato("Ny dato (åååå-mm-dd): ");
                    v.StartTid = LæsTid("Ny starttid (tt:mm): ");
                    v.SlutTid = LæsTid("Ny sluttid (tt:mm): ");
                    Console.WriteLine(vagtRepository.UpdateVagt(v)
                        ? "Opdateret: " + v
                        : "Vagt blev ikke opdateret.");
                    break;
                }

            case "10":
                {
                    int id = LæsTal("Id på vagt der skal slettes: ");
                    Console.WriteLine(vagtRepository.DeleteVagt(id)
                        ? "Vagt slettet."
                        : "Vagt ikke fundet.");
                    break;
                }

            case "0":
            case null: // input lukket (fx Ctrl+Z)
                kører = false;
                break;

            default:
                Console.WriteLine("Ugyldigt valg.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Fejl: " + ex.Message);
    }
}

static string LæsTekst(string prompt)
{
    Console.Write(prompt);
    return Console.ReadLine() ?? "";
}

static int LæsTal(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int tal))
            return tal;
        Console.WriteLine("Skriv et gyldigt tal.");
    }
}

static DateOnly LæsDato(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (DateOnly.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", out DateOnly dato))
            return dato;
        Console.WriteLine("Skriv en gyldig dato, fx 2026-10-05.");
    }
}

static TimeOnly LæsTid(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (TimeOnly.TryParseExact(Console.ReadLine(), "HH:mm", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out TimeOnly tid))
            return tid;
        Console.WriteLine("Skriv en gyldig tid, fx 09:30.");
    }
}
