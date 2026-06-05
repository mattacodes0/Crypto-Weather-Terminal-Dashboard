# 🪙 Crypto & Weather Terminal Dashboard

Moderni hakerski terminal dashboard napisan u jeziku **C#** koji uživo povlači i prikazuje trenutne cene najpopularnijih kriptovaluta i vremensku prognozu. Projekat koristi napredne komponente za kreiranje konzolnih interfejsa.

---

## 📸 Izgled aplikacije

![Terminal Dashboard]![Uploading Capture.PNG…]()

---

## ✨ Karakteristike

* **Podaci Uživo (Live Stream):** Automatski osvežava informacije na svakih 15 sekundi bez treperenja ekrana.
* **Kripto Berza:** Vuče stabilne i tačne podatke o ceni Bitcoina (BTC) i Eterijuma (ETH) preko *CryptoCompare* API-ja.
* **Meteo Stanica:** Geolocirani podaci za temperaturu i brzinu vetra preko *Open-Meteo* servisa.
* **Napredan Layout:** Podeljen ekran na nezavisne panele sa oštrim retro ivicama inspirisan hakerskim interfejsima.
* **Otpornost na greške:** Ako mreža pukne, aplikacija ispisuje crveni error panel, ali nastavlja da radi i pokušava ponovo kada se veza vrati.

---

## 🛠️ Tehnologije i Biblioteke

* **Jezik:** C# (.NET Framework / .NET Core)
* **Izgled:** [Spectre.Console](https://spectreconsole.net/) (za layout, panele i stilizaciju teksta)
* **Obrada podataka:** `System.Text.Json` (za asinhrono parsiranje JSON odgovora)
* **Mreža:** `System.Net.Http.HttpClient`

---

## 🚀 Kako Pokrenuti Projekat

### 1. Kloniranje
Klonirajte ovaj projekat na svoj računar:
```bash
git clone [https://github.com/TVOJ_USER/CryptoAndWeatherTerminalDashboard.git](https://github.com/TVOJ_USER/CryptoAndWeatherTerminalDashboard.git)
