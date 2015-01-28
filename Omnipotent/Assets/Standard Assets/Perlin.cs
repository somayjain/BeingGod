using System.Collections;
using System;
using UnityEngine;

/* Perlin noise use example:

Perlin perlin = new Perlin();
var value : float = perlin.Noise(2);
var value : float = perlin.Noise(2, 3, );
var value : float = perlin.Noise(2, 3, 4);


SmoothRandom use example:

var p = SmoothRandom.GetVector3(3);

*/

public class SmoothRandom
{
	// Range is between 0 and 1
	public static Vector3 GetVector3 (float speed)
	{
		float time = Time.time * 0.01F * speed;
		Vector3 value;
		value.x = Get().HybridMultifractal(time, 15.73F, 0.58F);
		value.y =  Get().HybridMultifractal(time, 63.94F, 0.58F);
		value.z = Get().HybridMultifractal(time, 0.2F, 0.58F);

		// Approximately in the range  0 - 0.6
		value /= 0.6F;
		value = new Vector3 (Mathf.Clamp01(value.x), Mathf.Clamp01(value.y), Mathf.Clamp01(value.z));
		return value;
	}
	
	public static float Get (float speed)
	{
		float time = Time.time * 0.01F * speed;
		float value =  Get().HybridMultifractal(time, 15.7F, 0.58F);
		value /= 0.6F;
		value = Mathf.Clamp01(value);
		return value;
	}

	private static FractalNoise Get () { 
		if (s_Noise == null)
			s_Noise = new FractalNoise (1.27F, 2.04F, 8.36F);
		return s_Noise;		
	 }

	private static FractalNoise s_Noise;
}

public class Perlin
{
	// Returns noise between 0 - 1
	static public float NoiseNormalized(float x, float y)
	{
		//-0.697 - 0.795 + 0.697
		float value = Noise(x, y);
		value = (value + 0.69F) / (0.793F + 0.69F);
                 return value;
	}

	static public float Noise(float x, float y)
	{
		int X = (int)Mathf.Floor(x) & 255,                  // FIND UNIT CUBE THAT
		Y = (int)Mathf.Floor(y) & 255;                  // CONTAINS POINT.
		x -= Mathf.Floor(x);                                // FIND RELATIVE X,Y,Z
		y -= Mathf.Floor(y);                                // OF POINT IN CUBE.
		float u = fade(x),                                // COMPUTE FADE CURVES
		v = fade(y);                                // FOR EACH OF X,Y,Z.
		int A = p[X  ]+Y, AA = p[A], AB = p[A+1],      // HASH COORDINATES OF
		B = p[X+1]+Y, BA = p[B], BB = p[B+1];      // THE 8 CUBE CORNERS,
		
		float res = lerp(v, lerp(u, grad2(p[AA  ], x  , y   ),  // AND ADD
                                     grad2(p[BA  ], x-1, y )), // BLENDED
                             lerp(u, grad2(p[AB  ], x  , y-1 ),  // RESULTS
                                     grad2(p[BB  ], x-1, y-1 )));// FROM  8
                 return res;
 	}
	
	static public float Noise(float x, float y, float z) {
		int X = (int)Mathf.Floor(x) & 255,                  // FIND UNIT CUBE THAT
		Y = (int)Mathf.Floor(y) & 255,                  // CONTAINS POINT.
		Z = (int)Mathf.Floor(z) & 255;
		x -= Mathf.Floor(x);                                // FIND RELATIVE X,Y,Z
		y -= Mathf.Floor(y);                                // OF POINT IN CUBE.
		z -= Mathf.Floor(z);
		float u = fade(x),                                // COMPUTE FADE CURVES
		v = fade(y),                                // FOR EACH OF X,Y,Z.
		w = fade(z);
		int A = p[X  ]+Y, AA = p[A]+Z, AB = p[A+1]+Z,      // HASH COORDINATES OF
		B = p[X+1]+Y, BA = p[B]+Z, BB = p[B+1]+Z;      // THE 8 CUBE CORNERS,
		
		return lerp(w, lerp(v, lerp(u, grad(p[AA  ], x  , y  , z   ),  // AND ADD
		grad(p[BA  ], x-1, y  , z   )), // BLENDED
		lerp(u, grad(p[AB  ], x  , y-1, z   ),  // RESULTS
		grad(p[BB  ], x-1, y-1, z   ))),// FROM  8
		lerp(v, lerp(u, grad(p[AA+1], x  , y  , z-1 ),  // CORNERS
		grad(p[BA+1], x-1, y  , z-1 )), // OF CUBE
		lerp(u, grad(p[AB+1], x  , y-1, z-1 ),
		grad(p[BB+1], x-1, y-1, z-1 ))));
	}
	
