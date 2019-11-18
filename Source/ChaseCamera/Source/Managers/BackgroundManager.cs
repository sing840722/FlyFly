using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace ChaseCameraSample
{
    public class BackgroundManager:IManager
    {
        #region Fields
        private Background background1;
        private Background background2;
        private Background background3;

        private float backgroundSize;
        public void SetBackgroundSize(float s)
        {
            backgroundSize = s;
        }

        private List<Background> layers;
        public List<Background> Layers
        {
            get { return layers; }
        }

        private int leftIndex;
        private int rightIndex;

        private float viewZone;
        public void SetViewZone(float v)
        {
            viewZone = v;
        }

        private Camera camera;
        public void SetCamera(Camera cam)
        {
            camera = cam;
        }
        #endregion



        /*
         * Background Scrolling
         * Background is consisted of 3 plane with the same texture
         * [1][2][3], originally camera should be looking at [1][2],
         * as the camera scrolling to the left, and background can no longer cover the whole viewport
         * Extend the background by placing [3] on the left of [1] => [3][1][2],
         * so the camera is now looking at [3][1] and the background is fully covering the viewport again
        */

        public BackgroundManager()
        {
            //the width of background
            backgroundSize = 42000f;

            //Viewport boundary from center
            viewZone = 16000;
        }

        public void Initialise()
        {
            background1 = new Background(new Vector3(0, 0, -backgroundSize));
            background2 = new Background(new Vector3(0, 0, 0));
            background3 = new Background(new Vector3(0, 0, backgroundSize));
            layers = new List<Background>();
            layers.Add(background1);
            layers.Add(background2);
            layers.Add(background3);
            leftIndex = 0;
            rightIndex = layers.Count - 1;
        }

        public void Update(GameTime gameTime)
        {
            //If the background has moved out from the viewport on the right
            if (camera.Position.Z <  (layers[leftIndex].Position.Z + viewZone))
            {
                //Extend the background by placing the right background to the left 
                ScrollLeft();
            }

            /*
            if (camera.Position.Z > (layers[leftIndex].Position.Z + viewZone))
            {
                //Console.Write("Scroll Right \n");
                //ScrollRight();
            }
            */
        }

        //
        private void ScrollLeft()
        {
            //Allow access to the last right
            int lastRight = rightIndex;

            //Place the right background to the left or left background
            layers[rightIndex].Position = new Vector3(0, 0, layers[leftIndex].Position.Z - backgroundSize);
            //Also update the index(id of the background)
            leftIndex = rightIndex;
            rightIndex--;
            if (rightIndex < 0)
                rightIndex = layers.Count - 1;
        }

        private void ScrollRight()
        {
            int lastLeft = leftIndex;
            layers[leftIndex].Position = new Vector3(0, 0, layers[rightIndex].Position.Z + backgroundSize);
            rightIndex = leftIndex;
            leftIndex++;
            if (leftIndex == layers.Count)
                leftIndex = 0;
        }

        public void Clear()
        {
            //clear
        }

    }
}
