#region File Description
//-----------------------------------------------------------------------------
// Game.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
#endregion

namespace ChaseCameraSample
{
    public class ChaseCameraGame : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        World gameWorld;
        Menu gameMenu;
        EndScreen endScreen;
        Camera camera;

        DebugDraw debug;

        //managers for game world;
        SceneManager sceneManager;
        CommandManager commandManager = new CommandManager();
        CollisionManager collisionManager = new CollisionManager();
        ProjectileManager projectileManager = new ProjectileManager();
        EnemyProjectileManager enemyProjectileManager = new EnemyProjectileManager();
        BackgroundManager backgroundManager = new BackgroundManager();
        PowerUpManager powerUpManager = new PowerUpManager();
        EnemyManager enemyManager = new EnemyManager();
        ScoreManager scoreManager;

        MenuContentManager menuContents;
        LevelContentManager levelContents;
        EndScreenContentManager endScreenContents;
        Loader gameInfoLoader = new Loader();

        WidgetManager widgetManager;

        //Manager for menus

        #endregion

        #region Initialization
        public ChaseCameraGame()
        {
            //set up window
            graphics = new GraphicsDeviceManager(this);
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            //get root directory
            Content.RootDirectory = "Content";

            //Set up content managers
            menuContents = new MenuContentManager(this);
            levelContents = new LevelContentManager(this);
            endScreenContents = new EndScreenContentManager(this);

            //IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            widgetManager = new WidgetManager(graphics.GraphicsDevice, menuContents, levelContents, endScreenContents);
            widgetManager.AddSpriteFont(spriteFont);


            scoreManager = new ScoreManager(GraphicsDevice, Content);
            scoreManager.InfoLoader = gameInfoLoader;


            camera = new Camera();
            camera.AspectRatio = (float)graphics.GraphicsDevice.Viewport.Width /graphics.GraphicsDevice.Viewport.Height;

            debug = new DebugDraw(GraphicsDevice);

            //Craete scenes
            gameMenu = new Menu(this);
            gameWorld = new World(this, 
               gameInfoLoader, levelContents,
               collisionManager, powerUpManager,
               backgroundManager, projectileManager, 
               enemyProjectileManager, enemyManager, 
               scoreManager, camera
               );
            endScreen = new EndScreen(this);


            widgetManager.SetSpriteBatch(spriteBatch);
            widgetManager.SetPlayer(gameWorld.Player);
            widgetManager.SetScoreManager(scoreManager);
            //Create scene manager with scenes
            sceneManager = new SceneManager(gameMenu, gameWorld, endScreen);
            sceneManager.Start();
        }
        #endregion

        #region Scene Event Listener
        //Load content, create widget (2D stuff), Setup keyBinding
        public void OnLoadScene(object sender, SceneEventArgs e)
        {
            switch (e.Name)
            {
                case "Menu":
                    menuContents.LoadContents();
                    widgetManager.CreateMenuWidgets();
                    SetMenuKeyBinding();
                    MediaPlayer.Play(menuContents.BackgroundMusic);
                    break;
                case "Level":
                    levelContents.LoadContents();
                    widgetManager.CreateLevelWidgets();
                    SetLevelKeyBinding();
                    MediaPlayer.Play(levelContents.BackgroundMusic);
                    break;
                case "EndScreen":
                    endScreenContents.LoadContents();
                    widgetManager.CreateEndScreenWidgets();
                    scoreManager.UpdateLeaderBoard();
                    SetEndScreenKeyBinding();
                    MediaPlayer.Play(endScreenContents.BackgroundMusic);
                    break;
                default:
                    break;
            }
        }

        //Unload content
        //Can do audio.play etc
        public void OnDestroyScene(object sender, SceneEventArgs e)
        {
            //Console.Write("EXIT: " + e.Name + "\n");
            switch (e.Name)
            {
                case "Menu":
                    //MediaPlayer.Stop();
                    widgetManager.DestroyWidget(widgetManager.Menu);
                    menuContents.UnloadContents();
                    break;
                case "Level":
                    //MediaPlayer.Stop();
                    widgetManager.DestroyWidget(widgetManager.LevelHUD);
                    levelContents.UnloadContents();
                    break;
                case "EndScreen":
                    widgetManager.DestroyWidget(widgetManager.EndScreen);
                    endScreenContents.UnloadContents();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Content Loader (Content that is needed for every scene)
        //Load content that is needed for every scene
        protected override void LoadContent()   //contents used for every scene
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("gameFontDS");
        }
        #endregion

        #region KeyBindings
        public void SetMenuKeyBinding()
        {
            commandManager.RemoveKeyboardBindings();
            commandManager.AddKeyboardBinding(Keys.Escape, StopGame);
            commandManager.AddKeyboardBinding(Keys.Enter, sceneManager.GotoNextScene);
        }

        public void SetLevelKeyBinding()
        {
            commandManager.RemoveKeyboardBindings();
            commandManager.AddKeyboardBinding(Keys.Escape, StopGame);
            commandManager.AddKeyboardBinding(Keys.W, gameWorld.Player.MoveUp);
            commandManager.AddKeyboardBinding(Keys.S, gameWorld.Player.MoveDown);
            commandManager.AddKeyboardBinding(Keys.A, gameWorld.Player.MoveLeft);
            commandManager.AddKeyboardBinding(Keys.D, gameWorld.Player.MoveRight);
            commandManager.AddKeyboardBinding(Keys.NumPad0, gameWorld.Player.EnterNumber0);
            commandManager.AddKeyboardBinding(Keys.NumPad1, gameWorld.Player.EnterNumber1);
            commandManager.AddKeyboardBinding(Keys.NumPad2, gameWorld.Player.EnterNumber2);
            commandManager.AddKeyboardBinding(Keys.NumPad3, gameWorld.Player.EnterNumber3);
            commandManager.AddKeyboardBinding(Keys.NumPad4, gameWorld.Player.EnterNumber4);
            commandManager.AddKeyboardBinding(Keys.NumPad5, gameWorld.Player.EnterNumber5);
            commandManager.AddKeyboardBinding(Keys.NumPad6, gameWorld.Player.EnterNumber6);
            commandManager.AddKeyboardBinding(Keys.NumPad7, gameWorld.Player.EnterNumber7);
            commandManager.AddKeyboardBinding(Keys.NumPad8, gameWorld.Player.EnterNumber8);
            commandManager.AddKeyboardBinding(Keys.NumPad9, gameWorld.Player.EnterNumber9);
        }
        public void SetEndScreenKeyBinding()
        {
            commandManager.RemoveKeyboardBindings();
            commandManager.AddKeyboardBinding(Keys.Escape, StopGame);
            commandManager.AddKeyboardBinding(Keys.Enter, sceneManager.GotoNextScene);
        }
        #endregion

        #region Game Actions
        public void StopGame(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                Exit();
            }
        }

