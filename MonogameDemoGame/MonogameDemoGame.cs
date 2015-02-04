using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game4
{
    public class MonogameDemoGame : Game
    {
        private const int ScreenWidth = 640;
        private const int ScreenHeight = 480;
        private const int WidthMidpoint = ScreenWidth / 2;
        private const int HeightMidpoint = ScreenHeight / 2;
        private readonly Point Midpoint = new Point(WidthMidpoint, HeightMidpoint);
        private const int NoFlexZone = 100;
        private const int GameBorder = 2000;
        private const int EnemiesToSpawn = 400;
        
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private Random _random = new Random();

        private Texture2D _texture;
        private Texture2D _bulletTexture;
        private Texture2D _enemyTexture;
        private Texture2D _collisionSplashTexture;
        private Texture2D _shrubberyTexture;
        private Texture2D _explosionTexture;
        private SpriteFont _font;

        private Point _cameraPosition;

        private Vector2 _facingDirection;
        private Point _moveDirection;
        private Point _playerPosition;
        private List<int> _gunAngles = new List<int>();
        private bool _firing;
        private List<BulletStruct> _bullets = new List<BulletStruct>();
        private int _playerXp;
        private int _playerLevel = 1;

        private List<EnemyStruct> _enemies = new List<EnemyStruct>();
        private List<CollisionSplashStruct> _collisionSplashes = new List<CollisionSplashStruct>();

        private bool _triggerPowerUpText;
        private int _powerUpCounter;
        
        private List<Point> _shrubbery = new List<Point>();
        
        private List<ExplosionStruct> _explosions = new List<ExplosionStruct>();

        public MonogameDemoGame()
        {
            InitializeMonogame();

            InitializeCamera();

            SpawnPlayer();
            SpawnEnemies();
            SpawnShrubbery();
        }

        private void SpawnPlayer()
        {
            _facingDirection = new Vector2(0f, 1f);
            _moveDirection = new Point();
            _playerPosition = new Point(WidthMidpoint, HeightMidpoint);
            _gunAngles.Add(0);
        }

        private void InitializeCamera()
        {
            _cameraPosition = new Point(WidthMidpoint, HeightMidpoint);
        }

        private void InitializeMonogame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            
            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        private void SpawnShrubbery()
        {
            var random = new Random(200); //I want the exact same seed
            for (int i = 0; i < EnemiesToSpawn; i++)
            {
                _shrubbery.Add(new Point(random.Next(-GameBorder, GameBorder), random.Next(-GameBorder, GameBorder)));
            }
        }

        private void SpawnEnemies()
        {
            var random = new Random(100);  //I want the exact same seed, not sure why honestly.
            foreach (var i in Enumerable.Range(1, 250))
            {
                int x = random.Next(-1, 2);
                int y = 0;
                if (x == 0)
                {
                    y = random.Next(0, 2);
                    if (y == 0)
                        y = -1;
                }

                _enemies.Add(new EnemyStruct()
                {
                    Position = new Vector2(random.Next(-GameBorder, GameBorder), random.Next(-GameBorder, GameBorder)),
                    Direction = new Vector2(x, y),
                    State = EnemyState.DoingNothing,
                    TicksUntilDone = 600,
                    Health = 100
                });
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadFont();
            LoadTexturesFromFile();
            LoadTexturesFromArray();
        }

        private void LoadFont()
        {
            _font = LoadFontByName("Font");
        }

        private void LoadTexturesFromFile()
        {
            _texture = LoadTextureFromFile("a.png");
            _enemyTexture = LoadTextureFromFile("b.png");
            _shrubberyTexture = LoadTextureFromFile("shrubbery.png");
        }

        private void LoadTexturesFromArray()
        {
            _bulletTexture = CreateSquareTexture(Color.Magenta, 4);
            _collisionSplashTexture = CreateSquareTexture(Color.Red, 3);
            _explosionTexture = CreateSquareTexture(Color.Red, 8);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            UnloadTextures();
        }

        private void UnloadTextures()
        {
            _texture.Dispose();
            _enemyTexture.Dispose();
            _bulletTexture.Dispose();
            _collisionSplashTexture.Dispose();
            _shrubberyTexture.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboardInput = ProcessKeyboardInput();
            var mouseInput = ProcessMouseInput();

            _moveDirection = keyboardInput.MoveDirection;
            _firing = mouseInput.IsFiring;
            _facingDirection = mouseInput.PlayerFacingDirection;

            MovePlayer();
            MoveCamera();

            UpdateEnemies();
            UpdateBullets();

            DetectCollisions();
            KillEnemies();
            UpdateSplashes();
            CheckLevel();
            UpdateExplosions();

            base.Update(gameTime);
        }

        private void MoveCamera()
        {
            int x2 = _cameraPosition.X;
            int y2 = _cameraPosition.Y;
            if (_cameraPosition.X - _playerPosition.X > NoFlexZone)
            {
                x2 += _moveDirection.X;
            }

            if (_cameraPosition.X - _playerPosition.X < -NoFlexZone)
            {
                x2 += _moveDirection.X;
            }

            if (_cameraPosition.Y - _playerPosition.Y > NoFlexZone)
            {
                y2 += _moveDirection.Y;
            }

            if (_cameraPosition.Y - _playerPosition.Y < -NoFlexZone)
            {
                y2 += _moveDirection.Y;
            }
            _cameraPosition = new Point(x2, y2);
        }

        private void MovePlayer()
        {
            _playerPosition = _playerPosition + _moveDirection;
        }

        private MouseInputStruct ProcessMouseInput()
        {
            var mouseState = Mouse.GetState();
            
            var input = new MouseInputStruct();

            input.IsFiring = mouseState.LeftButton == ButtonState.Pressed;

            var x = FitToScreen(mouseState.Position.X, ScreenWidth);
            var y = FitToScreen(mouseState.Position.Y, ScreenHeight);
            var mouse = new Point(x, y);

            var direction = (mouse - Midpoint - _playerPosition + _cameraPosition).ToVector2();
            input.PlayerFacingDirection = ShrinkVectorTo1Magnitude(direction);
            
            return input;
        }

        private KeyboardInputStruct ProcessKeyboardInput()
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

        private static bool IsMovingUp(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W);
        }

        private static bool IsMovingDown(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S);
        }

        private static bool IsMovingLeft(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A);
        }

        private static bool IsMovingRight(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D);
        }

        private void UpdateExplosions()
        {
            foreach (var explosion in _explosions.ToArray())
            {
                explosion.Ticks++;
                if (explosion.Ticks > 120)
                {
                    _explosions.Remove(explosion);
                }
            }
        }

        private void CheckLevel()
        {
            UpdatePowerUpText();
            if (ShouldLevelUp())
            {
                LevelUp();
                ShowPowerUpText();
            }
        }

        private void ShowPowerUpText()
        {
            _triggerPowerUpText = true;
            _powerUpCounter = 90;
        }

        private void LevelUp()
        {
            _playerLevel++;
            _gunAngles.Add((int) Math.Sqrt(_random.Next(2, 250)));
        }

        private void UpdatePowerUpText()
        {
            if (_triggerPowerUpText)
            {
                _powerUpCounter--;
                if (_powerUpCounter <= 0)
                {
                    _triggerPowerUpText = false;
                }
            }
        }

        private bool ShouldLevelUp()
        {
            return (_playerLevel * _playerLevel + 1) / 3 < _playerXp;
        }

        private void UpdateSplashes()
        {
            foreach (var splash in _collisionSplashes.ToArray())
            {
                splash.SplashCounter++;
                if (splash.SplashCounter > 10)
                    _collisionSplashes.Remove(splash);
            }
        }

        private void KillEnemies()
        {
            foreach (var enemy in _enemies.ToArray())
            {
                if (enemy.Health <= 0)
                {
                    KillEnemy(enemy);
                    CreateExplosion(enemy);
                    AwardPlayerExperience();
                }
            }
        }

        private void KillEnemy(EnemyStruct enemy)
        {
            _enemies.Remove(enemy);
        }

        private void CreateExplosion(EnemyStruct enemy)
        {
            var explosionStruct = new ExplosionStruct() {Position = enemy.Position, Ticks = 0};
            explosionStruct.Fragments = new List<Vector2>();
            for (int i = 0; i < 36; i++)
            {
                explosionStruct.Fragments.Add(new Vector2(1, 0).Rotate(_random.Next(0, 360))*_random.Next(0, 10));
            }
            _explosions.Add(explosionStruct);
        }

        private void AwardPlayerExperience()
        {
            _playerXp++;
        }

        private void DetectCollisions()
        {
            foreach (var bullet in _bullets.ToArray())
                foreach (var enemy in _enemies)
                    if (Collides(bullet.Position, 2, enemy.Position, 16))
                        Collide(bullet, enemy);
        }

        private void Collide(BulletStruct bullet, EnemyStruct enemy)
        {
            DestroyBullet(bullet);
            HurtEnemy(enemy);
            CreateSplashEffect(bullet);
        }

        private void DestroyBullet(BulletStruct bullet)
        {
            _bullets.Remove(bullet);
        }

        private static void HurtEnemy(EnemyStruct enemy)
        {
            enemy.Health--;
        }

        private void CreateSplashEffect(BulletStruct bullet)
        {
            _collisionSplashes.Add(new CollisionSplashStruct()
            {
                Position = bullet.Position,
                Direction = new Vector2() - bullet.Direction,
                SplashCounter = 0
            });
        }

        private bool Collides(Vector2 position1, int radius1, Vector2 position2, int radius2)
        {
            //collision horizontally
            if ((position1.X + radius1 > position2.X - radius2 && position1.X + radius1 < position2.X + radius2)
                || (position1.X - radius1 < position2.X + radius2 && position1.X - radius1 > position2.X - radius2))
            {
                //collision vertically
                if ((position1.Y + radius1 > position2.Y - radius2 && position1.Y + radius1 < position2.Y + radius2)
                    || (position1.Y - radius1 < position2.Y + radius2 && position1.Y - radius1 > position2.Y - radius2))
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateBullets()
        {
            MoveBullets();
            DeleteBullets();

            if (_firing)
                CreateBullets();
        }

        private void MoveBullets()
        {
            _bullets.ForEach(p => { p.Position = new Vector2(p.Position.X + p.Direction.X, p.Position.Y + p.Direction.Y); });
        }

        private void DeleteBullets()
        {
            var bulletsToDelete =
                _bullets.Where(x => Math.Abs(x.Position.X) > GameBorder || Math.Abs(x.Position.Y) > GameBorder)
                    .ToArray();
            foreach (var bulletToDelte in bulletsToDelete)
                _bullets.Remove(bulletToDelte);
        }

        private void CreateBullets()
        {
            var xDelta = _facingDirection.X*10f;
            var yDelta = _facingDirection.Y*10f;
            foreach (var gunAngle in _gunAngles)
            {
                var angle = (int) Math.Sqrt(_random.Next(0, 2*2*gunAngle*gunAngle)) - gunAngle;
                var direction = new Vector2(xDelta, yDelta).Rotate(angle);

                var bullet = new BulletStruct()
                {
                    Position = new Vector2(_playerPosition.X + 16*_facingDirection.X, _playerPosition.Y + 16*_facingDirection.Y),
                    Direction = direction
                };

                _bullets.Add(bullet);
            }
        }

        private void UpdateEnemies()
        {
            foreach (var enemy in _enemies)
                UpdateEnemy(enemy);
        }

        private static void UpdateEnemy(EnemyStruct enemy)
        {
            enemy.TicksUntilDone--;
            if (enemy.State == EnemyState.DoingNothing)
            {
                //do nothing

                if (enemy.TicksUntilDone == 0)
                    ChangeStateToMoving(enemy);
            }
            else if (enemy.State == EnemyState.Moving)
            {
                MoveEnemy(enemy);

                if (enemy.TicksUntilDone == 0)
                    ChangeStateToTurning(enemy);
            }
            else if (enemy.State == EnemyState.Turning)
            {
                TurnEnemy(enemy);
                if (enemy.TicksUntilDone == 0)
                    ChangeStateToDoingNothing(enemy);
            }
        }

        private static void TurnEnemy(EnemyStruct enemy)
        {
            enemy.Direction = enemy.Direction.Rotate(1);
        }

        private static void MoveEnemy(EnemyStruct enemy)
        {
            enemy.Position = enemy.Position + enemy.Direction;
        }

        private static void ChangeStateToDoingNothing(EnemyStruct enemy)
        {
            ChangeEnemyState(enemy, EnemyState.DoingNothing, 60);
        }

        private static void ChangeStateToTurning(EnemyStruct enemy)
        {
            ChangeEnemyState(enemy, EnemyState.Turning, 90);
        }

        private static void ChangeStateToMoving(EnemyStruct enemy)
        {
            ChangeEnemyState(enemy, EnemyState.Moving, 240);
        }

        private static void ChangeEnemyState(EnemyStruct enemy, EnemyState newState, int ticksUntilDone)
        {
            enemy.State = newState;
            enemy.TicksUntilDone = ticksUntilDone;
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            InitializeFrame();

            DrawShrubbery();
            DrawExplosions();
            DrawPlayer();
            DrawBullets();
            DrawEnemies();
            DrawSplashes();
            if (_triggerPowerUpText)
            {
                DrawPowerUpText();
            }

            EndFrame(gameTime);
        }

        private void InitializeFrame()
        {
            GraphicsDevice.Clear(Color.White);

            //http://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/
            var transform = Matrix.CreateTranslation(new Vector3(-_cameraPosition.X, -_cameraPosition.Y, 0))*
                            Matrix.CreateRotationZ(0)*
                            Matrix.CreateScale(new Vector3(1, 1, 1))*
                            Matrix.CreateTranslation(new Vector3(WidthMidpoint, HeightMidpoint, 0));
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);
        }

        private void EndFrame(GameTime gameTime)
        {
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawPlayer()
        {
            DrawEntityWithRotation(_texture, new Vector2(_playerPosition.X, _playerPosition.Y), _facingDirection);
        }

        private void DrawExplosions()
        {
            foreach (var explosion in _explosions)
                foreach (var fragment in explosion.Fragments)
                    DrawEntity(_explosionTexture, explosion.Position + fragment * explosion.Ticks);
        }

        private void DrawShrubbery()
        {
            foreach (var shrub in _shrubbery)
                DrawEntity(_shrubberyTexture, shrub.ToVector2());
        }

        private void DrawPowerUpText()
        {
            _spriteBatch.DrawString(_font, "POWER UP", (_playerPosition - new Point(50, -20)).ToVector2(), Color.Black);
        }

        private void DrawSplashes()
        {
            foreach (var splash in _collisionSplashes)
            {
                var directions = new List<int>();
                for (int i = 0; i < 3; i++)
                {
                    var randomNumber = _random.Next(-12, 12);

                    //like squaring, but keeping the negative-ness of the original number
                    directions.Add((int)randomNumber * Math.Abs(randomNumber));
                }

                foreach (var direction in directions)
                {
                    var particlePosition = splash.Position + (splash.Direction * splash.SplashCounter).Rotate(direction);
                    DrawEntity(_collisionSplashTexture, particlePosition);
                }
            }
        }

        private void DrawEnemies()
        {
            foreach (var enemy in _enemies)
            {
                DrawEntityWithRotation(_enemyTexture, enemy.Position, enemy.Direction);
            }
        }

        private void DrawBullets()
        {
            _bullets.ForEach(x => DrawEntity(_bulletTexture, new Vector2(x.Position.X - 1.5f, x.Position.Y - 1.5f)));
        }

        private void DrawEntity(Texture2D texture, Vector2 position)
        {
            _spriteBatch.Draw(texture, position);
        }

        private void DrawEntityWithRotation(Texture2D texture, Vector2 position, Vector2 direction)
        {
            _spriteBatch.Draw(texture, position, new Rectangle(0, 0, 32, 32),
                new Color(Color.White, 1f), ConvertToAngleInRadians(direction), new Vector2(16f, 16f), 1.0f, SpriteEffects.None, 1);
        }

        private float ConvertToAngleInRadians(Vector2 direction)
        {
            return (float)Math.Atan2(direction.Y, direction.X);
        }

        private SpriteFont LoadFontByName(string fontName)
        {
            return Content.Load<SpriteFont>(fontName);
        }

        private Texture2D LoadTextureFromFile(string fileName)
        {
            return Content.Load<Texture2D>(fileName);
        }

        private Texture2D CreateSquareTexture(Color color, int size)
        {
            var texture = new Texture2D(GraphicsDevice, size, size);

            texture.SetData(
                Enumerable.Range(0, size * size)
                .Select(cell => color)
                .ToArray());

            return texture;
        }

        private Vector2 ShrinkVectorTo1Magnitude(Vector2 vector)
        {
            var magnitude = 1f / (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            return  vector * magnitude;
        }

        private static int FitToScreen(int cursorPosition, int boundary)
        {
            return Math.Max(Math.Min(cursorPosition, boundary), -boundary);
        }
    }

    internal class KeyboardInputStruct
    {
        public Point MoveDirection { get; set; }
    }

    internal class MouseInputStruct
    {
        public bool IsFiring { get; set; }
        public Vector2 PlayerFacingDirection { get; set; }
    }

    internal class CollisionSplashStruct
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public int SplashCounter { get; set; }
    }

    internal class EnemyStruct
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public int TicksUntilDone { get; set; }

        public EnemyState State { get; set; }
        public int Health { get; set; }
    }

    internal enum EnemyState
    {
        DoingNothing,
        Moving,
        Turning
    }

    public class BulletStruct
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
    }

    public class ExplosionStruct
    {
        public Vector2 Position { get; set; }
        public int Ticks { get; set; }
        public List<Vector2> Fragments { get; set; }
    }

    public static class Vector2ExtensionMethods
    {
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float Deg2Rad = ((float)(2 * Math.PI)/ 360f);
            float sin = (float)Math.Sin(degrees * Deg2Rad);
            float cos = (float)Math.Cos(degrees * Deg2Rad);

            float tx = v.X;
            float ty = v.Y;
            v.X = (cos * tx) - (sin * ty);
            v.Y = (sin * tx) + (cos * ty);
            return v;
        }
    }
}
