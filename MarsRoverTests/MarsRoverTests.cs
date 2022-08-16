using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarsRover.Models;
using MarsRover.Models.Exceptions;
using System.Collections.Generic;
using System;

namespace MarsRoverTests
{
    [TestClass]
    public class MarsRoverTests
    {
        [TestMethod]
        public void TestReportInitialPosition()
        {
            Rover rover = new Rover();
            string expectedResult = "1 South";
            string actualResult = rover.Go();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestTurnLeft()
        {
            Rover rover = new Rover();
            string expectedResult = "1 East";
            rover.AddCommand("Left");
            string actualResult = rover.Go();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestTurnRight()
        {
            Rover rover = new Rover();
            string expectedResult = "1 West";
            rover.AddCommand("Right");
            string actualResult = rover.Go();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidCommandException))]
        public void AddInvalidCommand()
        {
            Rover rover = new Rover();
            rover.AddCommand("Open the pod bay doors, HAL");
        }
        [TestMethod]
        [ExpectedException(typeof(CommandBufferException))]
        public void ExceedCommandBufferSize()
        {
            Rover rover = new Rover();
            for (int i = 0; i < 6; i++)
            {
                rover.AddCommand("Left");
            }
        }

        [TestMethod]
        public void TestRotateClockwise()
        {
            Rover rover = new Rover();
            List<string> expectedResult = new List<string> { "1 West", "1 North", "1 East", "1 South" };
            List<string> actualResult = new List<string>();
            // Perform 4 right hand turns and report at each stage
            for (int i = 0; i < 4; i++)
            {
                rover.AddCommand("Right");
                actualResult.Add(rover.Go());
            }
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestRotateAntiClockwise()
        {
            Rover rover = new Rover();
            List<string> expectedResult = new List<string> { "1 East", "1 North", "1 West", "1 South" };
            List<string> actualResult = new List<string>();
            // Perform 4 left hand turns and report at each stage
            for (int i = 0; i < 4; i++)
            {
                rover.AddCommand("Left");
                actualResult.Add(rover.Go());
            }
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void Drive0m()
        {
            Rover rover = new Rover();
            string expectedResult = "1 South";
            rover.AddCommand("0m");
            Assert.AreEqual(expectedResult, rover.Go());
        }
        [TestMethod]
        public void DriveSouth50Metres()
        {
            Rover rover = new Rover();
            rover.AddCommand("50m");
            string expectedResult = "5001 South";
            string actualResult = rover.Go();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DriveEast99Metres()
        {
            Rover rover = new Rover();
            rover.AddCommand("Left");
            rover.AddCommand("99m");
            string expectedResult = "100 East";
            string actualResult = rover.Go();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void DriveEast1Metres()
        {
            Rover rover = new Rover();
            rover.AddCommand("Left");
            rover.AddCommand("1m");
            string expectedResult = "2 East";
            string actualResult = rover.Go();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void DriveEast60Metres()
        {
            Rover rover = new Rover();
            rover.AddCommand("Left");
            rover.AddCommand("60m");
            string expectedResult = "61 East";
            string actualResult = rover.Go();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void DriveInACircle()
        {
            Rover rover = new Rover();
            string startPos = "1 South";
            rover.AddCommand("50m");
            rover.AddCommand("Left");
            rover.AddCommand("50m");
            rover.AddCommand("Left");
            rover.AddCommand("50m");
            rover.Go(); // Rover is limited to 5 commands at a time.
            rover.AddCommand("Left");
            rover.AddCommand("50m");
            rover.AddCommand("Left");
            string currentPos = rover.Go();
            Assert.AreEqual(currentPos, startPos);
        }

        [TestMethod]
        public void TestStatedProblem()
        {
            Rover rover = new Rover();
            rover.AddCommand("50m");
            rover.AddCommand("Left");
            rover.AddCommand("23m");
            rover.AddCommand("Left");
            rover.AddCommand("4m");

            Assert.AreEqual(rover.Go(), "4624 North");
        }
        [TestMethod]
        public void DriveToSouthLimit()
        {
            Rover rover = new Rover();
            rover.AddCommand("100m");
            Assert.AreEqual("9901 South", rover.Go());
        }
        [TestMethod]
        public void DriveToEastLimit()
        {
            Rover rover = new Rover();
            rover.AddCommand("Left");
            rover.AddCommand("100m");
            Assert.AreEqual(rover.Go(), "100 East");
        }
        [TestMethod]
        public void DriveToEastBoundryThenTurnAround50m()
        {
            string expectedResult = "50 West";
            Rover rover = new Rover();
            rover.AddCommand("Left");
            rover.AddCommand("101m");
            try
            {
                rover.Go();
            }
            catch(BoundaryException)
            {
                rover.AddCommand("Left");
                rover.AddCommand("Left");
                rover.AddCommand("50m");
                Assert.AreEqual(expectedResult, rover.Go());
            }
        }
        [TestMethod]
        [ExpectedException(typeof(BoundaryException))]
        public void DriveBeyondEastLimit()
        {
            Rover rover = new Rover();
            rover.AddCommand("Left");
            rover.AddCommand("101m");
            rover.Go();
        }
        [TestMethod]
        [ExpectedException(typeof(BoundaryException))]
        public void DriveBeyondNorthLimit()
        {
            Rover rover = new Rover();
            rover.AddCommand("Left");
            rover.AddCommand("Left");
            rover.AddCommand("1m");
            rover.Go();
        }
        [TestMethod]
        [ExpectedException(typeof(BoundaryException))]
        public void DriveBeyondWestLimit()
        {
            Rover rover = new Rover();
            rover.AddCommand("Right");
            rover.AddCommand("1m");
            rover.Go();
        }
        [TestMethod]
        public void DriveToSouthEastLimit()
        {
            Rover rover=new Rover();
            rover.AddCommand("100m");
            rover.AddCommand("Left");
            rover.AddCommand("100m");
            Assert.AreEqual(rover.Go(), "10000 East");
        }
        [TestMethod]
        public void DriveAround()
        {
            Rover rover = new Rover();
            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    int commandType = rnd.Next(0, 2);
                    switch (commandType)
                    {
                        case 0:
                            rover.AddCommand("Right");
                            break;
                        case 1:
                            rover.AddCommand("Left");
                            break;
                        default:
                            rover.AddCommand($"{rnd.Next(0, 100)}m");
                            break;
                    }
                    if (i % 5 == 0)
                        rover.Go();
                }
                catch (BoundaryException)
                {
                    //Turn Rover around
                    rover.AddCommand("Left");
                    rover.AddCommand("Left");
                    rover.Go();
                }
            }
        }
    }
}