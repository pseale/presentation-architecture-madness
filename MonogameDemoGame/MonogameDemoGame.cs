using System;
using System.Collections.Generic;
using System.Linq;
using MonogameDemoGame.Core;
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
            _cameraPosition = CameraHelper.Spawn(Midpoint);
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
                _shrubbery.Add(ShrubberyHelper.Spawn(random, GameBorder));
            }
        }

        private void SpawnEnemies()
        {
            _enemies.AddRange(EnemyHelper.SpawnEnemies(RandomSeedForEnemies, NumberOfEnemiesToSpawn, GameBorder, TicksToWaitAtBeginning, EnemyHealth));
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
            KillEnemies(_enemies);
            UpdateSplashes();
            CheckLevel();
            UpdateExplosions();

            base.Update(gameTime);
        }

        private void MoveCamera()
        {
            _cameraPosition = CameraHelper.CalculateNewPosition(_cameraPosition, _playerPosition, _moveDirection, NoFlexZone);
        }

        private void MovePlayer()
        {
            _playerPosition = _playerPosition + _moveDirection;
        }

        private void UpdateExplosions()
        {
            foreach (var explosion in _explosions.ToArray())
            {
                var result = ExplosionHelper.Update(explosion, ExplosionTicks);
                if (result == ExplosionUpdateResult.Remove)
                    _explosions.Remove(explosion);
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

        private void KillEnemies(List<EnemyStruct> enemies)
        {
            foreach (var enemy in enemies.ToArray())
            {
                if (EnemyHelper.HasNoHealth(enemy))
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
            var explosion = ExplosionHelper.Spawn(_random, enemy, FragmentsPerExplosion, CollisionFragmentMaxSpeed);
            _explosions.Add(explosion);
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
            EnemyHelper.HurtEnemy(enemy);
            CreateSplashEffect(bullet);
        }

        private void DestroyBullet(BulletStruct bullet)
        {
            _bullets.Remove(bullet);
        }

        private void CreateSplashEffect(BulletStruct bullet)
        {
            _collisionSplashes.Add(CollisionHelper.Spawn(bullet));
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
            _bullets.ForEach(p => BulletHelper.Move(p));
        }

        private void DeleteBullets()
        {
            var bulletsToDelete = _bullets
                .Where(x => BulletHelper.ShouldBeDeleted(x, GameBorder))
                .ToArray();
            foreach (var bulletToDelte in bulletsToDelete)
                _bullets.Remove(bulletToDelte);
        }

        private void CreateBullets()
        {
            _bullets.AddRange(BulletHelper.Spawn(_random, _gunAngles, _facingDirection, _playerPosition, BulletSpeed, HalfPlayerSize));
        }

        private void UpdateEnemies()
        {
            foreach (var enemy in _enemies)
            {
                EnemyHelper.Update(enemy, EnemyTicksToDoNothing, EnemyTicksToTurn, EnemyTicksToMove);
            }
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
            EnemyHelper.DrawEnemies(_spriteBatch, _enemies, _enemyTexture, PlayerSize, HalfPlayerSize);
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
            _spriteBatch.DrawString(_font, PowerUpText, PowerUpHelper.CalculateTextPosition(_playerPosition), PowerUpTextColor);
        }

        private void DrawSplashes()
        {
            foreach (var item in BulletSplashHelper.Spawn(_random, _collisionSplashes, NumberOfCollisionSplashParticlesToCreate, MaximumSqrtOfAngleToThrowCollisionSplashParticleInDegrees))
                DrawHelper.DrawEntity(_spriteBatch, _collisionSplashTexture, item);
        }

        private void DrawBullets(SpriteBatch spriteBatch, IEnumerable<BulletStruct> bullets)
        {
            foreach (var bullet in bullets) 
                DrawHelper.DrawEntity(spriteBatch, _bulletTexture, new Vector2(bullet.Position.X - BulletSize/2, bullet.Position.Y - BulletSize/2));
        }
    }
}
