using MarsRover.Interfaces.Commands;

namespace MarsRover.Models
{
    internal class MoveCommand : IRoverCommand
    {
        internal int Distance { get; private set; }
        internal MoveCommand(int distance)
        {
            Distance = distance;
        }
    }
}
