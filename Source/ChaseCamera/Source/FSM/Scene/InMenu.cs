using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public class InMenu : State
    {
        private SceneManager sceneManager;
        public InMenu()
        {
            Name = "InMenu";
        }

        public override void Enter(object owner)
        {
            if (owner.GetType() == typeof(SceneManager))
            {
                sceneManager = owner as SceneManager;
                sceneManager.Menu.Initialise();
            }
        }

        public override void Exit(object owner)
        {
            sceneManager.Change = false;
            sceneManager.Menu.DestroyScene();
        }

        public override void Execute(object owner, GameTime gameTime)
        {

        }
    }

}
