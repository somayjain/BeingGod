//------------------------------//
//  HexChunk.cs                 //
//  Written by Alucard Jay      //
//  2014/8/29                   //
//------------------------------//


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ RequireComponent( typeof( MeshFilter ) ) ]
[ RequireComponent( typeof( MeshRenderer ) ) ]


public class HexChunk : MonoBehaviour 
{
	public HexWorld hexWorld; // reference to the HexWorld script (for reading terrainData)

	public float tileRadius = 2f; // size of each tile in the grid
	public float hexRadius = 1.95f; // size of hexagon in the grid (smaller to have space between each hexagon)

	public float offsetY = 0.01f; // Y offset for tile, combat z-fighting with terrain (or use overlay shader)

	public int chunkSize = 50;
	
	// mesh specific variables
	private Mesh mesh;

	private List< Vector3 > newVertices = new List< Vector3 >();
	private List< Vector2 > newUVs = new List< Vector2 >();
	private List< int > newTriangles = new List< int >();
	
	private int triIndex = 0;

	// UV values are hard-coded, based on texture used
	private float uvSize = 0.5f;  // 2x2 texture atlas
	private Vector2 uvDefault = new Vector2( 0f, 0f );
	private Vector2 uvHover = new Vector2( 0.5f, 0f );
	private Vector2 uvGreen = new Vector2( 0f, 0.5f );
	private Vector2 uvRed = new Vector2( 0.5f, 0.5f );

	// variables used in calculations
	private float halfTileRadius;
	private float sin30;
	private float cos30;
	private Vector3[] hexVertices;
	private Vector2[] hexUVs;
	
	
	//	-------------------------------------------------------  Persistent Functions
	
	
	void Awake() 
	{
		Initialize();
	}
	
	
	void Start() 
	{
		DoCalculations();

		GenerateMesh();
	}
	
	
	//	-------------------------------------------------------  Other Functions
	
	
	void Initialize() 
	{
		mesh = GetComponent< MeshFilter >().mesh;
	}
	
	
	void DoCalculations() 
	{
		// common variables

		halfTileRadius = tileRadius * 0.5f;
		
		sin30 = Mathf.Sin( 30f * Mathf.Deg2Rad );
		cos30 = Mathf.Cos( 30f * Mathf.Deg2Rad );


		// vertex calculations

		float halfHexRadius = hexRadius * 0.5f;
		
		hexVertices = new Vector3[7];
		
		hexVertices[0] = Vector3.zero;
		hexVertices[1] = new Vector3( halfHexRadius * -cos30, 0, halfHexRadius *  sin30 );
		hexVertices[2] = new Vector3( halfHexRadius *      0, 0, halfHexRadius *     1f );
		hexVertices[3] = new Vector3( halfHexRadius *  cos30, 0, halfHexRadius *  sin30 );
		hexVertices[4] = new Vector3( halfHexRadius *  cos30, 0, halfHexRadius * -sin30 );
		hexVertices[5] = new Vector3( halfHexRadius *      0, 0, halfHexRadius *    -1f );
		hexVertices[6] = new Vector3( halfHexRadius * -cos30, 0, halfHexRadius * -sin30 );


		// uv calculations

		hexUVs = new Vector2[7];
		
		hexUVs[0] = Vector2.one * 0.5f;
		hexUVs[1] = new Vector2( 0.5f + (0.5f * -cos30), 0.5f - (0.5f *  sin30) );
		hexUVs[2] = new Vector2( 0.5f + (0.5f *      0), 0.5f - (0.5f *     1f) );
		hexUVs[3] = new Vector2( 0.5f + (0.5f *  cos30), 0.5f - (0.5f *  sin30) );
		hexUVs[4] = new Vector2( 0.5f + (0.5f *  cos30), 0.5f - (0.5f * -sin30) );
		hexUVs[5] = new Vector2( 0.5f + (0.5f *      0), 0.5f - (0.5f *    -1f) );
		hexUVs[6] = new Vector2( 0.5f + (0.5f * -cos30), 0.5f - (0.5f * -sin30) );
	}

	
	void GenerateMesh() 
	{
		for ( int z = 0; z < chunkSize; z ++ ) 
		{
			for ( int x = 0; x < chunkSize; x ++ ) 
			{
				if(x%2==0 || z%2==0)
				AddHex( x, z );
			}
		}

		UpdateMesh();
	}
	
	
	void AddHex( int x, int z ) 
	{
		// - VERTICES -

		float _x = (float)x * tileRadius * cos30;
		float _z = (float)z * tileRadius * 1.5f * sin30;
		
		if ( z % 2 == 1 )
		{
			_x += ( halfTileRadius * cos30 );
		}

		Vector3 hexPos = new Vector3( _x + halfTileRadius, 0, _z + halfTileRadius );

		newVertices.Add( TerrainPoint( hexVertices[0] + hexPos ) );
		newVertices.Add( TerrainPoint( hexVertices[1] + hexPos ) );
		newVertices.Add( TerrainPoint( hexVertices[2] + hexPos ) );
		newVertices.Add( TerrainPoint( hexVertices[3] + hexPos ) );
		newVertices.Add( TerrainPoint( hexVertices[4] + hexPos ) );
		newVertices.Add( TerrainPoint( hexVertices[5] + hexPos ) );
		newVertices.Add( TerrainPoint( hexVertices[6] + hexPos ) );


		// - TRIANGLES -

		newTriangles.Add( triIndex + 1 );
		newTriangles.Add( triIndex + 2 );
		newTriangles.Add( triIndex + 0 );
		
		newTriangles.Add( triIndex + 2 );
		newTriangles.Add( triIndex + 3 );
		newTriangles.Add( triIndex + 0 );
		
		newTriangles.Add( triIndex + 3 );
		newTriangles.Add( triIndex + 4 );
		newTriangles.Add( triIndex + 0 );
		
		newTriangles.Add( triIndex + 4 );
		newTriangles.Add( triIndex + 5 );
		newTriangles.Add( triIndex + 0 );
		
		newTriangles.Add( triIndex + 5 );
		newTriangles.Add( triIndex + 6 );
		newTriangles.Add( triIndex + 0 );
		
		newTriangles.Add( triIndex + 6 );
		newTriangles.Add( triIndex + 1 );
		newTriangles.Add( triIndex + 0 );

		triIndex += 7;


		// - UVS -

		Vector2 uvOffset = uvDefault;

		newUVs.Add( new Vector2( (hexUVs[0].x * uvSize) + uvOffset.x, 1f - (hexUVs[0].y * uvSize) - uvOffset.y ) );
		newUVs.Add( new Vector2( (hexUVs[1].x * uvSize) + uvOffset.x, 1f - (hexUVs[1].y * uvSize) - uvOffset.y ) );
		newUVs.Add( new Vector2( (hexUVs[2].x * uvSize) + uvOffset.x, 1f - (hexUVs[2].y * uvSize) - uvOffset.y ) );
		newUVs.Add( new Vector2( (hexUVs[3].x * uvSize) + uvOffset.x, 1f - (hexUVs[3].y * uvSize) - uvOffset.y ) );
		newUVs.Add( new Vector2( (hexUVs[4].x * uvSize) + uvOffset.x, 1f - (hexUVs[4].y * uvSize) - uvOffset.y ) );
		newUVs.Add( new Vector2( (hexUVs[5].x * uvSize) + uvOffset.x, 1f - (hexUVs[5].y * uvSize) - uvOffset.y ) );
		newUVs.Add( new Vector2( (hexUVs[6].x * uvSize) + uvOffset.x, 1f - (hexUVs[6].y * uvSize) - uvOffset.y ) );
	}
	
	
	Vector3 TerrainPoint( Vector3 vertPos ) 
	{
		Vector3 worldPos = vertPos + transform.position;
		float height = hexWorld.terrain.SampleHeight( worldPos );
		vertPos.y = height + offsetY;
		return vertPos;
	}
	

