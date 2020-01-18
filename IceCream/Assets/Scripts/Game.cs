using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game : MonoBehaviour
{
    Transform[] Locations= new Transform[29];
    IceBalls[] AllBalls = new IceBalls[29];
    GameObject[] ExampleBalls = new GameObject[29];

    public Material Pink, Green;
    public Slider sl;
    public GameObject Ball,Taap,BaseCone,AddedBalls;
    public Text text;

    GameObject ActualExample;
    int count = 0,examplecount=0;    
    float timer=0,s,endtimer=0,match=0;
    public GameObject[] ExampleCone = new GameObject[2];
    bool endcondition = false;
    
    
    void Start()
    {
        count = 0;
       ActualExample= Instantiate(ExampleCone[examplecount], BaseCone.transform.position, Quaternion.Euler(180, 0, 0));       
        for (int i = 0;i < 29;i++)
        {
            Locations[i] = GameObject.Find(i.ToString()).transform;
            ExampleBalls[i] = ExampleCone[examplecount].transform.GetChild(3).transform.GetChild(i).gameObject;
        }

        


    }

    // Update is called once per frame
    void Update()
    {
        if (endcondition == false)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100) && count < 29)
                {
                    if (timer > 0.20)
                    {
                        if (hit.collider.name == "GreenButton")
                            CreateBall(Green);
                        else if (hit.collider.name == "PinkButton")
                            CreateBall(Pink);
                        s = (count + 1) / 29f;
                        sl.value = s;

                        timer = 0;
                    }
                }
            }
            for (int i = 0; i < 29; i++)
            {
                if (AllBalls[i] != null)
                    AllBalls[i].MoveBall();
            }
            timer += Time.deltaTime;

            if (count == 29)
            {
                for (int i = 0; i < 29; i++)
                {
                    if (ExampleBalls[i].GetComponent<Renderer>().sharedMaterial.name == AllBalls[i].getColor().name)
                        match++;
                }
                
                count = 30;
                
                text.text = ("%"+(match / 29) * 100).ToString();
            }
            else if(count==30)
            {
                endtimer += Time.deltaTime;
                if (endtimer > 2)
                {
                    text.text = "Click To Continue";
                    endcondition = true;
                }
            }
        }
        else
        {

            if (Input.GetMouseButtonUp(0))
            {
                endcondition = false;
                examplecount++;
                Destroy(ActualExample);
                ActualExample = Instantiate(ExampleCone[examplecount], BaseCone.transform.position, Quaternion.Euler(180, 0, 0));
                for (int i = 0; i < 29; i++)
                {
                    ExampleBalls[i] = ExampleCone[examplecount].transform.GetChild(3).transform.GetChild(i).gameObject;
                }
                ResetAllVariables();
                Destroy(AddedBalls);
                AddedBalls = Instantiate(AddedBalls, new Vector3(0, 0, 0), Quaternion.identity);
                AddedBalls.name = "AddedBalls";
                AllBalls = new IceBalls[29];
            }


        }
        
    }

   private class IceBalls
    {
        Transform Destination;
        Material Color;
        GameObject Ball;
        
       public IceBalls(Material color,Transform des,GameObject Sphere)
        {
            Color = color;
            Destination = des;
            Ball = Sphere;
            Ball.GetComponent<Renderer>().material = Color;
        }
     public  void  MoveBall()
        {
           Ball.transform.position= Vector3.Lerp(Ball.transform.position, Destination.position, 3*Time.deltaTime);            
        }
     public Material getColor()
        {
            return Color;
        }
    }

   void CreateBall(Material Button)
    {
        GameObject temp;
        temp=Instantiate(Ball, Taap.transform.position,Quaternion.identity);
        temp.transform.parent = GameObject.Find("AddedBalls").transform;
        AllBalls[count] = new IceBalls(Button, Locations[count], temp);
        count++;
    }
    void ResetAllVariables()
    {
        count = 0;
        timer = 0;
        endtimer = 0; match = 0;
        sl.value = 0;
    }
}
