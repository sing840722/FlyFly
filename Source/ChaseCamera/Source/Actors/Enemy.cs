using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaseCameraSample
{
    public class Enemy:Collidable
    {
        #region events
        //A delegate that handles the attack of enemy
        //An event that is fired everytime an enemy attack
        public delegate void EnemyAttackHandler(object sender, EventArgs e);
        public event EnemyAttackHandler EnemyAttackEvent;
        public void FireEnemyAttackEvent(EventArgs e)
        {
            if (EnemyAttackEvent != null)
            {
                EnemyAttackEvent(this, e);
            }
        }

        //A delegate that group the mathematic functions that are used to create the enemy movement i.e Trigonometry function
        public delegate double EnemyMovement(double x);
        public EnemyMovement movement; //= new EnemyMovement(Math.Sin);
        public EnemyMovement Movement
        {
            get { return movement; }
            set { movement = value; }
        }
        #endregion

        #region fields
        private string currentState;
        public string CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        private EnemyTypes enemyType;
        public EnemyTypes EnemyType
        {
            get { return enemyType; }
            set { enemyType = value; }
        }

        private bool onScreen;
        public bool OnScreen
        {
            get { return onScreen; }
            set { onScreen = value; }
        }

        private bool fleed;
        public bool Fleed
        {
            get { return fleed; }
            set { fleed = value; }
        }

        private bool finished;
        public bool Finished
        {
            get { return finished; }
            set { finished = value; }
        }

        private int hp;
        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }

        private int code;
        public int Code
        {
            get { return code; }
        }

        private float speed;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public void ReduceHP()
        {
            HP--;
        }

        private bool attack;
        public bool Attack
        {
            get { return attack; }
            set { attack = value; }
        }

        private int attackRate;
        public int AttackRate
        {
            get { return attackRate; }
            set
            {
                attackRate = value;
                if(attackRate != 0)
                fireTime = TimeSpan.FromMilliseconds(1000 / attackRate);
            }
        }

        private TimeSpan fireTime;
        private TimeSpan previousFireTime;


        private double t = 0;

        private Vector3 offsetPosition;
        public Vector3 OffsetPosition
        {
            get { return offsetPosition; }
            set { offsetPosition = value; }
        }

        private FSM fsm;
        private IdleState idle;
        private CombatState combat;
        private FleeState flee;
        #endregion

        public Enemy(EnemyTypes eType)
        {
            enemyType = eType;
        }

        #region Initialise
        public override void Initialise()
        {
            base.Initialise();
            HP = 3;
            active = true;
            angle = 180;
            RotateAround = new Vector3(0.0f, 1.0f, 0.0f);
            scale = 1.0f;
            height = 4500.0f;
            size = 7500.0f;
            code = RandomHelper.Rand() % 10;
            boundingBox = new BoundingBox();
            CalculateBoundingBox();
            fsm = new FSM(this);
            idle = new IdleState();
            combat = new CombatState();
            flee = new FleeState();
            fsm.AddState(idle);
            fsm.AddState(combat);
            fsm.AddState(flee);

            //idle transit to combat if enemy has enter the viewzone
            idle.AddTransition(new Transition(combat, () => onScreen));

            //combat transit to flee if enemy is about to die
            combat.AddTransition(new Transition(flee, () => HP < 2));
            Start();
        }
        #endregion

        public void Start()
        {
            fsm.Initialise("Idle");
        }


        public override void Update(GameTime gameTime)
        {
            elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            t = t + elapsed;

            fsm.Update(gameTime);

            base.Update(gameTime);  //last to call
        }

        #region Enemy AI
        //Simple moving to left, execute in FSM in the state 'idle'
        public void Idle()
        {
            position.Z = offsetPosition.Z += speed * elapsed;
        }

        //move to left, also move up and down if movement is defined
        //execute in FSM in the state 'combat'
        public void Combat()
        {
            position.Z = offsetPosition.Z += speed * elapsed;
            if (movement != null)
            {
                position.Y += offsetPosition.Y * 1.5f * (float)movement(2*t) * elapsed;
            }
            else
            {
                position.Y = offsetPosition.Y;
            }
            
        }

        //Attack, execute in FSM in state 'combat'
        public void AutoFire(GameTime gameTime)
        {
            //if enemy can attack
            if (!attack || attackRate == 0) { return; }

            //if the time passed since the previous fire is greater than the fire time(attack every (fireTime) second)
            if (gameTime.TotalGameTime - previousFireTime >= fireTime)
            {
                //set previous fire time
                previousFireTime = gameTime.TotalGameTime;

                //fire attack event
                FireEnemyAttackEvent(new EventArgs());
            }

        }

        //Flee if enemy only has 1 hp left, 
        //enemy will only move vertically to escape as soon as posible
        //execute in FSM in state 'flee'
        public void Flee()
        {
            //if enemy is closer to the top of the screen
            //move up
            if (position.Y > 0)
            {
                position.Y += speed * 2 * elapsed;
            }
            //else if enemy is closer to the bottom of the screen
            //move down
            else if (position.Y < 0)
            {
                position.Y -= speed * 2 * elapsed;
            }
            //else enemy is at the middle
            //randomly choose one direction to escape
            else
            {
                int i = RandomHelper.Rand() % 2;
                if (i == 0)
                {
                    position.Y -= speed * 2 * elapsed;
                }
                else if (i == 1)
                {
                    position.Y += speed * 2 * elapsed;
                }
            }
        }
        #endregion


        public override bool CollisionTest(Collidable obj)
        {
            return false;
        }

        public override void OnCollision(Collidable obj)
        {

        }


    }
}
