using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
	
	Terrain  terrain; 
	TerrainData tData;
	
	int xRes;
	int yRes;
	
	float[,] heights;
	
	void Start () { 
		terrain = transform.GetComponent<Terrain>(); 
		tData = terrain.terrainData;
		
		xRes = tData.heightmapWidth;
		yRes = tData.heightmapHeight;
	}
	
	void OnGUI() {
		if(GUI.Button (new Rect (10, 10, 100, 25), "Wrinkle")) {
			randomizePoints(0.1f);
		}
		
		if(GUI.Button (new Rect (10, 40, 100, 25), "Reset")) {
			resetPoints();
		} 
	}
	
	void randomizePoints(float strength) { 
		heights = tData.GetHeights(0, 0, xRes, yRes);

		for (int y = 0; y < yRes; y++) {
			for (int x = 0; x < xRes; x++) {
				heights[x,y] = Mathf.PerlinNoise((float) x / (float) xRes, (float) y / (float) yRes) * 0.01f;
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
	
	// Update is called once per frame
	void Update () {
		
	}
}