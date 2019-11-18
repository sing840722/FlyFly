using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaseCameraSample
{
    public class EnemyProjectile: Collidable
    {
        #region field
        private Enemy owner;
        private float projectileMoveSpeed;
        #endregion
 

        public EnemyProjectile(Enemy o)
        {
            //graphicsDevice = device;
            owner = o;
        }

        public override void Initialise()
        {
            height = 500;
            size = 1000;
            active = true;
            projectileMoveSpeed = 35000.0f;
            position = owner.Position;
            angle = 180.0f;
            rotateAround = new Vector3(0, 1, 0);
            scale = 1.0f;
            SetMatrix();

            boundingBox = new BoundingBox();
            CalculateBoundingBox();
        }

        public override void Update(GameTime gameTime)
        {
            elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //move left
            position.Z += projectileMoveSpeed * elapsed;

            base.Update(gameTime);  //last to call
        }

        public override bool CollisionTest(Collidable obj)
        {
            return false;
        }

        public override void OnCollision(Collidable obj)
        {

        }


    }
}
