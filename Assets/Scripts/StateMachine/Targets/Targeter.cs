using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Targeter : MonoBehaviour
{
   [field: SerializeField] public InputReader InputReader {get; private set;}
   [field: SerializeField] public CharacterController controller {get; private set;}

   [Range(0,360)]
   [field: SerializeField]  public float SphereRadius;

    [field: SerializeField]  public float viewAngle {get; private set;}

   [field: SerializeField] public LayerMask EnemyMask{get; private set;}

   [field: SerializeField] public LayerMask ObstacleMask{get; private set;}

   [field: SerializeField] public LayerMask PlayerMask{get; private set;}
  
  
  public Target currentTarget{get; private set;}




  private List<Transform> currentEnemiesList =  new List<Transform>();

  


    public Transform Maincamera {get; private set; }

   
   // the inputs relative to the camera
    private Vector3 inputs;
// get the height and ceter
    private Vector3 heightPlayer;
    private Vector3 centerPlayer;

// for the gizmos
    private bool enemyDected = false;


    //delegates for the selection of target
    private delegate void SelectionofTarget(bool isdetected, RaycastHit info, Vector3 currentInputs);
    private SelectionofTarget selectionofTarget;



private void Start() {
   
    Maincamera  = Camera.main.transform;
    StartCoroutine("SetClosestTargetDelay",5f);
   // StartCoroutine("SetSelectedTarget",2f);
  
}

private IEnumerator SetClosestTargetDelay(float delay){
    while(true){
        yield return new WaitForSeconds(delay);
        setClosestTarget();
    }
}


private IEnumerator SetSelectedTarget(float delay){
    while(true){
        yield return new WaitForSeconds(delay);
       selectionofTarget = selectTarget;
    }
}





private void Update() {
  
    currentEnemiesList.Clear();

    // the sphere collider
    Collider [] hitCollider = Physics.OverlapSphere(transform.position,SphereRadius,EnemyMask);
     // get the height and center of the player
    heightPlayer = new Vector3(transform.position.x,transform.position.y+controller.height,transform.position.z);
    centerPlayer = new Vector3(transform.position.x,transform.position.y+(controller.height/2),transform.position.z);
    
    inputs = GetCurrentInputs().normalized;

    RaycastHit info;
    for(int i =0; i< hitCollider.Length;i++){

        Transform target = hitCollider[i].transform;
        Vector3 direction = (target.position-transform.position).normalized;
      // get the angle between the target and the target
      // to do this we can compare the direction of where is the target and compare it with the 
      // player position, on the forward direction
      if(Vector3.Angle(transform.forward,direction) < viewAngle){
       float dist = Vector3.Distance(transform.position, target.position);
     
        // first check if user has seleted a target
        
        bool isDetectedAnEnemy = Physics.Raycast(transform.position,inputs,out info,dist,EnemyMask) || 
            Physics.Raycast(heightPlayer,inputs,out info,dist,EnemyMask) || 
            Physics.Raycast(centerPlayer,inputs,out info,dist,EnemyMask);
           if(isDetectedAnEnemy){
            enemyDected = true;
            //selectionofTarget?.Invoke(isDetectedAnEnemy,info,inputs);
            selectTarget(isDetectedAnEnemy,info,inputs);
                
            } else{
                enemyDected =false;
            }      
            
           // here we check if there is a target that is colliding with the obstacle layer
           // if it is colliding we can just avoid that and only add the ones that are not colliding
           // for now the player will only check if something is colliding depengin on the height of the player.
            bool isDetectedAnEnemyAvoidingObtacles = Physics.Raycast(heightPlayer,direction,out info,dist,ObstacleMask);
            if(!isDetectedAnEnemyAvoidingObtacles){
                currentEnemiesList.Add(target);
                //Debug.Log(GetClosestTarget().name);
             
            }
        
         
        }

        }

        checkIfitExists();
    
      
      


}





private void selectTarget(bool isdetected, RaycastHit info, Vector3 currentInputs){
   
   if(InputReader.MovementValue == Vector2.zero){return;}
   Debug.Log("The inputs selection functionis being call");
       // I can do a backwards raycast, from the target, and the direction be the negative inputs and if it hits the player, it would 
       //mean that the direction is the same
       if(info.collider.TryGetComponent<Target>(out Target target)){
        this.currentTarget = target;
       }
    //selectionofTarget -= selectTarget;
}

public Transform GetClosestTarget(){
// // here we are going to get the closet target
// // first we set the first element as the minimun distance
// Transform closesttarget = currentEnemiesList[0];
// for(int i = 0; i < currentEnemiesList.Count; i++){
//      //Transform target = currentEnemiesList[i];
//      float currentdistance= (transform.position-currentEnemiesList[i].position).sqrMagnitude;
//      float distance = (transform.position-closesttarget.position).sqrMagnitude;;
//      if(currentdistance < distance){
//         closesttarget = currentEnemiesList[i];
       
//      }
// }
// return closesttarget;
// new attempt


Transform closestTarget = null;
float shortDist = Mathf.Infinity;
foreach(Transform hit in currentEnemiesList){
    float currentdistance = Vector3.Distance(transform.position,hit.position);
    if(currentdistance<shortDist){
        closestTarget = hit;
        shortDist = currentdistance;
    }

}
return closestTarget;
}






    // get the inputs
private Vector3 GetCurrentInputs(){
   Vector3 cameraForward =  Maincamera.forward;
   Vector3 cameraRight = Maincamera.right;
   cameraForward.y = 0f;
   cameraRight.y = 0f;

   cameraForward.Normalize();
   cameraRight.Normalize();
// read more about the movement relative the camera
   return InputReader.MovementValue.x * cameraRight + 
   InputReader.MovementValue.y  * cameraForward;


}

// Get the direction, of the angle however, I need to watch this more

public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}


