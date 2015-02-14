#pragma strict
@script ExecuteInEditMode();
enum Eyes_Expressions{Happy=0,Mad=1,Sad=2,Tired=3,Closed=4,Closed_happy=5,Closed_smile=6,Closed_mad=7};
enum Mouth_Expressions{Happy_open=0,Terrified_open=1,Surprised_open=2,Surprised2_open=3,Unconcerned_closed=4,Sad_closed=5,Happy_closed=6,Cute=7};
var Eye:Eyes_Expressions;
var Mouth:Mouth_Expressions;

var faceAnims:FaceAnimations;

function Start () {
	faceAnims=gameObject.GetComponent(FaceAnimations);
}

function Update () {
	if(faceAnims.faceExpressions.Length>0){
		if (Eye==Eyes_Expressions.Happy){
			faceAnims.faceExpressions[0].rightEye.SetActive(true);
			faceAnims.faceExpressions[0].leftEye.SetActive(true);
		}
		else{	
			faceAnims.faceExpressions[0].rightEye.SetActive(false);
			faceAnims.faceExpressions[0].leftEye.SetActive(false);
		}
		if (Eye==Eyes_Expressions.Mad){
			faceAnims.faceExpressions[1].rightEye.SetActive(true);
			faceAnims.faceExpressions[1].leftEye.SetActive(true);
		}
		else{	
			faceAnims.faceExpressions[1].rightEye.SetActive(false);
			faceAnims.faceExpressions[1].leftEye.SetActive(false);
		}
		if (Eye==Eyes_Expressions.Sad){
			faceAnims.faceExpressions[2].rightEye.SetActive(true);
			faceAnims.faceExpressions[2].leftEye.SetActive(true);
		}
		else{	
			faceAnims.faceExpressions[2].rightEye.SetActive(false);
			faceAnims.faceExpressions[2].leftEye.SetActive(false);
		}
		if (Eye==Eyes_Expressions.Tired){
			faceAnims.faceExpressions[3].rightEye.SetActive(true);
			faceAnims.faceExpressions[3].leftEye.SetActive(true);
		}
		else{	
			faceAnims.faceExpressions[3].rightEye.SetActive(false);
			faceAnims.faceExpressions[3].leftEye.SetActive(false);
		}
		if (Eye==Eyes_Expressions.Closed){
			faceAnims.faceExpressions[4].rightEye.SetActive(true);
			faceAnims.faceExpressions[4].leftEye.SetActive(true);
		}
		else{	
			faceAnims.faceExpressions[4].rightEye.SetActive(false);
			faceAnims.faceExpressions[4].leftEye.SetActive(false);
		}
		if (Eye==Eyes_Expressions.Closed_happy){
			faceAnims.faceExpressions[5].rightEye.SetActive(true);
			faceAnims.faceExpressions[5].leftEye.SetActive(true);
		}
		else{	
			faceAnims.faceExpressions[5].rightEye.SetActive(false);
			faceAnims.faceExpressions[5].leftEye.SetActive(false);
		}
		if (Eye==Eyes_Expressions.Closed_smile){
			faceAnims.faceExpressions[6].rightEye.SetActive(true);
			faceAnims.faceExpressions[6].leftEye.SetActive(true);
		}
		else{	
			faceAnims.faceExpressions[6].rightEye.SetActive(false);
			faceAnims.faceExpressions[6].leftEye.SetActive(false);
		}
		if (Eye==Eyes_Expressions.Closed_mad){
			faceAnims.faceExpressions[7].rightEye.SetActive(true);
			faceAnims.faceExpressions[7].leftEye.SetActive(true);
		}
		else{	
			faceAnims.faceExpressions[7].rightEye.SetActive(false);
			faceAnims.faceExpressions[7].leftEye.SetActive(false);
		}	
	}
	if(faceAnims.mouths.Length>0){
		if (Mouth==Mouth_Expressions.Happy_open)faceAnims.mouths[0].SetActive(true);
		else faceAnims.mouths[0].SetActive(false);
		if (Mouth==Mouth_Expressions.Terrified_open)faceAnims.mouths[1].SetActive(true);
		else faceAnims.mouths[1].SetActive(false);
		if (Mouth==Mouth_Expressions.Surprised_open)faceAnims.mouths[2].SetActive(true);
		else faceAnims.mouths[2].SetActive(false);
		if (Mouth==Mouth_Expressions.Surprised2_open)faceAnims.mouths[3].SetActive(true);
		else faceAnims.mouths[3].SetActive(false);
		if (Mouth==Mouth_Expressions.Unconcerned_closed)faceAnims.mouths[4].SetActive(true);
		else faceAnims.mouths[4].SetActive(false);
		if (Mouth==Mouth_Expressions.Sad_closed)faceAnims.mouths[5].SetActive(true);
		else faceAnims.mouths[5].SetActive(false);
		if (Mouth==Mouth_Expressions.Happy_closed)faceAnims.mouths[6].SetActive(true);
		else faceAnims.mouths[6].SetActive(false);
		if (Mouth==Mouth_Expressions.Cute)faceAnims.mouths[7].SetActive(true);
		else faceAnims.mouths[7].SetActive(false);
		
	}
}

