using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public class Collidable
    {

        #region Actor Fields
        protected float elapsed;

        protected float height;
        public float Height
        {
            get { return height; }
        }
        protected float size;
        public float Size
        {
            get { return size; }
        }

        protected float angle;
        public float Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                SetMatrix();
            }
        }

        protected Vector3 rotateAround;
        public Vector3 RotateAround
        {
            get { return rotateAround; }
            set
            {
                rotateAround = value;
                SetMatrix();
            }
        }

        protected float scale;
        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                SetMatrix();
            }
        }

        protected Matrix modelViewMatrix;
        public Matrix ModelViewMatrix
        {
            get { return modelViewMatrix; }
        }


        public virtual void SetMatrix()
        {
            modelViewMatrix =
               Matrix.CreateScale(scale) *
               Matrix.CreateFromAxisAngle(rotateAround, MathHelper.ToRadians(angle)) *
               Matrix.CreateTranslation(position);
        }

        protected Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                CalculateBoundingBox();
                SetMatrix();
            }
        }

        protected bool active = true;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        protected void CalculateBoundingBox()
        {
            boundingBox.Min = position - Vector3.Backward * size / 2.0f;
            boundingBox.Max = position + Vector3.Up * height / 2.0f + Vector3.Backward * size / 2.0f;

        }
        #endregion


        #region Fields
        protected BoundingSphere boundingSphere = new BoundingSphere();
        protected BoundingBox boundingBox = new BoundingBox();
        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
        }
        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }
        #endregion


        #region events
        //Removal event that can be used for every collidable object including derived class
        //i.e player, enemy
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
        
        #endregion


        #region Member Functions
        public virtual void Initialise()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            //update matrix and calculate bounding box
            position.X = 10;
            CalculateBoundingBox();
            SetMatrix();
        }

        public virtual bool CollisionTest(Collidable obj)
        {
            return false;
        }

        public virtual void OnCollision(Collidable obj)
        {
            
        }
        #endregion

    }

}
