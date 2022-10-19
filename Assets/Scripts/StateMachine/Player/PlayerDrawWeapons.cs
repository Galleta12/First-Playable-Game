using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDrawWeapons : PlayerBaseState
{
    // this should still need to be checked
   
    private readonly int NormalStateBlendHash = Animator.StringToHash("MaskBlendTree"); 
    private readonly int NormalBlendSpeedHash = Animator.StringToHash("MaskSpeed"); 

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;
    
   
    

    private GameObject hand;

    private GameObject sword;

    private int KeyboardNumber;

     private string DeviceName;

     private WeaponsData currentWeapon;
    
    public PlayerDrawWeapons(PlayerStateMachine stateMachine, int keyboardNumber, string deviceName) : base(stateMachine)
    {
         
          this.KeyboardNumber = keyboardNumber;
          this.DeviceName = deviceName;
          this.currentWeapon = stateMachine.Weapon.getTypeWeapon(keyboardNumber); 
    }

    public override void Enter()
    {
   
       
     
         stateMachine.Animator.CrossFadeInFixedTime(currentWeapon.WeaponAnimationDrawName ,CrossFadeDuration);
         
       
    

      
      //this is just for print the data and check that everything  is ok
      // foreach(KeyValuePair<GameObject,WeaponsData> kvp in stateMachine.Weapon.WeapondsDataHash){
            
      //       Debug.Log("Key: " + kvp.Key + "," + "Value: " + kvp.Value.WeaponObject);
      // }
      
    }


      public override void Tick(float deltaTime)
    {
    
      
      if(!GetStateOfAnimation(stateMachine.Animator,currentWeapon.WeaponAnimationDrawName)){
         
         if(stateMachine.Controller.isGrounded){
           if(stateMachine.IsTargeting && stateMachine.Targeters.currentTarget != null){
            stateMachine.SwitchState(new PlayerTargetState(stateMachine));
            return;
           }else{
            stateMachine.SwitchState(new PlayerGroundState(stateMachine));
           }
         }else{
          stateMachine.SwitchState(new PlayerFallState(stateMachine));
         }
         
      }

      
    
       
    }

    public override void Exit()
    {
       
    }

    public override void IntiliazeSubState()
    {
      
    }



  
}
