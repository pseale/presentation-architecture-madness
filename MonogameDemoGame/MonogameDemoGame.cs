using System;
using System.Collections.Generic;
using System.Linq;
using MonogameDemoGame.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame
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
        private const int RandomSeedForShrubbery = 200;
        private const int RandomSeedForEnemies = 100;
        private const int NumberOfEnemiesToSpawn = 250;
        private const int TicksToWaitAtBeginning = 600;
        private const int EnemyHealth = 100;
        private const int BulletSize = 4;
        private const int EnemySize = 32;
        private const int CollisionSplashSize = 3;
        private const int ExplosionFragmentSize = 8;
        private const int ExplosionTicks = 120;
        private const int PowerUpTicks = 90;
        private const int CollisionSplashTicks = 10;
        private const int CollisionFragmentMaxSpeed = 10;
        private const int FragmentsPerExplosion = 36;
        private const float BulletSpeed = 10f;
        private const int PlayerSize = 32;
        private const int HalfPlayerSize = PlayerSize / 2;
        private const int EnemyTicksToDoNothing = 60;
        private const int EnemyTicksToTurn = 90;
        private const int EnemyTicksToMove = 240;
        private const string PowerUpText = "POWER UP";
        private const int NumberOfCollisionSplashParticlesToCreate = 3;
        private const int MaximumSqrtOfAngleToThrowCollisionSplashParticleInDegrees = 12;
        private readonly Color BackgroundColor = Color.White;
        private readonly Color PowerUpTextColor = Color.Black;

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
            var random = new Random(RandomSeedForShrubbery); //I want the exact same seed
            for (int i = 0; i < EnemiesToSpawn; i++)
            {
                _shrubbery.Add(BoundaryHelper.CreatePointInBoundary(random, GameBorder));
            }
        }

        private void SpawnEnemies()
        {
            var random = new Random(RandomSeedForEnemies);  //I want the exact same seed, not sure why honestly.
            foreach (var i in Enumerable.Range(1, NumberOfEnemiesToSpawn))
            {
                _enemies.Add(new EnemyStruct()
                {
                    Position = BoundaryHelper.CreatePointInBoundary(random, GameBorder).ToVector2(),
                    Direction = GenerateEnemyDirection(random),
                    State = EnemyState.DoingNothing,
                    TicksUntilDone = TicksToWaitAtBeginning,
                    Health = EnemyHealth
                });
            }
        }

        private Vector2 GenerateEnemyDirection(Random random)
        {
            int x = 0;
            int y = 0;
            if (RandomHelper.GetRandomBool(random))
                x = RandomHelper.GenerateRandomNegativeOrPositiveOne(random);
            else
                y = RandomHelper.GenerateRandomNegativeOrPositiveOne(random);

            var direction = new Vector2(x, y);
            return direction;
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
            _font = ContentHelper.LoadFontByName(Content, "Font");
        }

        private void LoadTexturesFromFile()
        {
            _texture = ContentHelper.LoadTextureFromFile(Content, "a.png");
            _enemyTexture = ContentHelper.LoadTextureFromFile(Content, "b.png");
            _shrubberyTexture = ContentHelper.LoadTextureFromFile(Content, "shrubbery.png");
        }

        private void LoadTexturesFromArray()
        {
            _bulletTexture = ContentHelper.CreateSquareTexture(GraphicsDevice, Color.Magenta, BulletSize);
            _collisionSplashTexture = ContentHelper.CreateSquareTexture(GraphicsDevice, Color.Red, CollisionSplashSize);
            _explosionTexture = ContentHelper.CreateSquareTexture(GraphicsDevice, Color.Red, ExplosionFragmentSize);
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
            if (InputHelper.UserIsTryingToExit())
                Exit();

            var keyboardInput = InputHelper.ProcessKeyboardInput();
            var mouseInput = InputHelper.ProcessMouseInput(Midpoint, _playerPosition, _cameraPosition, ScreenWidth, ScreenHeight);

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
        
            if (CameraHelper.IsOutsideOfFlexZone(_cameraPosition.X - _playerPosition.X, NoFlexZone))
                x2 += _moveDirection.X;

            if (CameraHelper.IsOutsideOfFlexZone(_cameraPosition.Y - _playerPosition.Y, NoFlexZone))
                y2 += _moveDirection.Y;

            _cameraPosition = new Point(x2, y2);
        }

        private void MovePlayer()
        {
            _playerPosition = _playerPosition + _moveDirection;
        }

        private void UpdateExplosions()
        {
            foreach (var explosion in _explosions.ToArray())
            {
                explosion.Ticks++;
                if (explosion.Ticks > ExplosionTicks)
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
            _powerUpCounter = PowerUpTicks;
        }

        private void LevelUp()
        {
            _playerLevel++;
            _gunAngles.Add((int) (2 + RandomHelper.GenerateRandomNumberClusteredTowardZero(_random, 15)));
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
                if (splash.SplashCounter > CollisionSplashTicks)
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
            for (int i = 0; i < FragmentsPerExplosion; i++)
            {
                explosionStruct.Fragments.Add(new Vector2(1, 0).Rotate(RandomHelper.NextRandomNumber(_random, 360)) * RandomHelper.NextRandomNumber(_random, CollisionFragmentMaxSpeed));
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
                    if (PhysicsHelper.Collides(bullet.Position, BulletSize / 2, enemy.Position, EnemySize / 2))
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

        private  void HurtEnemy(EnemyStruct enemy)
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
                _bullets.Where(x => BoundaryHelper.WithinBoundary(x.Position.X, GameBorder) || BoundaryHelper.WithinBoundary(x.Position.Y, GameBorder))
                    .ToArray();
            foreach (var bulletToDelte in bulletsToDelete)
                _bullets.Remove(bulletToDelte);
        }

        private void CreateBullets()
        {
            var xDelta = _facingDirection.X * BulletSpeed;
            var yDelta = _facingDirection.Y * BulletSpeed;
            foreach (var gunAngle in _gunAngles)
            {
                var angle = (int) RandomHelper.GenerateRandomNumberClusteredTowardZero(_random, gunAngle);
                if (RandomHelper.GetRandomBool(_random))
                    angle = -angle;

                var direction = new Vector2(xDelta, yDelta).Rotate(angle);

                var bullet = new BulletStruct()
                {
                    Position = new Vector2(_playerPosition.X + HalfPlayerSize * _facingDirection.X, _playerPosition.Y + HalfPlayerSize * _facingDirection.Y),
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

        private  void UpdateEnemy(EnemyStruct enemy)
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

        private  void TurnEnemy(EnemyStruct enemy)
        {
            enemy.Direction = enemy.Direction.Rotate(1);
        }

        private  void MoveEnemy(EnemyStruct enemy)
        {
            enemy.Position = enemy.Position + enemy.Direction;
        }

        private  void ChangeStateToDoingNothing(EnemyStruct enemy)
        {
            ChangeEnemyState(enemy, EnemyState.DoingNothing, EnemyTicksToDoNothing);
        }

        private  void ChangeStateToTurning(EnemyStruct enemy)
        {
            ChangeEnemyState(enemy, EnemyState.Turning, EnemyTicksToTurn);
        }

        private  void ChangeStateToMoving(EnemyStruct enemy)
        {
            ChangeEnemyState(enemy, EnemyState.Moving, EnemyTicksToMove);
        }

        private  void ChangeEnemyState(EnemyStruct enemy, EnemyState newState, int ticksUntilDone)
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
            DrawHelper.InitializeFrame(_spriteBatch, GraphicsDevice, _cameraPosition, WidthMidpoint, HeightMidpoint, BackgroundColor);

            DrawShrubbery();
            DrawExplosions();
            DrawPlayer();
            DrawBullets(_spriteBatch, _bullets);
            DrawEnemies();
            DrawSplashes();
            if (_triggerPowerUpText)
            {
                DrawPowerUpText();
            }

            DrawHelper.EndFrame(_spriteBatch, () => base.Draw(gameTime));
        }

        private void DrawPlayer()
        {
            DrawHelper.DrawEntityWithRotation(_spriteBatch, _texture, new Vector2(_playerPosition.X, _playerPosition.Y), _facingDirection, PlayerSize, HalfPlayerSize);
        }

        private void DrawExplosions()
        {
            foreach (var explosion in _explosions)
                foreach (var fragment in explosion.Fragments)
                    DrawHelper.DrawEntity(_spriteBatch, _explosionTexture, explosion.Position + fragment * explosion.Ticks);
        }

        private void DrawShrubbery()
        {
            foreach (var shrub in _shrubbery)
                DrawHelper.DrawEntity(_spriteBatch, _shrubberyTexture, shrub.ToVector2());
        }

        private void DrawPowerUpText()
        {
            _spriteBatch.DrawString(_font, PowerUpText, (_playerPosition - new Point(50, -20)).ToVector2(), PowerUpTextColor);
        }

        private void DrawSplashes()
        {
            foreach (var splash in _collisionSplashes)
            {
                var directions = new List<int>();
                for (int i = 0; i < NumberOfCollisionSplashParticlesToCreate; i++)
                {
                    int randomNumber = RandomHelper.NextRandomNumberBetweenPositiveAndNegative(_random, MaximumSqrtOfAngleToThrowCollisionSplashParticleInDegrees);

                    //like squaring, but keeping the negative-ness of the original number
                    directions.Add(randomNumber * Math.Abs(randomNumber));
                }

                foreach (var direction in directions)
                {
                    var particlePosition = splash.Position + (splash.Direction * splash.SplashCounter).Rotate(direction);
                    DrawHelper.DrawEntity(_spriteBatch, _collisionSplashTexture, particlePosition);
                }
            }
        }

        private void DrawEnemies()
        {
            foreach (var enemy in _enemies)
            {
                DrawHelper.DrawEntityWithRotation(_spriteBatch, _enemyTexture, enemy.Position, enemy.Direction, PlayerSize, HalfPlayerSize);
            }
        }

        private void DrawBullets(SpriteBatch spriteBatch, IEnumerable<BulletStruct> bullets)
        {
            foreach (var bullet in bullets) 
                DrawHelper.DrawEntity(spriteBatch, _bulletTexture, new Vector2(bullet.Position.X - BulletSize/2, bullet.Position.Y - BulletSize/2));
        }
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
