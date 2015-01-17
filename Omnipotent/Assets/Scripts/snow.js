#pragma strict
var status = true;
function Start () {

}

function Update () {
	if(Input.GetKeyUp("s"))
		status = !status;
	renderer.enabled = status;
}