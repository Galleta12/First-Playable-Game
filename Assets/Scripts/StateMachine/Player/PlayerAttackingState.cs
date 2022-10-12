using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    
    private ComboState ComboCurrent;
    private WeaponsData currentWeapon;
    
    public PlayerAttackingState(PlayerStateMachine stateMachine, int comboIdx) : base(stateMachine)
    {
        isRootState = true;
        Debug.Log(comboIdx);this.ComboCurrent = stateMachine.Weapon.selectedWeapon.ComboState[comboIdx];
      
        this.currentWeapon = stateMachine.Weapon.selectedWeapon;
    }

    public override void Enter()
    {
       
        stateMachine.moveDelegate -= stateMachine.Move;
        stateMachine.Animator.CrossFadeInFixedTime(ComboCurrent.AnimationNameCombo,ComboCurrent.TransitionDuration);

       
    }

    
    public override void Tick(float deltaTime)
    {
        float normalizedTime = GetNormalizedTime(stateMachine.Animator);
        // this means that we are on an animation if is greater than 1 we are not doing nothing therefore we can change the state

        if(normalizedTime < 1f){
         if(stateMachine.InputReader.isAttacking){
            Debug.Log("Inside this state need to change");
            TryNextCombo(normalizedTime);
         }
        }else{
            stateMachine.SwitchState(new PlayerGroundState(stateMachine));
        }
       
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
        // is the setted attack time is less than the normalized time, therefore we are still performing an attack
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

}
