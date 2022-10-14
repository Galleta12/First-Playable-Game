using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

 // all this logic need to be fix it since is not the best 
 // later on create an automate script
public class WeaponActiver : MonoBehaviour
{
    
[SerializeField] private GameObject weaponLogic;


   
   
   
   
   public void EnableWeaponSword(){
    weaponLogic.SetActive(true);
      //Debug.Log("Really");
     
    }


    public void DisableWeaponSword(){
      weaponLogic.SetActive(false);
    }

    
   
     


}
