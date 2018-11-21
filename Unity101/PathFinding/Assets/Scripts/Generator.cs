using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObjType
{
    Point,
    Obstacle,
    FootStep,
    Finder
}

public class Generator : MonoBehaviour {

    public GameObject point;
    public GameObject obstacle;
    public GameObject footStep;
    public GameObject finder;

    List<GameObject> pointsPool;
    List<GameObject> obstaclesPool;
    List<GameObject> footStepsPool;
    List<GameObject> findersPool;

    public int height;
    public int width;

    
    

	void Start () {
        pointsPool = new List<GameObject>();
        obstaclesPool = new List<GameObject>();
        footStepsPool = new List<GameObject>();
        findersPool = new List<GameObject>();
        InitEverthing();
	}

    public void DrawObject(ObjType type,int x,int y,int num)
    {
        var pos = x + Mathf.Abs(y) * width;
        switch (type)
        {
            case ObjType.Point:
                pointsPool[pos].SetActive(true);
                pointsPool[pos].tag = "Point";
                break;
            case ObjType.Obstacle:
                obstaclesPool[pos].SetActive(true);
                obstaclesPool[pos].transform.parent
                    = GameObject.FindGameObjectWithTag("Obstacle").transform; 
                break;
            case ObjType.Finder:
                findersPool[pos].SetActive(true);
                findersPool[pos].transform.parent
                    = GameObject.FindGameObjectWithTag("FootStep").transform;
                findersPool[pos].gameObject.GetComponentInChildren<TextMesh>().
                    text = num.ToString();
                break;
            case ObjType.FootStep:
                footStepsPool[pos].SetActive(true);
                break;
        }
    }

    void InitEverthing()
    {
        var position = new Vector3(0.5f, 0, -0.5f);
        for (int i = 0; i< height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                InitObj(ObjType.Point, point, position);
                InitObj(ObjType.Finder, finder, position);
                InitObj(ObjType.Obstacle, obstacle, position);
                InitObj(ObjType.FootStep, footStep, position);
                position.x++;
            }
            position.x -= 30;
            position.z--;
        }
    }

    void InitObj(ObjType type,GameObject obj,Vector3 position)
    {
        var o = Instantiate(obj, position, Quaternion.identity);
        o.SetActive(false);
        switch (type)
        {
            case ObjType.Point:
                pointsPool.Add(o);
                break;
            case ObjType.Obstacle:
                obstaclesPool.Add(o);
                break;
            case ObjType.FootStep:
                footStepsPool.Add(o);
                break;
            case ObjType.Finder:
                findersPool.Add(o);
                break;
        }
    }

}
