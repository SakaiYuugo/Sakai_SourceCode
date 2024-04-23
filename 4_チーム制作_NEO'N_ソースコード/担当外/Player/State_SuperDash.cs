using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DesignPatterns.State
{
    public class State_SuperDash : IState
    {
        private Player player;

        public State_SuperDash(Player player)
        {
            this.player = Player.player;
        }

        public void Enter()
        {
            
            player.StartCoroutine("Superdash");
        }
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) player.StartCoroutine("Adash");
            if (Input.GetKeyDown(KeyCode.D)) player.StartCoroutine("Ddash");
        }
        
    }
}

