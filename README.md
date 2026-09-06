<p align="center">
  <img src="Random password generator/Assets/icon.png" alt="KeyForge logo" width="120" height="120">
</p>

<h1 align="center">KeyForge</h1>

<p align="center">Desktopová aplikace v .NET / Avalonia pro generování náhodných hesel. Běží nativně na Windows, Linuxu i macOS.</p>

## Stažení

Nejnovější verzi ke stažení najdeš na stránce [Releases](https://github.com/Sodicek/KeyForge/releases/latest). Jde o samostatně spustitelné balíčky — .NET SDK není potřeba, stačí stáhnout, rozbalit a spustit:

| Platforma | Soubor |
|---|---|
| Windows (x64) | [KeyForge-win-x64.zip](https://github.com/Sodicek/KeyForge/releases/latest/download/KeyForge-win-x64.zip) |
| Linux (x64) | [KeyForge-linux-x64.tar.gz](https://github.com/Sodicek/KeyForge/releases/latest/download/KeyForge-linux-x64.tar.gz) |
| macOS (Intel) | [KeyForge-osx-x64.tar.gz](https://github.com/Sodicek/KeyForge/releases/latest/download/KeyForge-osx-x64.tar.gz) |
| macOS (Apple Silicon) | [KeyForge-osx-arm64.tar.gz](https://github.com/Sodicek/KeyForge/releases/latest/download/KeyForge-osx-arm64.tar.gz) |

Na Linuxu/macOS je po rozbalení potřeba nastavit spustitelné právo: `chmod +x KeyForge`.

## Funkce

- Volitelná délka hesla (posuvník)
- Volitelné zahrnutí velkých písmen, čísel a speciálních znaků
- Volitelné vyloučení matoucích znaků (0/O, 1/l/I)
- Vygenerované heslo vždy obsahuje alespoň jeden znak z každé zvolené kategorie
- Ukazatel síly hesla (entropie v bitech)
- Kryptograficky bezpečný generátor náhodných čísel (`RandomNumberGenerator`)
- Kopírování hesla do schránky jedním kliknutím, se automatickým smazáním schránky po 30 s
- Volitelné uložení hesla do souboru `password.txt` na ploše (nebo do její podsložky)
- Přepínání jazyka za běhu — čeština (výchozí) a angličtina

## Ovládání

Zaškrtni požadované kategorie znaků, nastav délku posuvníkem a klikni na **Generovat**. Heslo lze zkopírovat do schránky nebo uložit do textového souboru na ploše — ten je čistý text, takže po použití soubor smaž, ať v něm heslo neleží zbytečně dlouho.

## Spuštění ze zdrojového kódu

Pro vývoj nebo pokud nechceš stažený balíček, jde appka spustit i přímo ze zdrojů.

### Požadavky

- [.NET 8 SDK](https://dotnet.microsoft.com/download) nebo novější

```bash
dotnet run --project "Random password generator"
```

## Build

```bash
dotnet build
```

Aplikace je multiplatformní (Avalonia UI) a builduje se stejně na Windows, Linuxu i macOS.

## Lokalizace

Texty aplikace jsou v `Resources/Strings.resx` (čeština, výchozí) a `Resources/Strings.en.resx` (angličtina). Nový jazyk lze přidat vytvořením dalšího `Strings.<kód_jazyka>.resx` se stejnými klíči a přidáním položky do přepínače jazyka v `Views/MainWindow.axaml`.
