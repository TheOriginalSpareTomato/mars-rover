using MarsRover.Interfaces.Commands;

namespace MarsRover.Models
{
    internal class TurnCommand : IRoverCommand
    {
        public string TurnDirection { get; private set; }
        internal TurnCommand(string direction)
        {
            TurnDirection = direction;
        }
    }
}
