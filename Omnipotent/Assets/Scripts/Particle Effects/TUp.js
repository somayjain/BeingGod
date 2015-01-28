#pragma strict
var status = true;
function Start () {

}

function Update () {
	if(Input.GetKeyUp("t"))
		status = !status;
	renderer.enabled = status;
}