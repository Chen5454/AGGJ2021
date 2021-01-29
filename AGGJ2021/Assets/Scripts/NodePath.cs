using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePath : MonoBehaviour
{
    public GameObject[] Nodes;

    public int currNodeNum;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 getCurrNode()
    {
        return Nodes[currNodeNum].transform.position;
    
    }

    public void MoveToNextNode()
    {
        currNodeNum++;

        if (Nodes.Length <= currNodeNum)
        {
            currNodeNum = 0;
        }
    }

}
