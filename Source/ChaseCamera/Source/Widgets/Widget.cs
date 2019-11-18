using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaseCameraSample
{
    public class Widget
    {
        #region Fields
        public Object owner;

        private List<WidgetImage> images = new List<WidgetImage>();
        public List<WidgetImage> Images
        {
            get { return images; }
            set { images = value; }
        }

        private List<WidgetText> texts = new List<WidgetText>();
        public List<WidgetText> Texts
        {
            get { return texts; }
            set { texts = value; }
        }

        private WidgetImage image;
        public WidgetImage Image
        {
            get { return image; }
            set { image = value; }
        }

        private WidgetText text;
        public WidgetText Text
        {
            get { return text; }
            set { text = value; }
        }

        protected Color colour = Color.Black;
        public Color Colour
        {
            get { return colour; }
            set { colour = value; }
        }

        protected float rotation = 0;
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        protected Vector2 origin = new Vector2(0);
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        protected Vector2 scale = new Vector2(1);
        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        protected Vector2 position = new Vector2(0);
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        #endregion

        #region Initialise
        public Widget()
        {

        }

        public Widget(Object owner)
        {
            this.owner = owner;
        }

        public void Initialise()
        {

        }
        #endregion

        #region Add image to widget
        public WidgetImage AddWidgetImage()
        {
            image = new WidgetImage();
            images.Add(image);
            return image;
        }

        public WidgetImage AddWidgetImage(Texture2D texture)
        {
            image = new WidgetImage(texture);
            images.Add(image);
            return image;
        }

        public WidgetImage AddWidgetImage(Texture2D texture, Vector2 position)
        {
            image = new WidgetImage(texture, position);
            images.Add(image);
            return image;
        }

        public WidgetImage AddWidgetImage(Texture2D texture, Vector2 position, Vector2 scale)
        {
            image = new WidgetImage(texture, position, scale);
            images.Add(image);
            return image;
        }
        #endregion

        #region Add text to widget
        public WidgetText AddWidgetText()
        {
            this.text = new WidgetText();
            texts.Add(this.text);
            return this.text;
        }

        public WidgetText AddWidgetText(String text)
        {
            this.text = new WidgetText(text);
            texts.Add(this.text);
            return this.text;
        }

        public WidgetText AddWidgetText(String text, Vector2 position)
        {
            this.text = new WidgetText(text, position);
            texts.Add(this.text);
            return this.text;
        }

        public WidgetText AddWidgetText(String text, Vector2 position, Vector2 scale)
        {
            this.text = new WidgetText(text, position, scale);
            texts.Add(this.text);
            return this.text;
        }
        #endregion

        #region Update
        public void Update(GameTime gameTime)
        {
            //update every image in the widget
            foreach (WidgetImage i in images)
            {
                i.Update(gameTime);
            }

            //update every text in the widget
            foreach (WidgetText t in texts)
            {
                t.Update(gameTime);
            }
        }
        #endregion
    }
}
