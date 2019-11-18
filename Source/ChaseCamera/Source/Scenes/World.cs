using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Reflection;

namespace ChaseCameraSample
{
    public class World:Scene
    {


        #region field
        private List<IManager> managers = new List<IManager>();

        private ScoreManager scoreManager;


        private LevelContentManager levelContentManager;


        private CollisionManager collisionManager;


        private ProjectileManager projectileManager;


        private BackgroundManager backgroundManager;

        private EnemyManager enemyManager;
        public EnemyManager EnemyManager
        {
            get { return enemyManager; }
        }

        private EnemyProjectileManager enemyProjectileManager;

        public void SetEnemyManager(EnemyManager em)
        {
            enemyManager = em;
            enemyManager.SetWorld(this);
            enemyManager.SetCollisionManager(this.collisionManager);
        }

        private PowerUpManager powerUpManager;
        public void SetPowerUpManager(PowerUpManager pum)
        {
            powerUpManager = pum;
            powerUpManager.SetWorld(this);
            powerUpManager.SetCollisionManager(this.collisionManager);
        }

        private Loader loader;
        private int waveIndex;

        private float worldBoundLeft;
        public float WorldBoundLeft
        {
            get { return worldBoundLeft; }
        }

        private float worldBoundTop;
        public float WorldBoundTop
        {
            get { return worldBoundTop; }
        }

        private float worldBoundBot;
        public float WorldBoundBot
        {
            get { return worldBoundBot; }
        }

        private float worldBoundRight;
        public float WorldBoundRight
        {
            get { return worldBoundRight; }
        }

        private Player player;
        public Player Player
        {
            get { return player; }
        }


        public void SetPlayer(Player p)
        {
            player = p;
            player.GameWorld = this;
        }

        private EnvironmentInfo environmentInfo;
        public EnvironmentInfo EnvironmentInfo
        {
            get { return environmentInfo; }
        }

        private Camera camera;

        #endregion

        #region Initialise
        public World(
            ChaseCameraGame main,
            Loader l,
            LevelContentManager lcm,
            CollisionManager cm,
            PowerUpManager pum,
            BackgroundManager bgm,
            ProjectileManager pm,
            EnemyProjectileManager epm,
            EnemyManager em,
            ScoreManager sm,
            Camera cam):base(main)
        {
            //Store the refernce to variables
            loader = l;
            levelContentManager = lcm;
            collisionManager = cm;
            powerUpManager = pum;
            backgroundManager = bgm;
            enemyProjectileManager = epm;
            projectileManager = pm;
            enemyManager = em;
            scoreManager = sm;
            camera = cam;

            //Add mangers to a list for iteration
            managers.Add(collisionManager);
            managers.Add(enemyManager);
            managers.Add(enemyProjectileManager);
            managers.Add(projectileManager);
            managers.Add(backgroundManager);
            managers.Add(powerUpManager);

            //Initialise Player
            AddNewPlayer();

            //Set references for managers
            powerUpManager.SetWorld(this);
            powerUpManager.SetCollisionManager(this.collisionManager);

            projectileManager.SetWorld(this);
            projectileManager.SetCollisionManager(this.collisionManager);

            enemyManager.SetWorld(this);
            enemyManager.SetCollisionManager(this.collisionManager);
            enemyManager.SetScoreManager(scoreManager);
            enemyManager.SetProjectileManager(this.enemyProjectileManager);

            backgroundManager.SetCamera(camera);

            scoreManager.SetPlayer(player);

            powerUpManager.SetScoreManager(scoreManager);

            //Name of this scene
            name = "Level";
        }

        public override void Initialise()
        {
            base.Initialise();  //send created event;

            InitialiseLevel();
            //Set content after they are loaded
            //Contents are loaded after base Initialise
            //Actual execution in Main.cs (ChaseCameraGame.cs OnSceneLoad)
            enemyManager.SetHelicopter(levelContentManager);
            enemyManager.SetOldPlane(levelContentManager);
            enemyManager.SetSoundEffect(levelContentManager);

            enemyProjectileManager.SetSoundEffect(levelContentManager);

            scoreManager.SetSoundEffect(levelContentManager);

            projectileManager.SetSoundEffect(this.levelContentManager);

            powerUpManager.SetPowerUp(levelContentManager);

            waveIndex = 1;

            //Load Environment info and set up the world
            LoadEnvironmentInfo();

            //Load waves info and spawn game object
            LoadWaves();
        }

        public void InitialiseLevel()
        {
            foreach (IManager manager in managers)
            {
                manager.Initialise();
            }

            player.Initialise();
            collisionManager.AddCollidable(player);
            camera.Initialise();

        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            //world boundary follows the camera, so player does not get out of the viewzone
            worldBoundLeft -= environmentInfo.CameraRollingSpeed * elapsed;
            worldBoundRight -= environmentInfo.CameraRollingSpeed * elapsed;

            //Update player
            player.Update(gameTime);
            
            //Update every manager in the list
            foreach (IManager manager in managers)
            {
                manager.Update(gameTime);
            }
        }
        #endregion



        public void AddNewPlayer()
        {
            player = new Player();
            player.GameWorld = this;
            player.Initialise();
            projectileManager.SetPlayer(this.player);
        }

        #region Load and parse game info
        private void LoadEnvironmentInfo()
        {
            string file = "Content/GameInfo/environmentInfo.xml";
            environmentInfo = loader.ReadEnvironmentInfo(file);

            //try catch
            if (environmentInfo == null)
            {
                Console.Write("Environment Info is not copied \n");
            }
            else
            {
                ParseEnvironmentInfo();
            }
        }

