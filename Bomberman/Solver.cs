using System;
using System.IO;
using System.Linq;
using System.Text;
using Bomberman.Api;
using Bomberman.Logic;
using Bomberman.Logic.Handlers;
using Bomberman.Logic.Handlers.ActHandlers;

namespace Bomberman
{
    internal class Solver : AbstractSolver
    {
        private BoardState? _prevState = null;
        private bool _lastReset = false;

        public Solver(string server) : base(server) { }

        protected override string Get(Board board)
        {
            var bs = new BoardState {PrevState = _prevState};

            if (_prevState != null)
                bs.BombermanState = _prevState.BombermanState;

            if (board.IsMyBombermanDead)
            {
                bs.Reset();
                _lastReset = true;
            }
            else
            {
                _lastReset = false;
            }

            if (_lastReset && !board.IsMyBombermanDead)
            {
                _lastReset = false;
                return "NONE";
            }

            bs.Update(board);

            var direction = bs.DirectionWeights.OrderBy(kvp => kvp.Value).LastOrDefault().Key;

            Console.WriteLine($"Time to plant: {bs.BombermanState.TimeToPlant}");
            Console.WriteLine($"Element: {board.GetAt(board.GetBomberman())}");
            Console.WriteLine($"REMOTE_CONTROL: {bs.BombermanState.RemoteState}");
            Console.WriteLine($"INCREASE RADIUS TIME: {bs.BombermanState.IncreaseRadiusTime}");

            var act = new AllPathsActHandler().GetActType(board, bs, direction);
            var result = IActHandler.AddActToDirection(direction, act);

            Log(board, bs);

            _prevState = bs;
            BuffsHandler.Handle(board, bs);

            if (act == ActType.ACT_BEFORE || act == ActType.ACT_AFTER)
            {
                var bomberman = board.GetBomberman();
                var nextBomberman = bomberman.Shift(direction);

                if (bs.BombermanState.IncreaseRadiusTime > 0)
                {
                    if (act == ActType.ACT_BEFORE)
                        bs.IsIncreasedBlast[bomberman.X, bomberman.Y] = true;
                    else if (act == ActType.ACT_AFTER)
                        bs.IsIncreasedBlast[nextBomberman.X, nextBomberman.Y] = true;
                }

                bs.BombermanState.Act();
            }

            return result;
        }

        private static void Log(Board board, BoardState bs)
        {
            var path = "log.txt";
            const string separatorString = "****************************";

            var sb = new StringBuilder();
            sb.Append(board.BoardAsString());
            sb.Append(separatorString + Environment.NewLine);

            sb.Append($"Times to ghost move: {Environment.NewLine}");
            sb = AddArrToStringBuilder(bs.TimesToGhostMove, sb, a => a.ToString());

            sb.Append(separatorString);
            sb.Append($"Increased radius: {Environment.NewLine}");

            AddArrToStringBuilder(bs.IsIncreasedBlast, sb, a => (a ? 1 : 0).ToString());

            sb.Append("----------------------------------------------------------");
            sb.Append(Environment.NewLine);

            File.AppendAllText(path, sb.ToString());
        }

        private static StringBuilder AddArrToStringBuilder<T>(T[,] arr, StringBuilder stringBuilder, Func<T, string> converter)
        {
            var size = arr.GetLength(0);

            for (int i = size - 1; i >= 0; i--)
            {
                for (int j = 0; j < size; j++)
                {
                    stringBuilder.Append(converter(arr[j, i]));
                    stringBuilder.Append(" ");
                }

                stringBuilder.Append(Environment.NewLine);
            }

            stringBuilder.Append(Environment.NewLine);

            return stringBuilder;
        }
    }
}
