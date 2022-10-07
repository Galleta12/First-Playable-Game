using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerBaseState
{
    private readonly int NormalStateBlendHash = Animator.StringToHash("NormalBlendTree"); 
    private readonly int NormalBlendSpeedHash = Animator.StringToHash("SpeedNormalBlend"); 

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;



    public PlayerGroundState(PlayerStateMachine stateMachine): base (stateMachine){
      isRootState = true;
      //IntiliazeSubState();
    }
    
    
    
    public override void Enter()
    {
       
    
       stateMachine.Animator.CrossFadeInFixedTime(NormalStateBlendHash, CrossFadeDuration);
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.RollEvent += OnRoll;
        stateMachine.InputReader.DrawEvent += OnDraw;
        
    }

    
    
    public override void Tick(float deltaTime)
    {

     
      
      Vector3 currentMove = CalculateNormalMovement();
      stateMachine.currentMovement.x = currentMove.x;
      stateMachine.currentMovement.z = currentMove.z;

      stateMachine.currentMovement = stateMachine.currentMovement * stateMachine.FreeLookMovementSpeed;
        if(stateMachine.InputReader.MovementValue == Vector2.zero){
         
             stateMachine.Animator.SetFloat(NormalBlendSpeedHash, 0 , AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(NormalBlendSpeedHash, 1 , AnimatorDampTime, deltaTime);

        FaceLookMouse(stateMachine.currentMovement,deltaTime);

       checkEnemy();

      
       
    }

    private void checkEnemy()
    {
       
    }

    public override void Exit()
    {
       stateMachine.InputReader.JumpEvent -= OnJump;
       stateMachine.InputReader.RollEvent -= OnRoll;
        stateMachine.InputReader.DrawEvent -= OnDraw;
      
    }

    public override void IntiliazeSubState()
    {
     
       SetSubState(new PlayerWalkState(stateMachine));
      
    }


     private void OnJump(){
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
    }

   private void OnRoll(){
        
        if(stateMachine.coolDownTime <=0f){
          stateMachine.SwitchState(new PlayerRollstate(stateMachine, stateMachine.currentMovement));
        }
      
    }


}