        private void ParseEnvironmentInfo()
        {
            camera.SetCameraRollingSpeed(environmentInfo.CameraRollingSpeed);
            worldBoundLeft = environmentInfo.WorldBoundLeft;
            worldBoundRight = environmentInfo.WorldBoundRight;
            worldBoundTop = environmentInfo.WorldBoundTop;
            worldBoundBot = environmentInfo.WorldBoundBot;
        }

        private void LoadWaves(int start, int end)
        {
            waveIndex = start;
            for (int i = start; i <= end; i++, waveIndex++)
            {
                if (loader != null)
                {
                    string wave = string.Format("Content/GameInfo/Wave/wave{0}.xml", waveIndex);
                    loader.ReadWaveInfo(wave);
                }
                else
                {
                    loader = new Loader();
                    string wave = string.Format("Content/GameInfo/Wave/wave{0}.xml", waveIndex);
                    loader.ReadWaveInfo(wave);
                }

                ParseWaveInfo();
            }
            waveIndex = 1;
        }

        public void LoadWaves()
        {
            Console.Write("current wave: " + waveIndex + "\n");
            string wave = string.Format("Content/GameInfo/Wave/wave{0}.xml", waveIndex);
            if (loader.ReadWaveInfo(wave))
            {
                ParseWaveInfo();
                waveIndex++;
            }
            else
            {
                //Console.Write("game finish");
                Player.Active = false;
                Player.FireDestroyEvent(new RemoveGameObjectEventArgs(0));
                //wait and show victory
                //or boss
            }
        }

        private void ParseWaveInfo()
        {
            int width = loader.WaveInfo.WavePattern.Width;
            int height = loader.WaveInfo.WavePattern.Height;

            List<string> lines;// = new List<string>();
            lines = loader.WaveInfo.WavePattern.Patterns;

            //read every charcter and spawn the correspond object to the world
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    char tileType = lines[y][x];
                    LoadData(tileType, (1 - x) * waveIndex, 1 - (y - 2));
                }
            }
            
        }

        private void LoadData(char tileType, int x, int y)
        {
            switch (tileType)
            {
                case '.':
                    //do nothing
                    break;
                case 'A':   //OLD_PLANE that moves in different direction
                    CreateAggressiveEnemy(x, y, EnemyTypes.OLD_PLANE);
                    break;
                case 'a':   //OLD_PLANE that moves in one direction
                    CreatePassiveEnemy(x, y, EnemyTypes.OLD_PLANE);
                    break;
                case 'B':   //HELICOPTER that moves in one direction
                    CreateAggressiveEnemy(x, y , EnemyTypes.HELICOPTER);
                    break;
                case 'b':   //HELICOPTER that moves in one direction
                    CreatePassiveEnemy(x, y, EnemyTypes.HELICOPTER);
                    break;
                case 'P':
                    CreatePowerUp(x, y, PowerUpTypes.DOUBLE_ATTACK);
                    break;
                case 'Q':
                    CreatePowerUp(x, y, PowerUpTypes.SPEED_ATTACK);
                    break;
                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
                    //break;
            }
        }
        #endregion

        #region Spawn gameObject
        private void CreateAggressiveEnemy(int x, int y, EnemyTypes enemyType)
        {
            float waveZOffset = 6000;
            float waveYOffset = 5000;
            float waveOffset = 30000;
            float pX = 10;
            float pY = y * waveYOffset;
            float pZ = camera.Position.Z  + ((x * waveZOffset) - (waveIndex * waveOffset));
            Enemy enemy = new Enemy(enemyType);
            enemy.Initialise();
            enemy.OffsetPosition = new Vector3(pX, pY, pZ);
            enemy.Speed = loader.WaveInfo.Velocity;
            enemy.HP = loader.WaveInfo.HP;
            enemy.Attack = loader.WaveInfo.Attack;
            enemy.AttackRate = loader.WaveInfo.AttackRate;

            foreach (Enemy.EnemyMovement movement in loader.WaveInfo.GetWaveMovement())
            {
                enemy.Movement += movement;
                //amtitude --?
            }

            enemyManager.AddEnemy(enemy);
        }

        private void CreatePassiveEnemy(int x, int y, EnemyTypes enemyType)
        {
            float waveZOffset = 6000;
            float waveYOffset = 6000;
            float waveOffset = 20000;
            float pX = 10;
            float pY = y * waveYOffset;
            float pZ = camera.Position.Z + ((x * waveZOffset) - (waveIndex * waveOffset));
            Enemy enemy = new Enemy(enemyType);
            enemy.Initialise();
            enemy.OffsetPosition = new Vector3(pX, pY, pZ);
            enemy.Speed = loader.WaveInfo.Velocity;
            enemy.HP = loader.WaveInfo.HP;
            enemy.Attack = loader.WaveInfo.Attack;
            enemy.AttackRate = loader.WaveInfo.AttackRate;
            enemyManager.AddEnemy(enemy);
        }

        private void CreatePowerUp(int x, int y, PowerUpTypes powerUpType)
        {
            float waveZOffset = 6000;
            float waveYOffset = 6500;
            float waveOffset = 30000;
            float pX = 10;
            float pY = y + y * waveYOffset;
            float pZ = camera.Position.Z + (x + x * waveZOffset) - (waveIndex * waveOffset);

            PowerUp power = new PowerUp(powerUpType);
            power.Initialise();
            power.Position = new Vector3(pX, pY, pZ);
            powerUpManager.AddPowerUp(power);           
        }
        #endregion
    }
}
