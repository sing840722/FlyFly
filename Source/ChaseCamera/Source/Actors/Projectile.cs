using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaseCameraSample
{
    public class Projectile: Collidable
    {
        #region field
        private Player owner;
        private float projectileMoveSpeed;
        #endregion
        #region event
        /*
        public delegate void DestroyHandler(object sender, RemoveGameObjectEventArgs e);
        public delegate void FinishHandler(object sender, RemoveGameObjectEventArgs e);

        public event DestroyHandler DestroyEvent;
        public event FinishHandler FinishEvent;

        public virtual void FireDestroyEvent(RemoveGameObjectEventArgs e)
        {
            if (DestroyEvent != null)
            {
                DestroyEvent(this, e);
            }
        }

        public virtual void FireFinishEvent(RemoveGameObjectEventArgs e)
        {
            if (FinishEvent != null)
            {
                FinishEvent(this, e);
            }
        }
        */
        #endregion

        public Projectile(Player ship)
        {
            //graphicsDevice = device;
            owner = ship;
        }

        public override void Initialise()
        {
            height = 500;
            size = 1000;
            active = true;
            projectileMoveSpeed = 100000.0f;
            position = owner.Position;
            angle = 0.0f;
            rotateAround = new Vector3(0, 1, 0);
            scale = 1.0f;
            SetMatrix();

            boundingBox = new BoundingBox();
            CalculateBoundingBox();
        }

        public override void Update(GameTime gameTime)
        {
            elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position.Z -= projectileMoveSpeed * elapsed;

            base.Update(gameTime);  //last to call
        }

        public override bool CollisionTest(Collidable obj)
        {
            if (obj != null)
            {
                if (obj.GetType() == typeof(Enemy))
                return BoundingBox.Intersects(obj.BoundingBox);
            }

            return false;
        }

        public override void OnCollision(Collidable obj)
        {
            if (obj != owner)
            {
                if (typeof(Enemy) == obj.GetType() )    //compare class
                {
                    Enemy e = obj as Enemy;
                    e.ReduceHP();
                    active = false;
                }
            }
        }


    }
}