        #endregion

        #region Update and Draw
        protected override void Update(GameTime gameTime)
        {
            commandManager.Update();
            camera.Update(gameTime);
            sceneManager.Update(gameTime);
            widgetManager.Update(gameTime);
            scoreManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice device = graphics.GraphicsDevice;

            device.Clear(Color.CornflowerBlue);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            switch (sceneManager.StateMachine.CurrentState.Name)
            {
                case "InMenu":
                    DrawMenu();
                    break;
                case "InGame":
                    DrawLevel();
                    break;
                case "InEndScreen":
                    DrawEndScreen();
                    break;
                default:
                    break;
            }

            base.Draw(gameTime);
        }

        private void DrawMenu()
        {
            //Draw 3D
            //draw 3d object
            //draw 3d object

            //Draw 2D
            widgetManager.DrawMenu();
        }

        private void DrawLevel()
        {
            //Draw background
            foreach (Background background in backgroundManager.Layers)
            {
                DrawModel(levelContents.GroundModel, background.ModelViewMatrix);
            }

            //draw player
            DrawModel(levelContents.PlayerModel, gameWorld.Player.ModelViewMatrix);

            //draw enemy
            foreach (Enemy enemy in enemyManager.Enemies)
            {
                enemyManager.ChangeSkin(enemy.EnemyType, enemy.Code);
                DrawModel(enemyManager.ChangeModel(enemy.EnemyType), enemy.ModelViewMatrix);
            }

            //draw projectile
            foreach (Projectile projectile in projectileManager.PlayerProjectiles)
            {
                DrawModel(levelContents.ProjectileModel, projectile.ModelViewMatrix);
            }

            //draw powerup
            foreach (PowerUp powerUp in powerUpManager.PowerUps)
            {
                powerUpManager.ChangeSkin(powerUp.BonusEffect);
                DrawModel(levelContents.PowerUpModel, powerUp.ModelViewMatrix);
            }

            //draw enemy projectile
            foreach (EnemyProjectile projectile in enemyProjectileManager.EnemyProjectiles)
            {
                DrawModel(levelContents.EnemyProjectileModel, projectile.ModelViewMatrix);
            }

            //Draw 2D stuff
            widgetManager.DrawLevelHUD();

            ///////////DEBUG////////////////
            ////////Draw bounding box///////
            //////Draw World Boundary//////
            /*
            debug.Begin(camera.View, camera.Projection);
            debug.DrawWireBox(gameWorld.Player.BoundingBox, Color.Red);

            foreach (Enemy enemy in enemyManager.Enemies)
            {
                debug.DrawWireBox(enemy.BoundingBox, Color.Pink);
                //scoreManager.DrawEnemyNumber(enemy);
            }

            foreach (Projectile projectile in projectileManager.PlayerProjectiles)
            {
                debug.DrawWireBox(projectile.BoundingBox, Color.Purple);
            }

            foreach (PowerUp powerUp in powerUpManager.PowerUps)
            {
                debug.DrawWireBox(powerUp.BoundingBox, Color.Purple);
            }


            Vector3 leftTop = new Vector3(100, gameWorld.WorldBoundTop, gameWorld.WorldBoundLeft);
            Vector3 leftBot = new Vector3(100, gameWorld.WorldBoundBot, gameWorld.WorldBoundLeft);
            Vector3 rightTop = new Vector3(100, gameWorld.WorldBoundTop, gameWorld.WorldBoundRight);
            Vector3 rightBot = new Vector3(100, gameWorld.WorldBoundBot, gameWorld.WorldBoundRight);
            debug.DrawLine(leftTop, leftBot, Color.Red);            //draw left world bound
            debug.DrawLine(rightTop, rightBot, Color.Red);          //drwa right world bound
            debug.DrawLine(leftTop, rightTop, Color.Red);          //drwa top world bound
            debug.DrawLine(leftBot, rightBot, Color.Red);          //drwa bot world bound
            debug.End();
            */
            ////////////END////////////////////////////
        }

        private void DrawEndScreen()
        {
            //Draw 3D
            //draw 3d object
            //draw 3d object

            //Draw 2D
            widgetManager.DrawEndScreen();
        }


        private void DrawModel(Model model, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();                    
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                }
                mesh.Draw();
            }
        }
        #endregion
    }


    #region Entry Point
    static class Program
    {
        static void Main()
        {
            using (ChaseCameraGame game = new ChaseCameraGame())
            {
                game.Run();
            }
        }
    }

    #endregion
}
