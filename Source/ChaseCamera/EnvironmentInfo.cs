using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public class EnvironmentInfo
    {
        private float cameraRollingSpeed;
        public float CameraRollingSpeed
        {
            get { return cameraRollingSpeed; }
            set { cameraRollingSpeed = value; }
        }

        private float worldBoundTop;
        public float WorldBoundTop
        {
            get { return worldBoundTop; }
            set { worldBoundTop = value; }
        }

        private float worldBoundLeft;
        public float WorldBoundLeft
        {
            get { return worldBoundLeft; }
            set { worldBoundLeft = value; }
        }

        private float worldBoundRight;
        public float WorldBoundRight
        {
            get { return worldBoundRight; }
            set { worldBoundRight = value; }
        }

        private float worldBoundBot;
        public float WorldBoundBot
        {
            get { return worldBoundBot; }
            set { worldBoundBot = value; }
        }

        private float backgroundSize;
        public float BackgroundSize
        {
            get { return backgroundSize; }
            set { backgroundSize = value; }
        }

        private float viewZone;
        public float ViewZone
        {
            get { return viewZone; }
            set { viewZone = value; }
        }

        public EnvironmentInfo()
        {
        }

    }
}
