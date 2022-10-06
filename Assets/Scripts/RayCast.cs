using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
 [SerializeField] private CharacterController controller;
[Range(0,360)]
 public float SphereRadius;

public LayerMask targetMask;


private void Start() {
    
}


private void Update() {
      
    Collider [] hitCollider = Physics.OverlapSphere(transform.position,SphereRadius,targetMask);
    foreach(Collider hit in hitCollider){
       Vector3 dirToTarget = (hit.transform.position - transform.position).normalized;
       float dstToTarget = Vector3.Distance (transform.position, hit.transform.position);   

    }
}

private void OnDrawGizmosSelected() {
    Gizmos.color = Color.white;

    Gizmos.DrawWireSphere(transform.position, SphereRadius);

    
}



}
