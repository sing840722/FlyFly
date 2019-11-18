using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace ChaseCameraSample
{
    public class SpawnState : State
    {
        public SpawnState()
        {
            Name = "spawn";
        }

        public override void Enter(object owner)
        {
            Projectile projectile = owner as Projectile;
            if (projectile != null) { }
            //do something
            //add to colliable
            //Console.Write("State \n");
            //projectile.
                
        }

        public override void Exit(object owner)
        {
            Projectile projectile = owner as Projectile;
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            Projectile projectile = owner as Projectile;
            if (projectile == null) return;

        }
    }

}
