using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ChaseCameraSample
{
    public class WidgetText: Widget
    {
        #region Bind Text Handler
        public delegate void BindTextHandler(WidgetText widgetText);
        public BindTextHandler BindText;
        #endregion

        #region Fields
        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private SpriteFont fontStyle;
        public SpriteFont FontStyle
        {
            get { return fontStyle; }
            set { fontStyle = value; }
        }
        #endregion

        #region Initialise
        //Create empty text 
        public WidgetText()
        {
            //colour = Color.Black;
        }

        //Specify text
        public WidgetText(string text)
        {
            this.text = text;
        }

        //Specify text and postiion
        public WidgetText(string text, Vector2 position)
        {
            this.text = text;
            this.position = position;
        }

        //Specify text, position, scale (use default font style)
        public WidgetText(string text, Vector2 position, Vector2 scale)
        {
            this.text = text;
            this.position = position;
            this.scale = scale;
        }

        //Specify text, font style, position
        public WidgetText(string text, SpriteFont spriteFont, Vector2 position)
        {
            this.text = text;
            this.fontStyle = spriteFont;
            this.position = position;
        }

        //Specify text, font style, position, scale
        public WidgetText(string text, SpriteFont spriteFont, Vector2 position, Vector2 scale)
        {
            this.text = text;
            this.fontStyle = spriteFont;
            this.position = position;
            this.scale = scale;
        }
        #endregion

        #region Update
        public new void Update(GameTime gameTime)
        {
            //if binded text, bind it
            if(BindText != null)
                BindText(this);
        }
        #endregion
    }
}
