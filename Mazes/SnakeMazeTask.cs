namespace Mazes
{
	public static class SnakeMazeTask
	{       

        public static void MoveOut(Robot robot, int width, int height)
        {           
            while (!robot.Finished)
            {
                MoveRight(robot, width);
                MoveDown(robot);
                MoveLeft(robot, width);
                if (robot.Y < height - 2)
                {
                    MoveDown(robot);
                }               
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

        static void MoveDown(Robot robot)
        {
            robot.MoveTo(Direction.Down);
            robot.MoveTo(Direction.Down);
        }
    }
}