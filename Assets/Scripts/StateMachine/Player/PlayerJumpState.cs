using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState

{

    public PlayerJumpState(PlayerStateMachine stateMachine): base (stateMachine){
          isRootState = true;
    }
    public override void Enter()
    {
     
     stateMachine.InputReader.JumpEvent += OnDumbleJump;
     Jump();
    }  

    
    public override void Tick(float deltaTime)
    {
       
       
      stateMachine.currentMovement.x = CalculateNormalMovement().x;
        stateMachine.currentMovement.z = CalculateNormalMovement().z;
        stateMachine.currentMovement = stateMachine.currentMovement * stateMachine.JumpMoveSpeed;


        FaceLookMouse(stateMachine.currentMovement,deltaTime);
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


    private void Jump(){
      stateMachine.verticalVelocity = stateMachine.intialJumpVelocity;
    }



    private void OnDumbleJump(){
      stateMachine.SwitchState(new PlayerDoubleJumpState(stateMachine));
    }

}