private void OnDrawGizmosSelected() {
    
   
    Gizmos.color = Color.white;

    Gizmos.DrawWireSphere(transform.position, SphereRadius);
    
    Vector3 viewAngleA = DirFromAngle(viewAngle,false);
    // to get the direction on the negative axis of z
    Vector3 viewAngleB = DirFromAngle(-viewAngle,false);

    Gizmos.DrawLine(transform.position, transform.position + viewAngleA * SphereRadius);
    Gizmos.DrawLine(transform.position, transform.position + viewAngleB * SphereRadius);

     
     Gizmos.color = Color.red;
     Gizmos.DrawRay(transform.position,inputs);
     Gizmos.DrawRay(heightPlayer,inputs);
     Gizmos.DrawRay(centerPlayer,inputs);
    
    

  
     Gizmos.color = Color.blue;
    foreach(Transform visibleTarget in currentEnemiesList){
      Gizmos.DrawLine(transform.position, visibleTarget.position);
    }
   
 
    Gizmos.color = Color.green; 
    if(enemyDected){
   
    Gizmos.DrawRay(transform.position,inputs);
    Gizmos.DrawRay(heightPlayer,inputs);
     Gizmos.DrawRay(centerPlayer,inputs); 
    }

    
}


public Target returnTarget(){
    return this.currentTarget;
}


public void setClosestTarget(){
    Debug.Log("the set is being call");
    
    if(InputReader.MovementValue != Vector2.zero){return;}
    if(GetClosestTarget() != null){
        if(!GetClosestTarget().TryGetComponent<Target>(out Target target)){return;}
        this.currentTarget = target;
    }
   
}
private void checkIfitExists(){
    Debug.Log("This should work");
    if(currentEnemiesList.Count !=0){
    if(currentTarget !=null){
      if(currentEnemiesList.Contains(currentTarget.transform)){return;}
        this.currentTarget = null;
         }
         
    } else{
        this.currentTarget = null; 
    }
}








}
