#pragma strict
class eyeExpressions {
	var rightEye:GameObject;
	var leftEye:GameObject;
}
private var curEye:int;
private var oldEye:int;
private var curMouth:int;
private var oldMouth:int;

var faceExpressions : eyeExpressions[];

var mouths : GameObject[];

function Start () {
	curEye=0;
	oldEye=0;
	curMouth=0;
	oldMouth=0;
	if (faceExpressions.Length>0){
		faceExpressions[0].rightEye.SetActive(true);
		faceExpressions[0].leftEye.SetActive(true);
	}
	if (mouths.Length>0){
		mouths[0].SetActive(true);
	}
}

function Update () {

}
function switchMouthExpressions(prevMouth:int, nextMouth:int){
	mouths[prevMouth].SetActive(false); 
	mouths[nextMouth].SetActive(true);
}
function switchEyeExpressions(prevEye:int, nextEye:int){
	faceExpressions[prevEye].rightEye.SetActive(false);
	faceExpressions[prevEye].leftEye.SetActive(false);
	
	faceExpressions[nextEye].rightEye.SetActive(true);
	faceExpressions[nextEye].leftEye.SetActive(true);
}