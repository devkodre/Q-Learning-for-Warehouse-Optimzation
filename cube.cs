using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour
{
    // Start is called before the first frame update
    //BoxCollider boxCollider; 
    float t = 0f;
    public Vector3 target = new Vector3(-5, 0.5f, 5);
    public float speed = 4;
    public float timeToMove = 2;
    public Vector3 size = new Vector3(1,1,1);
    //GameObject boxCollider;

    void Start()
    {
        //print(target);
        //boxCollider = GetComponent<BoxCollider>();
        GetComponent<BoxCollider>().size = new Vector3(0.95f,0.95f,0.95f);
    }

    // Update is called once per frame
    void Update()
    {
        //move(target);
        //print("" + speed);
        //print("" + transform.position);
        move(target);
    }
    public void move(Vector3 t)
    {
        //Debug.Log("Target: " + t);
        //Debug.Log("Position: " + transform.position);

        float distance = Vector3.Distance(transform.position, t);
        if (distance > 5)
        {
            speed = distance / timeToMove;
        }   
        transform.position = Vector3.MoveTowards(transform.position, t, Time.deltaTime * speed);


    }
    public Vector3 getPos()
    {
        return transform.position;
    }
    public Vector3 getSize()
    {
        return transform.localScale;
    }
    public void setSize(int x, int y, int z)
    {
        size = new Vector3(x,y,z);
        transform.localScale = size;
        //GetComponent<BoxCollider>().size = size;
    }
    public void setSize(Vector3 s)
    {
        size = s;
        transform.localScale = size;
        //GetComponent<BoxCollider>().size = size;
    }
    void OnTriggerEnter(Collider other){
        //print("hit");
        //print("hit " + GetComponent<BoxCollider>().size);
        //if (other.)
    }
    void OnTriggerStay(Collider other){
        //print("hit " + GetComponent<BoxCollider>().size);
        //if (other.)
    }
}
