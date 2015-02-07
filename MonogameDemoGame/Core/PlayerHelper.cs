using System;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Helpers;
using MonogameDemoGame.Services;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Core
{
    public static class PlayerHelper
    {
        public static PlayerStruct Spawn(Point midpoint)
        {
            var player = new PlayerStruct();
            
            player.Level = 1;
            player.FacingDirection = new Vector2(0f, 1f);
            player.MoveDirection = new Point();
            player.Position = new Point(midpoint.X, midpoint.Y);
            player.GunAngles.Add(0);
            
            return player;
        }

        public static void Update(PlayerStruct player, KeyboardInputStruct keyboardInput, MouseInputStruct mouseInput)
        {
            player.MoveDirection = keyboardInput.MoveDirection;
            player.IsFiring = mouseInput.IsFiring;
            player.FacingDirection = mouseInput.PlayerFacingDirection;
        }

        public static void Move(PlayerStruct player)
        {
            player.Position = player.Position + player.MoveDirection;
        }

        public static void LevelUp(IRandomNumberService randomNumberService, PlayerStruct player)
        {
            player.Level++;
            player.GunAngles.Add((int)(1 + randomNumberService.GenerateRandomNumberClusteredTowardZero(15)));
        }

        public static bool ShouldLevelUp(PlayerStruct player)
        {
            return (player.Level * player.Level + 1) / 3 < player.Xp;
        }

        public static void AwardExperience(PlayerStruct player)
        {
            player.Xp++;
        }
    }
}