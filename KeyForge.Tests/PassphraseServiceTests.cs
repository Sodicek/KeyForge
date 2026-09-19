using Random_password_generator.Services;

namespace KeyForge.Tests;

public class PassphraseServiceTests
{
    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(8)]
    public void Generate_ReturnsRequestedNumberOfWords(int wordCount)
    {
        string passphrase = PassphraseService.Generate(wordCount, capitalize: false, appendNumber: false);

        Assert.Equal(wordCount, passphrase.Split('-').Length);
    }

    [Fact]
    public void Generate_WithAppendNumber_AddsOneMoreSegment()
    {
        string passphrase = PassphraseService.Generate(4, capitalize: false, appendNumber: true);
        string[] segments = passphrase.Split('-');

        Assert.Equal(5, segments.Length);
        Assert.True(int.TryParse(segments[^1], out _));
    }

    [Fact]
    public void Generate_WithCapitalize_CapitalizesEachWord()
    {
        string passphrase = PassphraseService.Generate(5, capitalize: true, appendNumber: false);

        foreach (string word in passphrase.Split('-'))
        {
            Assert.True(char.IsUpper(word[0]));
        }
    }

    [Fact]
    public void Generate_WithoutCapitalize_KeepsWordsLowercase()
    {
        string passphrase = PassphraseService.Generate(5, capitalize: false, appendNumber: false);

        foreach (string word in passphrase.Split('-'))
        {
            Assert.True(char.IsLower(word[0]));
        }
    }

    [Fact]
    public void EntropyBits_IncreasesWithWordCount()
    {
        double fewWords = PassphraseService.EntropyBits(3, appendNumber: false);
        double moreWords = PassphraseService.EntropyBits(6, appendNumber: false);

        Assert.True(moreWords > fewWords);
    }

    [Fact]
    public void EntropyBits_WithAppendNumber_IsHigherThanWithout()
    {
        double without = PassphraseService.EntropyBits(4, appendNumber: false);
        double with = PassphraseService.EntropyBits(4, appendNumber: true);

        Assert.True(with > without);
    }

    [Fact]
    public void WordCount_ReportsAMeaningfulWordListSize()
    {
        Assert.True(PassphraseService.WordCount > 100);
    }
}
