using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
public class PriorityQueue<T>
{
    public struct Element
    {
        public T value;
        public float priority;
        public static Element Create(T value, float priority)
        {
            Element element = new Element();
            element.value = value;
            element.priority = priority;

            return element;
        }
    }

    private List<Element> elements = new List<Element>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, float priority)
    {
        elements.Add(Element.Create(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].priority < elements[bestIndex].priority)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].value;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}


public class PathFinding : MonoBehaviour
{
    public int[,] map;

    const int walkable = 0;
    const int obstacle = 1;
    const int find = 2;
    const int start = 8;
    const int end = 9;
    const int step = 10;

    Generator mapGen;

    int height;
    int width;

    int startX;
    int startY;

    int endX;
    int endY;

    public float weight_H;
    public float weight_G;

    public enum FindingState
    {
        start,
        Wait,
        Find,
        Finding,
        Over
    }

    public enum FindingType
    {
        BFS,
        DFS,
        DFSI,
        DFSII,
        AStar
    }


    FindingState state;
    public FindingType type;

    private void Start()
    {
        mapGen = FindObjectOfType<Generator>();
        height = mapGen.height;
        width = mapGen.width;
        map = new int[height, width];
        searchedMap = new int[height, width];
        for (int i = 0; i < searchedMap.GetLength(0); i++)
        {
            for (int j = 0; j < searchedMap.GetLength(1); j++)
            {
                searchedMap[i, j] = int.MaxValue;
            }

        }

        
    }

    void Update()
    {
        Refresh();
        DoSomething();
    }

