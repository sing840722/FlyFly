using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ChaseCameraSample
{
    public class ProjectileManager:IManager
    {
        #region Events and delegates
        public delegate void PlayerAttackHandler(object sender, EventArgs e);
        public event PlayerAttackHandler PlayerAttackEvent;
        public void FirePlayerAttackEvent(EventArgs e)
        {
            if (PlayerAttackEvent != null)
            {
                PlayerAttackEvent(this, e);
            }
        }
        #endregion

        #region Fields
        private SoundEffect fire;
        public void SetSoundEffect(LevelContentManager lcm)
        {
            this.fire = lcm.Fire;
        }

        private World world;
        public void SetWorld(World w)
        {
            world = w;
        }

        private TimeSpan playerPreviousFireTime;
        private CollisionManager collisionManager;
        public void SetCollisionManager(CollisionManager cm)
        {
            collisionManager = cm;
        }

        private Player player;
        public void SetPlayer(Player p)
        {
            player = p;
        }

        private List<Projectile> playerProjectiles;
        public List<Projectile> PlayerProjectiles
        {
            get { return playerProjectiles; }
        }
        #endregion

        #region Initialise
        public ProjectileManager()
        {
            //Initialise();
            PlayerAttackEvent += OnPlayerAttack;
        }

        public void Initialise()
        {
            playerProjectiles = new List<Projectile>();
        }

        public void Clear()
        {
            playerProjectiles.Clear();
        }
        #endregion

        public bool Finished(Projectile projectile)
        {
            return world.WorldBoundRight > projectile.Position.Z + projectile.Size;
        }

        //Setup a projectil
        //add to the list
        //register event
        private void AddPlayerProjectile(int j)
        {
            int yOffset = 500;
            Projectile projectile = new Projectile(player);
            projectile.Initialise();
            Vector3 p = projectile.Position;
            p.Y += yOffset * j;
            projectile.Position = new Vector3(p.X, p.Y, p.Z);
            playerProjectiles.Add(projectile);
            collisionManager.AddCollidable(projectile);
            projectile.FinishEvent += OnFinish;
            projectile.DestroyEvent += OnDestroy;
        }



        private void Fire()
        {
            //Add projectils base on the number of attack player can make
            for (int i = 1; i <= player.NumberOfAttack; i++)
            {
                if (player.NumberOfAttack == 1) //if only fire a single line
                {
                    //spawn projectil at with position.Y same as player
                    AddPlayerProjectile(0);
                }
                else if(player.NumberOfAttack > 1)  //if fire more than one
                {
                    if (i % 2 == 0) //spawn bullet above
                    {
                        int j = i / -2;
                        AddPlayerProjectile(j);

                    }
                    else if (i % 2== 1) //spawn bullet below
                    {
                        int j = i / 2 + 1;
                        AddPlayerProjectile(j);
                    }
                }
            }
        }

        private void RemovePlayerProjecttile(int i)
        {
            playerProjectiles[i].DestroyEvent -= OnDestroy;
            playerProjectiles[i].FinishEvent -= OnFinish;
            collisionManager.RemoveCollidable(playerProjectiles[i]);
            playerProjectiles.RemoveAt(i);
        }

        private void UpdateProjectiles(GameTime gameTime)
        {
            for (int i = playerProjectiles.Count - 1; i >= 0; i--)
            {
                playerProjectiles[i].Update(gameTime);

                if (Finished(playerProjectiles[i]))
                {
                    playerProjectiles[i].FireFinishEvent(new RemoveGameObjectEventArgs(i));
                    return;
                }

                if (!playerProjectiles[i].Active)
                {
                    playerProjectiles[i].FireDestroyEvent(new RemoveGameObjectEventArgs(i));
                    return;
                }
            }
        }

        public void AutoFire(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - playerPreviousFireTime >= player.FireTime)
            {
                playerPreviousFireTime = gameTime.TotalGameTime;
                PlayerAttackEvent(this, new EventArgs());
            }
        }

        public void Update(GameTime gameTime)
        {
            UpdateProjectiles(gameTime);
            AutoFire(gameTime);
        }

        #region Events Listener
        public void OnPlayerAttack(object sender, EventArgs e)
        {
            
            fire.Play(0.5f, 0.0f, 0.0f);
            Fire();
        }

        public void OnDestroy(object sender, RemoveGameObjectEventArgs e)
        {
            RemovePlayerProjecttile(e.Index);
            //play audio?
            //play animation?
        }

        public void OnFinish(object sender, RemoveGameObjectEventArgs e)
        {
            RemovePlayerProjecttile(e.Index);
        }
        #endregion
    }
}
