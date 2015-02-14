//------------------------------//
//  HexWorld.cs                 //
//  Written by Alucard Jay      //
//  2014/9/5                    //
//------------------------------//


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HexWorld : MonoBehaviour 
{
	public HexChunk hexChunkPrefab;
	public Terrain terrain;

	public Vector2 worldSize = new Vector2( 1000, 1000 );
	public int chunkSize = 50;
	
	public float tileRadius = 2f; // size of each tile in the grid
	public float hexRadius = 1.95f; // size of hexagon in the grid (smaller to have space between each hexagon)

	public float offsetY = 0.01f; // Y offset for tile, combat z-fighting with terrain (or use overlay shader)
	
	
	private int[,] hexWorldData;

	private HexChunk[,] hexChunks;

	private bool isInitialized = false;

	public Vector3 test_demo = new Vector3();


	//	-------------------------------------------------------  Persistent Functions
	

	void Awake() 
	{
		//Initialize();
		//Debug.Log ("First");
		InitDamageHex (test_demo);
	}
	
	
	void Start() 
	{
		//Debug.Log ("second");
		StartCoroutine(GenerateHexWorld());
	}


	public void addDamageTexture(){
		GenerateHexWorld ();
	}
	
	//	-------------------------------------------------------  Other Functions

	public void InitDamageHex(Vector3 damageLoc){
		damageLoc.x -= 2.0f;
		damageLoc.z -= 2.0f;
		Vector3 startLoc = new Vector3 (damageLoc.x-2.0f,damageLoc.y,damageLoc.z-2.0f);
		Vector3 endLoc = new Vector3 (damageLoc.x+2.0f,damageLoc.y,damageLoc.z+2.0f);

		if ( !hexChunkPrefab )
			Debug.LogError( gameObject.name + " : NO hexChunkPrefab ASSIGNED IN THE INSPECTOR" );
		
		if ( !terrain )
			Debug.LogError( gameObject.name + " : NO terrain ASSIGNED IN THE INSPECTOR" );
		
		// check hexRadius is not greater than tileRadius (stops overlapping hexagons)
		if ( hexRadius > tileRadius )
			hexRadius = tileRadius;
		
		// check chunk size doesn't exceed max allowed vertices
		if ( chunkSize > 50 )
			chunkSize = 50;
		
		// check worldSize values are integers
		//worldSize.x = Mathf.RoundToInt( worldSize.x );
		//worldSize.y = Mathf.RoundToInt( worldSize.y );
		
		// create a data array to store the texture index value of each hexagon

		hexWorldData = new int[4,4];
		
		for ( int x = (int)startLoc.x; x < (int)endLoc.x; x ++ )
		{
			for ( int z = (int)startLoc.z; z < (int)endLoc.z; z ++ )
			{
				hexWorldData[ z+4, x+4 ] = 0; // default value
			}
		}
	}
	
	public void Initialize() 
	{
		if ( !hexChunkPrefab )
			Debug.LogError( gameObject.name + " : NO hexChunkPrefab ASSIGNED IN THE INSPECTOR" );

		if ( !terrain )
			Debug.LogError( gameObject.name + " : NO terrain ASSIGNED IN THE INSPECTOR" );

		// check hexRadius is not greater than tileRadius (stops overlapping hexagons)
		if ( hexRadius > tileRadius )
			hexRadius = tileRadius;
		
		// check chunk size doesn't exceed max allowed vertices
		if ( chunkSize > 50 )
			chunkSize = 50;
		
		// check worldSize values are integers
		worldSize.x = Mathf.RoundToInt( worldSize.x );
		worldSize.y = Mathf.RoundToInt( worldSize.y );

		// create a data array to store the texture index value of each hexagon
		hexWorldData = new int[ (int)worldSize.x, (int)worldSize.y ];

		for ( int y = 0; y < (int)worldSize.y; y ++ )
		{
			for ( int x = 0; x < (int)worldSize.x; x ++ )
			{
				hexWorldData[ x, y ] = 0; // default value
			}
		}
	}
	

	IEnumerator GenerateHexWorld()
	{
		int chunksX = Mathf.FloorToInt( (float)hexWorldData.Length / (float)chunkSize );
		int chunksZ = Mathf.FloorToInt((float)hexWorldData.Length / (float)chunkSize );

		hexChunks = new HexChunk[ chunksX, chunksZ ];

		Vector3 chunkPos;
		HexChunk hexChunk;
		
		for ( int z = 0; z < chunksZ; z ++ ) 
		{
			for ( int x = 0; x < chunksX; x ++ ) 
			{
				chunkPos.x = (float)x * (float)chunkSize * Mathf.Cos( 30f * Mathf.Deg2Rad ) * tileRadius-12.50f;
				chunkPos.y = 0;
				chunkPos.z = (float)z * (float)chunkSize * 1.5f * Mathf.Sin( 30f * Mathf.Deg2Rad ) * tileRadius-12.50f;

				hexChunk = (HexChunk)Instantiate( hexChunkPrefab, chunkPos, Quaternion.identity );
				hexChunk.gameObject.name = hexChunkPrefab.name + "_" + x.ToString() + "_" + z.ToString();
				hexChunk.transform.parent = transform;

				hexChunk.chunkSize = chunkSize;
				hexChunk.tileRadius = tileRadius;
				hexChunk.hexRadius = hexRadius;
				hexChunk.offsetY = offsetY;

				hexChunk.hexWorld = this;

				hexChunks[ x, z ] = hexChunk;

				// stagger to avoid bottleneck
				yield return new WaitForEndOfFrame();
			}
		}

		isInitialized = true;
		
		// debug for testing
		if ( Application.isEditor )
			Debug.Log( "HexWorld Mesh Generation Complete" );
	}
	
	
	//	-------------------------------------------------------  UV Modifying Functions
	
	
	public Vector2 SetHexUVs( Vector3 hitPoint, int i ) 
	{
		// check if initialized
		if ( !isInitialized )
			return -Vector2.one;


		// where is this hexagon?

		float _z = hitPoint.z;
		_z /= Mathf.Sin( 30f * Mathf.Deg2Rad );
		_z /= 1.5f;
		_z /= tileRadius;
		
		int z = Mathf.FloorToInt( _z );

		float _x = hitPoint.x;
		_x /= Mathf.Cos( 30f * Mathf.Deg2Rad );
		_x /= tileRadius;

		if ( z % 2 == 1 )
			_x -= 0.5f;

		int x = Mathf.FloorToInt( _x );


		// what chunk is it in?

		int cX = Mathf.FloorToInt( x / chunkSize );
		int cZ = Mathf.FloorToInt( z / chunkSize );


		// where is it relative to the chunk?

		int rX = x - ( chunkSize * cX );
		int rZ = z - ( chunkSize * cZ );


		// debug for testing
		if ( Application.isEditor )
			Debug.Log( "world pos " + x + " " + z + " : chunk " + cX + " " + cZ + " : chunk pos " + rX + " " + rZ );


		// check if in range 

		if ( x < 0 || x >= worldSize.x || z < 0 || z >= worldSize.y )
			return -Vector2.one;


		// if the value for i is -1, set the hexagon back to the last texture index value assigned
		if ( i == -1 )
		{
			i = hexWorldData[ x, z ];
		}


		// update hexWorldData with new value if not value for highlighted
		if ( i != 1 )
		{
			hexWorldData[ x, z ] = i;
		}


		// tell the chunk to update UVs for selected tile

		HexChunk currChunk = hexChunks[ cX, cZ ];
		currChunk.SetHexUVs( rX, rZ, i );

		// return the data coordinates of this hexagon
		return new Vector2( x, z );
	}
}
