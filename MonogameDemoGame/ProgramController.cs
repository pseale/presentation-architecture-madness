using System.Collections.Generic;
using MonogameDemoGame.Core.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameDemoGame.Core.Domain.Spawning;
using MonogameDemoGame.Core.Drawing;
using MonogameDemoGame.Services;

namespace MonogameDemoGame
{
    public class ProgramController : Game
    {
        private const int GameBorder = 2000;
        private const int NumberOfShrubbery = 500;
        private const int NumberOfEnemies = 250;
        private const int ScreenWidth = 640;
        private const int ScreenHeight = 480;
        private const int WidthMidpoint = ScreenWidth / 2;
        private const int HeightMidpoint = ScreenHeight / 2;
        private readonly Point Midpoint = new Point(WidthMidpoint, HeightMidpoint);
        private const int BulletSize = 4;
        private const int CollisionSplashSize = 3;
        private const int ExplosionFragmentSize = 8;
        private readonly Color BackgroundColor = Color.White;
        private Dictionary<EntityType, Texture2D> _entityTextures = new Dictionary<EntityType, Texture2D>();
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private Texture2D _texture;
        private Texture2D _bulletTexture;
        private Texture2D _enemyTexture;
        private Texture2D _collisionSplashTexture;
        private Texture2D _shrubberyTexture;
        private Texture2D _explosionTexture;
        private SpriteFont _font;

        
        private IContentService _contentService;
        private IInputService _inputService;
        private IDrawService _drawService;

        private LobGame _lob;

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

            _contentService = new ContentService(GraphicsDevice, Content);
            _inputService = new InputService();
            _drawService = new DrawService(_spriteBatch, GraphicsDevice);

            var gameData = LobGameStarter.CreateInitialGameState(Midpoint, GameBorder, NumberOfShrubbery, NumberOfEnemies);
            _lob = new LobGame(gameData);

            LoadFont();
            LoadTexturesFromFile();
            LoadTexturesFromArray();
            BuildTextureDictionary();
        }

        private void BuildTextureDictionary()
        {
            _entityTextures.Add(EntityType.Player, _texture);
            _entityTextures.Add(EntityType.Enemy, _enemyTexture);
            _entityTextures.Add(EntityType.Shrubbery, _shrubberyTexture);

            _entityTextures.Add(EntityType.Bullet, _bulletTexture);
            _entityTextures.Add(EntityType.ExplosionFragment, _explosionTexture);
            _entityTextures.Add(EntityType.Splash, _collisionSplashTexture);
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

            var input = _inputService.ProcessInput(_lob.GetPlayerPosition(), _lob.GetCameraPosition());

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

            var vm = ViewModelMapper.CreateViewModel(_lob);

            foreach (var entity in vm.Entities)
            {
                if (entity.HasRotation)
                    _drawService.DrawEntityWithRotation(_entityTextures[entity.Type], entity.Position, entity.Rotation, _entityTextures[entity.Type].Height, _entityTextures[entity.Type].Height/2);
                else
                    _drawService.DrawEntity(_entityTextures[entity.Type], entity.Position);
            }

            if (vm.Text.ShouldShowText)
                _spriteBatch.DrawString(_font, vm.Text.Text, vm.Text.Position, vm.Text.Color);

            _drawService.EndFrame(() => base.Draw(gameTime));
        }
    }
}