	void UpdateMesh() 
	{
		mesh.Clear();
		mesh.vertices = newVertices.ToArray();
		mesh.uv = newUVs.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.Optimize();
		mesh.RecalculateNormals();

		mesh.name = gameObject.name + "_mesh";

		newVertices.Clear();
		newUVs.Clear();
		newTriangles.Clear();
		triIndex = 0;
	}
	
	
	//	-------------------------------------------------------  UV Modifying Functions
	
	
	public void SetHexUVs( int x, int z, int i ) 
	{
		// calculate start UV index
		int uvIndex = ( ( z * chunkSize ) + x ) * 7;

		// get UV offset
		Vector2 uvOffset = GetUVoffset( i );

		// set new uvs
		Vector2[] uvs = mesh.uv;
		
		uvs[ uvIndex + 0 ] = new Vector2( (hexUVs[0].x * uvSize) + uvOffset.x, 1f - (hexUVs[0].y * uvSize) - uvOffset.y );
		uvs[ uvIndex + 1 ] = new Vector2( (hexUVs[1].x * uvSize) + uvOffset.x, 1f - (hexUVs[1].y * uvSize) - uvOffset.y );
		uvs[ uvIndex + 2 ] = new Vector2( (hexUVs[2].x * uvSize) + uvOffset.x, 1f - (hexUVs[2].y * uvSize) - uvOffset.y );
		uvs[ uvIndex + 3 ] = new Vector2( (hexUVs[3].x * uvSize) + uvOffset.x, 1f - (hexUVs[3].y * uvSize) - uvOffset.y );
		uvs[ uvIndex + 4 ] = new Vector2( (hexUVs[4].x * uvSize) + uvOffset.x, 1f - (hexUVs[4].y * uvSize) - uvOffset.y );
		uvs[ uvIndex + 5 ] = new Vector2( (hexUVs[5].x * uvSize) + uvOffset.x, 1f - (hexUVs[5].y * uvSize) - uvOffset.y );
		uvs[ uvIndex + 6 ] = new Vector2( (hexUVs[6].x * uvSize) + uvOffset.x, 1f - (hexUVs[6].y * uvSize) - uvOffset.y );


		// update mesh
		mesh.uv = uvs;
	}

	
	Vector2 GetUVoffset( int i ) 
	{
		Vector2 uvOffset = uvDefault;

		switch ( i )
		{
		case 1 :
			uvOffset = uvHover;
			break;
			
		case 2 :
			uvOffset = uvGreen;
			break;
			
		case 3 :
			uvOffset = uvRed;
			break;

		default :
			uvOffset = uvDefault;
			break;
		}

		return uvOffset;
	}
}

/*

             * 2

     * 1             * 3

             * 0

     * 6             * 4

             * 5

*/
