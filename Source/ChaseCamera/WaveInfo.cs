using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public class WaveInfo
    {
        private int hp;
        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }

        private WavePattern wavePatter;
        public WavePattern WavePattern
        {
            get { return wavePatter; }
            set { wavePatter = value; }
        }

        private float velocity;
        public float Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        private bool attack;
        public bool Attack
        {
            get { return attack; }
            set { attack = value; }
        }

        private TimeSpan attackRate;
        public TimeSpan AttackRate
        {
            get { return attackRate; }
            set { attackRate = value; }
        }

        

        public WaveInfo()
        {
            
        }

    }
}
