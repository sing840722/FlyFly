using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaseCameraSample
{
    public enum PlayerEventTypes
    {
        MOVE_LEFT,
        MOVE_RIGHT,
        MOVE_UP,
        MOVE_DOWN,
        SPECIAL_ATTACK
    }

    public enum PowerUpTypes
    {
        DOUBLE_ATTACK,
        SPEED_ATTACK,
    }

    public enum EnemyTypes
    {
        OLD_PLANE,
        HELICOPTER,
    }

    public class EnemyAttackEventArgs : EventArgs
    {
        
    }

    public class ScoreEventArgs : EventArgs
    {
        private int amount;
        public int Amount
        {
            get { return amount; }
        }
        public ScoreEventArgs(int a)
        {
            amount = a;
        }
    }

    public class RemoveGameObjectEventArgs : EventArgs
    {
        private int index;
        public int Index { get { return index; } }
        public RemoveGameObjectEventArgs(int i)
        {
            index = i;
        }
    }

    public class PowerUpEventArgs : EventArgs
    {
        private PowerUpTypes powerUpEventType;
        public PowerUpTypes PowerUpEventType
        {
            get { return powerUpEventType; }
        }

        public PowerUpEventArgs(PowerUpTypes eventType)
        {
            powerUpEventType = eventType;
        }
    }

    public class EnemyEventArgs : EventArgs
    {
        private int index;
        public int Index { get { return index; } }
        public EnemyEventArgs(int i)
        {
            index = i;
        }
    }


    public class PlayerEventArgs : EventArgs
    {


        private PlayerEventTypes playerEventType;
        public PlayerEventTypes PlayerEventType
        {
            get { return playerEventType; }
        }

        private float amount;
        public float Amount
        {
            get { return amount; }
        }

        public PlayerEventArgs(PlayerEventTypes eventType, float a)
        {
            playerEventType = eventType;
            amount = a;
        }
    }

    public class SceneEventArgs : EventArgs
    {
        private string name;
        public string Name { get { return name; } }
        public SceneEventArgs(string n)
        {
            name = n;
        }
    }

    public class EventManager
    {
        public delegate void CreateSceneHandler(object sender, SceneEventArgs e);
        public delegate void LoadSceneHandler(object sender, SceneEventArgs e);
        public delegate void DestroySceneHandler(object sender, SceneEventArgs e);
    }
}
