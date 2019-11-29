using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace rocket_bot
{
    public partial class Bot
    {
        public Rocket GetNextMove(Rocket rocket)
        {
            var moves = new ConcurrentBag<Tuple<Turn, double>>();
            var tasks = new Task[threadsCount];
            for (int i = 0; i < threadsCount; i++)
            {
                tasks[i] = new Task(() =>
                {
                    Random random = new Random();
                    var bestMoveInThread = SearchBestMove(rocket,
                        random, iterationsCount / threadsCount);
                    moves.Add(bestMoveInThread);
                });
                tasks[i].Start();
            }
            Task.WaitAll(tasks);
            var bestMove = moves.OrderBy(x => x.Item2).First();
            return rocket.Move(bestMove.Item1, level);
        }

        // Следующее решение проходит тесты через раз =>

        //public class TupleComparer : IComparer<Tuple<Turn, double>>
        //{
        //    public int Compare(Tuple<Turn, double> x, Tuple<Turn, double> y)
        //        => x.Item2.CompareTo(y.Item2);
        //}

        //public Rocket GetNextMove(Rocket rocket)
        //{
        //    var moves = new List<Tuple<Turn, double>>();
        //    Parallel.For(0, threadsCount, i =>
        //    {
        //        Random random = new Random();
        //        var bestMoveInThread = SearchBestMove(rocket,
        //            random, iterationsCount / threadsCount);
        //        lock (moves)
        //            moves.Add(bestMoveInThread);
        //    });
        //    moves.Sort(new TupleComparer());
        //    var bestMove = moves.First();
        //    return rocket.Move(bestMove.Item1, level);
        //}
    }
}