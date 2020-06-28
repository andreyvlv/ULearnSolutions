// В этом пространстве имен содержатся средства для работы с изображениями. Чтобы оно стало доступно, в проект был подключен Reference на сборку System.Drawing.dll
using System;
using System.Drawing;

namespace Fractals
{
	internal static class DragonFractalTask
	{      

        public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
        {
            double x = 1.0;
            double y = 0.0;
            const double angle45 = Math.PI / 4;
            const double angle135 = Math.PI - angle45;            
            Random rnd = new Random(seed);
            for (int i = 0; i < iterationsCount; i++)
            {               
                if (rnd.Next(0, 2) == 1)
                {
                    var x1 = (x * Math.Cos(angle45) - y * Math.Sin(angle45)) / Math.Sqrt(2);
                    var y1 = (x * Math.Sin(angle45) + y * Math.Cos(angle45)) / Math.Sqrt(2);
                    x = x1;
                    y = y1;
                }
                else
                {
                    var x1 = (x * Math.Cos(angle135) - y * Math.Sin(angle135)) / Math.Sqrt(2) + 1;
                    var y1 = (x * Math.Sin(angle135) + y * Math.Cos(angle135)) / Math.Sqrt(2);
                    x = x1;
                    y = y1;
                }               
                pixels.SetPixel(x, y);
            }
        }       

    }
}