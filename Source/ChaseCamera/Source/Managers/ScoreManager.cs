using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace ChaseCameraSample
{
    public class ScoreManager
    {
        #region Event
        public delegate void ScoringHanlder(object sender, ScoreEventArgs e);
        public event ScoringHanlder ScoringEvent;

        public void FireScoringEvent(ScoreEventArgs e)
        {
            ScoringEvent(this, e);
        }
        #endregion


        #region Fields
        private SoundEffect scoreSoundEffect;
        public void SetSoundEffect(LevelContentManager lcm)
        {
            scoreSoundEffect = lcm.Score;
        }

        private int scoreToAdd;
        public int ScoreToAdd
        {
            get { return scoreToAdd; }
        }

        private Player player;
        public void SetPlayer(Player p)
        {
            player = p;
        }

        private SpriteFont font;
        public void SetFont(SpriteFont f)
        {
            font = f;
        }

        private int score;
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        private SpriteBatch spriteBatch;
        public void SetSpriteBatch(SpriteBatch sb)
        {
            spriteBatch = sb;
        }

        private SpriteFont spriteFont;
        public void SetSpriteFont(SpriteFont sf)
        {
            spriteFont = sf;
        }

        private Loader loader;
        public Loader InfoLoader
        {
            get { return loader; }
            set { loader = value; }
        }

        private LeaderBoard leaderBoard;
        public LeaderBoard LeaderBoard
        {
            get
            {
                UpdateLeaderBoard();
                return leaderBoard;
            }
        }
        #endregion

        public ScoreManager(GraphicsDevice gd, ContentManager cm)
        {
            spriteBatch = new SpriteBatch(gd);
            spriteFont = cm.Load<SpriteFont>("gameFontDS");

            ScoringEvent += OnPlayerScores;
            leaderBoard = new LeaderBoard();

        }

        private void LoadLeaderBoard()
        {
            loader.ReadLeaderBoard("Content/GameInfo/leaderBoard.xml");
            leaderBoard = loader.LeaderBoard;
        }

        //Sort Leaderboard from highest to lowest
        private void SortLeaderBoard()
        {
            leaderBoard.Scores.Sort();
            leaderBoard.Scores.Reverse();
        }

        //Add new high score to leaderboard
        public void UpdateLeaderBoard()
        {
            //Read leaderboard
            LoadLeaderBoard();

            //Finish the adding if it is not completed
            score += scoreToAdd;
            scoreToAdd = 0;

            //Check if the current score higher than the score in leaderboard
            for (int i = 0; i < leaderBoard.Scores.Count; i++)
            {
                int higher = 0;
                higher = Math.Max(score, leaderBoard.Scores[i]);
                //if current score is higher than the score in leaderboard
                if (higher == score)    
                {
                    //Add the score to leaderboard
                    leaderBoard.Scores.Add(higher);
                    
                    //Sort it in the order of highest to lowest
                    SortLeaderBoard();
                    
                    //Remove the 6th place from leaderboard if there is one
                    leaderBoard.Scores.RemoveRange(5, leaderBoard.Scores.Count - 5);

                    //Save it
                    WriteToLeaderBoard();

                    //Reset current score
                    score = 0;
                    return;
                }
            }

            //If current score does not hit the leaderboard, reset score
            score = 0;
        }


        private void WriteToLeaderBoard()
        {
            loader.WriteLeaderBoard(leaderBoard.Scores, "Content/GameInfo/leaderBoard.xml");
        }


        public void OnPlayerScores(object sender, ScoreEventArgs e)
        {
            AddScore(e.Amount);
            //update score widget, HOW?
            //audio manager.play score sound
        }

        public void Update(GameTime gameTime)
        {
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (scoreToAdd > 0)
            {
                score++;
                scoreToAdd--;
                scoreSoundEffect.Play();
            }
        }

        private void AddScore(int amount)
        {
            scoreToAdd += amount;
        }

    }
}