	static float fade(float t) { return t * t * t * (t * (t * 6 - 15) + 10); }
	static float lerp(float t, float a, float b) { return a + t * (b - a); }
	static float grad(int hash, float x, float y, float z) {
	int h = hash & 15;                      // CONVERT LO 4 BITS OF HASH CODE
	float u = h<8 ? x : y,                 // INTO 12 GRADIENT DIRECTIONS.
	v = h<4 ? y : h==12||h==14 ? x : z;
	return ((h&1) == 0 ? u : -u) + ((h&2) == 0 ? v : -v);
	}

	static float grad2(int hash, float x, float y) {
		int h = hash & 15;                      // CONVERT LO 4 BITS OF HASH CODE
		float u = h < 8 ? x : y,                 // INTO 12 GRADIENT DIRECTIONS.
		v = h < 4 ? y : h==12 || h==14 ? x : 0;
		return ((h&1) == 0 ? u : -u) + ((h&2) == 0 ? v : -v);
	}
	
	static int[] p = { 151,160,137,91,90,15,
		131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
		190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
		88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
		77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
		102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
		135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
		5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
		223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
		129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
		251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
		49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
		138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
		151,160,137,91,90,15,
		131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
		190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
		88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
		77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
		102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
		135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
		5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
		223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
		129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
		251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
		49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
		138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
	};
}

