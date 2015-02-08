using UnityEngine;
using System.Collections;

public class houseManager : MonoBehaviour
{

		public int nosHouses;
		private bool f_buildHouse;
		private bool f_deleteHouse;
		public ArrayList houses;
		private Transform Buildings;
	private int sourceId;

		private class houseComparer: IComparer
		{
				int IComparer.Compare (object a, object b)
				{
						GameObject c1 = (GameObject)a;
						GameObject c2 = (GameObject)b;
						return (c1.transform.GetChild (0).name.CompareTo (c2.transform.GetChild (0).name));
						//return String.Compare(c1.transform.GetChild(0).name,c2.transform.GetChild(0).name);
				}
		}

		// Use this for initialization
		void Start ()
		{
				f_buildHouse = false;
				f_deleteHouse = false;
				houses = new ArrayList ();
				Buildings = GameObject.Find ("Buildings").transform;
				sourceId = 0;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (!f_buildHouse) {
						f_buildHouse = Input.GetKeyDown ("b");
				}
				if (!f_deleteHouse) {
						f_deleteHouse = Input.GetKeyDown ("d");
				}
				//Debug.Log (f_buildHouse.ToString () + " " + f_deleteHouse.ToString ());
				if (f_buildHouse && Input.GetMouseButtonUp (0)) {
						buildHouse ();
						
				}
				if (f_deleteHouse && Input.GetMouseButtonUp (0)) {
						deleteHouse ();
						
				}

		}

		void buildHouse ()
		{
				f_buildHouse = false;
				//Vector3 pos = 
				string myHouseType = houseTYPE (Random.Range (0, 4));
				GameObject house = Instantiate (Resources.Load (myHouseType)) as GameObject;
				house.transform.SetParent (Buildings);
				house.transform.GetChild (0).gameObject.name = "source_" + sourceId.ToString ();
		sourceId++;
			
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				Vector3 hitPoint = new Vector3();
				if (Physics.Raycast (ray, out hit)) {
						hitPoint = hit.point;
				}

				house.transform.position = hitPoint;//new Vector3 (-10.0f * nosHouses, 0f, 0f);
				houses.Add (house);
				nosHouses++;
				Debug.Log ("Inside buildHouse=" + nosHouses.ToString () + " source.name=" + house.transform.GetChild (0).gameObject.name);
		}

		void deleteHouse ()
		{		
				Debug.Log ("delete house");
				string name;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
						GameObject foo = hit.transform.gameObject;
						if (foo.CompareTag ("Building")) {
								nosHouses--;
								houseComparer foo_comparer = new houseComparer ();
								int index = houses.BinarySearch (foo, foo_comparer);
								houses.RemoveAt (index);
								//int index = houses.BinarySearch (hit.transform.gameObject);
				Debug.Log (foo.transform.GetChild(0).name+" " +index.ToString () + " " + houses.Count.ToString ());	
								Destroy (hit.transform.gameObject);
								f_deleteHouse = false;
						}
				}

		}
		
		

		// randomly picking the houseType
		public string houseTYPE (float i)
		{
				if (i >= 0 && i < 1) {
						return "house1";
				}
				if (i >= 1 && i < 2) {
						return "house2";
				}
				if (i >= 2 && i < 3) {
						return "house3";
				}
				if (i >= 3) {
						return "house4";
				}
				return "house1";
		}


}
