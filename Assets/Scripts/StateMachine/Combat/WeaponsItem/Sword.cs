using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
   [field: SerializeField] public Rigidbody RB {get; private set;}


[field: SerializeField] public Collider   mycharacterController {get; private set;}
// this is to keep track of the object that collide with the sword
private List<Collider> alreadyCollideWith = new List<Collider>();

 private int damage;


   
   
   //this is call when this object is emable on the weaponHandler
   private void OnEnable() {
    // we want to clear the list of the objects that we collide
    // as soon as this object is enable
    alreadyCollideWith.Clear();
   
   }

   
   private void OnTriggerEnter(Collider other) {
    //we dont want to do anything is the sword hits our collider
    if(other == mycharacterController){return;}
    //if the object already exists on the list
    //it means that we already hit it, therefore we dont want to do anything
    if(alreadyCollideWith.Contains(other)){return;}
    alreadyCollideWith.Add(other);
    Debug.Log(other.gameObject.name);
    // we want to get the health component of the object that we collide with
    if(other.TryGetComponent<Health>(out Health health)){
        health.DealDamage(damage);
        // Debug.Log(health.currentHealth);
    }
   }


   public void SetAttack(int damage){
    this.damage = damage;
   }
}
