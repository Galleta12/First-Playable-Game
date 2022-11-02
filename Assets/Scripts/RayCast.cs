using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{


[field: SerializeField] public InputReader InputReader {get; private set;}
[field: SerializeField] public CharacterController controller {get; private set;}

[Range(0,360)]
[field: SerializeField]  public float SphereRadius;


[field: SerializeField] public LayerMask targetMask{get; private set;}

public Transform MainCameraPlayer {get; private set; }

// set data structure
public HashSet<Collider> currentEnemies {get; private set;}= new HashSet<Collider>();

// to delete the enemies, since is better to loop on a copy of the set on a list and then delete it on the
// main set
public List<Collider> checkerHash {get; private set;}


//public event Action EnemyOnSphere;

// the inputs relative to the camera
private Vector3 inputs;
// get the height and ceter
 private Vector3 heightPlayer;
 private Vector3 centerPlayer;

// for the gizmos
private bool enemyDected = false;


private void Start() {
    MainCameraPlayer = Camera.main.transform;
}


private void Update() {
      
   
   // the sphere collider, this may change later on
    Collider [] hitCollider = Physics.OverlapSphere(transform.position,SphereRadius,targetMask);
     // get the height and center of the player
      heightPlayer = new Vector3(transform.position.x,transform.position.y+controller.height,transform.position.z);
     centerPlayer = new Vector3(transform.position.x,transform.position.y+(controller.height/2),transform.position.z);
   // we want the direction of the inputs
   inputs = GetCurrentInputs().normalized;
   
    // we check everynthig based on the shpere collider
    foreach(Collider hit in hitCollider){
          // get the direction of the targets and the distance
          Vector3 direction = (hit.transform.position-transform.position).normalized;
          float dist = Vector3.Distance(transform.position,hit.transform.position);
            // we cast an array on the player on the three main position,
            // we only check the ones of the targermask later and if is detected we can set it as true;
            if(Physics.Raycast(transform.position,inputs,dist,targetMask) || 
            Physics.Raycast(heightPlayer,inputs,dist,targetMask) || 
             Physics.Raycast(centerPlayer,inputs,dist,targetMask)){
                 enemyDected =true;
            }else{
                enemyDected = false;
            }
                  
                  currentEnemies.Add(hit);
                  //Debug.Log(hit.gameObject.name);     
    }

     checkerHash = new List<Collider>(currentEnemies);
    foreach(Collider hit in checkerHash){
        
        bool exists = Array.Exists(hitCollider, e =>e==hit);
        
        if(!exists){
          currentEnemies.Remove(hit);
        }
    }

   // checkElementsOnSet();
}

private void OnDrawGizmosSelected() {
    
   
    
    
    
    Gizmos.color = Color.white;

    Gizmos.DrawWireSphere(transform.position, SphereRadius);
    
      Gizmos.color = Color.red;
     Gizmos.DrawRay(transform.position,inputs);
     Gizmos.DrawRay(heightPlayer,inputs);
     Gizmos.DrawRay(centerPlayer,inputs);
    
    if(enemyDected){
    Gizmos.color = Color.blue;
    Gizmos.DrawRay(transform.position,inputs);
    Gizmos.DrawRay(heightPlayer,inputs);
     Gizmos.DrawRay(centerPlayer,inputs); 
    }

    
}

// private void checkElementsOnSet(){
//     if(currentEnemies.Count != 0){
//       foreach(Collider hit in currentEnemies){
//         Debug.Log(hit.gameObject.name);
//     }
//     }


    
// }

private Vector3 GetCurrentInputs(){
   Vector3 cameraForward =  MainCameraPlayer.forward;
   Vector3 cameraRight = MainCameraPlayer.right;
   cameraForward.y = 0f;
   cameraRight.y = 0f;

   cameraForward.Normalize();
   cameraRight.Normalize();

   return InputReader.MovementValue.x * cameraRight + 
   InputReader.MovementValue.y  * cameraForward;


}








}
