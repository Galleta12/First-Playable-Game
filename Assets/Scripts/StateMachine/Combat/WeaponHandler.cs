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
//get the current weapon selected by the user
 // extra layer of security
 private WeaponsData SelectedWeapon;
 public WeaponsData selectedWeapon{get{return SelectedWeapon;}set{SelectedWeapon = value;}}
 // to know if it is a sword or a gun
 private bool isSword;
 public bool IsSword{get{return isSword;}set{isSword=value;}}

 
 
 private void Start() {
    

   
    converWeaponsDataToHash();
    Selection();
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


    
    // this should be modified for the inventory
    // this is how the arms will be organized
    // it can be fixed later on
    private void Selection(){
       
       this.WeaponsSelectionHash.Add(0,Hand);
       this.WeaponsSelectionHash.Add(1,WeaponsDatas[0].WeaponObject);
       this.WeaponsSelectionHash.Add(2,WeaponsDatas[1].WeaponObject);
    }

   // this will return the gameobject selected
    public WeaponsData getTypeWeapon(int WeaponNumber){
   
      
      WeaponsData newWeaponSelected = new WeaponsData();
      
  
         GameObject weaponSelected = WeaponsSelectionHash[WeaponNumber];
         newWeaponSelected = WeapondsDataHash[weaponSelected];
         // we first check if it is a sword or a gun
         if(newWeaponSelected.WeaponObject.tag == "Sword"){
           isSword = true;
         }else{
           isSword = false;
         }
         this.selectedWeapon = newWeaponSelected;
         return newWeaponSelected;

 }

    // this will set as null the object if it is pressed 0
   public void setAsNull(){
   
      
        WeaponsData newWeaponSelected = new WeaponsData();
       
      
         this.SelectedWeapon = newWeaponSelected;
 }

 



    
    //events on the animation
    // I am not using this anymore
    public void DrawWeapon(){
     this.selectedWeapon.WeaponObject.SetActive(true);
     
    }


    public void Unsheathe(){
        this.selectedWeapon.WeaponObject.SetActive(false);
    }


    


}
