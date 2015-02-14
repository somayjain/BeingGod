using UnityEngine;
using System.Collections;

public class TerraTextureSetting : MonoBehaviour {

	public Vector3 TS; // terrain grid-space size
	public Vector2 AS; // control texture size

    TerrainData TD;

	public Vector3 ls;

	void Start() {
		TS = Terrain.activeTerrain.terrainData.size;
		AS.x = Terrain.activeTerrain.terrainData.alphamapWidth;
		AS.y = Terrain.activeTerrain.terrainData.alphamapHeight;
		TD = Terrain.activeTerrain.terrainData;
	}
	
	public void SetTexture(Vector3 newLoc){
		int AX = (int)((newLoc.x/TS.x)*AS.x+0.5f);
		int AY = (int)((newLoc.z/TS.z)*AS.y+0.5f);
		float[,,] alpMap=TD.GetAlphamaps (AX,AY,10,10);


		for(int i=0;i<4;i++)
		alpMap[0, 0, 0] = 0.0f;

		alpMap[0, 0, 2] = 1.0f;



		TD.SetAlphamaps(0, 0, alpMap);
		}

	void Update(){
		if (Input.GetKeyUp ("m")) {
						SetTexture (ls);
			Debug.Log("setting up terrain");		
		}
	}
}
