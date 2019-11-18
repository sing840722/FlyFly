using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public class CombatState : State
    {
        private Enemy enemy;
        public CombatState()
        {
            Name = "Combat";
        }

        public override void Enter(object owner)
        {
            enemy = owner as Enemy;
            enemy.CurrentState = this.Name;

            //Console.Write("Start Combat \n");
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            enemy.Combat();
            enemy.AutoFire(gameTime);
        }

        public override void Exit(object owner)
        {
            //throw new NotImplementedException();
            //Console.Write("Finish Combat \n");
        }
    }
}
