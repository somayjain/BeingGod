#pragma strict
var str = "Hello";
var status = true;
function Start () {

}

function Update () {
	if(Input.GetKeyUp("r"))
	{	str = "Status = " + status;
		status = !status;
	}
	//gameObject.SetActive(status);
	renderer.enabled = status;
	
}