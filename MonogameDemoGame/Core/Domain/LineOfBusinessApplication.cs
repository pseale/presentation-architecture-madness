using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Helpers;
using MonogameDemoGame.Services;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Core.Domain
{
    public class LineOfBusinessApplication
    {
        private readonly IBoundaryService _boundaryService;
        private readonly IRandomNumberService _randomNumberService;
        private const int EnemiesToSpawn = 400;
        private const int NumberOfEnemiesToSpawn = 250;
        private const int ScreenWidth = 640;
        private const int ScreenHeight = 480;
        private const int WidthMidpoint = ScreenWidth / 2;
        private const int HeightMidpoint = ScreenHeight / 2;
        private readonly Point Midpoint = new Point(WidthMidpoint, HeightMidpoint);
        private const int RandomSeedForShrubbery = 200;
        private const int RandomSeedForEnemies = 100;
        private const int PowerUpTicks = 90;
        private const int CollisionFragmentMaxSpeed = 10;
        private const int FragmentsPerExplosion = 36;
        private const float BulletSpeed = 10f;
        private const int BulletSize = 4;
        private const int EnemySize = 32;
        private const int PlayerSize = 32;
        private const int HalfPlayerSize = PlayerSize / 2;

        private Camera _camera;
        private Player _player;
        private List<Bullet> _bullets = new List<Bullet>();
        private List<Enemy> _enemies = new List<Enemy>();
        private List<CollisionSplash> _collisionSplashes = new List<CollisionSplash>();
        private List<Shrubbery> _shrubbery = new List<Shrubbery>();
        private List<Explosion> _explosions = new List<Explosion>();

        private bool _triggerPowerUpText;
        private int _powerUpCounter;

        public LineOfBusinessApplication(IBoundaryService boundaryService, IRandomNumberService randomNumberService)
        {
            //these services should not be here
            _boundaryService = boundaryService;
            _randomNumberService = randomNumberService;

            InitializeCamera();

            SpawnPlayer();
            SpawnEnemies();
            SpawnShrubbery();
        }

        private void SpawnPlayer()
        {
            _player = PlayerHelper.Spawn(Midpoint);
        }

        private void InitializeCamera()
        {
            _camera = CameraHelper.Spawn(new Point(0, 0));
        }

        private void SpawnShrubbery()
        {
            for (int i = 0; i < EnemiesToSpawn; i++)
            {
                _shrubbery.Add(ShrubberyHelper.Spawn(_boundaryService));
            }
        }

        private void SpawnEnemies()
        {
            _enemies.AddRange(EnemyHelper.SpawnEnemies(_boundaryService, RandomSeedForEnemies, NumberOfEnemiesToSpawn));
        }

        public Point GetPlayerPosition()
        {
            return _player.Position;
        }

        public Point GetCameraPosition()
        {
            return _camera.Position;
        }

        public void Update(InputStruct input)
        {

            _player.Update(_camera, input);

            MovePlayer();
            MoveCamera();

            UpdateEnemies();
            UpdateBullets();

            DetectCollisions();
            KillEnemies(_enemies);
            UpdateSplashes();
            CheckLevel();
            UpdateExplosions();

        }
        private void MoveCamera()
        {
            _camera.Move(_player);
        }

        private void MovePlayer()
        {
            _player.Move();
        }

        private void UpdateExplosions()
        {
            foreach (var explosion in _explosions.ToArray())
            {
                var result = explosion.Update();
                if (result == ExplosionUpdateResult.Remove)
                    _explosions.Remove(explosion);
            }
        }

        private void CheckLevel()
        {
            UpdatePowerUpText();
            var result = _player.TryLevelUp(_randomNumberService);

            if (result == LevelUpResult.LeveledUp)
                ShowPowerUpText();
        }


        private void UpdateSplashes()
        {
            foreach (var splash in _collisionSplashes.ToArray())
            {
                splash.Update();

                if (splash.ShouldBeDeleted())
                    _collisionSplashes.Remove(splash);
            }
        }

        private void KillEnemies(List<Enemy> enemies)
        {
            foreach (var enemy in enemies.ToArray())
            {
                if (enemy.HasNoHealth())
                {
                    KillEnemy(enemy);
                    CreateExplosion(enemy);
                    AwardPlayerExperience();
                }
            }
        }

        private void KillEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }

        private void CreateExplosion(Enemy enemy)
        {
            var explosion = ExplosionHelper.Spawn(_randomNumberService, enemy, FragmentsPerExplosion, CollisionFragmentMaxSpeed);
            _explosions.Add(explosion);
        }

        private void AwardPlayerExperience()
        {
            _player.AwardExperience();
        }

        private void DetectCollisions()
        {
            foreach (var bullet in _bullets.ToArray())
                foreach (var enemy in _enemies)
                    if (PhysicsHelper.Collides(bullet.Position, BulletSize / 2, enemy.Position, EnemySize / 2))
                        Collide(bullet, enemy);
        }

        private void Collide(Bullet bullet, Enemy enemy)
        {
            DestroyBullet(bullet);
            enemy.Hurt();
            CreateSplashEffect(bullet);
        }

        private void DestroyBullet(Bullet bullet)
        {
            _bullets.Remove(bullet);
        }

        private void CreateSplashEffect(Bullet bullet)
        {
            _collisionSplashes.Add(CollisionHelper.Spawn(bullet));
        }

        private void UpdateBullets()
        {
            MoveBullets();
            DeleteBullets();

            if (_player.IsFiring)
                CreateBullets();
        }

        private void MoveBullets()
        {
            _bullets.ForEach(p => p.Move());
        }

        private void DeleteBullets()
        {
            var bulletsToDelete = _bullets
                .Where(x => x.ShouldBeDeleted(_boundaryService))
                .ToArray();
            foreach (var bulletToDelte in bulletsToDelete)
                _bullets.Remove(bulletToDelte);
        }

        private void CreateBullets()
        {
            _bullets.AddRange(BulletHelper.Spawn(_randomNumberService, _player, BulletSpeed, HalfPlayerSize));
        }
        private void UpdateEnemies()
        {
            foreach (var enemy in _enemies)
                enemy.Update();
        }

        private void ShowPowerUpText()
        {
            _triggerPowerUpText = true;
            _powerUpCounter = PowerUpTicks;
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

        public Vector2 GetPlayerFacingDirection()
        {
            return _player.FacingDirection;
        }

        public IEnumerable<Bullet> GetBullets()
        {
            return _bullets;
        }

        public IEnumerable<Enemy> GetEnemies()
        {
            return _enemies;
        }

        public bool ShouldTriggerPowerUpText()
        {
            return _triggerPowerUpText;
        }

        public IEnumerable<Explosion> GetExplosions()
        {
            return _explosions;
        }

        public IEnumerable<Shrubbery> GetShrubbery()
        {
            return _shrubbery;
        }

        public IEnumerable<CollisionSplash> GetCollisionSplashes()
        {
            return _collisionSplashes;
        }
    }
}