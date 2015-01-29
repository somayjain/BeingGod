using UnityEngine;
using System.Collections;
public class randomscript : MonoBehaviour {
	private GameObject[] Spheres;
	private GameObject[] x;
	private float timer = 5.0f;
	private float ctr = 0.0f;
	public bool move = false;
	public bool movedone = false;
	public AudioClip a;
	public Vector3 velocity = new Vector3(0.0f, 0.2f, 0.0f);
	int i;
	// Use this for initialization
	void Start () {
		Spheres = GameObject.FindGameObjectsWithTag("FireBALL");
		x = new GameObject[Spheres.Length];
		Debug.Log(Spheres.Length.ToString());
		for(i=0;i<Spheres.Length;i++)
		{	x[i] = Instantiate(Resources.Load("Small Explosion")) as GameObject;
			x[i].SetActive(false);
		}
	}

	// Update is called once per frame
	void Update () 
	{	if(Input.GetKeyUp("s"))
			move = true;
		if(move)
		{	if(ctr<=timer)
			{	for(i=0;i<Spheres.Length;i++)
					Spheres[i].transform.position += velocity*Time.deltaTime;
				ctr+=Time.deltaTime;
			}
		}
		if(ctr>=timer)
		{	move = false;
			movedone = true;
		}
		if(movedone)
		{	for(i=0;i<Spheres.Length;i++)
			{	x[i].transform.position = Spheres[i].transform.position;
				x[i].SetActive(true);
			}
			Debug.Log("all flames active\n");
			StartCoroutine(W2SnD());
		}
	}
	IEnumerator W2SnD()
	{	for(i=0;i<Spheres.Length;i++)
			DestroyObject(Spheres[i]);
		yield return new WaitForSeconds(2);
		for(i=0;i<Spheres.Length;i++)
			DestroyObject(x[i]);
		movedone = false;
	}
}