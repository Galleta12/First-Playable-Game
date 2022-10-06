using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    
    
    private readonly int FallHash = Animator.StringToHash("FallIdle");
    private readonly int LandHash = Animator.StringToHash("Land");  
    
    private const float CrossFadeDuration = 0.1f;
    
    
    
    public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        isRootState = true;
    }

    public override void Enter()
    {
         stateMachine.InputReader.JumpEvent += OnDumbleJump;
         stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
    }


     public override void Tick(float deltaTime)
    {
    
    Debug.Log("This is fall");
       
       Fall(deltaTime);
      
         
      stateMachine.currentMovement.x = CalculateNormalMovement().x;
        stateMachine.currentMovement.z = CalculateNormalMovement().z;

                stateMachine.currentMovement = stateMachine.currentMovement * stateMachine.JumpMoveSpeed;

        FaceLookMouse(stateMachine.currentMovement,deltaTime);
        if(stateMachine.Controller.isGrounded){
             stateMachine.Animator.CrossFadeInFixedTime(LandHash, CrossFadeDuration);
            stateMachine.SwitchState(new PlayerGroundState(stateMachine));
        }
      
    }

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnDumbleJump;
    }

    public override void IntiliazeSubState()
    {
        
    }

    private void Fall(float deltaTime){
        float fallMultiplier = 2.0f;

        stateMachine.verticalVelocity +=  stateMachine.gravity *  fallMultiplier * deltaTime;
    }

     private void OnDumbleJump(){
      stateMachine.SwitchState(new PlayerDoubleJumpState(stateMachine));
    }

   
}
