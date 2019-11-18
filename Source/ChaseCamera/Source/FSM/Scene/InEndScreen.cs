using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public class InEndScreen : State
    {
        private SceneManager sceneManager;
        public InEndScreen()
        {
            Name = "InEndScreen";
        }

        public override void Enter(object owner)
        {
            if (owner.GetType() == typeof(SceneManager))
            {
                sceneManager = owner as SceneManager;
                sceneManager.EndScreen.Initialise();
               // Console.Write(Name + "\n");
            }
        }

        public override void Exit(object owner)
        {
            sceneManager.EndScreen.DestroyScene();
            sceneManager.Change = false;
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            //sceneManager.Menu.Update(gameTime);
        }
    }

}
