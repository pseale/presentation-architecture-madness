using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Helpers
{
    public static class InputHelper
    {
        public static bool IsMovingRight(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D);
        }

        public static bool IsMovingLeft(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A);
        }

        public static bool IsMovingDown(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S);
        }

        public static bool IsMovingUp(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W);
        }

        public static KeyboardInputStruct ProcessKeyboardInput()
        {
            var keyboardState = Keyboard.GetState();

            var moveDirection = new Point();
            if (IsMovingUp(keyboardState))
                moveDirection.Y--;
            if (IsMovingDown(keyboardState))
                moveDirection.Y++;
            if (IsMovingLeft(keyboardState))
                moveDirection.X--;
            if (IsMovingRight(keyboardState))
                moveDirection.X++;

            return new KeyboardInputStruct() { MoveDirection = moveDirection };
        }

        public static MouseInputStruct ProcessMouseInput(Point midpoint, Point playerPosition, Point cameraPosition, int screenWidth, int screenHeight)
        {
            var mouseState = Mouse.GetState();
            
            var isFiring = mouseState.LeftButton == ButtonState.Pressed;

            var x = CameraHelper.FitToScreen(mouseState.Position.X, screenWidth);
            var y = CameraHelper.FitToScreen(mouseState.Position.Y, screenHeight);
            var mouse = new Point(x, y);

            var direction = (mouse - midpoint - playerPosition + cameraPosition).ToVector2();
            var normalizedDirection = MathHelper.ShrinkVectorTo1Magnitude(direction);

            return new MouseInputStruct()
            {
                IsFiring = isFiring,
                PlayerFacingDirection = normalizedDirection
            };
        }

        public static bool UserIsTryingToExit()
        {
            return GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape);
        }
    }
}