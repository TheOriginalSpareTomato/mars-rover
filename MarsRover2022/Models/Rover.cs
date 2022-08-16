using MarsRover.Interfaces.Commands;
using MarsRover.Models.Enums;
using MarsRover.Models.Exceptions;
using System.Text.RegularExpressions;

namespace MarsRover.Models
{
    public class Rover
    {
        private Direction _currentDirection;
        private int _position;
        private List<IRoverCommand> _commandBuffer; // Generics solution means we can add more commands later, such as Drill, Shoot etc. Might be overkill for the problem as written though.

        private int[,] _grid;
        private int _xPos = 0;
        private int _yPos = 0;


        public Rover()
        {
            _grid = new int[100, 100];
            _currentDirection = Direction.South;
            _commandBuffer = new List<IRoverCommand>();
            for (int y = 0; y < 100; y++)
            {
                for (int x = 1; x < 101; x++)
                {
                    _grid[y, x - 1] = x + (y * 100);
                }
            }
            _position = _grid[_xPos, _yPos];
        }
        public string ReportPosition()
        {
            return $"{_position} {_currentDirection}";
        }

        public string Go()
        {
            for (int i = 0; i < _commandBuffer.Count(); i++)
            {
                if (_commandBuffer[i].GetType() == typeof(TurnCommand))
                {
                    var cmd = (TurnCommand)_commandBuffer[i];
                    Turn(cmd.TurnDirection);
                }
                else
                {
                    var cmd = (MoveCommand)_commandBuffer[i];
                    Drive(cmd.Distance);
                }
            }
            _commandBuffer.Clear();
            return ReportPosition();
        }

        private void Drive(int command)
        {
            switch (_currentDirection)
            {
                case Direction.North:
                    _yPos = _yPos - command;
                    if (_yPos < 0)
                    {
                        _yPos = 0;
                        _commandBuffer.Clear();
                        throw new BoundaryException("Reached the northernmost boundary");
                    }
                    break;
                case Direction.South:
                    _yPos = _yPos + command;
                    if (_yPos == _grid.GetLength(0))
                    {
                        _yPos = _grid.GetLength(0) - 1;
                    }
                    if (_yPos > _grid.GetLength(0))
                    {
                        _yPos = _grid.GetLength(0) - 1;
                        _commandBuffer.Clear();
                        throw new BoundaryException("Reached the southernmost boundary");
                    }
                    break;
                case Direction.East:
                    _xPos = _xPos + command;
                    if (_xPos == _grid.GetLength(1))
                    {
                        _xPos = _grid.GetLength(1) - 1;
                    }
                    if (_xPos > _grid.GetLength(1))
                    {
                        _xPos = _grid.GetLength(1) - 1;
                        _commandBuffer.Clear();
                        throw new BoundaryException("Reached the easternmost boundary");
                    }
                    break;
                case Direction.West:
                    _xPos = _xPos - command;
                    if (_xPos < 0)
                    {
                        _xPos = 0;
                        _commandBuffer.Clear();
                        throw new BoundaryException("Reached the westernmost boundary");
                    }
                    break;
            }

            _position = _grid[_yPos, _xPos];
        }



        public void AddCommand(string command)
        {
            ValidateCommand(command);
            if (_commandBuffer.Count == 5)
                throw new CommandBufferException("The Buffer is full, please send command GO to execute");
            if (command == "Left" || command == "Right")
                _commandBuffer.Add(new TurnCommand(command));
            else
                _commandBuffer.Add(new MoveCommand(Convert.ToInt32(command.Replace("m", ""))));
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
