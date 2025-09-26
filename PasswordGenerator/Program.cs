using System.CommandLine;

namespace PasswordGenerator;

class Program
{
    static int Main(string[] args)
    {
        RootCommand rootCommand = new RootCommand("An application for generating strong passwords.");

        Command passwordCommand = new Command("password", "Work with password generation, entropy verification, writing to a file etc.");

        Option<int> lengthOption = new("--length", "-l")
        {
            Description = "Sets the password length. Maximum value is 25.",
            DefaultValueFactory = result => 20
        };

        lengthOption.Validators.Add(result =>
        {
            if(result.GetValue(lengthOption) > 25)
            {
                result.AddError("Length must be less than or equal to 25.");
            }
            if(result.GetValue(lengthOption) < 4)
            {
                result.AddError("Length must be greater than or equal to 4");
            }
        });

        Option<bool> uppercaseOption = new("--uppercase", "-u")
        {
            Description = "Add capital letters to the password."
        };

        Option<bool> digitsOption = new("--digit", "-d")
        {
            Description = "Add digits too the password."
        };

        Option<bool> specialSymbolsOption = new("--special", "-s")
        {
            Description = "Add special characters to the password."
        };

        Option<int> countOption = new("--count", "-c")
        {
            Description = "Password count.",
            DefaultValueFactory = parseResult => 1
        };

        Option<bool> copyOption = new("--copy", "-cp")
        {
            Description = "The password will be copied to the clipboard."
        };

        Command generateCommand = new("generate", "Generate a password.") 
        {
            lengthOption,
            uppercaseOption,
            digitsOption,
            specialSymbolsOption,
            countOption,
            copyOption
        };

        generateCommand.SetAction(parseResult => PasswordGenerationHandler.Handle(
            parseResult.GetValue(lengthOption),
            parseResult.GetValue(uppercaseOption),
            parseResult.GetValue(digitsOption),
            parseResult.GetValue(specialSymbolsOption),
            parseResult.GetValue(countOption),
            parseResult.GetValue(copyOption)));

        passwordCommand.Subcommands.Add(generateCommand);

        Argument<string> passwordOption = new("password")
        {
            Description = "Password."
        };

        Command calculateEntropyCommand = new("entropy", "Calculate password`s entropy.")
        {
            passwordOption
        };

        calculateEntropyCommand.SetAction(parseResult => PasswordGenerationHandler.CalculateEntropy(parseResult.GetValue(passwordOption)));

        passwordCommand.Subcommands.Add(calculateEntropyCommand);

        Option<FileInfo> fileArgument = new("--file", "-f")
        {
            Description = "Path to file to write to.",
            DefaultValueFactory = parseResult => 
            {
                return new FileInfo($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\ps.txt");
            }
        };

        Argument<string> passwordArgument = new("password");

        Argument<string> labelArgument = new("label"); 

        Command writeCommand = new("write", "Write password to a file.") 
        { 
            fileArgument, 
            passwordArgument, 
            labelArgument 
        };

        writeCommand.SetAction(parseResult => PasswordGenerationHandler.WriteToFile(
            parseResult.GetValue(fileArgument),
            parseResult.GetValue(passwordArgument),
            parseResult.GetValue(labelArgument)
            ));

        passwordCommand.Subcommands.Add(writeCommand);

        rootCommand.Subcommands.Add(passwordCommand);

        ParseResult parseResult = rootCommand.Parse(args);

        if(parseResult.Errors.Count == 0)
        {
            parseResult.Invoke();
        }

        foreach (var error in parseResult.Errors)
        {
            Console.WriteLine(error.Message);
        }

        return 1;
    }
}