/*
public class Perlin
{
	// Original C code derived from 
	// http://astronomy.swin.edu.au/~pbourke/texture/perlin/perlin.c
	// http://astronomy.swin.edu.au/~pbourke/texture/perlin/perlin.h
	const int B = 0x100;
	const int BM = 0xff;
	const int N = 0x1000;

	int[] p = new int[B + B + 2];
	float[,] g3 = new float [B + B + 2 , 3];
	float[,] g2 = new float[B + B + 2,2];
	float[] g1 = new float[B + B + 2];

	float s_curve(float t)
	{
		return t * t * (3.0F - 2.0F * t);
	}
	
	float lerp (float t, float a, float b)
	{ 
		return a + t * (b - a);
	}

	void setup (float value, out int b0, out int b1, out float r0, out float r1)
	{ 
        float t = value + N;
        b0 = ((int)t) & BM;
        b1 = (b0+1) & BM;
        r0 = t - (int)t;
        r1 = r0 - 1.0F;
	}
	
	float at2(float rx, float ry, float x, float y) { return rx * x + ry * y; }
	float at3(float rx, float ry, float rz, float x, float y, float z) { return rx * x + ry * y + rz * z; }

	public float Noise(float arg)
	{
		int bx0, bx1;
		float rx0, rx1, sx, u, v;
		setup(arg, out bx0, out bx1, out rx0, out rx1);
		
		sx = s_curve(rx0);
		u = rx0 * g1[ p[ bx0 ] ];
		v = rx1 * g1[ p[ bx1 ] ];
		
		return(lerp(sx, u, v));
	}

	public float Noise(float x, float y)
	{
		int bx0, bx1, by0, by1, b00, b10, b01, b11;
		float rx0, rx1, ry0, ry1, sx, sy, a, b, u, v;
		int i, j;
		
		setup(x, out bx0, out bx1, out rx0, out rx1);
		setup(y, out by0, out by1, out ry0, out ry1);
		
		i = p[ bx0 ];
		j = p[ bx1 ];
		
		b00 = p[ i + by0 ];
		b10 = p[ j + by0 ];
		b01 = p[ i + by1 ];
		b11 = p[ j + by1 ];
		
		sx = s_curve(rx0);
		sy = s_curve(ry0);
		
		u = at2(rx0,ry0, g2[ b00, 0 ], g2[ b00, 1 ]);
		v = at2(rx1,ry0, g2[ b10, 0 ], g2[ b10, 1 ]);
		a = lerp(sx, u, v);
		
		u = at2(rx0,ry1, g2[ b01, 0 ], g2[ b01, 1 ]);
		v = at2(rx1,ry1, g2[ b11, 0 ], g2[ b11, 1 ]);
		b = lerp(sx, u, v);
		
		return lerp(sy, a, b);
	}
	
	public float Noise(float x, float y, float z)
	{
		int bx0, bx1, by0, by1, bz0, bz1, b00, b10, b01, b11;
		float rx0, rx1, ry0, ry1, rz0, rz1, sy, sz, a, b, c, d, t, u, v;
		int i, j;
		
		setup(x, out bx0, out bx1, out rx0, out rx1);
		setup(y, out by0, out by1, out ry0, out ry1);
		setup(z, out bz0, out bz1, out rz0, out rz1);
		
		i = p[ bx0 ];
		j = p[ bx1 ];
		
		b00 = p[ i + by0 ];
		b10 = p[ j + by0 ];
		b01 = p[ i + by1 ];
		b11 = p[ j + by1 ];
		
		t  = s_curve(rx0);
		sy = s_curve(ry0);
		sz = s_curve(rz0);
		
		u = at3(rx0,ry0,rz0, g3[ b00 + bz0, 0 ], g3[ b00 + bz0, 1 ], g3[ b00 + bz0, 2 ]);
		v = at3(rx1,ry0,rz0, g3[ b10 + bz0, 0 ], g3[ b10 + bz0, 1 ], g3[ b10 + bz0, 2 ]);
		a = lerp(t, u, v);
		
		u = at3(rx0,ry1,rz0, g3[ b01 + bz0, 0 ], g3[ b01 + bz0, 1 ], g3[ b01 + bz0, 2 ]);
		v = at3(rx1,ry1,rz0, g3[ b11 + bz0, 0 ], g3[ b11 + bz0, 1 ], g3[ b11 + bz0, 2 ]);
		b = lerp(t, u, v);
		
		c = lerp(sy, a, b);
		
		u = at3(rx0,ry0,rz1, g3[ b00 + bz1, 0 ], g3[ b00 + bz1, 2 ], g3[ b00 + bz1, 2 ]);
		v = at3(rx1,ry0,rz1, g3[ b10 + bz1, 0 ], g3[ b10 + bz1, 1 ], g3[ b10 + bz1, 2 ]);
		a = lerp(t, u, v);
		
		u = at3(rx0,ry1,rz1, g3[ b01 + bz1, 0 ], g3[ b01 + bz1, 1 ], g3[ b01 + bz1, 2 ]);
		v = at3(rx1,ry1,rz1,g3[ b11 + bz1, 0 ], g3[ b11 + bz1, 1 ], g3[ b11 + bz1, 2 ]);
		b = lerp(t, u, v);
		
		d = lerp(sy, a, b);
		
		return lerp(sz, c, d);
	}
	
	void normalize2(ref float x, ref float y)
	{
	   float s;
	
		s = (float)Math.Sqrt(x * x + y * y);
		x = y / s;
		y = y / s;
	}
	
	void normalize3(ref float x, ref float y, ref float z)
	{
		float s;
		s = (float)Math.Sqrt(x * x + y * y + z * z);
		x = y / s;
		y = y / s;
		z = z / s;
	}
	
	public Perlin()
	{
		int i, j, k;
		System.Random rnd = new System.Random();
	
	   for (i = 0 ; i < B ; i++) {
		  p[i] = i;
		  g1[i] = (float)(rnd.Next(B + B) - B) / B;
	
		  for (j = 0 ; j < 2 ; j++)
			 g2[i,j] = (float)(rnd.Next(B + B) - B) / B;
		  normalize2(ref g2[i, 0], ref g2[i, 1]);
	
		  for (j = 0 ; j < 3 ; j++)
			 g3[i,j] = (float)(rnd.Next(B + B) - B) / B;
			 
	
		  normalize3(ref g3[i, 0], ref g3[i, 1], ref g3[i, 2]);
	   }
	
	   while (--i != 0) {
		  k = p[i];
		  p[i] = p[j = rnd.Next(B)];
		  p[j] = k;
	   }
	
	   for (i = 0 ; i < B + 2 ; i++) {
		  p[B + i] = p[i];
		  g1[B + i] = g1[i];
		  for (j = 0 ; j < 2 ; j++)
			 g2[B + i,j] = g2[i,j];
		  for (j = 0 ; j < 3 ; j++)
			 g3[B + i,j] = g3[i,j];
	   }
	}
}
*/

[System.Serializable]
public class FractalNoiseParams
{
	public float h = 0.69F;
	public float lacunarity = 6.18F;
	public float octaves = 8.379F;
	public float offset = 0.75F;
	public float xScale = 1.0F;
	public float yScale = 1.0F;
	public float valueScale = 1.0F;
}


public class FractalNoise
{
	void PrecalculateNOoise () {}
	
