using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace ChaseCameraSample
{
    public class Background
    {
        #region fields
        private float backgroundSize;
        public float BackgroundSize
        {
            get { return backgroundSize; }
        }

        private World gameWorld;
        public World GameWorld
        {
            get { return gameWorld; }
            set { gameWorld = value; }
        }

        private float angle;
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        private Vector3 rotateAround;
        public Vector3 RotateAround
        {
            get { return rotateAround; }
            set { rotateAround = value; }
        }

        private float scale;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                SetMatrix();
            }
        }


        private Matrix modelViewMatrix;
        public Matrix ModelViewMatrix
        {
            get { return modelViewMatrix; }
        }
        #endregion

        public Background(Vector3 pos)
        {
            modelViewMatrix = Matrix.Identity;
            scale = 1.0f;
            angle = 90;
            RotateAround = new Vector3(0, 1, 0);
            position = pos;
            //Reset();
            SetMatrix();
        }

        //Reset 
        public void SetMatrix()
        {
            modelViewMatrix =
               Matrix.CreateScale(scale) *
               Matrix.CreateFromAxisAngle(rotateAround, MathHelper.ToRadians(angle)) *
               Matrix.CreateTranslation(position);
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
