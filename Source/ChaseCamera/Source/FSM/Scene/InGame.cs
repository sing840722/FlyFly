using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public class InGame : State
    {
        private SceneManager sceneManager;
        public InGame()
        {
            Name = "InGame";
        }

        public override void Enter(object owner)
        {
            if (owner.GetType() == typeof(SceneManager))
            {
                sceneManager = owner as SceneManager;
                sceneManager.Level.Initialise();
                //Console.Write(Name + "\n");
            }
        }

        public override void Exit(object owner)
        {
            //Clear()
            sceneManager.Level.DestroyScene();
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            sceneManager.Level.Update(gameTime);
        }
    }

}
