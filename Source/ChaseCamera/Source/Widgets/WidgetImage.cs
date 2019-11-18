using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaseCameraSample
{
    public class WidgetImage : Widget
    {
        #region Fields
        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        #endregion

        #region Initialise
        //Create empty image
        public WidgetImage()
        {

        }

        //Specify texture only
        public WidgetImage(Texture2D texture)
        {
            this.texture = texture;
        }

        //Specify texture, position
        public WidgetImage(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        //Specify texture, position ,scale
        public WidgetImage(Texture2D texture, Vector2 position, Vector2 scale)
        {
            this.texture = texture;
            this.position = position;
            this.scale = scale;
        }
        #endregion
    }
}
