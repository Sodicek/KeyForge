using System.Security.Cryptography;

namespace Random_password_generator.Services;

static class PassphraseService
{
    public static int WordCount => WordList.Words.Length;

    public static string Generate(int words, bool capitalize, bool appendNumber, string separator = "-")
    {
        var parts = new List<string>(words + 1);

        for (int i = 0; i < words; i++)
        {
            string word = WordList.Words[RandomNumberGenerator.GetInt32(WordList.Words.Length)];
            parts.Add(capitalize ? char.ToUpperInvariant(word[0]) + word[1..] : word);
        }

        if (appendNumber)
        {
            parts.Add(RandomNumberGenerator.GetInt32(0, 100).ToString());
        }

        return string.Join(separator, parts);
    }

    public static double EntropyBits(int words, bool appendNumber)
    {
        double bits = words * Math.Log2(WordList.Words.Length);

        if (appendNumber)
        {
            bits += Math.Log2(100);
        }

        return bits;
    }
}
