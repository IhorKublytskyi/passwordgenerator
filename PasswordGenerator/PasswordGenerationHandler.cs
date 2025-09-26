using System.Security.Cryptography;
using System.Text;
using TextCopy;

namespace PasswordGenerator;

internal static class PasswordGenerationHandler
{
    private static string _lowercaseAlphabet = "abcdefghijklmnopqrstuvwxyz";
    private static string _uppercaseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static string _digits = "1234567890";
    private static string _specialSymbols = "!@#$%^*_";

    internal static void Handle(int length, bool containsUppercaseLetters, bool containsDigits, bool containsSpecialSymbols, int count, bool isCopying)
    {
        for (int i = 0; i < count; i++)
        {
            string password = Generate(length, containsUppercaseLetters, containsDigits, containsSpecialSymbols, isCopying);

            Console.WriteLine(password);

            if (i == count - 1)
            {
                ClipboardService.SetText(password);
            }
        }
    }

    internal static string Generate(int length, bool containsUppercaseLetters, bool containsDigits, bool containsSpecialSymbols, bool isCopying)
    {
        string characters = _lowercaseAlphabet;

        StringBuilder stringBuilder = new StringBuilder();

        if (containsUppercaseLetters)
        {
            characters += _uppercaseAlphabet;
            stringBuilder.Append(_uppercaseAlphabet[RandomNumberGenerator.GetInt32(_uppercaseAlphabet.Length - 1)]);
        }

        if (containsDigits)
        {
            characters += _digits;
            stringBuilder.Append(_digits[RandomNumberGenerator.GetInt32(_digits.Length - 1)]);
        }

        if (containsSpecialSymbols)
        {
            characters += _specialSymbols;
            stringBuilder.Append(_specialSymbols[RandomNumberGenerator.GetInt32(_specialSymbols.Length - 1)]);
        }

        while (stringBuilder.Length < length)
        {
            stringBuilder.Append(characters[RandomNumberGenerator.GetInt32(characters.Length - 1)]);
        }

        var password = stringBuilder.ToString();

        RandomNumberGenerator.Shuffle(new Span<string>(ref password));

        return password;
    }

    internal static void CalculateEntropy(string password)
    {
        string characters = _lowercaseAlphabet;

        foreach (var character in password)
        {
            if (char.IsDigit(character))
            {
                characters += _digits;
            }

            if (char.IsUpper(character))
            {
                characters += _uppercaseAlphabet;
            }

            if (char.IsSymbol(character))
            {
                characters += _specialSymbols;
            }
        }

        double entropy = (double)password.Length * Math.Log2(characters.Length);

        if (entropy < 40)
        { 
            ShowEntropy(ConsoleColor.Red, entropy);
        }
        else if (entropy > 40 && entropy < 60)
        {
            ShowEntropy(ConsoleColor.Yellow, entropy);
        }
        else if (entropy > 60 && entropy < 80)
        {
            ShowEntropy(ConsoleColor.DarkGreen, entropy);
        }
        else
        {
            ShowEntropy(ConsoleColor.Green, entropy);
        }

        Console.ForegroundColor = ConsoleColor.Gray;
    }
    
    internal static void ShowEntropy(ConsoleColor foregroundColor, double entropy)
    {
        Console.Write("Approximate password entropy: ");
        Console.ForegroundColor = foregroundColor;
        Console.Write(Math.Round(entropy, 2));
    }

    internal static void WriteToFile(FileInfo file, string password, string label)
    {
        string text = $"{label} = {password}";

        try
        {
            File.WriteAllText(file.FullName, text);

            Console.WriteLine("Password has been succesfully written to the file.");
        }
        catch (IOException)
        {
            Console.WriteLine("Syntax error in file name, folder name, or volume label.");
        }
    }
}