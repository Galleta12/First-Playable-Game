using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
 

 // create and event that it will trigger when this component is removed

 public event Action<Target> OnDestroyed;
// this will call the event as soon as the component is dead
 private void OnDestroy() {
  
   OnDestroyed?.Invoke(this);
 }






 

}
