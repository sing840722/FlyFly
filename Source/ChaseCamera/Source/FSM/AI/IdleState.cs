using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public class IdleState : State
    {
        private Enemy enemy;
        public IdleState()
        {
            Name = "Idle";
        }

        public override void Enter(object owner)
        {
            //throw new NotImplementedException();
            //Console.Write("Start Idle\n");
            enemy = owner as Enemy;
            enemy.CurrentState = this.Name;

        }

        public override void Execute(object owner, GameTime gameTime)
        {
            enemy.Idle();
            //throw new NotImplementedException();
            //Console.Write("Execute");
        }

        public override void Exit(object owner)
        {
            //Console.Write("End Idle\n");
            //throw new NotImplementedException();
        }
    }
}
