using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public class FleeState : State
    {
        private Enemy enemy;
        public FleeState()
        {
            Name = "Flee";
        }

        public override void Enter(object owner)
        {
            enemy = owner as Enemy;
            enemy.CurrentState = this.Name;
            //throw new NotImplementedException();
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            enemy.Flee();
            //throw new NotImplementedException();
        }

        public override void Exit(object owner)
        {
            enemy.Finished = true;
        }
    }
}
