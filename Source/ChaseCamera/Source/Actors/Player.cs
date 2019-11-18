#region File Description
//-----------------------------------------------------------------------------
// Ship.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
#endregion

namespace ChaseCameraSample
{
    public class Player : Collidable
    {
        #region Fields
        private readonly int MAX_NUM_SPECIAL = 3;

        private int score;
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public void AddScore(int i)
        {
            score += i;
        }

        private TimeSpan playerPreviousFireTime;

        private int special;
        public int Special
        {
            get { return special; }
            set { special = value; }
        }

        private TimeSpan coldDown;
        public TimeSpan ColdDown
        {
            get { return coldDown; }
            set { coldDown = value; }
        }

        private TimeSpan fireTime;
        public TimeSpan FireTime
        {
            get { return fireTime; }
            set { fireTime = value; }
        }

        private World gameWorld;
        public World GameWorld
        {
            get { return gameWorld; }
            set { gameWorld = value; }
        }
        private const float MinimumAltitude = 350.0f;

        protected float speed;

        public float Speed
        {
            get { return speed; }
        }

        public PlayerController playerController = new PlayerController();

        private int numberOfAttack;
        public int NumberOfAttack
        {
            get { return numberOfAttack; }
        }

        private int attackPerSecond;
        public int AttackPerSecond
        {
            get { return attackPerSecond; }
        }

        #endregion

        #region Initialization

        public Player()
        {
            DestroyEvent += OnDestroy;
        }

        //call again to reset
        public override void Initialise()   
        {
            base.Initialise();
            active = true;
            angle = -45;
            RotateAround = new Vector3(0.0f, .0f, 1.0f);
            scale = 1.0f;
            Position = new Vector3(10, 0, 30000);
            speed = 10000.0f;
            numberOfAttack = 1;
            attackPerSecond = 4;

            boundingBox = new BoundingBox();
            CalculateBoundingBox();

            height = 3750.0f;
            size = 7500.0f;
            
            fireTime = TimeSpan.FromMilliseconds(1000 / attackPerSecond);

            special = 3;
            coldDown = TimeSpan.FromSeconds(5);

            score = 0;

            AddPlayerController();
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            //move right, keep up with the camera and world boundary
            position.Z -= speed * elapsed;

            //check world boundary
            BoundaryCheck();

            //reload special attack
            ReloadSpecialAttack(gameTime);
            
            //Calculate boundary, update matrix etc
            base.Update(gameTime);  //last to call
        }

        public void ReloadSpecialAttack(GameTime gameTime)
        {
            //if special attack is not full
            if (special < MAX_NUM_SPECIAL)
            {
                //reload 1 attack every 5 second
                if (gameTime.TotalGameTime - playerPreviousFireTime > coldDown)
                {
                    playerPreviousFireTime = gameTime.TotalGameTime;
                    special++;
                }
            }
        }
        #endregion

        #region Respond to Events
        //Player Controller handles the movement and special attack
        //add event listener
        public void AddPlayerController()
        {
            playerController.ChangeEvent += OnChanged;
            playerController.ChangeEvent += gameWorld.EnemyManager.OnPlayerSpecialAttack;
        }

        //On player character destroy, remove event listeners
        public void OnDestroy(object sender, RemoveGameObjectEventArgs e)
        {
            playerController.ChangeEvent -= gameWorld.EnemyManager.OnPlayerSpecialAttack;
            playerController.ChangeEvent -= OnChanged;
        }

        //event listener on player controller make changes (key press)
        public void OnChanged(object sender, PlayerEventArgs e)
        {
            switch (e.PlayerEventType)
            {
                case PlayerEventTypes.MOVE_LEFT:
                    position.Z += speed * e.Amount * elapsed;
                    break;
                case PlayerEventTypes.MOVE_RIGHT:
                    position.Z -= speed * e.Amount *elapsed;
                    break;
                case PlayerEventTypes.MOVE_UP:
                    angle -= 45.0f * elapsed;
                    position.Y += speed * e.Amount * elapsed;    //no X
                    break;
                case PlayerEventTypes.MOVE_DOWN:
                    angle += 45.0f * elapsed;
                    position.Y -= speed * e.Amount * elapsed;
                    break;
                case PlayerEventTypes.SPECIAL_ATTACK:
                    //play audio
                    //play animation
                    break;
                default:
                    break;
            }
        }

