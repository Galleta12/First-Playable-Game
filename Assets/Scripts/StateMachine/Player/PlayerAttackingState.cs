using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    
    private ComboState ComboCurrent;
    private WeaponsData currentWeapon;
    
    private Vector3 impact;
    
    private Vector3 dampingVelocity;

    private bool alreadyAppliedForce = false;
    public PlayerAttackingState(PlayerStateMachine stateMachine, int comboIdx) : base(stateMachine)
    {
        isRootState = true;
       //this is the way of how we can get the current data of the combo
       this.ComboCurrent = stateMachine.Weapon.selectedWeapon.ComboState[comboIdx];
       // we also want to get the current selected weapon
        this.currentWeapon = stateMachine.Weapon.selectedWeapon;
    }

    public override void Enter()
    {
        setDamageWeapon();
        stateMachine.moveDelegate -= stateMachine.Move;
        stateMachine.Animator.CrossFadeInFixedTime(ComboCurrent.AnimationNameCombo,ComboCurrent.TransitionDuration);

       
    }

    
    public override void Tick(float deltaTime)
    {
      

        MoveAttack(deltaTime);
        
        float normalizedTime = GetNormalizedTime(stateMachine.Animator);
        // this means that we are on an animation if is greater than 1 we are not doing nothing therefore we can change the state
        // we can change to the ground state
         // if the normal time is less than 1, it means that we are on an animation state
        if(normalizedTime < 1f){
         // if we are far enough the animation we can apply the forece
         if(normalizedTime >= ComboCurrent.ForceTime){
            TryApplyForce();
         }
         
         // we double check if we are still attacking therefore we can change to the next state
         if(stateMachine.InputReader.isAttacking){
          
            TryNextCombo(normalizedTime);
         }
        }else{
            stateMachine.SwitchState(new PlayerGroundState(stateMachine));
        }
        // need to know more about this, the dampingvelocity is only for this code, but we ensure to smootly go back to zero
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity,stateMachine.drag);
       
    }


    public override void Exit()
    {
       stateMachine.moveDelegate = stateMachine.Move; 
    }
    public override void IntiliazeSubState()
    {
       
    }

    private void TryNextCombo(float normalizedTime)
    {
        // check if we are on the last combo, lest remember that -1 means that is the last combo 
        if(ComboCurrent.ComboNextStateIndex == -1){return;}
        //check if it has pass the time set for the combo
        // is the setted attack time is greater than the normalized time, therefore we are still performing an attack
        if(ComboCurrent.ComboAttackTime > normalizedTime){return;}
        
        // if we don't satisfy the first two condition we can move
        // to the next comboAttack
        stateMachine.SwitchState(
          new PlayerAttackingState(
            stateMachine,
            ComboCurrent.ComboNextStateIndex
          )
        );
    }


    private void TryApplyForce(){
        // we only want to add the foce once per animaiton
        // we this we ensure that is only added once
        if(alreadyAppliedForce){return;}
        // we want to add force to the player forward direction times the force of the combo
        AddForce(stateMachine.transform.forward * ComboCurrent.Force);
        // if its added we can set it to true
        alreadyAppliedForce = true;
    }


  private void MoveAttack(float deltaTime){
 
     //Debug.Log("This should be call now move");
      stateMachine.Controller.Move((impact + stateMachine.Movement) * deltaTime);
 
} 

private void AddForce(Vector3 force){
    impact += force;
}

// this will call the set attack function of sword

private void setDamageWeapon(){
 if(currentWeapon.WeaponCollider.TryGetComponent<Sword>(out Sword sword)){
    sword.SetAttack(currentWeapon.Damage);
 }
  
}

}
