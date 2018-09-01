using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour {


    public GameObject bird;
    public Transform spawnPoint;
    public GameObject center;
    Vector2 centerPoint;

    public GameObject miniMap;


    public LineRenderer linerFront;
    public LineRenderer linerBack;

    Bird birdInGame;

    public Transform cataLeft;
    public Transform cataRight;

    public GameObject trailPointPrefab;

    public int birdCount;
    public int pigCount;

    [HideInInspector]
    public GameObject[] birdPool;

    [HideInInspector]
    public GameObject[] pigPool;

    public GameObject pigTempl1;
    public GameObject pigTempl2;

    public Transform spawnPointPig;
    public int maxTrail = 10;

    GameObject[] trailPoints;

    


	void Awake () {


        FindObjectOfType<CameraFollow>().enabled = false;
        centerPoint = GameObject.FindGameObjectWithTag("catapultCenter").transform.position;

        linerFront.SetPosition(0, linerFront.transform.position); 
        linerBack.SetPosition(0, linerBack.transform.position);
        linerFront.sortingLayerName = "foreGround";
        linerBack.sortingLayerName = "foreGround";
        linerFront.sortingOrder = 3;
        linerBack.sortingOrder = 1;

        linerBack.enabled = false;
        linerFront.enabled = false;

        trailPoints = new GameObject[maxTrail];
        InitTrailPointPool();
        InitPools();

        foreach(var p in GameObject.FindGameObjectsWithTag("pig"))
        {
            p.transform.parent.transform.position = spawnPointPig.position;
            p.transform.position = spawnPointPig.position;
        }


    }

    void Update () {

        SetMiniMap();
        CheckWin();




        if (birdInGame == null)
        {
            //Instantiate(bird, spawnPoint.position, new Quaternion());
            Instantiate(center, centerPoint, new Quaternion(),GameObject.FindGameObjectWithTag("Catapult").transform);
            SetNewBird(spawnPoint.position);
        }

        if(birdInGame != null)
        {
            if (birdInGame.isOnCatapult)
            {
                
                miniMap.SetActive(true);
                linerBack.enabled = true;
                linerFront.enabled = true;
                Debug.Log(birdInGame.transform.position);
                linerFront.SetPosition(1, birdInGame.transform.position);
                linerBack.SetPosition(1, birdInGame.transform.position);
                DrawTrail();
            }
            else
            {
                miniMap.SetActive(false);
                linerBack.enabled = false;
                linerFront.enabled = false;
            }

        }



	}

    void InitTrailPointPool()
    {
        for (int i = 0; i < maxTrail; i++)
        {
            var o = Instantiate(trailPointPrefab, new Vector2(-1000, -1000), new Quaternion(0, 0, 0, 1));
            trailPoints[i] = o;
        }
    }

    public void SetTrailVisible(bool visible)
    {
        for (int i = 0; i < maxTrail; i++)
        {
            trailPoints[i].SetActive(visible);
        }
        trailPoints[0].SetActive(false);
    }



    void DrawTrail()
    {
        Vector3 v2 = centerPoint - (Vector2)birdInGame.transform.position;
        Vector2[] segments = new Vector2[maxTrail];

        Vector2 segVelocity = v2 * birdInGame.speed;
        segments[0] = birdInGame.transform.position;

        var offset = segVelocity.magnitude;
        float angle = Vector2.Angle(segVelocity, Vector2.right);
        for (int i = 1; i < maxTrail; i++)
        {
            float time2 = i * Time.fixedDeltaTime * 10;
            segments[i] = segments[0] + segVelocity * time2 + 0.5f * Physics2D.gravity * Mathf.Pow(time2, 2);
        }

        

        for (int i = 0; i < maxTrail; ++i)
        {
            trailPoints[i].transform.position = segments[i];
        }

    }

    void SetMiniMap()
    {
        var cameraPos = GameObject.FindGameObjectWithTag("miniCamera");
        var pigs = GameObject.FindGameObjectsWithTag("pig");
        
        Vector2 point = cameraPos.transform.position;
        if (pigs.Length != 0)
        {
            if (pigs.Length == 1)
            {
                point = pigs[0].transform.position + Vector3.back;
                Debug.Log("Setted");

            }
            else
            {
                Vector2 sum = new Vector2(0, 0);
                foreach (var p in pigs)
                {
                    sum += (Vector2)p.transform.position;
                }

                point = (Vector3)sum / pigs.Length + Vector3.back;
                Debug.Log("Setted");

            }

        }
        cameraPos.transform.position = (Vector3)point + Vector3.back;
    }

    void CheckWin()
    {
        if (birdCount == 0 && pigCount > 0)
        {
            FindObjectOfType<HudController>().EndGame(false);
            FindObjectOfType<SoundController>().EndGame();

        }
        else if (pigCount == 0)
        {
            FindObjectOfType<HudController>().EndGame(true);
            FindObjectOfType<SoundController>().EndGame();
        }
    }

    
    void InitPools()
    {
        
        birdPool = new GameObject[birdCount];
        pigPool = new GameObject[pigCount];

        for(int i = 0; i < birdCount; i++)
        {

            birdPool[i] = Instantiate(bird,new Vector3(1000,1000),Quaternion.identity);
        }

        for (int j = 0; j < pigCount; j++)
        {
            var index = Random.Range(1, 2);
            if (index == 1)
            {
                pigPool[j] = Instantiate(pigTempl1, new Vector3(1000, 1000), Quaternion.identity);
            }
            if(index == 2)
            {
                pigPool[j] = Instantiate(pigTempl2, new Vector3(1000, 1000), Quaternion.identity);
            }
        }


    }


    void SetNewBird(Vector2 pos)
    {
        foreach (var b in birdPool)
        {
            if (b != null)
            {
                birdInGame = b.GetComponent<Bird>();
                birdInGame.gameObject.name = "birdInGame";
                birdInGame.transform.position = pos;
                birdInGame.gameObject.SetActive(true);
                break;
            }
        }

        foreach (var b in birdPool)
        {
            if (b != null)
            {
                if (b.gameObject.name != "birdInGame")
                {
                    b.SetActive(false);
                }
            }
        }
    }
}
