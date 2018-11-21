using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingWithG : MonoBehaviour {


    class Graph<T>
    {
        public Dictionary<T, T[]> edges = null;

        public T[] GetNeighbors(T node)
        {
            return edges[node]; 
        }
    }

    struct Point
    {
        public int x, y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Point other)
        {
            if (other.x == x && other.y == y)
            {
                return true;
            }
            return false;
        }

    }

    Graph<Point> map;

    Generator mapGen;

    int height;
    int width;

    

	void Start () {
        mapGen = FindObjectOfType<Generator>();

        height = mapGen.height;
        width = mapGen.height;

        map = new Graph<Point>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
