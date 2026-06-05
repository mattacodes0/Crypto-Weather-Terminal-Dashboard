using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;
using Spectre.Console;
using System.Net.Http;

namespace CryptoAndWeatherTerminalDashboard
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AnsiConsole.Clear();

            // 1. STRUKTURA KOJA PRESLIKAVA TVOJU SLIKU
            var raspored = new Layout("Glavni")
                .SplitRows(
                    new Layout("Gornji").Size(10),
                    new Layout("Donji")
                );

            raspored["Donji"].SplitColumns(
                new Layout("Levi"),
                new Layout("Desni")
            );

            raspored["Levi"].SplitRows(
                new Layout("MaliTop").Size(3),
                new Layout("Kripto")
            );

            using (HttpClient klijent = new HttpClient())
            {
                klijent.DefaultRequestHeaders.Add("User-Agent", "C# Dashboard App");

                while (true)
                {
                    try
                    {
                        // --- 2. OSVEŽAVANJE PODATAKA ---
                        string kriptoURL = "https://min-api.cryptocompare.com/data/pricemulti?fsyms=BTC,ETH&tsyms=USD";
                        string kriptoODGOVOR = await klijent.GetStringAsync(kriptoURL);

                        double btcCena = 0;
                        double ethCena = 0;

                        using (JsonDocument kriptoDoc = JsonDocument.Parse(kriptoODGOVOR))
                        {
                            btcCena = kriptoDoc.RootElement.GetProperty("BTC").GetProperty("USD").GetDouble();
                            ethCena = kriptoDoc.RootElement.GetProperty("ETH").GetProperty("USD").GetDouble();
                        }

                        string vremeURL = "https://api.open-meteo.com/v1/forecast?latitude=43.3211&longitude=21.8959&current_weather=true";
                        string vremeODGOVOR = await klijent.GetStringAsync(vremeURL);

                        double temp = 0;
                        double vetar = 0;

                        using (JsonDocument vremeDoc = JsonDocument.Parse(vremeODGOVOR))
                        {
                            var trenutnoVreme = vremeDoc.RootElement.GetProperty("current_weather");
                            temp = trenutnoVreme.GetProperty("temperature").GetDouble();
                            vetar = trenutnoVreme.GetProperty("windspeed").GetDouble();
                        }


                        // --- 3. REKREIRANJE STILA SA SLIKE (Belo-sivi oštri dizajn) ---

                        // Gornji panel: Naslov
                        var asciiText = @"
  ██████╗  █████╗ ███████╗██╗  ██╗██████╗  ██████╗  █████╗ ██████╗ ██████╗ 
  ██╔══██╗██╔══██╗██╔════╝██║  ██║██╔══██╗██╔═══██╗██╔══██╗██╔══██╗██╔══██╗
  ██║  ██║███████║███████╗███████║██████╔╝██║   ██║███████║██████╔╝██║  ██║
  ██║  ██║██╔══██║╚════██║██╔══██║██╔══██╗██║   ██║██╔══██║██╔══██╗██║  ██║
  ██████╔╝██║  ██║███████║██║  ██║██████╔╝╚██████╔╝██║  ██║██║  ██║██████╔╝
  ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚═════╝  ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═════╝ ";

                        var gornjiPanel = new Panel(new Text(asciiText, new Style(Color.White)));
                        gornjiPanel.Border = BoxBorder.Square;
                        gornjiPanel.BorderColor(Color.White);
                        raspored["Gornji"].Update(gornjiPanel);

                        // Mali gornji levi panel
                        string vremeSada = DateTime.Now.ToString("HH:mm:ss");
                        var statusniTekst = new Markup($"[bold white]Sistem status:[/] [green]ONLINE[/]  |  [grey]Sat:[/] [white]{vremeSada}[/]");

                        var maliTopPanel = new Panel(statusniTekst);
                        maliTopPanel.Border = BoxBorder.Square;
                        maliTopPanel.BorderColor(Color.White);
                        raspored["MaliTop"].Update(maliTopPanel);

                        // Donji levi panel: Lista kriptovaluta (Zamenjene uglaste zagrade običnim crticama da ne brljavi)
                        string kriptoLista = $"\n  [yellow]> [/][bold white]Bitcoin (BTC):[/]   [green]${btcCena:N2}[/]\n" +
                                             $"  [grey]- [/][bold white]Ethereum (ETH):[/] [green]${ethCena:N2}[/]";

                        var kriptoPanel = new Panel(new Markup(kriptoLista));
                        kriptoPanel.Border = BoxBorder.Square;
                        kriptoPanel.BorderColor(Color.White);
                        kriptoPanel.Expand();
                        raspored["Kripto"].Update(kriptoPanel);

                        // Desni veliki panel: Meteo podaci (Izbačene uglaste zagrade oko vremena)
                        string istorijaMeteo = $"\n [white]{vremeSada}[/] | [cyan]Učitavanje meteo stanice...[/]\n" +
                                               $" [white]{vremeSada}[/] | Lokacija: Niš, Srbija\n" +
                                               $" [white]{vremeSada}[/] | [orange3]Temperatura: {temp}°C[/]\n" +
                                               $" [white]{vremeSada}[/] | [blue3]Brzina vetra: {vetar} km/h[/]\n\n" +
                                               $" [grey]> Dashboard aktivan i prati promene...[/]";

                        var desniPanel = new Panel(new Markup(istorijaMeteo));
                        desniPanel.Border = BoxBorder.Square;
                        desniPanel.BorderColor(Color.White);
                        desniPanel.Expand();
                        raspored["Desni"].Update(desniPanel);


                        // --- 4. REFRESH BEZ SECKANJA ---
                        AnsiConsole.Cursor.Show(false);
                        AnsiConsole.Clear();
                        AnsiConsole.Write(raspored);

                        await Task.Delay(15000);
                    }
                    catch (Exception ex)
                    {
                        var greskaPanel = new Panel(new Markup($"[red]Mrežni error:[/] {ex.Message}")).BorderColor(Color.Red);
                        raspored["MaliTop"].Update(greskaPanel);

                        AnsiConsole.Clear();
                        AnsiConsole.Write(raspored);

                        await Task.Delay(5000);
                    }
                }
            }
        }
    }
}