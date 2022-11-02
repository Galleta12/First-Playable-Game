using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDrawWeapons : PlayerBaseState
{
   
    
   // this is for the keyboard
    

    private GameObject hand;

    private GameObject sword;

    private int KeyboardNumber;

     private string DeviceName;

     private WeaponsData currentWeapon;
    
    public PlayerDrawWeapons(PlayerStateMachine stateMachine, int keyboardNumber) : base(stateMachine)
    {
         
     
            this.KeyboardNumber = keyboardNumber;
          // set active the wepon, so you can see it on the screen
    }

    public override void Enter()
    {
    
      //check if the selected number is 0
      if(KeyboardNumber == 0){
      // we first check if there is an active  weapon, if there it is we just set it as false, so we are left with the hand
      foreach(WeaponsData weapons in stateMachine.Weapon.WeaponsDatas){
        if(weapons.WeaponObject.activeSelf){
          weapons.WeaponObject.SetActive(false);
     
          
        }
      }
      // we set it as the selected weapon to be null
      stateMachine.Weapon.setAsNull();
       return;
      
      }else{
        // we now check if weapon has already been selected by an specific keyboard
        // if the same number is press twice we just select hand
        if(weaponHasAlreadyBeenSelected()){
      
           // we set it as the selected weapon to be null
         stateMachine.Weapon.setAsNull();
          return;
        }else{
          checkCurrentWeapon();
          this.currentWeapon=stateMachine.Weapon.getTypeWeapon(KeyboardNumber);
          this.currentWeapon.WeaponObject.SetActive(true);
          return;
        }
      }
      
      
      //this is just for print the data and check that everything  is ok
      // foreach(KeyValuePair<GameObject,WeaponsData> kvp in stateMachine.Weapon.WeapondsDataHash){
            
      //       Debug.Log("Key: " + kvp.Key + "," + "Value: " + kvp.Value.WeaponObject);
      // }
      
    }


    public override void Tick(float deltaTime)
    {
    
      // check if we need to change state
     checkChangeofState();   
       
    }

    public override void Exit()
    {
       
    }

    public override void IntiliazeSubState()
    {
      
    }

    private void checkChangeofState()
    {
     
          if(stateMachine.IsTargeting && stateMachine.Targeters.currentTarget != null){
            stateMachine.SwitchState(new PlayerTargetState(stateMachine,false));
            return;     
          }else{
            stateMachine.SwitchState(new PlayerGroundState(stateMachine));
            return;
             
          }
          
      
        
    }


    private bool weaponHasAlreadyBeenSelected(){
      // check on the weapons data hash
      GameObject weaponSelected = stateMachine.Weapon.WeaponsSelectionHash[KeyboardNumber];
      if(weaponSelected.activeSelf){
        
        weaponSelected.SetActive(false);
        return true;
      }else{
        return false;
      }
    }

   private void checkCurrentWeapon()
    {
      //since the weapon selected is not the same that is on the hand
      // we can simply put false to the active weapon
     foreach(WeaponsData weapons in stateMachine.Weapon.WeaponsDatas){
        if(weapons.WeaponObject.activeSelf){
          weapons.WeaponObject.SetActive(false);      
        }
      }     
    }



  
}
