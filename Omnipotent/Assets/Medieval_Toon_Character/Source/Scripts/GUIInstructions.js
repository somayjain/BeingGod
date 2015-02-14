#pragma strict
var customSkin : GUISkin;
var characterPrefab:GameObject;
function Start () {

}

function Update () {

}

function OnGUI(){
	GUI.skin = customSkin;	
	GUI.Label(Rect(30,70,150,120),"Instructions: Move around with WASD keys, hold shift to run, and press space while running to jump, click mouse to change facial expression");
	if (GUI.Button(Rect(30,30,60,30),"Reset")){
		characterPrefab.transform.position=Vector3.zero;
	}
}