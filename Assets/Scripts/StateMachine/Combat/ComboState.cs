using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is what let you call it on another class and see it on the inspector
[Serializable]
public class ComboState 
{

[field: SerializeField] public string AnimationNameCombo {get; private set;}

// this is the variable that will take the transition duration 
[field: SerializeField] public float TransitionDuration {get; private set;}
[field: SerializeField] public int ComboNextStateIndex {get; private set;} = -1;

// this how far an attack will let you to do the next attack
//this means if we are ready to perform the next combo
[field: SerializeField] public float ComboAttackTime {get; private set;} 
}
