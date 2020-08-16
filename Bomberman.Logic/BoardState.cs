using System.Collections.Generic;
using System.Linq;
using Bomberman.Api;
using Bomberman.Logic.Handlers;
using Bomberman.Logic.Handlers.DirectionsWeightsHandlers;

namespace Bomberman.Logic
{
    public class BoardState
    {
        private const bool UseFreeDirections = true;
        private const bool UseSafeFromGhostsDirections = true;
        private const bool UseSafeFromAngryGhostsDirections = true;
        private const int TimeToDefineAfkPlayer = 4;

        public IList<Direction> FreeDirections { get; private set; } = new List<Direction>();
        public IList<Direction> SafeFromGhostsDirections { get; private set; } = new List<Direction>();
        public IList<Direction> SafeFromAngryGhostsDirections { get; private set; } = new List<Direction>();
        public int[,] TimesToBoom { get; private set; } = null!;
        public float[,] Weights { get; private set; } = null!;
        public int[,] AfkTimes { get; private set; } = null!;
        public int[,] TimesToGhostMove { get; private set; } = null!;
        public int[,,] GhostsWeights { get; private set; } = null!;
        public bool[,] CanDamageBuff { get; private set; } = null!;
        public bool[,] CanRemoteDamaged { get; private set; } = null!;
        public bool[,] IsIncreasedBlast { get; private set; } = null!;
        public IDictionary<Direction, float> DirectionWeights { get; private set; } = new Dictionary<Direction, float>();

        public BoardState? PrevState { get; set; }
        public Board Board { get; set; } = null!;

        public BombermanState BombermanState { get; set; } = new BombermanState();

        public bool IsAfkPlayer(Point point) => AfkTimes[point.X, point.Y] >= TimeToDefineAfkPlayer;

        public void Reset()
        {
            PrevState = null;
            BombermanState = new BombermanState();
        }

        public void Update(Board board)
        {
            Board = board;
            Weights = new float[board.Size, board.Size];
            AfkTimes = new int[board.Size, board.Size];

            IsIncreasedBlast = IncreasedRadiusHandler.IsIncreased(board, this);
            CanRemoteDamaged = RemoveControlHandler.CanDamaged(board, this);

            AfkPlayersHandler.UpdateAfkTimes(board, this);

            BombermanState.Update(board);

            FreeDirections = FreeDirectionsHandler.GetFreeDirections(board, Utils.DirectionsWithStop);
            SafeFromGhostsDirections = GhostHandler.GetSafeDirections(board, Utils.DirectionsWithStop);
            SafeFromAngryGhostsDirections = AngryGhostHandler.GetSafeDirections(board, this, Utils.DirectionsWithStop);
            TimesToBoom = TimesToBoomHandler.GetTimesToBoom(board, this);

            CanDamageBuff = CanDamageBuffHandler.GetCanDamageBuffMass(board, this);

            TimesToGhostMove = GhostPositionsHandler.GetTimeToGhostMove(board, this);

            // TODO: fix this method
            // GhostsWeights = GhostsWeightsHandler.GetGhostsWeights(board, this);

            Weights = DestroyableStaticObjectsWeightsHandler.Handle(board, this, Weights);

            BuffsWeightsHandler.UpdateWeights(board, this);

            IEnumerable<Direction> directions = Utils.DirectionsWithStop;
            if (UseFreeDirections)
            {
                var intersected = directions.Intersect(FreeDirections).ToArray();
                if (intersected.Any())
                    directions = intersected;
            }

            if (UseSafeFromGhostsDirections)
            {
                var intersected = directions.Intersect(SafeFromGhostsDirections).ToArray();
                if (intersected.Any())
                    directions = intersected;
            }

            if (UseSafeFromAngryGhostsDirections)
            {
                var intersected = directions.Intersect(SafeFromAngryGhostsDirections).ToArray();
                if (intersected.Any())
                    directions = intersected;
            }

            DirectionWeights =
                new DeepDirectionsWeightsHandler().GetWeights(board, this, directions, board.GetBomberman());

            if (DirectionWeights.Max(kvp => kvp.Value) <= 0) // костыль для обработки некорректного поведения, когда бомберман стоит впритык к мит чопперу
                DirectionWeights = new DeepDirectionsWeightsHandler().GetWeights(board, this, FreeDirections.ToArray(), board.GetBomberman());
        }
    }
}
