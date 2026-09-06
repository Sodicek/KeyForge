# Generátor náhodných hesel

Desktopová aplikace v .NET / Avalonia pro generování náhodných hesel. Běží nativně na Windows, Linuxu i macOS.

## Funkce

- Volitelná délka hesla (posuvník)
- Volitelné zahrnutí velkých písmen, čísel a speciálních znaků
- Vygenerované heslo vždy obsahuje alespoň jeden znak z každé zvolené kategorie
- Kryptograficky bezpečný generátor náhodných čísel (`RandomNumberGenerator`)
- Kopírování hesla do schránky jedním kliknutím
- Volitelné uložení hesla do souboru `password.txt` na ploše (nebo do její podsložky)
- Přepínání jazyka za běhu — čeština (výchozí) a angličtina

## Požadavky

- [.NET 8 SDK](https://dotnet.microsoft.com/download) nebo novější

## Spuštění

```bash
dotnet run --project "Random password generator"
```

Otevře se okno aplikace. Zaškrtni požadované kategorie znaků, nastav délku posuvníkem a klikni na **Generovat**. Heslo lze zkopírovat do schránky nebo uložit do textového souboru na ploše — ten je čistý text, takže po použití soubor smaž, ať v něm heslo neleží zbytečně dlouho.

## Build

```bash
dotnet build
```

Aplikace je multiplatformní (Avalonia UI) a builduje se stejně na Windows, Linuxu i macOS.

## Lokalizace

Texty aplikace jsou v `Resources/Strings.resx` (čeština, výchozí) a `Resources/Strings.en.resx` (angličtina). Nový jazyk lze přidat vytvořením dalšího `Strings.<kód_jazyka>.resx` se stejnými klíči a přidáním položky do přepínače jazyka v `Views/MainWindow.axaml`.
