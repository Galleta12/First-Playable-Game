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

    private Vector3 targetMovement;
    public PlayerTargetState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        isRootState = true;
        
    }

    public override void Enter()
    {
       stateMachine.moveDelegate -=  stateMachine.Move;
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);

        stateMachine.InputReader.RollEvent += OnDodge;
        
    }


      public override void Tick(float deltaTime)
    {
        

        targetMovement = CalculateTargetMovement();
        NewMoveTarget(targetMovement * stateMachine.TargetMovementSpeed,deltaTime);
        UpdateAnimator(deltaTime);
        
        if(stateMachine.InputReader.isAttacking && stateMachine.Weapon.selectedWeapon.WeaponObject != null){
     
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine,0));
        return;
        
      }
        
       
           RotateToTarget();
   
      
    }

    public override void Exit()
    {
        
         stateMachine.moveDelegate =  stateMachine.Move;
         stateMachine.IsTargeting = true;
         stateMachine.InputReader.RollEvent -= OnDodge;
    }

    public override void IntiliazeSubState()
    {
       
    }





    private void NewMoveTarget(Vector3 motion, float deltaTime){
       stateMachine.Controller.Move((stateMachine.Movement + motion) * deltaTime);
    }


  

    private void UpdateTargeter(){

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
          //stateMachine.SwitchState(new PlayerRollstate(stateMachine,stateMachine.currentMovement));
       
      
    }


 

  
}
