var speed = 1.0;
var moveSize = Vector3.one;
private var basePosition : Vector3;
var status = true;
function Start ()
{
	basePosition = transform.localPosition;	
}

function Update () {
	var offset = SmoothRandom.GetVector3(speed);
	offset -= Vector3 (0.5, 0.5, 0.5);
	offset = Vector3.Scale(moveSize, offset);
	transform.localPosition = offset + basePosition;
	if(Input.GetKeyUp("t"))
		status = !status;
	renderer.enabled = status;
}

@script AddComponentMenu("Noise/Transform Position")