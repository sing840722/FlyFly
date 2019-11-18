using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace ChaseCameraSample
{
    public class PowerUp: Collidable
    {
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

        public delegate void PowerUpHandler(object sender, PowerUpEventArgs e);
        public event PowerUpHandler PowerUpEvent;
        public void FirePowerUpEvent(PowerUpEventArgs e)
        {
            if (PowerUpEvent != null)
            {
                PowerUpEvent(this, e);
            }
        }

        protected PowerUpTypes bonusEffect;
        public void SetBonusEffect(PowerUpTypes eventType)
        {
            bonusEffect = eventType;
        }
        public PowerUpTypes BonusEffect
        {
            get { return bonusEffect; }
        }

        public PowerUp(PowerUpTypes powerUpType)
        {
            bonusEffect = powerUpType;
        }

        public override void Initialise()
        {
            active = true;
            height = 2000;
            size = 2500;
            rotateAround = new Vector3(0, 1, 0);
            angle = 0.0f;
            scale = 1.0f;
            boundingBox = new BoundingBox();
            CalculateBoundingBox();
            SetMatrix();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override bool CollisionTest(Collidable obj)
        {
            return base.CollisionTest(obj);
        }

        public override void OnCollision(Collidable obj)
        {
            base.OnCollision(obj);
        }
    }
}
