using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	private StarSystem[] allSystems;

	// Use this for initialization
	void Start () {
		GameObject[] starObjects = GameObject.FindGameObjectsWithTag("Star");
		allSystems = new StarSystem[starObjects.Length];
		for(int i = 0; i < starObjects.Length; i++)
		{
			allSystems[i] = starObjects[i].GetComponent<StarSystem>();
			allSystems[i].setStarSystemIndex(i);
			Debug.Log(i + ": " + allSystems[i]);
		}
	}

	public Stack<StarSystem> getStackTo(StarSystem from, StarSystem to)
    {
		int[] bfsReturn = BFSShortestReach(from);
		return retraceBFSPath(from, to, bfsReturn);
    }

	public int[] BFSShortestReach(StarSystem from)
	{
		Queue<StarSystem> q = new Queue<StarSystem>();
		q.Enqueue(from);
		int count = 1;
		int j = 0;

		int[] previousSystem = new int[allSystems.Length];
		bool[] haveVisited = new bool[allSystems.Length];

		haveVisited[from.getStarSystemIndex()] = true;
		previousSystem[from.getStarSystemIndex()] = from.getStarSystemIndex();

		while (q.Count != 0)
		{
			StarSystem currentNode = q.Dequeue();
			count--;
			StarSystem[] neighbors = currentNode.getNeighboringStars();

			for(int i = 0; i < neighbors.Length; i ++)
			{
				if(!haveVisited[neighbors[i].getStarSystemIndex()]) // issue
				{
					previousSystem[neighbors[i].getStarSystemIndex()] = currentNode.getStarSystemIndex();
					haveVisited[neighbors[i].getStarSystemIndex()] = true;
					q.Enqueue(neighbors[i]);
					count ++;
				}
			}
			j++;
		}

		Debug.Log(previousSystem[0] + ", " + previousSystem[1] + ", " + previousSystem[2]);
		

		return previousSystem;
	}

	public Stack<StarSystem> retraceBFSPath(StarSystem from, StarSystem to, int[] BFSOutput)
	{
		Stack<StarSystem> directions = new Stack<StarSystem>();

		
		for (int at = to.getStarSystemIndex(); at != BFSOutput[at]; at = BFSOutput[at])
		{
			Debug.Log("Pushing: " + allSystems[at]);
			directions.Push(allSystems[at]);
		}

		if(directions.Peek() == from)
		{
			return null;
		}

		return directions;
	}
}
