# mars-rover
My solution to the Mars Rover challenge.
## Overall Thoughts

I could see two possible solutions to the problem, one was to use a 2-dimensional array as demonstrated in this repo, and the other was by using pure maths to calculate the answer to the algorithm.

With the mathematical solution I was able to produce the expected responses by multiplying/dividing the position by 100 for north/south movement and adding/subtracting the movement to the position for lateral movements. This was pretty efficient, and didn't require the construction of a 2 dimensional array to represent the grid. However, the solution was not particularly OO, and made it harder for the developer to read.

The presented solution is the simplest to read and conceptualise, although it presents one problem with the maximum movement range. As the rover is assumed to be within the 1 metre square, the problem does not specify where within the square the rover should be positioned. Therefore, assuming that the rover is in the middle of the 1m square, and we are measuring the position from the middle of the square, the maximum movement of 100m would put it outside of the 100m boundary.

I've taken the creative liberty to say that the boundary of the movement is right to the edge of the 100mx100m square, and therefore a movement of 100m from one of the edge sqares will put you inside the 100th element of the array, but a movement of 101m will put you outside.

## Design considerations
I've implemented the control actions as a generic interface so that the command buffer can contain different types of commands (and handle them appropriately at runtime). This model could allow us to add new command types for our rover, such as "Drill" or "Shoot", but maintain the command buffer.

Direction is implemented as an enum to allow us to add or subtract from it.

## Future Considerations
As well as new command types, I would probably add the following functionality:
- Custom Grid sizes (passed into the constructor of the Rover)
- Custom Command Buffer size
- List current commands
- Clear command list
- Validate path will not exceed boundary before running for real
- Return to start (reset rover position back to square 1)
- Odometer to track how far the rover has travelled

### Note
The Console application is a .NET2022 application using the new application templates and global usings. It will not compile in VS2019. The MSTest projet should run in either VS2019 or VS2022.
