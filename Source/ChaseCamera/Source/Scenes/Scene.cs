using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public abstract class Scene
    {
        #region EventHandler
        public EventManager.CreateSceneHandler OnCreateEvent;
        protected void SendCreateEvent(SceneEventArgs e)
        {
            if (OnCreateEvent != null)
            {
                OnCreateEvent(this, e);
            }
        }

        public EventManager.LoadSceneHandler OnLoadEvent;
        protected void SendLoadEvent(SceneEventArgs e)
        {
            if (OnLoadEvent != null)
            {
                OnLoadEvent(this, e);
            }
        }

        public EventManager.DestroySceneHandler OnDestroyEvent;
        protected void SendDestroyEvent(SceneEventArgs e)
        {
            if (OnDestroyEvent != null)
            {
                OnDestroyEvent(this, e);
            }
        }
        #endregion

        //public string Name;
        protected string name;
        private ChaseCameraGame main;

        protected Scene(ChaseCameraGame m)
        {
            main = m;
            
            //this.OnCreateEvent += main.OnCreateScene;
            this.OnLoadEvent += main.OnLoadScene;
            this.OnDestroyEvent += main.OnDestroyScene;
            SendCreateEvent(new SceneEventArgs(name));
        }



        public virtual void Initialise()
        {
            SendLoadEvent(new SceneEventArgs(name));
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void DestroyScene()
        {
            SendDestroyEvent(new SceneEventArgs(name));
            //this.OnCreateEvent -= main.OnCreateScene;
            //this.OnLoadEvent -= main.OnLoadScene;
            //this.OnDestroyEvent -= main.OnDestroyScene;
        }

    }
}
