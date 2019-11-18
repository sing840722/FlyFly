using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaseCameraSample
{
    public class PowerUpManager:IManager
    {
        #region Fields
        private ScoreManager scoreManager;
        public void SetScoreManager(ScoreManager sm)
        {
            scoreManager = sm;
        }

        private World world;
        public void SetWorld(World w)
        {
            world = w;
        }

        private CollisionManager collisionManager;
        public void SetCollisionManager(CollisionManager cm)
        {
            collisionManager = cm;
        }

        private List<PowerUp> powerUps;
        public List<PowerUp> PowerUps
        {
            get { return powerUps; }
        }

        private List<Texture2D> textures;
        public List<Texture2D> Textures
        {
            get { return textures; }
        }

        private Model model;
        public Model Model
        {
            get { return model; }
        }

        public void SetPowerUp(LevelContentManager lcm)
        {
            model = lcm.PowerUpModel;
            textures = lcm.PowerUpTextures;
        }
        #endregion

        #region Initialise
        public PowerUpManager()
        {
            //Initialise();
        }

        public void Initialise()
        {
            powerUps = new List<PowerUp>();
            textures = new List<Texture2D>();
        }
        #endregion

        #region Update
        public void Update(GameTime gameTime)
        {
            //Console.Write(powerUps.Count + "\n");
            for (int i = 0; i < powerUps.Count; i++) {
                if (powerUps[i].Active == false) {
                    powerUps[i].FireDestroyEvent(new RemoveGameObjectEventArgs(i));
                    return;
                }

                if (powerUps[i].Position.Z > world.WorldBoundLeft + powerUps[i].Size)
                {
                    powerUps[i].FireFinishEvent(new RemoveGameObjectEventArgs(i));
                    return;
                }
            }
        }
        #endregion

        #region Event listener
        //if player has collected the powerup
        //fire the power up event along with the power up bonus effect
        //remove the powerup from the list
        //add score
        //can add animation/ audio
        public void OnCollected(object sender, RemoveGameObjectEventArgs e)
        {  
            powerUps[e.Index].FirePowerUpEvent(new PowerUpEventArgs(PowerUps[e.Index].BonusEffect));
            RemovePowerUp(e.Index);
            scoreManager.FireScoringEvent(new ScoreEventArgs(50));
        }

        //if the object has left the viewzone, no longer avaible, remove object
        public void OnFinished(object sender, RemoveGameObjectEventArgs e)
        {
            RemovePowerUp(e.Index);
        }
        #endregion

        //add power up to the list
        //regisiter event
        public void AddPowerUp(PowerUp pu)
        {
            powerUps.Add(pu);
            collisionManager.AddCollidable(pu);
            pu.DestroyEvent += OnCollected;
            pu.FinishEvent += OnFinished;
            pu.PowerUpEvent += world.Player.OnPowerUp;
        }

        //remove power up from the list
        //remove eevents
        public void RemovePowerUp(int i)
        {
            powerUps[i].PowerUpEvent += world.Player.OnPowerUp;
            powerUps[i].FinishEvent -= OnFinished;
            powerUps[i].DestroyEvent -= OnCollected;
            collisionManager.RemoveCollidable(powerUps[i]);
            powerUps.RemoveAt(i);
        }

        //Change the texture of the model based on the enemy code
        public void ChangeSkin(PowerUpTypes powerUpType)
        {
            switch (powerUpType)
            {
                case PowerUpTypes.DOUBLE_ATTACK:
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.Texture = textures[0];
                        }
                    }
                    break;
                case PowerUpTypes.SPEED_ATTACK:
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.Texture = textures[1];
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void Clear()
        {
            powerUps.Clear();
        }
    }
}
