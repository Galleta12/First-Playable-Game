using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    
     // this is the roll state is similar to the dash with slightly difference
    // the first difference is that roll only occur on the ground state

    private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree"); 

    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
    
    private const float CrossFadeDuration = 0.1f;

    private Vector3 DodgeInput;

    private float remainingRollTime;
    
    
    
    
    
    public PlayerDodgeState(PlayerStateMachine stateMachine, Vector3 currentMotion) : base(stateMachine)
    {
    isRootState = true;
    this.DodgeInput = currentMotion;
    }

    public override void Enter()
    {
        remainingRollTime = stateMachine.RollTime;
    
        stateMachine.moveDelegate -= stateMachine.Move;
        stateMachine.Animator.SetFloat(DodgeForwardHash,DodgeInput.y);
        stateMachine.Animator.SetFloat(DodgeRightHash,DodgeInput.x);
         stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);
        
    }

    
     public override void Tick(float deltaTime)
    {
        
        
        Vector3 dodgemove = new Vector3(); 
        dodgemove+= stateMachine.transform.right * DodgeInput.x * stateMachine.RollForce/stateMachine.RollTime;
        dodgemove+= stateMachine.transform.forward * DodgeInput.y * stateMachine.RollForce/stateMachine.RollTime;
        NewMoveTargetRoll( dodgemove,deltaTime);
        
       RotateToTarget();
        
        remainingRollTime -=deltaTime;
       
          
        if ( remainingRollTime <= 0f)
        {
           
              stateMachine.SwitchState(new PlayerTargetState(stateMachine));
        } 
    }
    
    public override void Exit()
    {
       stateMachine.coolDownTimeRoll = stateMachine.RoolCoolDown;
       stateMachine.setCoolDown += handleCoolDownRoll;
       stateMachine.moveDelegate = stateMachine.Move;
    }

    public override void IntiliazeSubState()
    {
       
    }

    
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
