using System;
using Bomberman.Api;

namespace Bomberman.Logic
{
    public class BombermanState
    {
        private const int PerkTime = 27;

        private int _timeToPlant = 0;
        private int _remoteControlBombsCount = 0;
        private int _increaseBombCountTime = 0;
        private int _increaseRadiusTime = 0;
        private int _immortalityTime = 0;

        public bool CanPlant => _timeToPlant == 0 || IncreaseBombCountTime > 0 || RemoteState == RemoteState.CAN_USE || RemoteState == RemoteState.CAN_ACT;
        public Point Position { get; private set; }

        public int TimeToPlant
        {
            get => _timeToPlant;
            set => _timeToPlant = Math.Max(0, value);
        }

        public int RemoteControlBombsCount
        {
            get => _remoteControlBombsCount;
            set => _remoteControlBombsCount = Math.Min(Math.Max(0, value), 3);
        }

        public int IncreaseBombCountTime
        {
            get => _increaseBombCountTime;
            set => _increaseBombCountTime = Math.Min(Math.Max(0, value), PerkTime);
        }

        public int IncreaseRadiusTime
        {
            get => _increaseRadiusTime;
            set => _increaseRadiusTime = Math.Max(0, value);
        }

        public int ImmortalityTime
        {
            get => _immortalityTime;
            set => _immortalityTime = Math.Min(Math.Max(0, value), PerkTime);
        }

        public RemoteState RemoteState { get; set; } = RemoteState.NONE;

        public void Update(Board board)
        {
            Position = board.GetBomberman();
            TimeToPlant--;
            IncreaseBombCountTime--;
            IncreaseRadiusTime--;
            ImmortalityTime--;
        }

        public void Act()
        {
            if (RemoteState == RemoteState.CAN_ACT)
                RemoteState = RemoteState.CAN_USE;
            else if (RemoteState == RemoteState.CAN_USE)
            {
                RemoteControlBombsCount--;

                RemoteState = RemoteControlBombsCount > 0 ? RemoteState.CAN_ACT : RemoteState.NONE;
            }
        }
    }

    public enum RemoteState
    {
        NONE = 0,
        CAN_ACT = 1,
        CAN_USE = 2
    }
}
