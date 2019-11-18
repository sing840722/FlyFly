using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaseCameraSample
{
    public class PlayerController
    {
        public delegate void ChangeHandler(object sender, PlayerEventArgs e);
        public event ChangeHandler ChangeEvent;

        public void FireChangeEvent(PlayerEventArgs e)
        {
            if (ChangeEvent != null)
            {
                ChangeEvent(this, e);
            }
        }


        
        public PlayerController()
        {

        }

        public void Initialise()
        {

        }


        public void MoveLeft(float amount)
        {
            FireChangeEvent(new PlayerEventArgs(PlayerEventTypes.MOVE_LEFT, amount));
        }

        public void MoveRight(float amount)
        {
            FireChangeEvent(new PlayerEventArgs(PlayerEventTypes.MOVE_RIGHT, amount));
        }

        public void MoveUp(float amount)
        {
            FireChangeEvent(new PlayerEventArgs(PlayerEventTypes.MOVE_UP, amount));
        }

        public void MoveDown(float amount)
        {
            FireChangeEvent(new PlayerEventArgs(PlayerEventTypes.MOVE_DOWN, amount));
        }

        public void SpecialAttack(float amount)
        {
            FireChangeEvent(new PlayerEventArgs(PlayerEventTypes.SPECIAL_ATTACK, amount));
        }
    }
}
