using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ChaseCameraSample
{
    public class EnemyProjectileManager : IManager
    {
        #region Fields
        private SoundEffect fireSoundEffect;
        public void SetSoundEffect(LevelContentManager lcm)
        {
            fireSoundEffect = lcm.Fire;
        }

        private EnemyManager enemyManager;
        public void SetEnemyManager(EnemyManager em)
        {
            enemyManager = em;
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


        private List<EnemyProjectile> enemyProjectiles;
        public List<EnemyProjectile> EnemyProjectiles
        {
            get { return enemyProjectiles; }
        }
        #endregion

        #region Initialise
        public EnemyProjectileManager()
        {

        }

        public void Initialise()
        {
            enemyProjectiles = new List<EnemyProjectile>();
        }

        public void Clear()
        {
            enemyProjectiles.Clear();
        }
        #endregion

        public bool Finished(EnemyProjectile projectile)
        {
            return world.WorldBoundLeft < projectile.Position.Z - projectile.Size;
        }


        public void OnEnemyAttack(object sender, EventArgs e)
        {
            Enemy enemy = sender as Enemy;
            fireSoundEffect.Play();
            AddEnemyProjectile(enemy);
        }


        private void AddEnemyProjectile(Enemy owner)
        {
            //Console.Write(owner + "\n");
            EnemyProjectile projectile = new EnemyProjectile(owner);
            projectile.Initialise();
            enemyProjectiles.Add(projectile);
            collisionManager.AddCollidable(projectile);
            projectile.FinishEvent += OnFinish;
            projectile.DestroyEvent += OnDestroy;
        }

        private void RemoveEnemyProjectile(int i)
        {
            enemyProjectiles[i].DestroyEvent -= OnDestroy;
            enemyProjectiles[i].FinishEvent -= OnFinish;
            collisionManager.RemoveCollidable(enemyProjectiles[i]);
            enemyProjectiles.RemoveAt(i);
        }

        private void UpdateProjectiles(GameTime gameTime)
        {
            for (int i = enemyProjectiles.Count - 1; i >= 0; i--)
            {
                enemyProjectiles[i].Update(gameTime);
                
                if (Finished(enemyProjectiles[i]))
                {
                    enemyProjectiles[i].FireFinishEvent(new RemoveGameObjectEventArgs(i));
                    return;
                }

                if (!enemyProjectiles[i].Active)
                {
                    enemyProjectiles[i].FireDestroyEvent(new RemoveGameObjectEventArgs(i));
                    return;
                }
                
            }
        }

        public void Update(GameTime gameTime)
        {
            UpdateProjectiles(gameTime);
        }

        public void OnDestroy(object sender, RemoveGameObjectEventArgs e)
        {
            RemoveEnemyProjectile(e.Index);
            //play audio?
            //play animation?
        }

        public void OnFinish(object sender, RemoveGameObjectEventArgs e)
        {
            RemoveEnemyProjectile(e.Index);
        }
    }
}
