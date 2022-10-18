using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollstate : PlayerBaseState
{
    // this is the roll state is similar to the dash with slightly difference
    // the first difference is that roll only occur on the ground state
    private readonly int RollHash = Animator.StringToHash("Roll");
    private const float CrossFadeDuration = 0.1f;

    private Vector3 RollInput;

    private float remainingRollTime;
    
    public PlayerRollstate(PlayerStateMachine stateMachine, Vector3 currentMotion) : base(stateMachine)
    {
     isRootState = true;
     Debug.Log(currentMotion);
    
    if(currentMotion == Vector3.zero){
        this.RollInput = getMouseDirection() * stateMachine.RollStationaryForce;
    }else{
        this.RollInput = currentMotion;
    }
    }

    public override void Enter()
    {
     
       remainingRollTime = stateMachine.RollTime;
         stateMachine.Animator.CrossFadeInFixedTime(RollHash,CrossFadeDuration);
       
    }


     public override void Tick(float deltaTime)
    {
        
       
        stateMachine.currentMovement = this.RollInput * stateMachine.RollForce / stateMachine.RollTime;
      
        FaceLookMouse(this.RollInput, deltaTime);
        
       
       
       remainingRollTime -=deltaTime;
       
          
        if ( remainingRollTime <= 0f)
        {
           
            if(stateMachine.Targeters.currentTarget == null){
               stateMachine.SwitchState(new PlayerGroundState(stateMachine));
               return;
            }else if(stateMachine.Targeters.currentTarget != null){
                stateMachine.SwitchState(new PlayerTargetState(stateMachine));
               return;
            } 
        } 
    }

    public override void Exit()
    {
       
    
    stateMachine.coolDownTimeRoll = stateMachine.RoolCoolDown;
    stateMachine.setCoolDown += handleCoolDownRoll;
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

   
}
