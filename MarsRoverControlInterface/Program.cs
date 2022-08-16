using MarsRover.Models;
using MarsRover.Models.Exceptions;

string currentCommand = string.Empty;
Rover rover = new Rover();

Console.WriteLine("*** Welcome to the Mars Rover command interface ***");
Console.WriteLine("*** Enter command or 'GO' to drive Rover ***");
while (currentCommand.ToLower() != "exit")
{
    currentCommand = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(currentCommand))
    {
        Console.WriteLine("No Command specified. Enter a command or type ? for help");
    }
    else
    {
        switch (currentCommand.ToLower())
        {
            case "go":
                Console.WriteLine("*** Moving Rover ***");
                try
                {
                    string position = rover.Go();
                    Console.WriteLine($"New Position: {position}");
                }
                catch (BoundaryException ex)
                { Console.WriteLine(ex.Message); }

                break;
            case "?":
                Console.WriteLine("Valid commands are [distance]m, Left, Right, GO or EXIT");
                break;
            case "exit":
                break;
            default:
                try
                {
                    rover.AddCommand(currentCommand);
                }
                catch (InvalidCommandException ex)

                { Console.WriteLine($"Could not add command: {ex.Message}"); }
                catch (CommandBufferException ex)
                { Console.WriteLine(ex.Message); }
                break;
        }
    }
}