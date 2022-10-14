using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this the the data of the weapon and the animation
[Serializable]
public struct WeaponsData 
{
  // the name of the weapon
  [field: SerializeField] public string WeaponName {get; private set;}
// the name of the draw animation
  [field: SerializeField] public string WeaponAnimationDrawName {get; private set;}
// name of the draw animation while is moving
   [field: SerializeField] public string WeaponAnimationDrawNameMovement {get; private set;}
 // game object 
  [field: SerializeField] public GameObject WeaponObject {get;private set;}

    [field: SerializeField] public GameObject WeaponCollider {get;private set;}

    [field: SerializeField] public int Damage {get; private set;}

     [field:SerializeField] public  ComboState [] ComboState {get; private set;}

        
}
