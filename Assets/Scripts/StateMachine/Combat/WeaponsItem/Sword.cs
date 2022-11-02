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
 //this is for the knockback of the sword
 private float knockback;


   
   
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
    //Debug.Log(other.gameObject.name);
    // we want to get the health component of the object that we collide with
    if(other.TryGetComponent<Health>(out Health health)){
       // we want to pass the damage and the knockback, therefore we can hanlde the impact state
        // we pass the direction for the knockback
        Vector3 direction = (other.transform.position - mycharacterController.transform.position).normalized;
        
        health.DealDamage(damage,direction * knockback);
        
    }
    
   }

    // with this we set the attack damage
   public void SetAttack(int damage, float knockback){
    this.damage = damage;
    this.knockback = knockback;
   }
}
