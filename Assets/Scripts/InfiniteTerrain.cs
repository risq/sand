using UnityEngine;
using System.Collections;

public class InfiniteTerrain : MonoBehaviour
{
	public GameObject PlayerObject;


	Terrain[,] terrainGrid = new Terrain[3,3];
	Terrain baseTerrain;
	TerrainData tData;

	int xRes;
	int yRes;
	
	float[,] heights;
	
	void Start ()
	{
		baseTerrain = gameObject.GetComponent<Terrain> ();

		tData = baseTerrain.terrainData;
		
		xRes = tData.heightmapWidth;
		yRes = tData.heightmapHeight;

		terrainGrid[0,0] = Terrain.CreateTerrainGameObject(baseTerrain.terrainData).GetComponent<Terrain>();
		terrainGrid[0,1] = Terrain.CreateTerrainGameObject(baseTerrain.terrainData).GetComponent<Terrain>();
		terrainGrid[0,2] = Terrain.CreateTerrainGameObject(baseTerrain.terrainData).GetComponent<Terrain>();
		terrainGrid[1,0] = Terrain.CreateTerrainGameObject(baseTerrain.terrainData).GetComponent<Terrain>();
		terrainGrid[1,1] = baseTerrain;
		terrainGrid[1,2] = Terrain.CreateTerrainGameObject(baseTerrain.terrainData).GetComponent<Terrain>();
		terrainGrid[2,0] = Terrain.CreateTerrainGameObject(baseTerrain.terrainData).GetComponent<Terrain>();
		terrainGrid[2,1] = Terrain.CreateTerrainGameObject(baseTerrain.terrainData).GetComponent<Terrain>();
		terrainGrid[2,2] = Terrain.CreateTerrainGameObject(baseTerrain.terrainData).GetComponent<Terrain>();

		UpdateTerrainPositionsAndNeighbors();
	}

	void OnGUI() {
		//if(GUI.Button (new Rect (10, 10, 100, 25), "Wrinkle")) {
		//	randomizePoints(0.1f);
		//}
		
		//if(GUI.Button (new Rect (10, 40, 100, 25), "Reset")) {
		//	resetPoints();
		//} 
	}

	void randomizePoints(float strength) { 
		heights = tData.GetHeights(0, 0, xRes, yRes);
		
		for (int y = 0; y < yRes; y++) {
			for (int x = 0; x < xRes; x++) {
				heights[x,y] = Mathf.PerlinNoise(x,y);
				Debug.Log (heights[x,y]);
			}
		}
		
		tData.SetHeights(0, 0, heights);
	}
	
	void resetPoints() { var heights = tData.GetHeights(0, 0, xRes, yRes);
		for (int y = 0; y < yRes; y++) {
			for (int x = 0; x < xRes; x++) {
				heights[x,y] = 0;
			}
		}
		
		tData.SetHeights(0, 0, heights);
	} 
	
