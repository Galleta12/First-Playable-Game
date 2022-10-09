using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : PlayerBaseState
{
    
    // double jump animator variables
    private readonly int DoubleJumpHash = Animator.StringToHash("DoubleJump");
    private const float CrossFadeDuration = 0.1f;
    
    
    public PlayerDoubleJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        isRootState = true;
    }

    public override void Enter()
    {
         stateMachine.InputReader.DrawEvent += OnDraw;
        stateMachine.Animator.CrossFadeInFixedTime(DoubleJumpHash,CrossFadeDuration);
        // call this method
        DoubleJump();
    }


    public override void Tick(float deltaTime)
    {
       // set the inputs same way as how it was done on the other states
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
        stateMachine.InputReader.DrawEvent -= OnDraw;
    }

    public override void IntiliazeSubState()
    {
        
    }


    private void DoubleJump(){
          // this is the variable for the double jump
          stateMachine.verticalVelocity = stateMachine.intialJumpVelocity * 1.2f;
    }

  
}
