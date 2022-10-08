using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct WeaponsData 
{
  
  [field: SerializeField] public string WeaponName {get; private set;}
  [field: SerializeField] public GameObject WeaponObject {get; private set;}

  [field: SerializeField] public float ForceWeaponTime {get; private set;} 

  [field: SerializeField] public float ForceWeapon {get; private set;} 

  [field: SerializeField] public int Damage {get; private set;}

  [field: SerializeField] public float Knockback {get; private set;}

  


}
