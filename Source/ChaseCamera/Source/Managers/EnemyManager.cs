using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ChaseCameraSample
{
    public class EnemyManager:IManager
    {
        #region fields

        private EnemyProjectileManager projectileManager;
        public void SetProjectileManager(EnemyProjectileManager epm)
        {
            projectileManager = epm;
            projectileManager.SetEnemyManager(this);
            projectileManager.SetCollisionManager(this.collisionManager);
            projectileManager.SetWorld(this.world);
        }

        private World world;
        public void SetWorld(World w)
        {
            world = w;
        }

        private SoundEffect explosion;
        public void SetSoundEffect(LevelContentManager lcm)
        {
            this.explosion = lcm.Explosion;
        }

        private CollisionManager collisionManager;
        public void SetCollisionManager(CollisionManager cm)
        {
            collisionManager = cm;
        }

        private ScoreManager scoreManager;
        public void SetScoreManager(ScoreManager sm)
        {
            scoreManager = sm;
        }

        private List<Enemy> enemies;
        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        private Model helicopterModel;
        private List<Texture2D> helicpterTextures;
        public void SetHelicopter(LevelContentManager lcm)
        {
            helicopterModel = lcm.HelicopterModel;
            helicpterTextures = lcm.HelicopterTextures;
        }

        private Model oldPlaneModel;
        private List<Texture2D> oldPlaneTextures;
        public void SetOldPlane(LevelContentManager lcm)
        {
            oldPlaneModel = lcm.OldPlaneModel;
            oldPlaneTextures = lcm.OldPlaneTextures;
        }
        #endregion

        #region EnterNumberFunctions
        public void OnPlayerSpecialAttack(object sender, PlayerEventArgs e)
        {
            if (e.PlayerEventType == PlayerEventTypes.SPECIAL_ATTACK)
            {
                switch ((int)e.Amount)
                {
                    case 0:
                        CheckEnemyCode(e.Amount);
                        break;
                    case 1:
                        CheckEnemyCode(e.Amount);
                        break;
                    case 2:
                        CheckEnemyCode(e.Amount);
                        break;
                    case 3:
                        CheckEnemyCode(e.Amount);
                        break;
                    case 4:
                        CheckEnemyCode(e.Amount);
                        break;
                    case 5:
                        CheckEnemyCode(e.Amount);
                        break;
                    case 6:
                        CheckEnemyCode(e.Amount);
                        break;
                    case 7:
                        CheckEnemyCode(e.Amount);
                        break;
                    case 8:
                        CheckEnemyCode(e.Amount);
                        break;
                    case 9:
                        CheckEnemyCode(e.Amount);
                        break;
                    default:
                        break;
                }
            }
        }

        public void CheckEnemyCode(float code)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.OnScreen)
                {
                    if (enemy.Code == code)
                    {
                        enemy.HP = 0;
                        return;
                    }
                }
            }
        }
        #endregion

        #region CheckEnemyState
        //check if enemy enter viewzone
        private bool InViewport(Enemy enemy)
        {
            return (enemy.Position.Z < world.WorldBoundLeft + enemy.Size) && (enemy.Position.Z > world.WorldBoundRight - enemy.Size);
        }
        //check if enemy leave the viewzone
        private bool Finished(Enemy enemy)
        {
            return (enemy.Position.Z > world.WorldBoundLeft + enemy.Size);
        }
        //check if enemy get killed
        private bool GetKilled(Enemy enemy)
        {
            return (enemy.HP < 1);
        }
        //check if enemy successfully fleed
        private bool Fleed(Enemy enemy)
        {
            return (enemy.Position.Y > world.WorldBoundTop) || (enemy.Position.Y < world.WorldBoundBot);
        }
        #endregion

        #region Initialise
        public EnemyManager()
        {
            //Initialise();
        }

        public void Initialise()
        {
            enemies = new List<Enemy>();
            oldPlaneTextures = new List<Texture2D>();
            helicpterTextures = new List<Texture2D>();
        }

        
        #endregion

        #region Update
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < enemies.Count; i++) {
                //update every enemy in the list
                enemies[i].Update(gameTime);

                //check and update the status of OnScreen
                enemies[i].OnScreen = InViewport(enemies[i]);

                //Check and update the status of finished and fleed, remove enemy if one of them return true
                if (enemies[i].Finished = Finished(enemies[i]))
                {
                    enemies[i].FireFinishEvent(new RemoveGameObjectEventArgs(i));
                    return;
                }

                if (enemies[i].Fleed = Fleed(enemies[i]) && enemies[i].CurrentState == "Flee")
                {
                    enemies[i].FireFinishEvent(new RemoveGameObjectEventArgs(i));
                    return;
                }

                //check and update the status of getkilled, destroy if true
                if (enemies[i].Active = GetKilled(enemies[i]))
                {
                    enemies[i].FireDestroyEvent(new RemoveGameObjectEventArgs(i));
                    return;
                }
            }
        }
        #endregion

        #region Event Listener
        public void OnEnemyDestroy(object sender, RemoveGameObjectEventArgs e)
        {
            //Play Sound Effect
            explosion.Play();

            //remove enemy
            RemoveEnemy(e.Index);

            //add 30 score
            scoreManager.FireScoringEvent(new ScoreEventArgs(30));

            //if this is the last enemy in the world
            //load a new wave
            if (enemies.Count == 0)
            {
                world.LoadWaves();  
            }

            //animation
            //audio
        }

        public void OnEnemyFinish(object sender, RemoveGameObjectEventArgs e)
        {
            //remove enemy and load a new wave if no enemy left in the world
            RemoveEnemy(e.Index);
            if (enemies.Count == 0)
            {
                world.LoadWaves();
            }
        }
        #endregion

        //Add enemy to the list, 
        //Add collision to the enemy
        //Register events
        public void AddEnemy(Enemy e)
        {
            enemies.Add(e);
            collisionManager.AddCollidable(e);
            e.FinishEvent += OnEnemyFinish;
            e.DestroyEvent += OnEnemyDestroy;
            e.EnemyAttackEvent += projectileManager.OnEnemyAttack;
        }

        //Remove enemy from the list
        //remove collision comparision
        //remove events
        public void RemoveEnemy(int i)
        {
            enemies[i].EnemyAttackEvent -= projectileManager.OnEnemyAttack;
            enemies[i].DestroyEvent -= OnEnemyDestroy;
            enemies[i].FinishEvent -= OnEnemyFinish;
            collisionManager.RemoveCollidable(enemies[i]);
            enemies.RemoveAt(i);
        }

        //Change the texture of the model based on the enemy code
        public void ChangeSkin(EnemyTypes enemyType, int code)
        {
            switch (enemyType)
            {
                case EnemyTypes.OLD_PLANE:
                    foreach (ModelMesh mesh in oldPlaneModel.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.Texture = oldPlaneTextures[code];
                        }
                    }
                    break;
                case EnemyTypes.HELICOPTER:
                    foreach (ModelMesh mesh in helicopterModel.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.Texture = helicpterTextures[code];
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        //return the corresponded model based on the enemy type
        public Model ChangeModel(EnemyTypes enemyType)
        {
            switch (enemyType)
            {
                case EnemyTypes.OLD_PLANE:
                    return oldPlaneModel;
                case EnemyTypes.HELICOPTER:
                    return helicopterModel;
                default:
                    return null;
            }
        }


        public void Clear()
        {
            enemies.Clear();
        }
    }
}
