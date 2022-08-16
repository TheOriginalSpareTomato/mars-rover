using MarsRover.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MarsRover.Models.Exceptions;
using MarsRover.Interfaces.Commands;
using MarsRover.Models.Enums;

namespace MarsRover.Models
{
    public class Rover
    {
        private Direction _currentDirection;
        private int _position;
        private List<IRoverCommand> commandBuffer; // Generics solution means we can add more commands later, such as Drill, Shoot etc. Might be overkill for the problem as written though.

        private int[,] grid = new int[100, 100];
        private int xPos = 0;
        private int yPos = 0;
        

        public Rover()
        {
            _currentDirection = Direction.South;
            commandBuffer = new List<IRoverCommand>();
            for (int y = 0; y < 100; y++)
            {
                for (int x = 1; x < 101; x++)
                {
                    grid[y, x-1] = x + (y * 100);
                }
            }
            _position = grid[xPos, yPos];
        }
        public string ReportPosition()
        {
            return $"{_position} {_currentDirection}";
        }

        public string Go()
        {
            for (int i = 0; i < commandBuffer.Count(); i++)
            {
                if (commandBuffer[i].GetType() == typeof(TurnCommand))
                {
                    var cmd = (TurnCommand)commandBuffer[i];
                    Turn(cmd.TurnDirection);
                }
                else
                {
                    var cmd = (MoveCommand)commandBuffer[i];
                    Drive(cmd.Distance);
                }
            }
            commandBuffer.Clear();
            return ReportPosition();
        }

        private void Drive(int command)
        {
            switch (_currentDirection)
            {
                case Direction.North:
                    yPos = yPos - command;
                    if (yPos < 0)
                    {
                        yPos = 0;
                        commandBuffer.Clear();
                        throw new BoundaryException("Reached the northernmost boundary");
                    }
                    break;
                case Direction.South:
                    yPos = yPos + command;
                    if(yPos == grid.GetLength(0))
                    {
                        yPos = grid.GetLength(0) -1 ;
                    }
                    if (yPos > grid.GetLength(0))
                    {
                        yPos = grid.GetLength(0) - 1;
                        commandBuffer.Clear();
                        throw new BoundaryException("Reached the southernmost boundary");
                    }
                    break;
                case Direction.East:
                    xPos = xPos + command;
                    if(xPos == grid.GetLength(1))
                    {
                        xPos = grid.GetLength(1) - 1;
                    }
                    if (xPos > grid.GetLength(1))
                    {
                        xPos = grid.GetLength(1) - 1;
                        commandBuffer.Clear();
                        throw new BoundaryException("Reached the easternmost boundary");
                    }
                    break;
                case Direction.West:
                    xPos = xPos - command;
                    if( xPos < 0)
                    {
                        xPos= 0;
                        commandBuffer.Clear();
                        throw new BoundaryException("Reached the westernmost boundary");
                    }
                    break;
            }

             _position = grid[yPos, xPos];
        }



        public void AddCommand(string command)
        {
            ValidateCommand(command);
            if (commandBuffer.Count == 5)
                throw new CommandBufferException("The Buffer is full, please send command GO to execute");
            if (command == "Left" || command == "Right")
                commandBuffer.Add(new TurnCommand(command));
            else
                commandBuffer.Add(new MoveCommand(Convert.ToInt32(command.Replace("m", ""))));
        }

        private void Turn(string turnDirection)
        {
            switch (turnDirection)
            {
                case "Left":
                    if (_currentDirection == Direction.North)
                        _currentDirection = Direction.West;
                    else
                        _currentDirection--;
                    break;
                case "Right":
                    if (_currentDirection == Direction.West)
                        _currentDirection = Direction.North;
                    else
                        _currentDirection++;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(turnDirection));
            }
        }

        private void ValidateCommand(string command)
        {
            if (Regex.Matches(command, "\\d+[m]|^Left$|^Right$").Count() != 1)
            {
                throw new InvalidCommandException($"{command} is not a valid command for this rover.");
            }
        }

    }
}
