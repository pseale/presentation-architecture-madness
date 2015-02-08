using System.Collections.Generic;
using System.Linq;
using MonogameDemoGame.Core;
using MonogameDemoGame.Core.Domain;
using MonogameDemoGame.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameDemoGame.Services;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame
{
    public class ProgramController : Game
    {
        private const int ScreenWidth = 640;
        private const int ScreenHeight = 480;
        private const int WidthMidpoint = ScreenWidth / 2;
        private const int HeightMidpoint = ScreenHeight / 2;
        private readonly Point Midpoint = new Point(WidthMidpoint, HeightMidpoint);
        private const int BulletSize = 4;
        private const int EnemySize = 32;
        private const int CollisionSplashSize = 3;
        private const int ExplosionFragmentSize = 8;
        private const int PlayerSize = 32;
        private const int HalfPlayerSize = PlayerSize / 2;
        private const string PowerUpText = "POWER UP";
        private const int NumberOfCollisionSplashParticlesToCreate = 3;
        private const int MaximumSqrtOfAngleToThrowCollisionSplashParticleInDegrees = 12;
        private readonly Color BackgroundColor = Color.White;
        private readonly Color PowerUpTextColor = Color.Black;

        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private IRandomNumberService _randomNumberService;

        private Texture2D _texture;
        private Texture2D _bulletTexture;
        private Texture2D _enemyTexture;
        private Texture2D _collisionSplashTexture;
        private Texture2D _shrubberyTexture;
        private Texture2D _explosionTexture;
        private SpriteFont _font;

        
        private IBoundaryService _boundaryService;
        private IContentService _contentService;
        private IInputService _inputService;
        private IDrawService _drawService;

        private LineOfBusinessApplication _lob;

        public ProgramController()
        {
            InitializeMonogame();
        }

        private void InitializeMonogame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            
            IsMouseVisible = true;

            Content.RootDirectory = "Content";
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

            _randomNumberService = new RandomNumberService();
            _boundaryService = new BoundaryService(new RandomNumberService());
            _contentService = new ContentService(GraphicsDevice, Content);
            _inputService = new InputService();
            _drawService = new DrawService(_spriteBatch, GraphicsDevice);

            _lob = new LineOfBusinessApplication(_boundaryService, _randomNumberService);

            LoadFont();
            LoadTexturesFromFile();
            LoadTexturesFromArray();
        }

        private void LoadFont()
        {
            _font = _contentService.LoadFontByName("Font");
        }

        private void LoadTexturesFromFile()
        {
            _texture = _contentService.LoadTextureFromFile("a.png");
            _enemyTexture = _contentService.LoadTextureFromFile("b.png");
            _shrubberyTexture = _contentService.LoadTextureFromFile("shrubbery.png");
        }

        private void LoadTexturesFromArray()
        {
            _bulletTexture = _contentService.CreateSquareTexture(Color.Magenta, BulletSize);
            _collisionSplashTexture = _contentService.CreateSquareTexture(Color.Red, CollisionSplashSize);
            _explosionTexture = _contentService.CreateSquareTexture(Color.Red, ExplosionFragmentSize);
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
            if (_inputService.UserIsTryingToExit())
                Exit();

            var input = _inputService.ProcessInput(Midpoint, _lob.GetPlayerPosition(), _lob.GetCameraPosition());

            _lob.Update(input);
            base.Update(gameTime);
        }




        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _drawService.InitializeFrame(_lob.GetCameraPosition(), WidthMidpoint, HeightMidpoint, BackgroundColor);

            DrawShrubbery();
            DrawExplosions();
            DrawPlayer();
            DrawBullets(_lob.GetBullets());
            EnemyHelper.DrawEnemies(_drawService, _lob.GetEnemies(), _enemyTexture, PlayerSize, HalfPlayerSize);
            DrawSplashes();
            if (_lob.ShouldTriggerPowerUpText())
            {
                DrawPowerUpText();
            }

            _drawService.EndFrame(() => base.Draw(gameTime));
        }

        private void DrawPlayer()
        {
            _drawService.DrawEntityWithRotation(_texture, _lob.GetPlayerPosition().ToVector2(), _lob.GetPlayerFacingDirection(), PlayerSize, HalfPlayerSize);
        }

        private void DrawExplosions()
        {
            foreach (var explosion in _lob.GetExplosions())
                foreach (var fragment in explosion.Fragments)
                    _drawService.DrawEntity(_explosionTexture, explosion.Position + fragment.Position * explosion.Ticks);
        }

        private void DrawShrubbery()
        {
            foreach (var shrub in _lob.GetShrubbery())
                _drawService.DrawEntity(_shrubberyTexture, shrub.Position.ToVector2());
        }

        private void DrawPowerUpText()
        {
            _spriteBatch.DrawString(_font, PowerUpText, PowerUpHelper.CalculateTextPosition(_lob.GetPlayerPosition()), PowerUpTextColor);
        }

        private void DrawSplashes()
        {
            foreach (var item in BulletSplashHelper.Spawn(_randomNumberService,_lob.GetCollisionSplashes(), NumberOfCollisionSplashParticlesToCreate, MaximumSqrtOfAngleToThrowCollisionSplashParticleInDegrees))
                _drawService.DrawEntity(_collisionSplashTexture, item);
        }

        private void DrawBullets(IEnumerable<Bullet> bullets)
        {
            foreach (var bullet in bullets) 
                _drawService.DrawEntity(_bulletTexture, new Vector2(bullet.Position.X - BulletSize/2, bullet.Position.Y - BulletSize/2));
        }
    }
}
