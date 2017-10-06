﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI.UIManagement
{
    /**
      * class StateManager implements the management of states of the game.
      * It will display the states that should be displayed based on inputs
      * from users and triggers from the game.
      **/
    class StateManager : State
    {
        public StateManager m_stateManager { get; set; }
        public int m_stateID { get; set; }
        public bool m_stateLoaded { get; set; }
        public List<int> m_screenIds { get; set; }
        public Screen m_currentScreen { get; set; }

        public void Initialize()
        {

        }
        public void Load()
        {

        }
        public void Update(float delta)
        {

        }

        public Screen GetScreen(int index)
        {
            return null;
        }
    }
}