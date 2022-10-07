using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    
  private readonly int DashGroundHash = Animator.StringToHash("DashGround");
  private readonly int DashAirHash = Animator.StringToHash("DashAir");
  private const float CrossFadeDuration = 0.1f;




    private Vector3 MovementDash;


    private float remainingDashtTime;

  

    
    
    public PlayerDashState(PlayerStateMachine stateMachine, Vector3 currentMotion) : base(stateMachine)
    {
       
       isRootState = true;
    
      if(currentMotion == Vector3.zero){
             // I should check this.
             this.MovementDash = getMouseDirection() * stateMachine.DashStationaryForce;
      }else{
             this.MovementDash = currentMotion;
      }
     
    }

    public override void Enter()
    {
       stateMachine.InputReader.DashEvent -= stateMachine.OnDash;
      
       remainingDashtTime = stateMachine.DashTime;
      

     
     
    
    }

    
    
     public override void Tick(float deltaTime)
    {
      
      stateMachine.currentMovement =  this.MovementDash * stateMachine.DashForce / stateMachine.DashTime;
      FaceLookMouse(MovementDash, deltaTime);
     UpdateAnimator();
     
     if(!stateMachine.Controller.isGrounded){
       stateMachine.moveDelegate -= stateMachine.Move;
       stateMachine.Controller.Move(stateMachine.currentMovement * deltaTime);
      
     }
        
      
      remainingDashtTime-= deltaTime;


        if ( remainingDashtTime <= 0f)
        {
           
            
            if(!stateMachine.Controller.isGrounded){
              
              stateMachine.SwitchState(new PlayerFallState(stateMachine));
              return;
            }

              stateMachine.SwitchState(new PlayerGroundState(stateMachine));
        }


         

    }

 

    public override void Exit()
    {
       
        stateMachine.InputReader.DashEvent += stateMachine.OnDash;
        stateMachine.moveDelegate = stateMachine.Move;
        
        stateMachine.coolDownTime = stateMachine.DashCoolDown;
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


    private void UpdateAnimator(){
     if(stateMachine.Controller.isGrounded){
      stateMachine.Animator.CrossFadeInFixedTime(DashGroundHash,CrossFadeDuration);
     }else{
      stateMachine.Animator.CrossFadeInFixedTime(DashAirHash,CrossFadeDuration);
     }


    }


    


 

   
}
