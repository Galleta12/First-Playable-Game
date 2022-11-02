using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawControllerWeaponState : PlayerBaseState
{
    
    private WeaponsData currentWeapon;

    private int countWeapon;
    
    public PlayerDrawControllerWeaponState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // we need to keep a track of the inventory
        // the inventory is of lenght 3
        // for now
        //first we check if a weapon is selected  
        checkIFWeaponActive();
    }


    public override void Tick(float deltaTime)
    {
        // this would mean that there is not an active weapon, therefore we can shift to the first weapon
        if(this.countWeapon == 0){
           //we can select the first weapon, the sword
           
           this.currentWeapon = stateMachine.Weapon.getTypeWeapon(1);
           this.currentWeapon.WeaponObject.SetActive(true); 
         }else{
           
            //checkif it is outside the lenght of the inventory
            // if it is true we are can select the new item
            
            if(checkerLenghtIventory()){
            
              checkCurrentWeapon();
              this.currentWeapon = stateMachine.Weapon.getTypeWeapon(this.countWeapon+1);
              this.currentWeapon.WeaponObject.SetActive(true); 
            }
            
        }

        // we get out
        checkChangeofState();

    }


    public override void Exit()
    {
        
    }

    public override void IntiliazeSubState()
    {
        
    }
    

    private void checkIFWeaponActive()
    {
     // we this we found track on the current weapon that we are
     int count = 0;
    
     foreach(WeaponsData w in stateMachine.Weapon.WeapondsDataHash.Values){
       count ++;
       if(w.WeaponObject.activeSelf){
        this.countWeapon = count;
        
       }
     }    
    }
  

    private bool checkerLenghtIventory()
    {
        // we increment by one, since this will be call everytime that the controller is pressed
        int counterNewWeapon = this.countWeapon + 1;
        // if it is bigger we can set everything to be false
        if(counterNewWeapon > stateMachine.Weapon.WeapondsDataHash.Count){
         // we can just use the array to set everything to false
         foreach(WeaponsData w in stateMachine.Weapon.WeaponsDatas){
            if(w.WeaponObject.activeSelf){
                w.WeaponObject.SetActive(false);
                
            }
         }
         // we set it as the selected weapon to be null
        stateMachine.Weapon.setAsNull();
         return false;
          
        }
        return true;
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

// to put false to the active wepoan
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
