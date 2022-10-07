using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollstate : PlayerBaseState
{
    
    private readonly int RollHash = Animator.StringToHash("Roll");
    private const float CrossFadeDuration = 0.1f;

    private Vector3 RollInput;

    private float remainingRollTime;
    
    public PlayerRollstate(PlayerStateMachine stateMachine, Vector3 currentMotion) : base(stateMachine)
    {
     isRootState = true;
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
        
        Debug.Log("Wtfff");
        stateMachine.currentMovement = this.RollInput * stateMachine.RollForce / stateMachine.RollTime;
        FaceLookMouse(this.RollInput, deltaTime);
       
       remainingRollTime -=deltaTime;
       
          
        if ( remainingRollTime <= 0f)
        {
           
              stateMachine.SwitchState(new PlayerGroundState(stateMachine));
        } 
    }

    public override void Exit()
    {
       
    
    stateMachine.coolDownTime = stateMachine.RoolCoolDown;
    stateMachine.setCoolDown = handleCoolDown;
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