    void GetInput()
    {
        var ps = GameObject.FindGameObjectsWithTag("Point");
        if (ps.Length == 2)
        {
            state = FindingState.Find;
        }

        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = new RaycastHit();
            Physics.Raycast(ray, out hit, 100);
            if (hit.collider != null && hit.collider.name == "Plane")
            {
                var x = Mathf.CeilToInt(hit.point.x) - 1;
                var y = Mathf.Abs(Mathf.CeilToInt(hit.point.z));
                if (ps.Length == 0)
                {
                    map[y, x] = start;
                    startX = x;
                    startY = y;
                }

                if (ps.Length == 1)
                {
                    map[y, x] = end;
                    endX = x;
                    endY = y;
                }
            }
        }
    }

    void DoSomething()
    {
        switch (state)
        {
            case FindingState.start:
                ReadMap();
                break;
            case FindingState.Wait:
                GetInput();
                break;
            case FindingState.Find:
                Find();
                state = FindingState.Finding;
                break;
            case FindingState.Finding:
                state = FindingState.Over;
                break;
            case FindingState.Over:
                StartCoroutine(Over());
                break;
        }
    }

    //BFS
    enum Dir
    {
        up,
        down,
        left,
        right
    }


    int[,] searchedMap;
    PriorityQueue<Vector2> queue = null;
    int curStep;
    float priority;

    void Find()
    {
        Vector2 pos = new Vector2(startX, startY);
        if (queue == null)
        {
            queue = new PriorityQueue<Vector2>();
            queue.Enqueue(pos,priority);
            priority++;
        }

        searchedMap[startY, startX] = 0;

        if (curStep == 0)
        {
            curStep = 1;
        }

        while (queue.Count > 0)
        {
            Vector2 cur = Vector2.zero;

            cur = queue.Dequeue();

            //distances.Add()




            if (cur.y > 0)
                if (Step(cur, Dir.up))
                {
                    break;
                }
            if (cur.y < height - 1)
                if (Step(cur, Dir.down))
                {
                    break;
                }

            if (cur.x > 0)
                if (Step(cur, Dir.left))
                {
                    break;
                }

            if (cur.x < width - 1)
                if (Step(cur, Dir.right))
                {
                    break;
                }

            curStep = searchedMap[(int) cur.y, (int) cur.x];

        }
    }

    public int move;

    bool Step(Vector2 cur, Dir dir)
    {

        int ox = 0;
        int oy = 0;
        switch (dir)
        {
            case Dir.up:
                ox = 0;
                oy = -1;
                break;
            case Dir.down:
                ox = 0;
                oy = 1;
                break;
            case Dir.right:
                ox = 1;
                oy = 0;
                break;
            case Dir.left:
                ox = -1;
                oy = 0;
                break;
        }

        if (map[(int) cur.y + oy, (int) cur.x + ox] == end)
        {
            searchedMap[(int)cur.y + oy, (int)cur.x + ox]
                = searchedMap[(int)cur.y, (int)cur.x] + 1;
            curStep = searchedMap[(int)cur.y, (int)cur.x];
            state = FindingState.Over;
            return true;
        }

        if (map[(int) cur.y + oy, (int) cur.x + ox] == walkable)
        {
            if (searchedMap[(int)cur.y + oy, (int)cur.x + ox] > searchedMap[(int)cur.y, (int)cur.x] + 1)
            {
                map[(int)cur.y + oy, (int)cur.x + ox] = find;
                searchedMap[(int)cur.y + oy, (int)cur.x + ox]
                    = searchedMap[(int)cur.y, (int)cur.x] + 1;
                switch (type)
                {
                    case FindingType.BFS:
                        queue.Enqueue(new Vector2((int)cur.x + ox, (int)cur.y + oy), ++priority); break;
                    case FindingType.DFS:
                        queue.Enqueue(new Vector2((int)cur.x + ox, (int)cur.y + oy), --priority); break;
                    case FindingType.DFSI:
                        queue.Enqueue(new Vector2((int)cur.x + ox, (int)cur.y + oy), ((new Vector2(cur.x + ox, cur.y + oy))-(new Vector2(endX,endY))).magnitude);
                        break;
                    case FindingType.DFSII:
                        queue.Enqueue(new Vector2((int)cur.x + ox, (int)cur.y + oy), Mathf.Abs(cur.x + ox - endX) + Mathf.Abs(cur.y + oy - endY));
                        break;
                    case FindingType.AStar:
                        var totalCost = curStep + 1;
                        queue.Enqueue(new Vector2((int)cur.x + ox, (int)cur.y + oy), 
                            (Mathf.Abs(cur.x + ox - endX) + Mathf.Abs(cur.y + oy - endY)) * weight_H
                            + totalCost * weight_G);
                        break;
                }
            }
        }

        return false;
    }



    //todo: Astar
    //void FindAstar(){}


    Vector2 endPath;
    Vector2[] pathes;

    IEnumerator Over()
    {
        if (pathes == null) pathes = new Vector2[curStep];
        if (endPath == Vector2.zero)
            endPath = new Vector2(endX, endY);
        if (curStep > 0)
        {
            if (endPath.y > 0)
                if (searchedMap[(int) endPath.y - 1, (int) endPath.x] == curStep)
                {
                    endPath.y = (int) endPath.y - 1;
                    mapGen.DrawObject(ObjType.FootStep, (int) endPath.x, (int) endPath.y, curStep);
                    curStep--;
                    yield return new WaitForSeconds(0.005f);
                }

            if (endPath.y < height)

                if (searchedMap[(int) endPath.y + 1, (int) endPath.x] == curStep)
                {
                    endPath.y = (int) endPath.y + 1;
                    mapGen.DrawObject(ObjType.FootStep, (int) endPath.x, (int) endPath.y, curStep);
                    curStep--;
                    yield return new WaitForSeconds(0.005f);
                }

            if (endPath.x > 0)
                if (searchedMap[(int) endPath.y, (int) endPath.x - 1] == curStep)
                {
                    endPath.x = (int) endPath.x - 1;
                    mapGen.DrawObject(ObjType.FootStep, (int) endPath.x, (int) endPath.y, curStep);
                    curStep--;
                    yield return new WaitForSeconds(0.005f);
                }

            if (endPath.x < width)

                if (searchedMap[(int) endPath.y, (int) endPath.x + 1] == curStep)
                {
                    endPath.x = (int) endPath.x + 1;
                    mapGen.DrawObject(ObjType.FootStep, (int) endPath.x, (int) endPath.y, curStep);
                    curStep--;
                    yield return new WaitForSeconds(0.001f);
                }
        }
        yield return null;
    }
    void Refresh()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                switch (map[i, j])
                {
                    case walkable:
                        break;
                    case obstacle:
                        mapGen.DrawObject(ObjType.Obstacle, j, i, searchedMap[i, j]);
                        break;
                    case start:
                    case end:
                        mapGen.DrawObject(ObjType.Point, j, i, searchedMap[i, j]);
                        break;
                    case find:
                        mapGen.DrawObject(ObjType.Finder, j, i, searchedMap[i, j]);
                        break;
                    case step:
                        mapGen.DrawObject(ObjType.FootStep, j, i, searchedMap[i, j]);
                        break;
                }
            }
        }
    }

    void ReadMap()
    {

        string path = Application.dataPath + "//" + "map.txt";
        //if (!File.Exists(path))
        //{
        //    return;
        //}
        string[] str = new string[height];
        str = File.ReadAllLines(path);


        for (int i = 0; i < str.Length; i++)
        {
            for (int j = 0; j < str[i].Length; j++)
            {
                switch (str[i][j])
                {
                    case '1':
                        map[i, j] = obstacle;
                        break;
                    default:
                        break;
                }
            }
        }

        state = FindingState.Wait;
    }
}