	private void UpdateTerrainPositionsAndNeighbors()
	{
		terrainGrid[0,0].transform.position = new Vector3(
			terrainGrid[1,1].transform.position.x - terrainGrid[1,1].terrainData.size.x,
			terrainGrid[1,1].transform.position.y,
			terrainGrid[1,1].transform.position.z + terrainGrid[1,1].terrainData.size.z);
		terrainGrid[0,1].transform.position = new Vector3(
			terrainGrid[1,1].transform.position.x - terrainGrid[1,1].terrainData.size.x,
			terrainGrid[1,1].transform.position.y,
			terrainGrid[1,1].transform.position.z);
		terrainGrid[0,2].transform.position = new Vector3(
			terrainGrid[1,1].transform.position.x - terrainGrid[1,1].terrainData.size.x,
			terrainGrid[1,1].transform.position.y,
			terrainGrid[1,1].transform.position.z - terrainGrid[1,1].terrainData.size.z);
		
		terrainGrid[1,0].transform.position = new Vector3(
			terrainGrid[1,1].transform.position.x,
			terrainGrid[1,1].transform.position.y,
			terrainGrid[1,1].transform.position.z + terrainGrid[1,1].terrainData.size.z);
		terrainGrid[1,2].transform.position = new Vector3(
			terrainGrid[1,1].transform.position.x,
			terrainGrid[1,1].transform.position.y,
			terrainGrid[1,1].transform.position.z - terrainGrid[1,1].terrainData.size.z);
		
		terrainGrid[2,0].transform.position = new Vector3(
			terrainGrid[1,1].transform.position.x + terrainGrid[1,1].terrainData.size.x,
			terrainGrid[1,1].transform.position.y,
			terrainGrid[1,1].transform.position.z + terrainGrid[1,1].terrainData.size.z);
		terrainGrid[2,1].transform.position = new Vector3(
			terrainGrid[1,1].transform.position.x + terrainGrid[1,1].terrainData.size.x,
			terrainGrid[1,1].transform.position.y,
			terrainGrid[1,1].transform.position.z);
		terrainGrid[2,2].transform.position = new Vector3(
			terrainGrid[1,1].transform.position.x + terrainGrid[1,1].terrainData.size.x,
			terrainGrid[1,1].transform.position.y,
			terrainGrid[1,1].transform.position.z - terrainGrid[1,1].terrainData.size.z);
		
		terrainGrid[0,0].SetNeighbors(             null,              null, terrainGrid[1,0], terrainGrid[0,1]);
		terrainGrid[0,1].SetNeighbors(             null, terrainGrid[0,0], terrainGrid[1,1], terrainGrid[0,2]);
		terrainGrid[0,2].SetNeighbors(             null, terrainGrid[0,1], terrainGrid[1,2],              null);
		terrainGrid[1,0].SetNeighbors(terrainGrid[0,0],              null, terrainGrid[2,0], terrainGrid[1,1]);
		terrainGrid[1,1].SetNeighbors(terrainGrid[0,1], terrainGrid[1,0], terrainGrid[2,1], terrainGrid[1,2]);
		terrainGrid[1,2].SetNeighbors(terrainGrid[0,2], terrainGrid[1,1], terrainGrid[2,2],              null);
		terrainGrid[2,0].SetNeighbors(terrainGrid[1,0],              null,              null, terrainGrid[2,1]);
		terrainGrid[2,1].SetNeighbors(terrainGrid[1,1], terrainGrid[2,0],              null, terrainGrid[2,2]);
		terrainGrid[2,2].SetNeighbors(terrainGrid[1,2], terrainGrid[2,1],              null,              null);
	}
	
	void Update ()
	{
		Vector3 playerPosition = new Vector3(PlayerObject.transform.position.x, PlayerObject.transform.position.y, PlayerObject.transform.position.z);
		Terrain playerTerrain = null;
		int xOffset = 0;
		int yOffset = 0;
		for (int x = 0; x < 3; x++)
		{
			for (int y = 0; y < 3; y++)
			{
				if ((playerPosition.x >= terrainGrid[x,y].transform.position.x) &&
					(playerPosition.x <= (terrainGrid[x,y].transform.position.x + terrainGrid[x,y].terrainData.size.x)) &&
					(playerPosition.z >= terrainGrid[x,y].transform.position.z) &&
					(playerPosition.z <= (terrainGrid[x,y].transform.position.z + terrainGrid[x,y].terrainData.size.z)))
				{
					playerTerrain = terrainGrid[x,y];
					xOffset = 1 - x;
					yOffset = 1 - y;
					break;
				}
			}
			if (playerTerrain != null)
				break;
		}
		
		if (playerTerrain != terrainGrid[1,1])
		{
			Terrain[,] newTerrainGrid = new Terrain[3,3];
			for (int x = 0; x < 3; x++)
				for (int y = 0; y < 3; y++)
				{
					int newX = x + xOffset;
					if (newX < 0)
						newX = 2;
					else if (newX > 2)
						newX = 0;
					int newY = y + yOffset;
					if (newY < 0)
						newY = 2;
					else if (newY > 2)
						newY = 0;
					newTerrainGrid[newX, newY] = terrainGrid[x,y];
				}
			terrainGrid = newTerrainGrid;
			UpdateTerrainPositionsAndNeighbors();
		}
	}
}