        //event listener on powerup (collected power up)
        public void OnPowerUp(object sender, PowerUpEventArgs e)
        {
            switch (e.PowerUpEventType)
            {
                case PowerUpTypes.DOUBLE_ATTACK:
                    //increase the number of bullet fire at once, maximum 4 bullet can be fire at once
                    numberOfAttack = Math.Min(4, numberOfAttack+1);
                    break;
                case PowerUpTypes.SPEED_ATTACK:
                    //increase the number of attack can be done in 1 second
                    //maximum 6 attack can be done in 1 second
                    attackPerSecond = Math.Min(6, attackPerSecond+1);
                    fireTime = TimeSpan.FromMilliseconds(1000 / attackPerSecond);
                    break;
                default:
                    break;
            }
        }
        #endregion



        #region Collision
        public override bool CollisionTest(Collidable obj)
        {
            if (obj != null)
            {
                //Check for bounding box intersection
                return BoundingBox.Intersects(obj.BoundingBox);
            }

            return false;
        }

        public override void OnCollision(Collidable obj)
        {
            //if player is already destroy, return
            if (!active) return;
            
            //if collide with an enemy, destroy player
            if (obj.GetType() == typeof(Enemy))
            {
                this.active = false;
                FireDestroyEvent(new RemoveGameObjectEventArgs(0));
            }
            //if collide with a projectile from enemy, destory player
            if (obj.GetType() == typeof(EnemyProjectile))
            {
                this.active = false;
                FireDestroyEvent(new RemoveGameObjectEventArgs(0));
            }
            //if ocllide with a power up, remove the object from the world, 
            //event will be sent to here from powerup manager
            if (obj.GetType() == typeof(PowerUp))
            {
                PowerUp pu = obj as PowerUp;    //if cast succeessed
                if (pu != null)
                {
                    pu.Active = false;
                }
            }
        }

        //Prevent player get out of the viewzone,
        //also limiting the rotation of player character
        private void BoundaryCheck()
        {
            if (position.Z > gameWorld.WorldBoundLeft - size/2)
            {
                position.Z = gameWorld.WorldBoundLeft - size/2;
            }

            else if (position.Z < gameWorld.WorldBoundRight + size/4)
            {
                position.Z = gameWorld.WorldBoundRight + size/4;
            }

            if (position.Y > gameWorld.WorldBoundTop - height/2)
            {
                position.Y = gameWorld.WorldBoundTop - height/2;
            }
            
            else if (position.Y < gameWorld.WorldBoundBot )
            {
                position.Y = gameWorld.WorldBoundBot;
            }

            if (angle < -45)
            {
                angle = -45;
            }
            else if (angle > 0)
            {
                angle = 0;
            }
        }

        #endregion

        #region keypress
        public void MoveUp(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN || buttonState == eButtonState.PRESSED)
            {
                playerController.MoveUp(1.0f);
            }
        }

        public void MoveDown(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN || buttonState == eButtonState.PRESSED)
            {
                playerController.MoveDown(1.0f);
            }
        }

        public void MoveLeft(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN || buttonState == eButtonState.PRESSED)
            {
                playerController.MoveLeft(1.0f);
            }
        }

        public void MoveRight(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN || buttonState == eButtonState.PRESSED)
            {
                playerController.MoveRight(1.0f);
            }
        }

        public void EnterNumber0(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN && special > 0)
            {
                special--;
                playerController.SpecialAttack(0);
            }
        }

        public void EnterNumber1(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN && special > 0)
            {
                special--;
                playerController.SpecialAttack(1);
            }
        }

        public void EnterNumber2(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN && special > 0)
            {
                special--;
                playerController.SpecialAttack(2);
            }
        }

        public void EnterNumber3(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN && special > 0)
            {
                special--;
                playerController.SpecialAttack(3);
            }
        }

        public void EnterNumber4(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN && special > 0)
            {
                special--;
                playerController.SpecialAttack(4);
            }
        }

        public void EnterNumber5(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN && special > 0)
            {
                special--;
                playerController.SpecialAttack(5);
            }
        }

        public void EnterNumber6(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN && special > 0)
            {
                special--;
                playerController.SpecialAttack(6);
            }
        }

        public void EnterNumber7(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN && special > 0)
            {
                special--;
                playerController.SpecialAttack(7);
            }
        }

        public void EnterNumber8(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN && special > 0)
            {
                special--;
                playerController.SpecialAttack(8);
            }
        }

        public void EnterNumber9(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN && special > 0)
            {
                special--;
                playerController.SpecialAttack(9);
            }
        }
        #endregion
    }
}