	public FractalNoise (FractalNoiseParams para)
		: this (para.h, para.lacunarity, para.octaves, null)
	{
		m_XScale = para.xScale;
		m_YScale = para.yScale;
		m_ValueScale = para.valueScale;
		
	}

	public FractalNoise (float inH, float inLacunarity, float inOctaves)
		: this (inH, inLacunarity, inOctaves, null)
	{
	}

	public FractalNoise (float inH, float inLacunarity, float inOctaves, Perlin noise)
	{
		m_Lacunarity = inLacunarity;
		m_Octaves = inOctaves;
		m_IntOctaves = (int)inOctaves;
		m_Exponent = new float[m_IntOctaves+1];
		float frequency = 1.0F;
		for (int i = 0; i < m_IntOctaves+1; i++)
		{
			m_Exponent[i] = (float)Math.Pow (m_Lacunarity, -inH);
			frequency *= m_Lacunarity;
		}
	}
	
	
	public float HybridMultifractal(float x, float y, float offset)
	{
		x *= m_XScale;
		y *= m_YScale;
		
		float weight, signal, remainder, result;
		
		result = (Perlin.Noise (x, y)+offset) * m_Exponent[0];
		weight = result;
		x *= m_Lacunarity; 
		y *= m_Lacunarity;
		int i;
		for (i=1;i<m_IntOctaves;i++)
		{
			if (weight > 1.0F) weight = 1.0F;
			signal = (Perlin.Noise (x, y) + offset) * m_Exponent[i];
			result += weight * signal;
			weight *= signal;
			x *= m_Lacunarity; 
			y *= m_Lacunarity;
		}
		remainder = m_Octaves - m_IntOctaves;
		result += remainder * Perlin.Noise (x,y) * m_Exponent[i];
		
		return result * m_ValueScale;
	}
	
	public float RidgedMultifractal (float x, float y, float offset, float gain)
	{
		x *= m_XScale;
		y *= m_YScale;

		float weight, signal, result;
		int i;
		
		signal = Mathf.Abs (Perlin.Noise (x, y));
		signal = offset - signal;
		signal *= signal;
		result = signal;
		weight = 1.0F;
	
		for (i=1;i<m_IntOctaves;i++)
		{
			x *= m_Lacunarity; 
			y *= m_Lacunarity;
			
			weight = signal * gain;
			weight = Mathf.Clamp01 (weight);
			
			signal = Mathf.Abs (Perlin.Noise (x, y));
			signal = offset - signal;
			signal *= signal;
			signal *= weight;
			result += signal * m_Exponent[i];
		}
	
		return result * m_ValueScale;
	}

	public float BrownianMotion (float x, float y)
	{
		x *= m_XScale;
		y *= m_YScale;
		
		float value, remainder;
		long i;
		
		value = 0.0F;
		for (i=0;i<m_IntOctaves;i++)
		{
			value = Perlin.Noise (x,y) * m_Exponent[i];
			x *= m_Lacunarity;
			y *= m_Lacunarity;
		}
		remainder = m_Octaves - m_IntOctaves;
		value += remainder * Perlin.Noise (x,y) * m_Exponent[i];

		return value * m_ValueScale;
	}
	
