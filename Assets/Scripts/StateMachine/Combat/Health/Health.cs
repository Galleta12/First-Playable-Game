using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

private int maxhealth = 100;

public int currentHealth;


private void Start() {
   currentHealth = maxhealth;
}

public void DealDamage(int damage){
    // if the health is 0, we want dont want to do anything
    if(currentHealth == 0){return;}
    // currentHealth -= damage;
    // if(currentHealth < 0){
    //     currentHealth = 0;
    // }
    // this is a best wat to do the same code that is abobe
    currentHealth = Mathf.Max(currentHealth - damage,0);

     Debug.Log(currentHealth);

}


}
