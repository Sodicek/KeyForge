using Random_password_generator.Services;

namespace KeyForge.Tests;

public class PasswordServiceTests
{
    [Theory]
    [InlineData(false, false, false, 1)]
    [InlineData(true, false, false, 2)]
    [InlineData(true, true, false, 3)]
    [InlineData(true, true, true, 4)]
    public void MinLength_MatchesNumberOfSelectedCategories(bool capitals, bool special, bool numbers, int expected)
    {
        Assert.Equal(expected, PasswordService.MinLength(capitals, special, numbers));
    }

    [Theory]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(64)]
    public void Generate_ReturnsPasswordOfRequestedLength(int length)
    {
        string password = PasswordService.Generate(length, includeCapitals: true, includeSpecial: true, includeNumbers: true, excludeAmbiguous: false);

        Assert.Equal(length, password.Length);
    }

    [Fact]
    public void Generate_ContainsAtLeastOneCharacterFromEachSelectedCategory()
    {
        for (int i = 0; i < 200; i++)
        {
            string password = PasswordService.Generate(8, includeCapitals: true, includeSpecial: true, includeNumbers: true, excludeAmbiguous: false);

            Assert.Contains(password, c => char.IsLower(c));
            Assert.Contains(password, c => char.IsUpper(c));
            Assert.Contains(password, c => char.IsDigit(c));
            Assert.Contains(password, c => !char.IsLetterOrDigit(c));
        }
    }

    [Fact]
    public void Generate_WithOnlyLowercase_ContainsOnlyLowercaseLetters()
    {
        string password = PasswordService.Generate(20, includeCapitals: false, includeSpecial: false, includeNumbers: false, excludeAmbiguous: false);

        Assert.All(password, c => Assert.True(char.IsLower(c)));
    }

    [Fact]
    public void Generate_ExcludingAmbiguous_NeverContainsAmbiguousCharacters()
    {
        const string ambiguous = "0O1lI";

        for (int i = 0; i < 200; i++)
        {
            string password = PasswordService.Generate(32, includeCapitals: true, includeSpecial: true, includeNumbers: true, excludeAmbiguous: true);

            Assert.All(password, c => Assert.DoesNotContain(c, ambiguous));
        }
    }

    [Fact]
    public void CharsetSize_GrowsAsMoreCategoriesAreSelected()
    {
        int lowercaseOnly = PasswordService.CharsetSize(false, false, false, excludeAmbiguous: false);
        int allCategories = PasswordService.CharsetSize(true, true, true, excludeAmbiguous: false);

        Assert.True(allCategories > lowercaseOnly);
    }

    [Fact]
    public void CharsetSize_ExcludingAmbiguous_IsSmallerThanIncludingThem()
    {
        int withAmbiguous = PasswordService.CharsetSize(true, true, true, excludeAmbiguous: false);
        int withoutAmbiguous = PasswordService.CharsetSize(true, true, true, excludeAmbiguous: true);

        Assert.True(withoutAmbiguous < withAmbiguous);
    }
}
