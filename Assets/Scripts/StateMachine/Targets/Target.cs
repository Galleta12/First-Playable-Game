using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
 

 
 private void OnColliderEnter(Collider other) {
   Debug.Log("Hello it enter");
 }

 private void OnColliderExit(Collider other) {
    Debug.Log("Hello it exit");
 }





 

}
