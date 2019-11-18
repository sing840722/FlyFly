using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaseCameraSample
{
    public class WidgetManager
    {
        #region Fields
        private readonly int SCREEN_WIDTH;
        private readonly int SCREEN_HEIGHT;

        private GraphicsDevice graphicDevice;
        private MenuContentManager menuContent;
        private LevelContentManager levelContent;
        private EndScreenContentManager endScreenContent;

        private Player player;
        public void SetPlayer(Player p)
        {
            player = p;
        }

        private ScoreManager scoreManager;
        public void SetScoreManager(ScoreManager sm)
        {
            scoreManager = sm;
        }

        private SpriteBatch spriteBatch;
        public void SetSpriteBatch(SpriteBatch sb)
        {
            spriteBatch = sb;
        }

        private SpriteFont spriteFont;
        public void AddSpriteFont(SpriteFont sf)
        {
            spriteFont = sf;
        }

        private Widget menu;
        public Widget Menu
        {
            get { return menu; }
        }

        private Widget endScreen;
        public Widget EndScreen
        {
            get { return endScreen; }
        }

        private Widget levelHUD;
        public Widget LevelHUD
        {
            get { return levelHUD; }
        }
        #endregion

        #region Intilise
        public WidgetManager(GraphicsDevice graphicDevice, MenuContentManager menuContent, LevelContentManager levelContent, EndScreenContentManager endScreenContent)
        {
            this.graphicDevice = graphicDevice;
            this.menuContent = menuContent;
            this.levelContent = levelContent;
            this.endScreenContent = endScreenContent;

            SCREEN_HEIGHT = graphicDevice.Viewport.Height;
            SCREEN_WIDTH = graphicDevice.Viewport.Width;
        }

        
        public void Initialise()
        {
            menu = new Widget();
            levelHUD = new Widget();
            endScreen = new Widget();
        }
        #endregion

        #region Update
        public void Update(GameTime gameTime)
        {
            //update every widget if not null
            if(menu!=null)
            menu.Update(gameTime);

            if(levelHUD !=null)
            levelHUD.Update(gameTime);

            if(endScreen != null)
            endScreen.Update(gameTime);
        }
        #endregion

        #region Create Widget
        //Bind score to a widget text object
        public void BindPlayerScore(WidgetText text)
        {
            text.Text = scoreManager.Score.ToString();
        }

        //Bind the number of player special attack to a widget text object
        public void BindPlayerSpecialAttack(WidgetText text)
        {
            text.Text = player.Special.ToString();
        }

        public void CreateMenuWidgets()
        {
            //Create widget object
            menu = new Widget(this);
            //add an image to the widget, with default setting (full screen)
            menu.AddWidgetImage(menuContent.MenuTexture);
        }

        //Create HUD for the level
        public void CreateLevelWidgets()
        {
            //create widget object
            levelHUD = new Widget(this);

            //create widget text object
            WidgetText score = levelHUD.AddWidgetText("Score", new Vector2(SCREEN_WIDTH/2 , 50), new Vector2(0.75f));
            //bind the text value to player score
            score.BindText += BindPlayerScore;

            //create widget text object
            WidgetText special = levelHUD.AddWidgetText("SpecialAttack", new Vector2(SCREEN_WIDTH - 150, 50), new Vector2(0.75f));
            //bind the text value to the number of remaining special attack
            special.BindText += BindPlayerSpecialAttack;
        }

        //Create widgets for the end screen
        public void CreateEndScreenWidgets()
        {
            //Create widget object
            endScreen = new Widget(this);

            //Add background image, with default setting (full screen)
            endScreen.AddWidgetImage(endScreenContent.LeaderBoardTexture);

            //Add high scores
            for (int i = 0; i < scoreManager.LeaderBoard.Scores.Count; i++) {
                string score = scoreManager.LeaderBoard.Scores[i].ToString();
                WidgetText highScore = endScreen.AddWidgetText(score, new Vector2(SCREEN_WIDTH / 2, 475 + i * 85), new Vector2(0.75f));
            }           
        }
        #endregion

        #region
        public void DestroyWidget(Widget w)
        {

        }
        #endregion

        #region Draw widgets for scene
        public void DrawMenu()
        {
            DrawWidget(this.menu);
        }

        public void DrawLevelHUD()
        {
            DrawWidget(this.levelHUD);
        }

        public void DrawEndScreen()
        {
            DrawWidget(this.endScreen);
        }
        #endregion

        #region Draw Widget item
        //Draw every images and texts of the widget
        public void DrawWidget(Widget w)
        {
            //Draw Images before texts
            spriteBatch.Begin();
            DrawWidgetImages(w);
            DrawWidgetTexts(w);
            spriteBatch.End();
        }


        //Draw the current image of the widget
        public void DrawWidgetImage(Widget w)
        {
            //Apply image setting and draw image to the screen
            spriteBatch.Draw(w.Image.Texture, w.Image.Position, null, Color.AliceBlue, w.Image.Rotation, w.Image.Origin, w.Image.Scale, SpriteEffects.None, 0);
        }

        //Draw every image of the widget
        public void DrawWidgetImages(Widget w)
        {
            //For every image in the widget, Apply image setting and draw image to the screen
            foreach (WidgetImage item in w.Images)
            {
                spriteBatch.Draw(item.Texture, item.Position, null, Color.AliceBlue, item.Rotation, item.Origin, item.Scale, SpriteEffects.None, 0);
            }
        }

        //Draw the current text of the widget
        public void DrawWidgetText(Widget w)
        {
            //calculate the size of the text, left shift for half of the size = centered alignment
            Vector2 size = spriteFont.MeasureString(w.Text.Text) * w.Text.Scale;
            //Apply setting and draw string on the screen
            spriteBatch.DrawString(null, w.Text.Text, (w.Text.Position - size/2) , w.Text.Colour, 0.0f, w.Text.Origin, w.Text.Scale, SpriteEffects.None, 1);
        }

        //Draw every widget text of the widget
        public void DrawWidgetTexts(Widget w)
        {
            foreach (WidgetText item in w.Texts)
            {
                //calculate the size of the text, left shift for half of the size = centered alignment
                Vector2 size = spriteFont.MeasureString(item.Text) * item.Scale;

                //For every text in the widget, Apply text setting and draw text to the screen
                spriteBatch.DrawString(spriteFont, item.Text, item.Position - size/2, Color.Black, 0.0f, item.Origin, item.Scale, SpriteEffects.None, 1);
            }
        }


        #endregion
    }
}
