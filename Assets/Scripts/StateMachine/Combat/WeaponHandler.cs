using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
  [field: SerializeField] public WeaponsData[] WeaponsDatas {get; private set;}
  [field: SerializeField] public GameObject Hand {get; private set;}


  public Dictionary<GameObject,WeaponsData> WeapondsDataHash  {get; private set;}


   public Dictionary<int,GameObject> WeaponsSelectionHash {get; set;} = new Dictionary<int, GameObject>();


// get the weapon that you want to draw, otherwise just return an empyt hand
 
 
 private void Start() {
    

   
    converWeaponsDataToHash();
    Selection();
 }


    public WeaponsData getTypeWeapon(int WeaponNumber){
   
      GameObject weaponSelected = WeaponsSelectionHash[WeaponNumber];
      return WeapondsDataHash[weaponSelected];
    
 }


 
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
