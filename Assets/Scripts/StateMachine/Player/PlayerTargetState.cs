using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetState : PlayerBaseState
{
     // animators variable
    protected readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetBlendTree"); 

    protected readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    protected  readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    protected  const float CrossFadeDuration = 0.1f;

    
    public PlayerTargetState(PlayerStateMachine stateMachine, bool shouldChangeTarget) : base(stateMachine)
    {
       // call the set target to select the closest target
       // everytime that we change of state this will be called
       if(shouldChangeTarget){
         stateMachine.Targeters.setTheClosestTarget();
       }
       else if(stateMachine.Targeters.currentTarget == null){
         stateMachine.Targeters.setTheClosestTarget();
       }
      
        
    }

    public override void Enter()
    {
        // we want to handle our moves
        stateMachine.moveDelegate -=  stateMachine.Move;
        
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);

        stateMachine.InputReader.DodgeEvent += OnDodge;
        // press the target button to exit the target mode
        stateMachine.InputReader.CancelTargetEvent+= OnExitTarget;

        stateMachine.InputReader.TargetEvent+= ChangeTarget;
       
       
         stateMachine.InputReader.JumpEvent += OnJump;
         stateMachine.InputReader.DrawEvent += OnDraw;

         stateMachine.InputReader.RollEvent += OnRollTarget;
         
        
    }


      public override void Tick(float deltaTime)
    {
        
        
        
        // get the target movement
        Vector3 targetMovement = CalculateTargetMovement();
        // move and update the player, with the new movements for get a correct targeting mode
        NewMoveTarget(targetMovement * stateMachine.TargetMovementSpeed,deltaTime);
        UpdateAnimator(deltaTime);

        checkpossibleChangeofState();
        
      
      
      
      if(stateMachine.InputReader.isAttacking && stateMachine.Weapon.selectedWeapon.WeaponObject != null
      && stateMachine.Weapon.IsSword){
     
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine,0));
        return;
        
      }
     
      if(stateMachine.Targeters.currentTarget == null){
         stateMachine.Targeters.setTheClosestTarget();
         //here I need to do a courotine in order to check if the targets is null;
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
        
        stateMachine.InputReader.CancelTargetEvent-= OnExitTarget;
        
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        
        stateMachine.InputReader.JumpEvent -= OnJump;

        stateMachine.InputReader.DrawEvent -= OnDraw;

        stateMachine.InputReader.RollEvent -= OnRollTarget;

         stateMachine.InputReader.TargetEvent-= ChangeTarget;
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
        
         // we only dodge if the movments is not zero
         if(stateMachine.InputReader.MovementValue != Vector2.zero){
           stateMachine.SwitchState(new PlayerDodgeState(stateMachine,stateMachine.InputReader.MovementValue));
         }
        
    }


    private void OnExitTarget()
    {
       //press tab again to get out from the ground state;
       //this will already set up target as false, therefore we don't get unexpected behaviours.
       stateMachine.SwitchState(new PlayerGroundState(stateMachine));
    }


      private void OnRollTarget()
    {
      // we only roll if we the inputs are not zero
         if(stateMachine.InputReader.MovementValue != Vector2.zero){
            stateMachine.SwitchState(new PlayerRollTargetState(stateMachine, stateMachine.InputReader.MovementValue));
         }
         
        
    }


    private void OnJump(){
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
    }


    private void ChangeTarget(){
      stateMachine.SwitchState(new PlayerTargetState(stateMachine,true));
    }
      private void checkpossibleChangeofState(){
     
      if(stateMachine.InputReader.isTargeting){
       stateMachine.SwitchState(new PlayerSelectTargetState(stateMachine,false));
       
      }
    }


   
    



    
    


  
}


