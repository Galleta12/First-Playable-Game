using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerBaseState
{
   
   // variables for the state
    private readonly int NormalStateBlendHash = Animator.StringToHash("NormalBlendTree"); 
    private readonly int NormalBlendSpeedHash = Animator.StringToHash("SpeedNormalBlend"); 

    //  private readonly int WeaveHash = Animator.StringToHash("Weave"); 

    //  private readonly int SpeedAttempthHash= Animator.StringToHash("MaskSpeed"); 


   

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;



    public PlayerGroundState(PlayerStateMachine stateMachine): base (stateMachine){
     
      //IntiliazeSubState();
    }
    
    
    
    public override void Enter()
    {
       
    
       stateMachine.Animator.CrossFadeInFixedTime(NormalStateBlendHash, CrossFadeDuration);
      
        // suscribe to all the methods
        stateMachine.InputReader.JumpEvent += OnJump;
        // you can only roll if you are grounded
        stateMachine.InputReader.RollEvent += OnRoll;
        stateMachine.InputReader.DrawEvent += OnDraw;

        stateMachine.InputReader.TargetEvent += OnTarget;
        // if we are ground we can double jump again;
        // lets remeber that you can only double jump if you are on the fall or jumping state
        stateMachine.DidITDoubleJump = false;
        // we are seting that the target state is false for now
        stateMachine.IsTargeting = false;
        
    }

    
    
    public override void Tick(float deltaTime)
    {

      
      // get the current movements input
      Vector3 currentMove = CalculateNormalMovement();
      stateMachine.currentMovement.x = currentMove.x;
      stateMachine.currentMovement.z = currentMove.z;

      checkChanges();

      // to the variable for the movement set it as the inputs and multiply it by the speed 
      stateMachine.currentMovement = stateMachine.currentMovement * stateMachine.FreeLookMovementSpeed;
        // if the inputs are zero we want idle animation otherwise it will run
        if(stateMachine.InputReader.MovementValue == Vector2.zero){
         
             stateMachine.Animator.SetFloat(NormalBlendSpeedHash, 0 , AnimatorDampTime, deltaTime);
            
            return;
        }

        stateMachine.Animator.SetFloat(NormalBlendSpeedHash, 1 , AnimatorDampTime, deltaTime);

    

        // rotate the character depending on the inputs and the mouse 
        FaceLookMouse(stateMachine.currentMovement,deltaTime);

       

      
       
    }

   

    public override void Exit()
    {
       //unsubscribe of the events
       stateMachine.InputReader.JumpEvent -= OnJump;
       stateMachine.InputReader.RollEvent -= OnRoll;
        stateMachine.InputReader.DrawEvent -= OnDraw;
         stateMachine.InputReader.TargetEvent -= OnTarget;
      
    }

    public override void IntiliazeSubState()
    {
     
      
      
    }



    //change state to attack

    private void checkChanges(){
     
      if(stateMachine.InputReader.isAttacking && stateMachine.Weapon.selectedWeapon.WeaponObject != null){
     
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine,0));
        return;
        
      }
    }

  // jump and roll event
     private void OnJump(){
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
    }

   private void OnRoll(){
        
        if(stateMachine.coolDownTimeRoll <=0f){
          stateMachine.SwitchState(new PlayerRollstate(stateMachine, stateMachine.currentMovement));
        }
      
    }


      private void OnTarget(){
  
           stateMachine.SwitchState(new PlayerTargetState(stateMachine,false));
       
       
        
      
    }


 

}
