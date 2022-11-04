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

    private Vector3 currentInput;


    private Target currentTargetAttack;

    private bool IsAirAttack = false;

    private float jumpForAttack;

    //to check if it is 3, so we can perform the jump
    private int indForAttackAir;
    
    public PlayerAttackingState(PlayerStateMachine stateMachine, int comboIdx) : base(stateMachine)
    {
        
       
       
       //this is the way of how we can get the current data of the combo
       this.ComboCurrent = stateMachine.Weapon.selectedWeapon.ComboState[comboIdx];
       // we also want to get the current selected weapon
        this.currentWeapon = stateMachine.Weapon.selectedWeapon;
        
        this.currentTargetAttack = stateMachine.Targeters.currentTarget;
        
        this.indForAttackAir = comboIdx;
    }

    public override void Enter()
    {
        setDamageWeapon();
        
        stateMachine.moveDelegate -= stateMachine.Move;
        
        stateMachine.Animator.CrossFadeInFixedTime(ComboCurrent.AnimationNameCombo,ComboCurrent.TransitionDuration);

         stateMachine.InputReader.JumpEvent += OnJump;
         
         stateMachine.InputReader.DodgeEvent += OnDodge;

          stateMachine.InputReader.RollEvent += OnRollTarget;
         
         //for the air Attack
         stateMachine.InputReader.AirAttackEvent += OnAirAttack;
       
    }

    
    public override void Tick(float deltaTime)
    {
        
       
      // this is for the movemtn if it is without a target
       if(currentTargetAttack == null){
         this.currentInput = CalculateNormalMovement() * 2f;
         SetRotation(currentInput,deltaTime);
        
       }

          
        //check if we should rotate
        if(currentTargetAttack !=null && stateMachine.IsTargeting){
         RotateToTarget();

        }

        //move the character taking into count the impulse force
        MoveAttack(deltaTime);  
        
        float normalizedTime = GetNormalizedTime(stateMachine.Animator,"Attack");
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
           if(stateMachine.IsTargeting){
            stateMachine.SwitchState(new PlayerTargetState(stateMachine,false));
            return;
           }else{
            stateMachine.SwitchState(new PlayerGroundState(stateMachine));
           }
           
        }
        // need to know more about this, the dampingvelocity is only for this code, but we ensure to smootly go back to zero
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity,stateMachine.drag);
       
    }

   

    public override void Exit()
    {
       stateMachine.moveDelegate = stateMachine.Move; 


        stateMachine.InputReader.DodgeEvent -= OnDodge;

        stateMachine.InputReader.RollEvent -= OnRollTarget;

        stateMachine.InputReader.JumpEvent -= OnJump;

         stateMachine.InputReader.AirAttackEvent -= OnAirAttack;

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
        
        
        
        //condition to know if we want the air attack
        //we are passing 3 since is the index for the attacking state
        if(IsAirAttack){
          stateMachine.SwitchState(
            new PlayerAttackingState(
              stateMachine,
              3
            )
          );          
        }else{
        // if we don't satisfy the first three condition we can move
        // to the next comboAttack
        stateMachine.SwitchState(
          new PlayerAttackingState(
            stateMachine,
            ComboCurrent.ComboNextStateIndex
          )
        );

        }
    }


    private void TryApplyForce(){
        // we only want to add the foce once per animaiton
        // we this we ensure that is only added once
        if(alreadyAppliedForce){return;}
        // we want to add force to the player forward direction times the force of the combo
        AddForce(stateMachine.transform.forward * ComboCurrent.Force);
        // if its added we can set it to true
        alreadyAppliedForce = true;
      // if we are on a correct attack time of the animation for the air and
      // we are on the index of animation we can set up the variable for the jump
      if(indForAttackAir==3){
          stateMachine.verticalVelocity = stateMachine.intialJumpVelocity * 0.35f;
       }

    }


  private void MoveAttack(float deltaTime){
 
     //Debug.Log("This should be call now move");
      stateMachine.Controller.Move((impact + stateMachine.Movement + currentInput) * deltaTime);
 
} 

private void AddForce(Vector3 force){
    impact += force;
}

// this will call the set attack function of sword

private void setDamageWeapon(){
 // this is for the sword therefore we can use it to handle the damage and the knockback
 if(currentWeapon.WeaponCollider.TryGetComponent<Sword>(out Sword sword)){
    sword.SetAttack(currentWeapon.Damage,ComboCurrent.Knockback);
 }
  
}



  private void OnDodge(){
        
         // we only dodge if the movments is not zero
         if(stateMachine.InputReader.MovementValue != Vector2.zero){
           stateMachine.SwitchState(new PlayerDodgeState(stateMachine,stateMachine.InputReader.MovementValue));
         }
         
        
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


    private void SetRotation(Vector3 direction,float deltaTime){
    if(direction == Vector3.zero){return;}
    stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
    Quaternion.LookRotation(direction),
    deltaTime * stateMachine.RotationDampSpeed
    );
  
  

}

private void OnAirAttack(){
  IsAirAttack=true;

  
}







}
