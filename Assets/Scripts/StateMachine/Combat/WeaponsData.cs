using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this the the data of the weapon and the animation
[Serializable]
public struct WeaponsData 
{
  
  [field: SerializeField] public string WeaponName {get; private set;}

  [field: SerializeField] public string WeaponAnimationDrawName {get; private set;}

   [field: SerializeField] public string WeaponAnimationDrawNameMovement {get; private set;}
  [field: SerializeField] public GameObject WeaponObject {get;private set;}

  [field: SerializeField] public float ForceWeaponTime {get; private set;} 

  [field: SerializeField] public float ForceWeapon {get; private set;} 

  [field: SerializeField] public int Damage {get; private set;}

  [field: SerializeField] public float Knockback {get; private set;}

  


}
