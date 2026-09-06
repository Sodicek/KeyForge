# Random Password Generator

A simple .NET console app that generates a random password based on your preferences.

## Features

- Choose the password length
- Optionally include capital letters, numbers, and special characters
- Uses a cryptographically secure random number generator (`RandomNumberGenerator`)
- Optionally save the generated password to a `password.txt` file on your Desktop (or a subfolder of it)

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or newer

## Usage

```bash
dotnet run --project "Random password generator"
```

Follow the prompts to set the password length and character options. If you choose to save the password, it is written as plain text to `password.txt` — treat that file as sensitive and delete it once you no longer need it.

## Build

```bash
dotnet build
```
