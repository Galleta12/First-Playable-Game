using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState

{
   // jump animator variables
    private readonly int JumpHash = Animator.StringToHash("Jumping"); 
    
    private const float CrossFadeDuration = 0.1f;


    public PlayerJumpState(PlayerStateMachine stateMachine): base (stateMachine){
          
    }
    public override void Enter()
    {
      stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
     //subscribe to the double jump
     stateMachine.InputReader.JumpEvent += OnDumbleJump;

   
     // call the jump method
     Jump();
    }  

    
    public override void Tick(float deltaTime)
    {
       
       // set the inputs and the velocity for the movement while jumping
      stateMachine.currentMovement.x = CalculateNormalMovement().x;
        stateMachine.currentMovement.z = CalculateNormalMovement().z;
        stateMachine.currentMovement = stateMachine.currentMovement * stateMachine.JumpMoveSpeed;

        // set the rotation
        FaceLookMouse(stateMachine.currentMovement,deltaTime);
        //  if the velocity is less or equal than 0 we assume that we reach the max height therefore we can change to fall state
        // this need to be more checked
        if(stateMachine.Controller.velocity.y <=0){
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }
      
    }


    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnDumbleJump;
       
    }

    public override void IntiliazeSubState()
    {
       
    }

   // set the vertical velocity with the initial jump velocity
    private void Jump(){
      stateMachine.verticalVelocity = stateMachine.intialJumpVelocity;
    }



    private void OnDumbleJump(){
      // if we have already double jump on the jump state
      // we dont want to double jump until we are on the ground again
      if(!stateMachine.DidITDoubleJump){
       stateMachine.SwitchState(new PlayerDoubleJumpState(stateMachine));
      }
      
    }



}