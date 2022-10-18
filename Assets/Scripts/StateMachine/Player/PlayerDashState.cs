using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    
    // dash variables
  private readonly int DashGroundHash = Animator.StringToHash("DashGround");
  private readonly int DashAirHash = Animator.StringToHash("DashAir");
  private const float CrossFadeDuration = 0.1f;



   // this save the dash inputs
    private Vector3 MovementDash;

   // the ramianing time to stop the dash
    private float remainingDashtTime;


  

    
    
    public PlayerDashState(PlayerStateMachine stateMachine, Vector3 currentMotion) : base(stateMachine)
    {
       
       isRootState = true;
      // if the inputs are zero we want to move depending on the mouse direction, otherwise we just move depending on the inputs
      if(currentMotion == Vector3.zero){
             
             this.MovementDash = getMouseDirection() * stateMachine.DashStationaryForce;
      }else{
             this.MovementDash = currentMotion;
      }
     
    }

    public override void Enter()
    {
       // we want to unsubscribe the to the dash event
       // we first subscribe on the start of the statemachine but as soon as we enter to this state we don't want to trigger more this event
       //therefore we unsubscribe it while we are on the dash state
       stateMachine.InputReader.DashEvent -= stateMachine.OnDash;
      // the ramining time is the same as the dash time set on the statemachine
       remainingDashtTime = stateMachine.DashTime;
      
    
    }

    
    
     public override void Tick(float deltaTime)
    {
      
      // we want the current movement input to be the last inputs multiply by the dash force
      // we divide by the time, since we want to move with this force during the set lenght of the dash time on the statemachine
      stateMachine.currentMovement =  this.MovementDash * stateMachine.DashForce / stateMachine.DashTime;
      
      FaceLookMouse(MovementDash, deltaTime);
      UpdateAnimator();
     // if we are not grounded therefore we are going to disregard the gravity will we are dashing
     if(!stateMachine.Controller.isGrounded){
       // first we unsubscribe from the normla move delegate, we dont want to call more this method since it takes into count the gravity
       stateMachine.moveDelegate -= stateMachine.Move;
       // Then we move the chracter without taking into count the gravity
       stateMachine.Controller.Move(stateMachine.currentMovement * deltaTime);
      
     }
        
      // we reduce the time every single frame, so like that we know when we should stop
      remainingDashtTime-= deltaTime;


        if ( remainingDashtTime <= 0f)
        {
           
            
            if(!stateMachine.Controller.isGrounded){
              
              stateMachine.SwitchState(new PlayerFallState(stateMachine));
              return;
            }

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
       
        // since we change state we can subscribe again to the dash, therefore we can trigger this state again
        stateMachine.InputReader.DashEvent += stateMachine.OnDash;
        // we can subscrine to the delegate for the normal movement again
        // I should check this not sure if it is working properly
        stateMachine.moveDelegate = stateMachine.Move;
        
        // this is the cooldown time variable, as soon as we exit we want to start the cool down time
        // therefore we set the cooldown variable to be the same as the dash cool down set on the statemachine
        stateMachine.coolDownTimeDash = stateMachine.DashCoolDown;
        // then we set the cool down delegate to this method on the base state.
        stateMachine.setCoolDown += handleCoolDownDash;
        

    }

    public override void IntiliazeSubState()
    {
       
    }

    // get to where the mouse if facing if the last inputs are null
    private Vector3 getMouseDirection()
    {
        Vector3 camera_z = stateMachine.MainCameraPlayer.forward;

        camera_z.y =0f;
        camera_z.Normalize();
        return camera_z;

       
    }

    // this should be checked
    private void UpdateAnimator(){
     if(stateMachine.Controller.isGrounded){
      stateMachine.Animator.CrossFadeInFixedTime(DashGroundHash,CrossFadeDuration);
     }else{
      stateMachine.Animator.CrossFadeInFixedTime(DashAirHash,CrossFadeDuration);
     }


    }

    
    

 

   
}
