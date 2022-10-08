using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
  // reference to the weapons data object
  [field: SerializeField] public WeaponsData[] WeaponsDatas {get; private set;}
  //reference to the hand where the character will hold the arm
  [field: SerializeField] public GameObject Hand {get; private set;}

// the list weapons data convert to a hash, for optimazation and the ket for the hash is the gameobject
  public Dictionary<GameObject,WeaponsData> WeapondsDataHash  {get; private set;}

// this is for the inventory, it will return the game object depending on the number input
   public Dictionary<int,GameObject> WeaponsSelectionHash {get; set;} = new Dictionary<int, GameObject>();


// get the weapon that you want to draw, otherwise just return an empyt hand
 
 
 private void Start() {
    

   
    converWeaponsDataToHash();
    Selection();
 }

  // this will return the gameobject selected
    public WeaponsData getTypeWeapon(int WeaponNumber){
   
      GameObject weaponSelected = WeaponsSelectionHash[WeaponNumber];
      return WeapondsDataHash[weaponSelected];
    
 }

  
    // convert the array to a hash or dictionary
    private void converWeaponsDataToHash()
    {
        WeaponsData []  weapons = WeaponsDatas;

      
       this.WeapondsDataHash = new Dictionary<GameObject, WeaponsData>();
        
        
        foreach(WeaponsData w in weapons)
        {
           WeapondsDataHash.Add(w.WeaponObject,w);
        }

    }


    
    // this should be modified
    private void Selection(){
      this.WeaponsSelectionHash.Add(1,WeaponsDatas[0].WeaponObject);
       this.WeaponsSelectionHash.Add(2,WeaponsDatas[1].WeaponObject);
    }


}
