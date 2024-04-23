using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DesignPatterns.State
{
    public class State_RightSideSlide : IState
    {
        private Player player;

        public State_RightSideSlide(Player player)
        {
            this.player = Player.player;
        }

        public void Enter()
        {
            
            player.StartCoroutine("Ddash");
        }

        public void FixedUpdate()
        {
            /*速度制限の処理*/
            if (player.rb.velocity.z >= player.velMax) //プレイヤーの速度が限界値を超えたとき
            {
                //プレイヤーの速度を補正
                player.rb.velocity = new Vector3(player.rb.velocity.x, player.rb.velocity.y, player.velMax);
            }
        }

    }
}

