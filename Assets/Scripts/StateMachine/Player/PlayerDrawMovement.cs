using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawMovement : PlayerBaseState
{
    private readonly int  DefaultBlendHash = Animator.StringToHash("Default"); 
    private readonly int DrawArmLayerHash = Animator.StringToHash("DrawSwordRun"); 

     private string LayerName = "Arm";

    private const float CrossFadeDuration = 0.1f;
    
   
    

    private GameObject hand;

    private GameObject sword;

    private int KeyboardNumber;

     private string DeviceName;

     private WeaponsData currentWeapon;


     private int Layer;
    
    public PlayerDrawMovement(PlayerStateMachine stateMachine, int keyboardNumber, string deviceName) : base(stateMachine)
    {
          
          this.KeyboardNumber = keyboardNumber;
          this.DeviceName = deviceName;
          this.currentWeapon = stateMachine.Weapon.getTypeWeapon(keyboardNumber); 
    }

    public override void Enter()
    {
     
     this.Layer = stateMachine.Animator.GetLayerIndex(LayerName);

     
     
     stateMachine.Animator.CrossFadeInFixedTime(currentWeapon.WeaponAnimationDrawNameMovement ,CrossFadeDuration,Layer);
       
    }


      public override void Tick(float deltaTime)
    {
    
       if(!GetStateOfAnimationNewLayer(stateMachine.Animator,currentWeapon.WeaponAnimationDrawNameMovement)){
          if(stateMachine.IsTargeting && stateMachine.Targeters.currentTarget != null){
            stateMachine.SwitchState(new PlayerTargetState(stateMachine));
            return;
           }else{
            stateMachine.SwitchState(new PlayerGroundState(stateMachine));
           }
      }
    
       
    }

    public override void Exit()
    {
        stateMachine.Animator.CrossFadeInFixedTime(DefaultBlendHash ,CrossFadeDuration,Layer);
    }

    public override void IntiliazeSubState()
    {
      
    }



private bool GetStateOfAnimationNewLayer(Animator animator, string animationName){
   AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(1);
    if(currentAnimation.normalizedTime > 1.0f && currentAnimation.IsTag(animationName)){
      
     
     return false;
    }else{
      
       return true;
      
      
    }
}



  
}


