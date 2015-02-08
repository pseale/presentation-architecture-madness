using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Services;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Core.Domain
{
    public class Player
    {
        private int _xp;
        private int _level;

        public Player(Point position, Vector2 facingDirection)
        {
            Position = position;
            FacingDirection = facingDirection;

            FiringAngles = new List<int>();
            MoveDirection = new Point();
            FiringAngles.Add(0);
            _level = 1;
        }

        public Point Position { get; private set; }
        public Vector2 FacingDirection { get; private set; }
        public Point MoveDirection { get; private set; }
        public List<int> FiringAngles { get; private set; }
        public bool IsFiring { get; private set; }

        public void Update(InputStruct input)
        {
            MoveDirection = input.MoveDirection;
            IsFiring = input.IsFiring;
            FacingDirection = input.PlayerFacingDirection;
        }

        public void Move()
        {
            Position = Position + MoveDirection;
        }

        private void LevelUp(IRandomNumberService randomNumberService)
        {
            _level++;
            FiringAngles.Add((int)(1 + randomNumberService.GenerateRandomNumberClusteredTowardZero(15)));
        }

        private bool ShouldLevelUp()
        {
            return (_level * _level + 1) / 3 < _xp;
        }

        public void AwardExperience()
        {
            _xp++;
        }

        public LevelUpResult TryLevelUp(IRandomNumberService _randomNumberService)
        {
            if (ShouldLevelUp())
            {
                LevelUp(_randomNumberService);
                return LevelUpResult.LeveledUp;
            }

            return LevelUpResult.NothingHappened;
        }
    }
}