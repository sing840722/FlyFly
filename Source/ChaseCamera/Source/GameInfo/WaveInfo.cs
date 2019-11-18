using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public class WaveInfo
    {
        private HashSet<Enemy.EnemyMovement> waveMovement = new HashSet<Enemy.EnemyMovement>();
        public HashSet<Enemy.EnemyMovement> GetWaveMovement()
        {
            waveMovement.Clear();
            CreateWaveMovement();
            return waveMovement;
        }
        
        public void CreateWaveMovement()
        {
            foreach (string s in movements)
            {
                switch (s)
                {
                    case "Sin":

                        waveMovement.Add(Math.Sin);
                        break;
                    case "Cos":

                        waveMovement.Add(Math.Cos);
                        break;
                    case "Tan":

                        waveMovement.Add(Math.Tan);
                        break;
                    default:
                        break;
                }
            }
        }

        private List<string> movements;
        public List<string> Movements
        {
            get { return movements; }
            set
            {
                movements = value;
            }
        }

        private int hp;
        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }

        private WavePattern wavePattern = new WavePattern();
        public WavePattern WavePattern
        {
            get { return wavePattern; }
            set { wavePattern = value; }
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

        private int attackRate;
        public int AttackRate
        {
            get { return attackRate; }
            set { attackRate = value; }
        }


        public WaveInfo()
        {
           
        }

    }
}
