using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    
    // variables for the fall
    // land animator need to be checked.
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
          stateMachine.InputReader.DrawEvent += OnDraw;
         stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
    }


     public override void Tick(float deltaTime)
    {
    // for debuggin
    Debug.Log("This is fall");
    // we call this method
       Fall(deltaTime);
      
      // set the inputs   
      stateMachine.currentMovement.x = CalculateNormalMovement().x;
        stateMachine.currentMovement.z = CalculateNormalMovement().z;

                stateMachine.currentMovement = stateMachine.currentMovement * stateMachine.JumpMoveSpeed;

        FaceLookMouse(stateMachine.currentMovement,deltaTime);
        // change state and need to be checked the animations
        if(stateMachine.Controller.isGrounded){
             stateMachine.Animator.CrossFadeInFixedTime(LandHash, CrossFadeDuration);
            stateMachine.SwitchState(new PlayerGroundState(stateMachine));
        }
      
    }

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnDumbleJump;
         stateMachine.InputReader.DrawEvent -= OnDraw;
    }

    public override void IntiliazeSubState()
    {
        
    }
   // we want to fall faster once we jump
    private void Fall(float deltaTime){
        float fallMultiplier = 2.0f;

        stateMachine.verticalVelocity +=  stateMachine.gravity *  fallMultiplier * deltaTime;
    }

     private void OnDumbleJump(){
      // if we have already double jump on the fall state
      // we dont want to double jump until we are on the ground again
      if(!stateMachine.DidITDoubleJump){
       stateMachine.SwitchState(new PlayerDoubleJumpState(stateMachine));
      }
      
    }

   
}
