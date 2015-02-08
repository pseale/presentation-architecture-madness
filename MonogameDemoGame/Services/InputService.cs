using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonogameDemoGame.Structs;
using MathHelper = MonogameDemoGame.Helpers.MathHelper;

namespace MonogameDemoGame.Services
{
    public class InputService : IInputService
    {
        public InputStruct ProcessInput(Point midpoint, Point playerPosition, Point cameraPosition)
        {
            var moveDirection = GetMoveDirection(Keyboard.GetState());
            var mouseState = Mouse.GetState();

            bool isFiring = GetFiringStatus(mouseState);
            var direction = ProcessMouseInput(mouseState, midpoint, playerPosition, cameraPosition);

            return new InputStruct()
            {
                IsFiring = isFiring,
                MoveDirection = moveDirection,
                PlayerFacingDirection = direction
            };
        }

        private Point GetMoveDirection(KeyboardState keyboardState)
        {
            var moveDirection = new Point();
            if (IsMovingUp(keyboardState))
                moveDirection.Y--;
            if (IsMovingDown(keyboardState))
                moveDirection.Y++;
            if (IsMovingLeft(keyboardState))
                moveDirection.X--;
            if (IsMovingRight(keyboardState))
                moveDirection.X++;

            return moveDirection;
        }

        public bool UserIsTryingToExit()
        {
            return GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape);
        }

        private bool GetFiringStatus(MouseState mouseState)
        {
            return mouseState.LeftButton == ButtonState.Pressed;
        }

        private Vector2 ProcessMouseInput(MouseState mouseState, Point midpoint, Point playerPosition, Point cameraPosition)
        {
            
            var direction = (mouseState.Position - midpoint - playerPosition + cameraPosition).ToVector2();
            var normalizedDirection = MathHelper.ShrinkVectorTo1Magnitude(direction);

            return normalizedDirection;
        }

        private bool IsMovingRight(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D);
        }

        private bool IsMovingLeft(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A);
        }

        private bool IsMovingDown(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S);
        }

        private bool IsMovingUp(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W);
        }
    }
}