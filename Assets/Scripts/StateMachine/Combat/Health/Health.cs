using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{

// the max health that it can have each character on the game

// we want to manually set the health of each character
[field: SerializeField] public int maxhealth {get;private set;} = 100;
//the current health
private int currentHealth;
//set ivunerable for testing and for blocking
private bool isInvunerable;

// events to check if the player is death or if is taking damage

public event Action<Vector3> OnTakeDamage;
//
public event Action OnDeath;
//bool se to check if is dead

public bool isDead => currentHealth == 0;




private void Start() {
   currentHealth = maxhealth;
}



public void setInvunerable(bool isInvunerable){
   this.isInvunerable = isInvunerable;
}

public void DealDamage(int damage,Vector3 directionKnockback){
    // if the health is 0, we want dont want to do anything
    if(currentHealth == 0){return;}
    // if the character is invunerable we don't want to do anything
    if(isInvunerable){return;}




    // currentHealth -= damage;
    // if(currentHealth < 0){
    //     currentHealth = 0;
    // }
    // this is a best wat to do the same code that is abobe
    currentHealth = Mathf.Max(currentHealth - damage,0);

    
    //trigger the on take damage action and on death action for the enemy state machine
    //first we get the direction
    OnTakeDamage?.Invoke(directionKnockback);
    if(currentHealth == 0){
      OnDeath?.Invoke();
    }
     
     Debug.Log(currentHealth);

}

   
}
