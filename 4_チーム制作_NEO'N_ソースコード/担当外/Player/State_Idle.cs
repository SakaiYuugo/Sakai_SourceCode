using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.State
{
    public class State_Idle : IState
    {
        private Player player;
        public State_Idle(Player player)
        {
            this.player = Player.player;
        }

        public void Enter()
        {
           
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !player.isNitro && player.SuperDashReady) player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.SuperDash);
            if (Input.GetKeyDown(KeyCode.W) && !player.SuperDashMode) player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.frontDash);
            if (Input.GetKeyDown(KeyCode.A)) player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.LeftSideSlide);
            if (Input.GetKeyDown(KeyCode.D)) player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.RightSideSlide);
            if (Input.GetKeyDown(KeyCode.Space)) player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.Jump);
        }

        public void FixedUpdate()
        {
            player.t = Mathf.Min((player.t + Time.deltaTime * 0.5f) + 0.5f, 10f);
            
            
            
            /*速度制限の処理*/
            if (player.rb.velocity.z >= player.velMax) //プレイヤーの速度が限界値を超えたとき
            {
                    //プレイヤーの速度を補正
                    player.rb.velocity = new Vector3(player.rb.velocity.x, player.rb.velocity.y, player.velMax);
            }
        }

        public void Exit()
        {
           
        }

        
    }
}
