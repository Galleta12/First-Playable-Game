using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargeterDotProduct : MonoBehaviour
{
  
   [field: SerializeField] public InputReader InputReader {get; private set;}
   [field: SerializeField] public CharacterController controller {get; private set;}

   [Range(0,360)]
   [field: SerializeField]  public float SphereRadius;

    [field: SerializeField]  public float viewAngle {get; private set;}

   [field: SerializeField] public LayerMask EnemyMask{get; private set;}

   [field: SerializeField] public LayerMask ObstacleMask{get; private set;}

   [field: SerializeField] public LayerMask PlayerMask{get; private set;}
   
   // threshold for the dot product
   [field: SerializeField] public float threshold{get; private set;}

  public Target currentTarget{get; private set;} = null;




  public List<Transform> currentEnemiesList{get;private set;} =  new List<Transform>();

  


    public Transform Maincamera {get; private set; }

   
   // the inputs relative to the camera
    private Vector3 inputs;

    private Vector3 centerPlayer;

    private Vector3 heightPlayer;


    //delegates for the selection of target
    private delegate void SelectionofTarget(bool isdetected, RaycastHit info, Vector3 currentInputs);
    private SelectionofTarget selectionofTarget;



private void Start() {
   
    
    Maincamera  = Camera.main.transform;
    StartCoroutine("TargetSelection",.5f);


}


private IEnumerator TargetSelection(float delay){
    
    while(true){
        yield return new WaitForSeconds(delay);
        StartSelectionCourotine();
    }


}






//we are going to use dot product with two vectors to get the look pecentage of seeing a target, with a threshold will be the best
// way to check if a player is looking an object.
// the two vectors must be the raycast direction and the direction from the player to the target




private void StartSelectionCourotine() {
  
    currentEnemiesList.Clear();

    // the sphere collider
    Collider [] hitCollider = Physics.OverlapSphere(transform.position,SphereRadius,EnemyMask);
     // get the height and center of the player
   
    centerPlayer = new Vector3(transform.position.x,transform.position.y+(controller.height/2),transform.position.z);
    heightPlayer = new Vector3(transform.position.x,transform.position.y+(controller.height),transform.position.z);
    inputs = GetCurrentInputs().normalized;
   
        for(int i =0;i<hitCollider.Length;i++){
           
          Transform target = hitCollider[i].transform;
          Vector3 direction = (target.position-transform.position).normalized;
          
          if(Vector3.Angle(transform.forward,direction)< viewAngle){
           float dist = Vector3.Distance(transform.position,target.position);
            selectTarget(inputs,target,dist);
            currentEnemiesList.Add(target);      
             
          }
        
        }

   checkIfitExists();

}

// we need to keep track of the closest selectable

private void selectTarget( Vector3 currentInputs,Transform target, float dist){
  
  //RaycastHit info;
 

  Ray [] currentRay = CreateRay(currentInputs); 
   // this is for the center of the player ray
  Vector3 vector1 = currentRay[0].direction;
  Vector3 vector2= target.position - currentRay[0].origin;
  // this is the height of the ray
  Vector3 vector3 = currentRay[1].direction;
  Vector3 vector4 = target.position - currentRay[1].origin;

  float lookPecentagecenter = Vector3.Dot(vector1.normalized,vector2.normalized);
  float lookPecentageheight = Vector3.Dot(vector3.normalized,vector4.normalized);
  //bool sphereCast = Physics.Raycast(centerPlayer,currentInputs,out info,dist,EnemyMask);
  
  
  if(lookPecentagecenter > threshold || lookPecentageheight > threshold){
   //closest = lookPecentage;
    
    if(target.TryGetComponent<Target>(out Target target1)){
        if(target != currentTarget){
            this.currentTarget = target1;
            target1.OnDestroyed += SetNullDestroyed;
        }
        
    }
   
  }
  

}


private Ray[] CreateRay(Vector3 inputs){

    Ray [] ray = new Ray[2];
    ray[0]=new Ray(centerPlayer,inputs);
    ray[1]=new Ray(heightPlayer,inputs);
    
    return ray;
}


private void checkIfitExists(){
    if(currentTarget == null){return;}
    if(currentEnemiesList.Count != 0){return;}
    if(currentEnemiesList.Contains(currentTarget.transform)){return;}
    this.currentTarget = null;
}


// this is saved on a event
//therefore it will trigger is the enemy is dead
// at the same time we unsubscribe from the event
private void SetNullDestroyed(Target target1){

   this.currentTarget = null;
   target1.OnDestroyed -= SetNullDestroyed;
   
  
}




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

// need to read more about bool
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
    
    
     Gizmos.DrawRay(centerPlayer,inputs );
     Gizmos.DrawRay(heightPlayer,inputs );
    
    

  
    //  Gizmos.color = Color.blue;
    // foreach(Transform visibleTarget in currentEnemiesList){
    //   Gizmos.DrawLine(transform.position, visibleTarget.position);
    // }
   
 
    Gizmos.color = Color.green; 
    if(currentTarget == null){return;}
     Gizmos.DrawWireSphere(currentTarget.transform.position,0.2f) ;
     
     
    

    
}





}
