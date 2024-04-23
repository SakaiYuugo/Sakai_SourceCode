using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DesignPatterns.State
{
    [Serializable]
    public class StateMachine : MonoBehaviour
    {
        public IState CurrentState { get; private set; }
        //private Player player = Player.player;

        
        public State_Idle            Idle;
        public State_LeftSideSlide   LeftSideSlide;
        public State_RightSideSlide  RightSideSlide;
        public State_FrontDash       frontDash;
        public State_SuperDash       SuperDash;
        public State_Jump            Jump;

        public event Action<IState> stateChanged;

 
        public StateMachine(Player p)
        {
            this.Idle           = new State_Idle(p); 
            this.LeftSideSlide  = new State_LeftSideSlide(p);
            this.RightSideSlide = new State_RightSideSlide(p);
            this.frontDash      = new State_FrontDash(p);
            this.SuperDash      = new State_SuperDash(p);
            this.Jump           = new State_Jump(p);
        }

        public void Initialize(IState state) 
        {
            Player.player.speedEffect.Stop();
            Player.player.sideSpeedEffect.Stop();
           
            Player.player.cameraPosZ = -9.5f;
            Player.player.lanenum = 0;
            Player.player.nitro = 0;
            
            Player.player.sound = Camera.main.GetComponent<GameSoundPlayer>();
            Player.player.audiosource = Player.player.GetComponent<AudioSource>();

            for (int i = 0; i < 3; i++)
            {
                Player.player.nitroGaugeBack[i].fillAmount = 0;
                Player.player.nitroGaugeFront[i].fillAmount = 0;
                
            }
           
            
            CurrentState = state; 
            state.Enter(); 
            stateChanged?.Invoke(state); 
        }

        public void TransitionTo(IState nextState) 
        {
            CurrentState.Exit(); 
            CurrentState = nextState; 
            nextState.Enter(); 

            stateChanged?.Invoke(nextState); 
        }

        public void Update()
        {
            if (CurrentState != null) CurrentState.Update();
        }

        public void FixedUpdate()
        {
                Physics.gravity = new Vector3(0, -Player.player.gravity, 0);
                //カメラがプレイヤーの後を追う処理
                if (!Player.player.isGoal && Camera.main.transform.parent != null) Camera.main.transform.DOLocalMove(new Vector3(0.0f, 6, Player.player.cameraPosZ), 0.5f);
                
                /*プレイヤーの基本移動*/
                Player.player.rb.AddForce(Player.player.transform.forward * Player.player.accelerate * Player.player.t, ForceMode.Acceleration);
            
                Player.player.n[0] = (int)Player.player.nitro - 200;
                Player.player.n[1] = (int)Player.player.nitro - 100;
                Player.player.n[2] = (int)Player.player.nitro;
        
                /*ゲージの演出*/
                if (Player.player.useNitro)
                {
                    for (int i = 0; i < Player.player.nitroGaugeBack.Length; i++)
                    {
                        {
                            float gaugepoint = Player.player.n[i];
                            float gaugepointmax = Player.player.nitroMax / Player.player.nitroGaugeBack.Length;

                            float shifter = Mathf.Lerp(gaugepoint + Player.player.lerpValue, gaugepoint, Player.player.lerpTime);
                            float shifter2 = Mathf.Lerp(gaugepoint + Player.player.lerpValue, gaugepoint, Player.player.lerpTime2);

                            Player.player.lerpTime += Time.deltaTime / 9f;
                            Player.player.lerpTime2 += Time.deltaTime / 12f;

                            Player.player.nitroGaugeFront[i].fillAmount = shifter / gaugepointmax;
                            Player.player.nitroGaugeBack[i].fillAmount = shifter2 / gaugepointmax;
                        }
                    }
                }
                
                if (CurrentState != null) CurrentState.FixedUpdate();
            
        }
    }
}
