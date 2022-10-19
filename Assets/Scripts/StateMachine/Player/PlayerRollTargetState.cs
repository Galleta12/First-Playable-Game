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
        
        if(currentInputs != Vector3.zero){
            this.RollInput =currentInputs;
        }

    }

    public override void Enter()
    {
        remainingRollTargetTime = stateMachine.RollTargetTime;
        stateMachine.Animator.CrossFadeInFixedTime(RollHash,CrossFadeDuration);
        stateMachine.moveDelegate -= stateMachine.Move;
    }

    public override void Tick(float deltaTime)
    {
       if(RollInput != Vector3.zero){
          Vector3 rollmove = new Vector3();
          // it should work please
          rollmove+= stateMachine.transform.right * RollInput.x * stateMachine.RollTargetForce/stateMachine.RollTargetTime;
          rollmove+= stateMachine.transform.forward * RollInput.y * stateMachine.RollTargetForce/stateMachine.RollTargetTime;
           NewMoveTargetRoll(  rollmove,deltaTime);
       }else{
           Vector3 rollmoveMouse = new Vector3(); 
            rollmoveMouse+= stateMachine.transform.right * getMouseDirection().x * stateMachine.DodgeForce/stateMachine.RollTargetTime;
            rollmoveMouse+= stateMachine.transform.forward *  getMouseDirection().z * stateMachine.DodgeForce/stateMachine.RollTargetTime;
            NewMoveTargetRoll( rollmoveMouse,deltaTime);
       }

         RotateToTarget();

        remainingRollTargetTime -=deltaTime;
       
          
        if (  remainingRollTargetTime <= 0f)
        {  
        
            stateMachine.SwitchState(new PlayerTargetState(stateMachine));
          
        } 
    }

    public override void Exit()
    {
       stateMachine.moveDelegate = stateMachine.Move;
    }

    public override void IntiliazeSubState()
    {
        
    }

    // where the mouse is looking
    private Vector3 getMouseDirection()
    {
        Vector3 camera_z = stateMachine.MainCameraPlayer.forward;
      
        camera_z.y =0f;
        
        camera_z.Normalize();
       
        
        
        return camera_z;

       
    }


    private void NewMoveTargetRoll(Vector3 motion, float deltaTime){
       stateMachine.Controller.Move((stateMachine.Movement + motion) * deltaTime);
    }
}
