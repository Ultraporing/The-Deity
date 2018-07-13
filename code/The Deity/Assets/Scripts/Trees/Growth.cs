using Assets.Scripts.Environment.Planet;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : MonoBehaviour {

    //Growth - Lea Kohl, Added Astar - Tobias Lenz
    public float maxSize;
    float currentSize;
    bool isGrown = false;

	// Use this for initialization
	void Start () {
		
	}
	
	/// <summary>
    /// Grow the Tree/Bush/Grameobject until the max size is reached and update the Pathfinding
    /// </summary>
	void Update () {
        currentSize = transform.localScale.y;

        if (currentSize < maxSize)
        {
            transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
            
            var guo = new GraphUpdateObject(GetComponentInChildren<Collider>().bounds);
            guo.updatePhysics = true;
            AstarPath.active.UpdateGraphs(guo);
        }
        else if (!isGrown)
        {
            isGrown = true;
            var guo = new GraphUpdateObject(GetComponentInChildren<Collider>().bounds);
            guo.updatePhysics = true;
            AstarPath.active.UpdateGraphs(guo);
        }
	}
}
