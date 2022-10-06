using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : PlayerBaseState
{
    public PlayerDoubleJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        isRootState = true;
    }

    public override void Enter()
    {
        DoubleJump();
    }


    public override void Tick(float deltaTime)
    {
       stateMachine.currentMovement.x = CalculateNormalMovement().x;
        stateMachine.currentMovement.z = CalculateNormalMovement().z;

        stateMachine.currentMovement = stateMachine.currentMovement * stateMachine.JumpMoveSpeed;
        FaceLookMouse(stateMachine.currentMovement,deltaTime);
        if(stateMachine.Controller.velocity.y <= 0f){
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }
    }

    public override void Exit()
    {
       
    }

    public override void IntiliazeSubState()
    {
        
    }


    private void DoubleJump(){
          stateMachine.verticalVelocity = stateMachine.intialJumpVelocity * 1.2f;
    }

  
}