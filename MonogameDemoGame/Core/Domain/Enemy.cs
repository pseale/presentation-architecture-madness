using Microsoft.Xna.Framework;
using MonogameDemoGame.Helpers;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Core.Domain
{
    public class Enemy
    {
        private const int TicksToWaitAtBeginning = 600;
        private const int EnemyHealth = 100;

        private const int EnemyTicksToDoNothing = 60;
        private const int EnemyTicksToTurn = 90;
        private const int EnemyTicksToMove = 240;
        
        private int _health;
        private EnemyState _state { get; set; }
        private int _ticksUntilDone;

        public Enemy(Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction;
            _health = EnemyHealth;

            _state = EnemyState.DoingNothing;
            _ticksUntilDone = TicksToWaitAtBeginning;
        }

        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }


        public bool HasNoHealth()
        {
            return _health <= 0;
        }

        public void Hurt()
        {
            _health--;
        }

        public void Update()
        {
            _ticksUntilDone--;
            if (_state == EnemyState.DoingNothing)
            {
                //do nothing

                if (_ticksUntilDone == 0)
                    ChangeStateToMoving();
            }
            else if (_state == EnemyState.Moving)
            {
                Move();

                if (_ticksUntilDone == 0)
                    ChangeStateToTurning();
            }
            else if (_state == EnemyState.Turning)
            {
                Turn();

                if (_ticksUntilDone == 0)
                    ChangeStateToDoingNothing();
            }
        }

        private void Turn()
        {
            Direction = Direction.Rotate(1);
        }

        private void Move()
        {
            Position = Position + Direction;
        }

        private void ChangeStateToDoingNothing()
        {
            ChangeEnemyState(EnemyState.DoingNothing, EnemyTicksToDoNothing);
        }

        private void ChangeStateToTurning()
        {
            ChangeEnemyState(EnemyState.Turning, EnemyTicksToTurn);
        }

        private void ChangeStateToMoving()
        {
            ChangeEnemyState(EnemyState.Moving, EnemyTicksToMove);
        }

        private void ChangeEnemyState(EnemyState newState, int ticksUntilDone)
        {
            _state = newState;
            _ticksUntilDone = ticksUntilDone;
        }
    }
}