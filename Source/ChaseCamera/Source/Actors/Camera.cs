#region File Description
//-----------------------------------------------------------------------------
// ChaseCamera.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace ChaseCameraSample
{
    public class Camera
    {
        #region fields
        private float cameraRollingSpeed;
        public void SetCameraRollingSpeed(float f)
        {
            cameraRollingSpeed = f;
        }
        public Vector3 Up
        {
            get { return up; }
            set { up = value; }
        }
        private Vector3 up = Vector3.Up;

        public Vector3 LookAt
        {
            get
            {
                return lookAt;
            }
        }
        private Vector3 lookAt;


        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        private Vector3 position;


        public float AspectRatio
        {
            get { return aspectRatio; }
            set { aspectRatio = value; Reset(); }
        }
        private float aspectRatio = 4.0f / 3.0f;

        public float FieldOfView
        {
            get { return fieldOfView; }
            set { fieldOfView = value; }
        }
        private float fieldOfView = MathHelper.ToRadians(45.0f);

        public float NearPlaneDistance
        {
            get { return nearPlaneDistance; }
            set { nearPlaneDistance = value; }
        }
        private float nearPlaneDistance = 1.0f;

        public float FarPlaneDistance
        {
            get { return farPlaneDistance; }
            set { farPlaneDistance = value; }
        }
        private float farPlaneDistance = 50000.0f;

        public Matrix View
        {
            get { return view; }
        }
        private Matrix view;

 
        public Matrix Projection
        {
            get { return projection; }
        }
        private Matrix projection;

        private World gameWorld;
        public void SetGameWorld(World w)
        {
            gameWorld = w;
        }
        #endregion


        #region Methods
        public Camera()
        {

        }

        public void Initialise()
        {
            Reset();
        }

        private void UpdateMatrices()
        {
            view = Matrix.CreateLookAt(this.Position, this.LookAt, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView,
                AspectRatio, NearPlaneDistance, FarPlaneDistance);
        }

        public void Reset()
        {
            position = new Vector3(50000, 10, 0);
            lookAt = new Vector3(0, 0, 0);          //direction
            UpdateMatrices();
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (gameTime == null)
                throw new ArgumentNullException("gameTime");

            position.Z -= cameraRollingSpeed * elapsed;
            lookAt.Z -= cameraRollingSpeed * elapsed;

            UpdateMatrices();

        }

        #endregion
    }
}
