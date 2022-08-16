using MarsRover.Interfaces;
using MarsRover.Interfaces.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Models
{
    internal class TurnCommand :IRoverCommand
    {
        public string TurnDirection { get; private set; }
        internal TurnCommand(string direction)
        {
            TurnDirection = direction;
        }
    }
}
