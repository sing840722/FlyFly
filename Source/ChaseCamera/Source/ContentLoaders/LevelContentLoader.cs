using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace ChaseCameraSample
{
    public class LevelContentManager
    {
        #region Fields
        private SoundEffect score;
        public SoundEffect Score
        {
            get { return score; }
        }

        private SoundEffect fire;
        public SoundEffect Fire
        {
            get { return fire; }
        }

        private Song backgroundMusic;
        public Song BackgroundMusic
        {
            get { return backgroundMusic; }
        }

        private SoundEffect explosion;
        public SoundEffect Explosion
        {
            get { return explosion; }
        }

        private List<Texture2D> powerUpTextures;
        public List<Texture2D> PowerUpTextures
        {
            get { return powerUpTextures; }
        }

        private List<Texture2D> oldPlaneTextures;
        public List<Texture2D> OldPlaneTextures
        {
            get { return oldPlaneTextures; }
        }

        private List<Texture2D> helicopterTextures;
        public List<Texture2D> HelicopterTextures
        {
            get { return helicopterTextures; }
        }

        //private List<Model> models;
        private Model playerModel;
        public Model PlayerModel
        {
            get { return playerModel; }
        }

        private Model groundModel;
        public Model GroundModel
        {
            get { return groundModel; }
        }

        private Model oldPlaneModel;
        public Model OldPlaneModel
        {
            get { return oldPlaneModel; }
        }

        private Model helicopterModel;
        public Model HelicopterModel
        {
            get { return helicopterModel; }
        }

        private Model enemyProjectilModel;
        public Model EnemyProjectileModel
        {
            get { return enemyProjectilModel; }
        }

        private Model projectileModel;
        public Model ProjectileModel
        {
            get { return projectileModel; }
        }

        private Model powerUpModel;
        public Model PowerUpModel
        {
            get { return powerUpModel; }
        }

        private ContentManager Content;
        private ChaseCameraGame game;
        #endregion

        public LevelContentManager(ChaseCameraGame game)
        {
            this.game = game;
            Initialise();
        }

        public void Initialise()
        {
            this.Content = new ContentManager(game.Services, "Content");
            this.oldPlaneTextures = new List<Texture2D>();
            this.helicopterTextures = new List<Texture2D>();
            this.powerUpTextures = new List<Texture2D>();
        }

        #region Load content
        //Load everything is needed for the level
        public void LoadContents()
        {
            Initialise();
            LoadModels();
            LoadTextures();
            LoadSound();
        }

        public void LoadSound()
        {
            backgroundMusic = Content.Load<Song>("Sound\\gameMusic");
            explosion = Content.Load<SoundEffect>("Sound\\explosion");
            fire = Content.Load<SoundEffect>("Sound\\fire");
            score = Content.Load<SoundEffect>("Sound\\score");
            
        }

        //Load all the models
        public void LoadModels()
        {
            playerModel = Content.Load<Model>("Model\\Player");
            groundModel = Content.Load<Model>("Model\\Background");
            oldPlaneModel = Content.Load<Model>("Model\\Enemy");
            helicopterModel = Content.Load<Model>("Model\\Enemy2");
            powerUpModel = Content.Load<Model>("Model\\Powerup1");
            enemyProjectilModel = Content.Load<Model>("Model\\EnemyBullet");
            projectileModel = Content.Load<Model>("Model\\Bullet");
        }

        //Load all the textures
        public void LoadTextures()
        {
            //Console.Write("Load texture\n");
            for (int i = 0; i < 10; i++)
            {
                string path = String.Format("Model\\Enemy.fbm\\body00_{0}", i);
                Texture2D texture = Content.Load<Texture2D>(path);
                oldPlaneTextures.Add(texture);
            }

            for (int i = 0; i < 10; i++)
            {
                string path = String.Format("Model\\Enemy2.fbm\\body200_{0}", i);
                Texture2D texture = Content.Load<Texture2D>(path);
                helicopterTextures.Add(texture);
            }

            Texture2D PowerDouble = Content.Load<Texture2D>("Model\\Powerup1.fbm\\outUV_Double");
            powerUpTextures.Add(PowerDouble);

            Texture2D PowerSpeed = Content.Load<Texture2D>("Model\\Powerup1.fbm\\outUV_Speed");
            powerUpTextures.Add(PowerSpeed);
        }

        //Load a single model and return it
        public Model LoadModel(string modelPath)
        {
            return Content.Load<Model>(modelPath);
        }
        #endregion


        #region Unload content
        //Unload all the contents
        public void UnloadContents()
        {
            //unload content
            Content.Unload();

            //destroy created content
            Content.Dispose();
        }
        #endregion
    }
}
