using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Core.Domain.Spawning;
using MonogameDemoGame.Helpers;
using MonogameDemoGame.Services;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Core.Domain
{
    public class LobGame
    {
        private const int PowerUpTicks = 90;
        private const int CollisionFragmentMaxSpeed = 10;
        private const int FragmentsPerExplosion = 36;
        private const float BulletSpeed = 10f;
        private const int BulletSize = 4;
        private const int EnemySize = 32;
        private const int PlayerSize = 32;
        private const int HalfPlayerSize = PlayerSize / 2;
        private readonly int _gameBorder;

        private Camera _camera;
        private Player _player;
        private List<Bullet> _bullets = new List<Bullet>();
        private List<Enemy> _enemies = new List<Enemy>();
        private List<CollisionSplash> _collisionSplashes = new List<CollisionSplash>();
        private List<Shrubbery> _shrubbery = new List<Shrubbery>();
        private List<Explosion> _explosions = new List<Explosion>();

        private bool _triggerPowerUpText;
        private int _powerUpCounter;

        public LobGame(InitialGameState initialGameState)
        {
            _gameBorder = initialGameState.GameBorder;

            _camera = new Camera(new Point(initialGameState.Camera.Position.X, initialGameState.Camera.Position.Y));
            _player = new Player(initialGameState.Player.Position, initialGameState.Player.FacingDirection);

            foreach (var enemy in initialGameState.Enemies)
                _enemies.Add(new Enemy(enemy.Position, enemy.FacingDirection));
            foreach (var shrubbery in initialGameState.Shrubbery)
                _shrubbery.Add(new Shrubbery(shrubbery.Position));
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
            var result = _player.TryLevelUp();

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
            var fragments = new List<Vector2>();
        
            for (int i = 0; i < FragmentsPerExplosion; i++)
                fragments.Add(new Vector2(1, 0).Rotate(RandomNumberService.NextRandomNumber(360)) * RandomNumberService.NextRandomNumber(CollisionFragmentMaxSpeed));
            var explosion = new Explosion(enemy.Position, fragments);
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
            int splashes = 10;
            for (int i = 0; i < splashes; i++)
            {
                var duration = (int)RandomNumberService.GenerateRandomNumberClusteredTowardZero(10);
                var oppositeDirection = new Vector2() - bullet.Direction;
                var direction = oppositeDirection.Rotate(RandomNumberService.GenerateRandomNegativeOrPositiveOne() * (int)RandomNumberService.GenerateRandomNumberClusteredTowardZero(70));
                direction = direction* ((float)RandomNumberService.NextRandomNumber(1, 100)/50f);
                _collisionSplashes.Add(new CollisionSplash(bullet.Position + direction, direction, duration));
            }
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
                .Where(x => x.ShouldBeDeleted(this))
                .ToArray();
            foreach (var bulletToDelte in bulletsToDelete)
                _bullets.Remove(bulletToDelte);
        }

        private void CreateBullets()
        {
            var list = new List<Bullet>();

            var xDelta = _player.FacingDirection.X * BulletSpeed;
            var yDelta = _player.FacingDirection.Y * BulletSpeed;
            foreach (var gunAngle in _player.FiringAngles)
            {
                var angle = (int)RandomNumberService.GenerateRandomNumberClusteredTowardZero(gunAngle);
                if (RandomNumberService.GetRandomBool())
                    angle = -angle;

                var direction = new Vector2(xDelta, yDelta).Rotate(angle);

                var bullet = new Bullet(_player.Position.ToVector2() + (HalfPlayerSize*_player.FacingDirection), direction);

                list.Add(bullet);
            }

            _bullets.AddRange(list);
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

        public IEnumerable<Shrubbery> GetShrubbery()
        {
            return _shrubbery;
        }

        public IEnumerable<CollisionSplash> GetCollisionSplashes()
        {
            return _collisionSplashes;
        }

        public bool OutOfBounds(float position)
        {
            return Math.Abs(position) > _gameBorder;
        }

        public IEnumerable<ExplosionFragment> GetFragments()
        {
            return _explosions.SelectMany(x => x.Fragments).ToArray();
        }
    }
}