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
    }
}