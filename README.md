# Generátor náhodných hesel

Jednoduchá konzolová aplikace v .NET, která vygeneruje náhodné heslo podle tvých preferencí.

## Funkce

- Volitelná délka hesla
- Volitelné zahrnutí velkých písmen, čísel a speciálních znaků
- Vygenerované heslo vždy obsahuje alespoň jeden znak z každé zvolené kategorie
- Kryptograficky bezpečný generátor náhodných čísel (`RandomNumberGenerator`)
- Volitelné uložení hesla do souboru `password.txt` na ploše (nebo do její podsložky)
- Podpora dvou jazyků — čeština (výchozí) a angličtina

## Požadavky

- [.NET 8 SDK](https://dotnet.microsoft.com/download) nebo novější

## Použití

```bash
dotnet run --project "Random password generator"
```

Na začátku zvol jazyk a dále postupuj podle výzev pro délku hesla a zahrnuté znaky. Pokud si heslo necháš uložit, zapíše se jako čistý text do `password.txt` — po použití soubor smaž, ať v něm heslo neleží zbytečně dlouho.

## Build

```bash
dotnet build
```

## Lokalizace

Texty aplikace jsou v `Resources/Strings.resx` (čeština, výchozí) a `Resources/Strings.en.resx` (angličtina). Nový jazyk lze přidat vytvořením dalšího `Strings.<kód_jazyka>.resx` se stejnými klíči.
