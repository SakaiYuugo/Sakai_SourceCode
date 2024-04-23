using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DesignPatterns.State
{
    public class State_FrontDash : IState
    {
        private Player player;

        public State_FrontDash(Player player)
        {
            this.player = Player.player;
        }

        public void Enter()
        {
            player.StartCoroutine("Wdash");
        }


        public void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.A)) player.StartCoroutine("Adash");
            
            if (Input.GetKeyDown(KeyCode.D)) player.StartCoroutine("Ddash");
        }


    }
}

