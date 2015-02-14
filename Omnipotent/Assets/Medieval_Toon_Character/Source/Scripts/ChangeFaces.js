#pragma strict
var faceAnimations:PlayFaceAnimations;
var totalEyeExpressions:int=7;
var totalMouthExpressions:int=7;
function Start () {
	faceAnimations=gameObject.GetComponent(PlayFaceAnimations);
}

function Update () {

}

function OnGUI(){
	if (GUI.Button (Rect (200,65,80,30), "Eyes")){
		if (faceAnimations.Eye < totalEyeExpressions) faceAnimations.Eye+=1;
		else faceAnimations.Eye=0;
	}
	if (GUI.Button (Rect (200,100,80,30), "Mouth")){
		if (faceAnimations.Mouth < totalMouthExpressions) faceAnimations.Mouth+=1;
		else faceAnimations.Mouth=0;
	}
}