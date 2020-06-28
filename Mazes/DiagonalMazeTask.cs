using System;

namespace Mazes
{
	public static class DiagonalMazeTask
    {        
        public static void MoveOut(Robot robot, int width, int height)
        {           
            int rightStep = LengthOfRightStep(width, height);
            int downStep = LengthOfDownStep(width, height);
            if (height / width >= 1)
            {
                IfHeightIsGreather(robot, width, height, rightStep, downStep);
            }
            else
            {
                IfWidthIsGreather(robot, width, height, rightStep, downStep);
            }
        }

        static void IfHeightIsGreather(Robot robot, int width, int height, int rightStep, int downStep)
        {
            while (!robot.Finished)
            {
                Move(robot, downStep, Direction.Down);
                if (robot.Y < height - 2)
                    Move(robot, rightStep, Direction.Right);
            }          
        }

        static void IfWidthIsGreather(Robot robot, int width, int height, int rightStep, int downStep)
        {
            while (!robot.Finished)
            {
                Move(robot, rightStep, Direction.Right);
                if (robot.Y < height - 2)
                    Move(robot, downStep, Direction.Down);
            }           
        }       

        static int LengthOfRightStep(int width, int height)
        {
            if (width > height && (width / height > 1.5))
            {
                return (int)Math.Ceiling((double)width / height);                
            }
            else
            {
                return (int)Math.Ceiling((double)width / height);               
            }
        }

        static int LengthOfDownStep(int width, int height)
        {
            if (width > height && (width / height > 1.5))
            {
                return (int)Math.Ceiling((double)height / width);
            }
            else
            {
                return (int)Math.Round((double)height / width);
            }
        }

        static void Move(Robot robot, int count, Direction direction)
        {
            for (int i = 0; i < count; i++)
            {
                robot.MoveTo(direction);
            }
        }
       
    }
}