using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BotController : MonoBehaviour
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
    public Transform CubesParent;
    public List<Transform> fourCubeTransform = new List<Transform>();
    public List<Vector3> botTartgesList = new List<Vector3>();
    //public Transform fourCube;
    public Vector3 currentDirection;
    public Transform finishLine;
    public Transform scoreLine;
    public int random1 = 0;
    public int random2 = 0;
    public bool haveTarget = false;
    
    public int GiveTargetInt = -1;
    

    public GameObject floatingPoint;
    
    private void Start() {
        moveVector = Vector3.forward;
        animator = GetComponent<Animator>();
        prevRightPos = rightPos.transform.localPosition;
        prevLeftPos = leftPos.transform.localPosition;
        AddAllTargetsToList();
        
    }
    private void FixedUpdate() {
        
            
            if(isRunning && GameManager.instance.hasGameStarted)
            {
                animator.SetBool("isRunning",true);
                Movement();
            }
            else
            {
                animator.SetBool("isRunning",false);
            }
            if(isFlying && GameManager.instance.hasGameStarted )
            {
                animator.SetBool("isFlying",true);
                Movement();
                Flying();
                
            }
            else
            {
                animator.SetBool("isFlying",false);
            }
        
        
        
            
       
    }
    public void AddAllTargetsToList()
    {
        for( int i = 0 ; i < CubesParent.childCount ; i++)
        {
            fourCubeTransform.Add(CubesParent.GetChild(i).transform);
        }
        while( random2 < fourCubeTransform.Count-1)
        {
            random1 = Random.Range(random2+1,random2+3);
            if(random1 >= fourCubeTransform.Count-1)
            {
                random1 = fourCubeTransform.Count-1;
                break;
            }
            botTartgesList.Add(fourCubeTransform[random1].position);
            random2 = random1;
            
        }
        botTartgesList.Add(finishLine.position);
        botTartgesList.Add(scoreLine.position);

    }



    public void Movement()
    {
        //axisX = Input.GetAxis("Mouse X");
        //moveVector.x += axisX*Time.deltaTime*moveSensitivity;
       // currentDirection = GiveTarget() - transform.position;
        
        
        GiveTarget();
        transform.position += moveVector*movespeed*Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVector),rotateLerp*Time.deltaTime);
         
        

    }
    
    public void GiveTarget()
    {
        if(botTartgesList.Count-1 > GiveTargetInt && !haveTarget)
        {
            GiveTargetInt+=1;
            currentDirection = botTartgesList[GiveTargetInt] - transform.position;
            currentDirection.Normalize();
            currentDirection.y = 0;
        
            moveVector = currentDirection;
            haveTarget = true;

        }
        else
        {
           isRunning = false;

        }
        

        
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
            collectedCubes[collectedCubes.Count-1].GetComponent<Rigidbody>().AddForce(new Vector3(-moveVector.x, -8f,-moveVector.z)*cubespeed,ForceMode.Impulse);
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
    

    

    private void OnTriggerEnter(Collider other) {
        if(other.transform.CompareTag("FourCube"))
        {
            Debug.Log("four cube");
            haveTarget = false;
        }
        
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
    }
    private void OnTriggerStay(Collider other) {
        if(other.gameObject.CompareTag("RoadTrigger") && !isFlying)
        {
            isRunning = true;
        }
    }
        
    

}
