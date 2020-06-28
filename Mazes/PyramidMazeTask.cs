namespace Mazes
{
	public static class PyramidMazeTask
	{
        
        public static void MoveOut(Robot robot, int width, int height)
        {          
            int resizeScale = 0;
            while (!robot.Finished)
            {
                MoveRight(robot, width - resizeScale);
                MoveUp(robot);
                resizeScale += 2;
                MoveLeft(robot, width - resizeScale);
                if (robot.Y > 1)
                    MoveUp(robot);
                resizeScale += 2;               
            }
        }

        static void MoveRight(Robot robot, int width)
        {
            for (int i = 0; i < width - 3; i++)
            {
                robot.MoveTo(Direction.Right);
            }
        }

        static void MoveLeft(Robot robot, int width)
        {
            for (int i = width - 3; i > 0; i--)
            {
                robot.MoveTo(Direction.Left);
            }
        }       

        static void MoveUp(Robot robot)
        {
            robot.MoveTo(Direction.Up);
            robot.MoveTo(Direction.Up);
        }
    }
}