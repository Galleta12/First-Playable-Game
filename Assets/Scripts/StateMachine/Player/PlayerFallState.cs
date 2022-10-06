using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        isRootState = true;
    }

    public override void Enter()
    {
         stateMachine.InputReader.JumpEvent += OnDumbleJump;
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
