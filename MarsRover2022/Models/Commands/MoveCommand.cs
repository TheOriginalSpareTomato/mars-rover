using MarsRover.Interfaces;
using MarsRover.Interfaces.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
