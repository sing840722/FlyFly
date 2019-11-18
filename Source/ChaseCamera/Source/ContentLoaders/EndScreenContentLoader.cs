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
    public class EndScreenContentManager
    {
        private Song backgroundMusic;
        public Song BackgroundMusic
        {
            get { return backgroundMusic; }
        }

        private Texture2D leaderBoardTexture;
        public Texture2D LeaderBoardTexture
        {
            get { return leaderBoardTexture; }
        }

        private ContentManager Content;
        private ChaseCameraGame game;

        public EndScreenContentManager(ChaseCameraGame game)
        {
            this.game = game;
            Initilise();
        }

        public void Initilise()
        {
            Content = new ContentManager(game.Services, "Content");
        }

        public void LoadSong()
        {
            backgroundMusic = Content.Load<Song>("Sound\\menuMusic");
        }

        public void LoadContent()
        {

        }

        public void LoadContents()
        {
            Initilise();
            leaderBoardTexture = Content.Load<Texture2D>("Menu\\leaderBoard");
            LoadSong();
        }

        public void UnloadContents()
        {
            //unload loaded content
            Content.Unload();

            //dispose created content
            Content.Dispose();
        }

    }

}
