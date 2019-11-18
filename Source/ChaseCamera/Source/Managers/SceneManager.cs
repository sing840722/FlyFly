using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public class SceneManager
    {
        private EndScreen endScreen;
        public EndScreen EndScreen
        {
            get { return endScreen; }
            set { endScreen = value; }
        }

        private World level;
        public World Level
        {
            get { return level; }
            set { level = value; }
        }

        private Menu menu;
        public Menu Menu
        {
            get { return menu; }
            set { menu = value; }
        }


        private FSM fsm;
        public FSM StateMachine
        {
            get { return fsm; }
        }

        private InMenu inMenu = new InMenu();
        private InGame inGame = new InGame();
        private InEndScreen inEndScreen = new InEndScreen();

        private bool bChange;
        public bool Change
        {
            get { return bChange; }
            set { bChange = value; }
        }

        public SceneManager( Menu m, World l, EndScreen e)
        {
            menu = m;
            level = l;
            endScreen = e;

            fsm = new FSM(this);

            inMenu.AddTransition(new Transition(inGame, () => bChange));
            inGame.AddTransition(new Transition(inEndScreen, () => !level.Player.Active));
            inEndScreen.AddTransition(new Transition(inMenu, () => bChange));

            fsm.AddState(inMenu);
            fsm.AddState(inGame);
            fsm.AddState(inEndScreen);
        }

        public void Start()
        {
            fsm.Initialise("InMenu");
        }

        public void Update(GameTime gameTime)
        {
            fsm.Update(gameTime);
        }

        public void GotoNextScene(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                bChange = true;
            }
        }

        public void Draw()
        {
            
        }
    }
}