	private float[] m_Exponent;
	private int     m_IntOctaves;
	private float   m_Octaves;
	private float   m_Lacunarity;
	private float   m_XScale = 1.0F;
	private float   m_YScale = 1.0F;
	private float   m_ValueScale = 1.0F;
}
/*

/// This is an alternative implementation of perlin noise
public class Noise
{
	public float Noise(float x) 
	{
		return Noise(x, 0.5F);
	}

	public float Noise(float x, float y) 
	{
		int Xint = (int)x;
		int Yint = (int)y;
		float Xfrac = x - Xint;
		float Yfrac = y - Yint;

		float x0y0 = Smooth_Noise(Xint, Yint);  //find the noise values of the four corners
		float x1y0 = Smooth_Noise(Xint+1, Yint);
		float x0y1 = Smooth_Noise(Xint, Yint+1);
		float x1y1 = Smooth_Noise(Xint+1, Yint+1);

		//interpolate between those values according to the x and y fractions
		float v1 = Interpolate(x0y0, x1y0, Xfrac); //interpolate in x direction (y)
		float v2 = Interpolate(x0y1, x1y1, Xfrac); //interpolate in x direction (y+1)
		float fin = Interpolate(v1, v2, Yfrac);  //interpolate in y direction

		return fin;
	}

	private float Interpolate(float x, float y, float a) 
	{
		float b = 1-a;
		float fac1 = (float)(3*b*b - 2*b*b*b);
		float fac2 = (float)(3*a*a - 2*a*a*a);

		return x*fac1 + y*fac2; //add the weighted factors
	}

	private float GetRandomValue(int x, int y)
	{
		x = (x+m_nNoiseWidth) % m_nNoiseWidth;
		y = (y+m_nNoiseHeight) % m_nNoiseHeight;
		float fVal = (float)m_aNoise[(int)(m_fScaleX*x), (int)(m_fScaleY*y)];
		return fVal/255*2-1f;
	}

	private float Smooth_Noise(int x, int y) 
	{
		float corners = ( Noise2d(x-1, y-1) + Noise2d(x+1, y-1) + Noise2d(x-1, y+1) + Noise2d(x+1, y+1) ) / 16.0f;
		float sides = ( Noise2d(x-1, y) +Noise2d(x+1, y) + Noise2d(x, y-1) + Noise2d(x, y+1) ) / 8.0f;
		float center = Noise2d(x, y) / 4.0f;
		return corners + sides + center;
	}

	private float Noise2d(int x, int y)
	{
		x = (x+m_nNoiseWidth) % m_nNoiseWidth;
		y = (y+m_nNoiseHeight) % m_nNoiseHeight;
				
		float fVal = (float)m_aNoise[(int)(m_fScaleX*x), (int)(m_fScaleY*y)];
		
		return fVal/255*2-1f;
	}

	public Noise()
	{
		m_nNoiseWidth = 100;
		m_nNoiseHeight = 100;
		m_fScaleX = 1.0F;
		m_fScaleY = 1.0F;
		System.Random rnd = new System.Random();
		m_aNoise = new int[m_nNoiseWidth,m_nNoiseHeight];
		for (int x = 0; x<m_nNoiseWidth; x++)
		{
			for (int y = 0; y<m_nNoiseHeight; y++)
			{
				m_aNoise[x,y] = rnd.Next(255);
			}
		}
	}
	
	private int[,] m_aNoise;
	protected int m_nNoiseWidth, m_nNoiseHeight;
	private float m_fScaleX, m_fScaleY;
}


/*	Yet another perlin noise implementation. This one is not even completely ported to C#

	
	float noise1[];
	float noise2[];
	float noise3[];
	int indices[];

	float PerlinSmoothStep (float t)
	{
		return t * t * (3.0f - 2.0f * t);
	}
	
	float PerlinLerp(float t, float a, float b)
	{
		return a + t * (b - a);
	}
	
	float PerlinRand()
	{
		return Random.rand () / float(RAND_MAX)  * 2.0f - 1.0f;
	}
	
	
	PerlinNoise::PerlinNoise ()
	{
		long i, j, k;
		float x, y, z, denom;
		
		Random rnd = new Random();

				
		noise1 = new float[1 * (PERLIN_B + PERLIN_B + 2)];
		noise2 = new float[2 * (PERLIN_B + PERLIN_B + 2)];
		noise3 = new float[3 * (PERLIN_B + PERLIN_B + 2)];
		indices = new long[PERLIN_B + PERLIN_B + 2];
		
		for (i = 0; i < PERLIN_B; i++)
		{
			indices[i] = i;
	
			x = PerlinRand();
			y = PerlinRand();
			z = PerlinRand();
	
			noise1[i] = x;
	
			denom = sqrt(x * x + y * y);
			if (denom > 0.0001f) denom = 1.0f / denom;
	
			j = i << 1;
			noise2[j + 0] = x * denom;
			noise2[j + 1] = y * denom;
	
			denom = sqrt(x * x + y * y + z * z);
			if (denom > 0.0001f) denom = 1.0f / denom;
	
			j += i;
			noise3[j + 0] = x * denom;
			noise3[j + 1] = y * denom;
			noise3[j + 2] = z * denom;
		}
	
		while (--i != 0)
		{
			j = rand() & PERLIN_BITMASK;
			std::swap (indices[i], indices[j]);
		}
	
		for (i = 0; i < PERLIN_B + 2; i++)
		{
			j = i + PERLIN_B;
	
			indices[j] = indices[i];
	
			noise1[j] = noise1[i];
	
			j = j << 1;
			k = i << 1;
			noise2[j + 0] = noise2[k + 0];
			noise2[j + 1] = noise2[k + 1];
	
			j += i + PERLIN_B;
			k += i + PERLIN_B;
			noise3[j + 0] = noise3[k + 0];
			noise3[j + 1] = noise3[k + 1];
			noise3[j + 2] = noise3[k + 2];
		}
	}
	
	PerlinNoise::~PerlinNoise ()
	{
		delete []noise1;
		delete []noise2;
		delete []noise3;
		delete []indices;
	}
	
	void PerlinSetup (float v, long& b0, long& b1, float& r0, float& r1);
	void PerlinSetup(
		float v,
		long& b0,
		long& b1,
		float& r0,
		float& r1)
	{
		v += PERLIN_N;
	
		long vInt = (long)v;
	
		b0 = vInt & PERLIN_BITMASK;
		b1 = (b0 + 1) & PERLIN_BITMASK;
		r0 = v - (float)vInt;
		r1 = r0 - 1.0f;
	}
	
	
	float PerlinNoise::Noise1 (float x)
	{
		long bx0, bx1;
		float rx0, rx1, sx, u, v;
	
		PerlinSetup(x, bx0, bx1, rx0, rx1);
	
		sx = PerlinSmoothStep(rx0);
	
		u = rx0 * noise1[indices[bx0]];
		v = rx1 * noise1[indices[bx1]];
	
		return PerlinLerp (sx, u, v);
	}
	
	float PerlinNoise::Noise2(float x, float y)
	{
		long bx0, bx1, by0, by1, b00, b01, b10, b11;
		float rx0, rx1, ry0, ry1, sx, sy, u, v, a, b;
	
		PerlinSetup (x, bx0, bx1, rx0, rx1);
		PerlinSetup (y, by0, by1, ry0, ry1);
	
		sx = PerlinSmoothStep (rx0);
		sy = PerlinSmoothStep (ry0);
	
		b00 = indices[indices[bx0] + by0] << 1;
		b10 = indices[indices[bx1] + by0] << 1;
		b01 = indices[indices[bx0] + by1] << 1;
		b11 = indices[indices[bx1] + by1] << 1;
	
		u = rx0 * noise2[b00 + 0] + ry0 * noise2[b00 + 1];
		v = rx1 * noise2[b10 + 0] + ry0 * noise2[b10 + 1];
		a = PerlinLerp (sx, u, v);
	
		u = rx0 * noise2[b01 + 0] + ry1 * noise2[b01 + 1];
		v = rx1 * noise2[b11 + 0] + ry1 * noise2[b11 + 1];
		b = PerlinLerp (sx, u, v);
	
		u = PerlinLerp (sy, a, b);
		
		return u;
	}
	
	float PerlinNoise::Noise3(float x, float y, float z)
	{
		long bx0, bx1, by0, by1, bz0, bz1, b00, b10, b01, b11;
		float rx0, rx1, ry0, ry1, rz0, rz1, *q, sy, sz, a, b, c, d, t, u, v;
	
		PerlinSetup (x, bx0, bx1, rx0, rx1);
		PerlinSetup (y, by0, by1, ry0, ry1);
		PerlinSetup (z, bz0, bz1, rz0, rz1);
	
		b00 = indices[indices[bx0] + by0] << 1;
		b10 = indices[indices[bx1] + by0] << 1;
		b01 = indices[indices[bx0] + by1] << 1;
		b11 = indices[indices[bx1] + by1] << 1;
	  
		t = PerlinSmoothStep (rx0);
		sy = PerlinSmoothStep (ry0);
		sz = PerlinSmoothStep (rz0);
		
		#define at3(rx,ry,rz) ( rx * q[0] + ry * q[1] + rz * q[2] )
	
		q = &noise3[b00 + bz0]; u = at3(rx0,ry0,rz0);
		q = &noise3[b10 + bz0]; v = at3(rx1,ry0,rz0);
		a = PerlinLerp(t, u, v);
	
		q = &noise3[b01 + bz0]; u = at3(rx0,ry1,rz0);
		q = &noise3[b11 + bz0]; v = at3(rx1,ry1,rz0);
		b = PerlinLerp(t, u, v);
	
		c = PerlinLerp(sy, a, b);
	
		q = &noise3[b00 + bz1]; u = at3(rx0,ry0,rz1);
		q = &noise3[b10 + bz1]; v = at3(rx1,ry0,rz1);
		a = PerlinLerp(t, u, v);
	
		q = &noise3[b01 + bz1]; u = at3(rx0,ry1,rz1);
		q = &noise3[b11 + bz1]; v = at3(rx1,ry1,rz1);
		b = PerlinLerp(t, u, v);
	
		d = PerlinLerp(sy, a, b);
	
		return PerlinLerp (sz, c, d);
	}
*/