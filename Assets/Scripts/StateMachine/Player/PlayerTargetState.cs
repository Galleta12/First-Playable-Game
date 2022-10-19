using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetState : PlayerBaseState
{
     // animators variable
     private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetBlendTree"); 

    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    private const float CrossFadeDuration = 0.1f;

    
    public PlayerTargetState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        isRootState = true;
        
    }

    public override void Enter()
    {
        // we want to hande our moves
        stateMachine.moveDelegate -=  stateMachine.Move;
        
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);

        stateMachine.InputReader.DodgeEvent += OnDodge;
        // press the target button to exit the target mode
        stateMachine.InputReader.TargetEvent+= OnExitTarget;
       
       
         stateMachine.InputReader.JumpEvent += OnJump;
         stateMachine.InputReader.DrawEvent += OnDraw;
        
    }


      public override void Tick(float deltaTime)
    {
        
        // get the target movement
        Vector3 targetMovement = CalculateTargetMovement();
        // move and update the player, with the new movements for get a correct targeting mode
        NewMoveTarget(targetMovement * stateMachine.TargetMovementSpeed,deltaTime);
        UpdateAnimator(deltaTime);
        
        if(stateMachine.InputReader.isAttacking && stateMachine.Weapon.selectedWeapon.WeaponObject != null){
     
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine,0));
        return;
        
      }
      // we check if there are enemies inside the list of the sphere overlap
      //if there is not enemies we can get back to ground state
     
        // rotote so the player if always facing the current target;
        RotateToTarget();
   
      
    }

    public override void Exit()
    {
        
         stateMachine.moveDelegate =  stateMachine.Move;
         // if we exit this means that the last state was target state
         stateMachine.IsTargeting = true;
          stateMachine.InputReader.TargetEvent-= OnExitTarget;
         stateMachine.InputReader.DodgeEvent -= OnDodge;
          stateMachine.InputReader.RollEvent -= OnRollTarget;
           stateMachine.InputReader.JumpEvent -= OnJump;

           stateMachine.InputReader.DrawEvent -= OnDraw;
    }

  

    public override void IntiliazeSubState()
    {
       
    }

    private Vector3 CalculateTargetMovement(){
       Vector3 movement = new Vector3();
       movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
       movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

       return movement;

    }





    private void NewMoveTarget(Vector3 motion, float deltaTime){
       stateMachine.Controller.Move((stateMachine.Movement + motion) * deltaTime);
    }


  

    private void UpdateAnimator(float deltaTime){
       if (stateMachine.InputReader.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(TargetingForwardHash, 0, 0.1f, deltaTime);
        }
        else
        {
            // if the input y is grater than 0 we set it up as 1f other wise -1f
            float value = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingForwardHash, value, 0.1f, deltaTime);
        }

        if (stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(TargetingRightHash, 0, 0.1f, deltaTime);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingRightHash, value, 0.1f, deltaTime);
        }
    }


       private void OnDodge(){
        
         stateMachine.SwitchState(new PlayerDodgeState(stateMachine,stateMachine.InputReader.MovementValue));
        
    }


    private void OnExitTarget()
    {
       //press tab again to get out from the ground state;
       //this will already set up target as false, therefore we don't get unexpected behaviours.
       stateMachine.SwitchState(new PlayerGroundState(stateMachine));
    }


      private void OnRollTarget()
    {
       if(stateMachine.coolDownTimeRoll <=0f){
          stateMachine.SwitchState(new PlayerRollstate(stateMachine, stateMachine.currentMovement));
        }
    }


           private void OnJump(){
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
    }


 




 

  
}
