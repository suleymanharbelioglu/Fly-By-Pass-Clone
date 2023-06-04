using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CharacterController : MonoBehaviour
{
    public Rigidbody rb;
    private Animator animator;
    public float axisX ; 
    public Vector3 moveVector;
    public float movespeed;
    public float flySpeed;
    public float flyUpLimit;
    public float rotateLerp;
    public bool isFlying = false;
    public bool isRunning = false;
    // char collected cubes
    public TextMeshProUGUI charCollectedCubesText;
    
    // timer 
    public float timerValue = 0;
    public int charCollectedCube = 0;
    // flying wings
    public GameObject cube;
    public Transform charCubesParent;
    public Transform rightPos;
    public Transform leftPos;
    public Vector3 prevRightPos;
    public Vector3 prevLeftPos;
    public List<GameObject> collectedCubes = new List<GameObject>();
    public int i = 0;
    public int j = 0;
    public Transform firtCubeTakenParent;

    public float cubespeed;
    public float moveSensitivity;
    // floating point
    public GameObject floatingPoint;
    // score
    public Transform bot1;
    public Transform bot2;
    
    // fly distance
    public TextMeshProUGUI charFlyDistance;
    public Transform flystartPoint;
    public Vector3 flyEndPoint;
    public float flyingDistance;
    public bool distanceMeasureStart = false;
    public GameObject floatingOrder;
    
    
    private void Start() {
        moveVector = Vector3.forward;
        animator = GetComponent<Animator>();
        prevRightPos = rightPos.transform.localPosition;
        prevLeftPos = leftPos.transform.localPosition;
    }
    private void FixedUpdate() {
        
            
            if(isRunning && GameManager.instance.hasGameStarted && !GameManager.instance.hasgameEnded)
            {
                animator.SetBool("isRunning",true);
                Movement();
            }
            else
            {
                animator.SetBool("isRunning",false);
            }
            if(isFlying && GameManager.instance.hasGameStarted &&!GameManager.instance.hasgameEnded )
            {
                animator.SetBool("isFlying",true);
                Movement();
                Flying();
                
            }
            else
            {
                animator.SetBool("isFlying",false);
            }
        GameOver();
            if(GameManager.instance.hasgameEnded)
            {
                rb.isKinematic = true;
            }
        
        MeasureDistance();
        
            
       
    }
    public void MeasureDistance()
    {   if(distanceMeasureStart)
    {
        flyingDistance = transform.position.z - flystartPoint.position.z;
        charFlyDistance.enabled = true;
        
        charFlyDistance.text = flyingDistance.ToString();

    }
        

    }
    public void GameOver()
    {
        if(transform.position.y <= -10f)
        {
            GameManager.instance.GameOverFunc();
        }
    }



    public void Movement()
    {
        axisX = Input.GetAxis("Mouse X");
        moveVector.x += axisX*Time.deltaTime*moveSensitivity;
        moveVector.x = Mathf.Clamp(moveVector.x, -1f, 1f);
        
        if(moveVector.x >= 0 )
        {
            moveVector.z -= axisX*Time.deltaTime*moveSensitivity;
        }
        else if(moveVector.x < 0)
        {
            moveVector.z += axisX*Time.deltaTime*moveSensitivity;
        }
        moveVector.z = Mathf.Clamp(moveVector.z, 0f, 1f);
        transform.position += moveVector*movespeed*Time.deltaTime;
         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVector),rotateLerp*Time.deltaTime);
        

    }

    public void Flying()
    {
        rb.useGravity = false;
        Vector3 delta = transform.position;
        delta.y += flySpeed*Time.deltaTime;
        delta.y = Mathf.Clamp(delta.y, -50f, flyUpLimit);
        transform.position = delta;
        
        discreaseWings();

    }
    public void discreaseWings()
    {
        Timer();

    }

    public void CubeCollection()
    {
        charCollectedCube += 3;
        charCollectedCubesText.text = charCollectedCube.ToString();
        

    }

    public void Timer()
    {
        timerValue += Time.deltaTime*10f;
        if(timerValue >=1)
        {
            timerValue = 0;
            if(collectedCubes.Count  >=1){
            
            collectedCubes[collectedCubes.Count-1].transform.SetParent(null);
            
            collectedCubes[collectedCubes.Count-1].GetComponent<Rigidbody>().useGravity = true;
            collectedCubes[collectedCubes.Count-1].GetComponent<Rigidbody>().isKinematic = false;
            collectedCubes[collectedCubes.Count-1].GetComponent<Rigidbody>().AddForce(new Vector3(-moveVector.x, -1f,-moveVector.z)*cubespeed,ForceMode.Impulse);
            Destroy(collectedCubes[collectedCubes.Count-1],1f);
            collectedCubes.Remove(collectedCubes[collectedCubes.Count-1]);
            

            }
            charCollectedCube -=1;
            charCollectedCubesText.text = charCollectedCube.ToString();

        }
        if(charCollectedCube <= 0)
        {
            print("collected cubes 0");
            rb.AddForce(moveVector * movespeed/2f, ForceMode.Impulse);
            isFlying = false;
            rb.useGravity = true;
            prevRightPos = rightPos.transform.localPosition;
            prevLeftPos = leftPos.transform.localPosition;
            i = 0; 
            j = 0;
        }
    }

    public void CreateWings()
    {
        print("create wings");
        if(j == 0)
        {
            FirstRight();
            j = 1;

        }
        else if(j ==1)
        {
            FirstLeft();
            j = 2;

        }
        else if(j == 2)
        {
            if(i == 0)
            {
                AdtoRight();
                i = 1;

            }
            else if(i == 1)
            {
                AdToLeft();
                i = 0;
            }

        }

        

    }
    public void FirstRight()
    {
    
        GameObject wingCube = Instantiate(cube);
        
        wingCube.transform.SetParent(charCubesParent);
        wingCube.transform.localPosition = prevRightPos;
        collectedCubes.Add(wingCube);
        
        

    }
    public void FirstLeft()
    {
        
        GameObject wingCube = Instantiate(cube);
        
        wingCube.transform.SetParent(charCubesParent);
        wingCube.transform.localPosition = prevLeftPos;
        collectedCubes.Add(wingCube);
        
        
        
    }

    public void AdToLeft()
    {
        GameObject wingCube = Instantiate(cube);
        prevLeftPos.x -= 0.1f;
        prevLeftPos.y = 0;
        prevLeftPos.z = 0;
        wingCube.transform.SetParent(charCubesParent);
        wingCube.transform.localPosition = prevLeftPos;
        collectedCubes.Add(wingCube);
        
        


    }
    public void AdtoRight()
    {
        GameObject wingCube = Instantiate(cube);
        prevRightPos.x += 0.1f;
        prevRightPos.y = 0;
        prevRightPos.z = 0;
        wingCube.transform.SetParent(charCubesParent);
        wingCube.transform.localPosition = prevRightPos;
        collectedCubes.Add(wingCube);
        
        


        
        

    }
    
    public void EndedOrder()
    {
        if(transform.position.z > bot1.position.z && transform.position.z > bot2.position.z)
        {
            print("you win");
            
            GameManager.instance.finishOrder = "1st";
            if(GameManager.instance.currentLevelBuildIndex == 2)
            {
                GameManager.instance.GameEndedFunc();
            }
            else
            {
                GameManager.instance.Winfunc();

            }
            
        }
        else if(transform.position.z > bot1.position.z && transform.position.z <bot2.position.z || transform.position.z < bot1.position.z && transform.position.z > bot2.position.z)
        {
            print("second");
            GameManager.instance.finishOrder = "2nd";
            GameManager.instance.GameOverFunc();
        }
        else if(transform.position.z < bot1.position.z && transform.position.z < bot2.position.z)
        {
            print("your are an idiot try again");
            GameManager.instance.finishOrder = "3rd";
            GameManager.instance.GameOverFunc();
        }
    }
    
    
  

    

    private void OnTriggerEnter(Collider other) {
        
        
        // cube collection
        if(other.transform.CompareTag("Cube")){
            Instantiate(floatingPoint, other.transform.position, Quaternion.identity);
            firtCubeTakenParent.transform.gameObject.SetActive(true);
            CubeGenerate.instance.Generate(other.transform.localPosition,other.transform.parent);
            Destroy(other.gameObject);
            CubeCollection();
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.transform.CompareTag("RoadTrigger") && !isFlying)
        {
            Debug.Log("road trigger exit");
            
            isRunning = false;
            firtCubeTakenParent.transform.gameObject.SetActive(false);
            for(int i = 0; i < charCollectedCube; i++)
        {
            
            CreateWings();
            print("i : "+i);
        }
         isFlying = true;
        }
        if(other.transform.CompareTag("FlyPoint"))
        {
            print("fly start");
            distanceMeasureStart = true;

        }
    }
    private void OnTriggerStay(Collider other) {
        if(other.gameObject.CompareTag("RoadTrigger") && !isFlying)
        {
            isRunning = true;
        }
    }
        
    
   
   private void OnCollisionEnter(Collision other) {
    if(other.transform.CompareTag("Finish1"))
    {
        GameManager.instance.finishCamMove = true;
        
        StartCoroutine("GOCoroutine");

    }
   }
   IEnumerator GOCoroutine()
   {
    yield return new WaitForSecondsRealtime(2f);
    EndedOrder();
    
    //GameManager.instance.GameOverFunc();
   }

    
}
