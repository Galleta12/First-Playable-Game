using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollTargetState : PlayerBaseState
{
   
   private readonly int RollHash = Animator.StringToHash("Roll");
    private const float CrossFadeDuration = 0.1f;

    private Vector3 RollInput;

    private float remainingRollTargetTime;

    public PlayerRollTargetState(PlayerStateMachine stateMachine, Vector3 currentInputs) : base(stateMachine)
    {
        
       
            this.RollInput =currentInputs;
        

    }

    public override void Enter()
    {
        remainingRollTargetTime = stateMachine.RollTargetTime;
        stateMachine.Animator.CrossFadeInFixedTime(RollHash,CrossFadeDuration);
        stateMachine.moveDelegate -= stateMachine.Move;
    }

    public override void Tick(float deltaTime)
    {
       
          // we get the current direction for the movemnt for the roll
          Vector3 rollmove = new Vector3();
          rollmove+= stateMachine.transform.right * RollInput.x * stateMachine.RollTargetForce/stateMachine.RollTargetTime;
          rollmove+= stateMachine.transform.forward * RollInput.y * stateMachine.RollTargetForce/stateMachine.RollTargetTime;
        NewMoveTargetRoll(rollmove,deltaTime);
       

         RotateToTarget();

        remainingRollTargetTime -=deltaTime;
       
          
        if (  remainingRollTargetTime <= 0f)
        {  
        
            stateMachine.SwitchState(new PlayerTargetState(stateMachine,false));
          
        } 
    }

    public override void Exit()
    {
       stateMachine.moveDelegate = stateMachine.Move;
    }

    public override void IntiliazeSubState()
    {
        
    }

  


    private void NewMoveTargetRoll(Vector3 motion, float deltaTime){
       stateMachine.Controller.Move((stateMachine.Movement + motion) * deltaTime);
    }
}
