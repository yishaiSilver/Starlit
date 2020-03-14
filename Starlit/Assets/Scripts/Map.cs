using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	private MapNode[] allNodes;
	[SerializeField] private Color defaultColor;
	[SerializeField] private Color activeColor;
	public GameObject mapLine;

	// Use this for initialization
	void Start()
	{
		GameObject[] starObjects = GameObject.FindGameObjectsWithTag("MapNode");
		allNodes = new MapNode[starObjects.Length];
		for (int i = 0; i < starObjects.Length; i++)
		{
			allNodes[i] = starObjects[i].GetComponent<MapNode>();
			allNodes[i].setNodeIndex(i);
			Debug.Log(i + ": " + allNodes[i]);
		}

		drawAllLinks();
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

		int[] previousSystem = new int[allNodes.Length];
		bool[] haveVisited = new bool[allNodes.Length];

		haveVisited[from.getStarSystemIndex()] = true;
		previousSystem[from.getStarSystemIndex()] = from.getStarSystemIndex();

		while (q.Count != 0)
		{
			StarSystem currentNode = q.Dequeue();
			StarSystem[] neighbors = currentNode.getNeighboringStars();

			for (int i = 0; i < neighbors.Length; i++)
			{
				if (!haveVisited[neighbors[i].getStarSystemIndex()]) // issue
				{
					previousSystem[neighbors[i].getStarSystemIndex()] = currentNode.getStarSystemIndex();
					haveVisited[neighbors[i].getStarSystemIndex()] = true;
					q.Enqueue(neighbors[i]);
				}
			}
		}

		return previousSystem;
	}

	public Stack<StarSystem> retraceBFSPath(StarSystem from, StarSystem to, int[] BFSOutput)
	{
		Stack<StarSystem> directions = new Stack<StarSystem>();


		for (int at = to.getStarSystemIndex(); at != BFSOutput[at]; at = BFSOutput[at])
		{
			Debug.Log("Pushing: " + allNodes[at]);
			directions.Push(allNodes[at].getStar());
		}

		if (directions.Peek() == from)
		{
			return null;
		}

		return directions;
	}

	public void drawAllLinks()
	{
		Queue<MapNode> q = new Queue<MapNode>();
		q.Enqueue(allNodes[0]);

		bool[] haveVisited = new bool[allNodes.Length];

		haveVisited[allNodes[0].getNodeIndex()] = true;

		while (q.Count != 0)
		{
			MapNode currentNode = q.Dequeue();
			MapNode[] neighbors = currentNode.getNeighbors();
			MapLink[] links = currentNode.getLinks();

			for(int i = 0; i < neighbors.Length; i ++)
			{
				if (links[i] == null)
				{
					MapLink link = currentNode.makeNewMapLink(mapLine, i, defaultColor);
					neighbors[i].setNewMapLink(currentNode, link);
					if (!haveVisited[neighbors[i].getNodeIndex()])
					{
						q.Enqueue(neighbors[i]);
						haveVisited[neighbors[i].getNodeIndex()] = true;
					}
				}
			}
		}
	}

	public Stack<MapNode> getStackTo(MapNode from, MapNode to)
    {
		int[] bfsReturn = BFSShortestReach(from);
		return retraceBFSPath(from, to, bfsReturn);
    }

	public int[] BFSShortestReach(MapNode from)
	{
		Queue<MapNode> q = new Queue<MapNode>();
		q.Enqueue(from);

		int[] previousSystem = new int[allNodes.Length];
		bool[] haveVisited = new bool[allNodes.Length];

		haveVisited[from.getNodeIndex()] = true;
		previousSystem[from.getNodeIndex()] = from.getNodeIndex();

		while (q.Count != 0)
		{
			MapNode currentNode = q.Dequeue();
			MapNode[] neighbors = currentNode.getNeighbors();

			for(int i = 0; i < neighbors.Length; i ++)
			{
				if(!haveVisited[neighbors[i].getNodeIndex()]) // issue
				{
					previousSystem[neighbors[i].getNodeIndex()] = currentNode.getNodeIndex();
					haveVisited[neighbors[i].getNodeIndex()] = true;
					q.Enqueue(neighbors[i]);
				}
			}
		}

		Debug.Log(previousSystem[0] + ", " + previousSystem[1] + ", " + previousSystem[2]);
		

		return previousSystem;
	}

	public Stack<MapNode> retraceBFSPath(MapNode from, MapNode to, int[] BFSOutput)
	{
		Stack<MapNode> directions = new Stack<MapNode>();

		
		for (int at = to.getNodeIndex(); at != BFSOutput[at]; at = BFSOutput[at])
		{
			Debug.Log("Pushing: " + allNodes[at]);
			directions.Push(allNodes[at]);
		}

		if(directions.Peek() == from)
		{
			return null;
		}

		return directions;
	}
}
