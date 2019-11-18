using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ChaseCameraSample
{
    public class MenuContentManager:SceneContentManager
    {
        private Song backgroundMusic;
        public Song BackgroundMusic
        {
            get { return backgroundMusic; }
        }

        private ChaseCameraGame game;

        private Texture2D menuTexture;
        public Texture2D MenuTexture
        {
            get { return menuTexture; }
        }

        private ContentManager Content;
        public void SetContentManager()
        {
            
        }

        public MenuContentManager(ChaseCameraGame ccg)
        {
            game = ccg;
            Initilise();
            //Content.RootDirectory = "Content";
        }

        public void Initilise()
        {
            Content = new ContentManager(game.Services, "Content");
        }

        public void DrawMenu(SpriteBatch sb)
        {
            sb.Draw(menuTexture, new Vector2(0, 0), Color.AliceBlue);
        }

        public void LoadContent()
        {

        }

        public void LoadTextures()
        {
            menuTexture = Content.Load<Texture2D>("Menu\\menu");
        }

        public void LoadSound()
        {
            backgroundMusic = Content.Load<Song>("Sound\\menuMusic");
        }

        public void LoadContents()
        {
            Initilise();
            LoadTextures();
            LoadSound();
        }

        public void UnloadContents()
        {
            //unload content
            Content.Unload();
            //dispose created content
            Content.Dispose();
        }

    }
}
