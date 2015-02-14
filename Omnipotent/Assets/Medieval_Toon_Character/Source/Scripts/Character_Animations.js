#pragma strict
internal var animator:Animator;
var v:float;
var h:float;
var run:float;

function Start () {
	animator=GetComponent (Animator);
}

function Update () {
	v=Input.GetAxis("Vertical");
	h=Input.GetAxis("Horizontal");
	if (animator.GetFloat("Run")==0.2){
		if (Input.GetKeyDown("space")){
			animator.SetBool("Jump",true);
		}
	}
	Sprinting();
	
}

function FixedUpdate (){
	animator.SetFloat("Walk",v);
	animator.SetFloat("Run",run);
	animator.SetFloat("Turn",h);
}

function Sprinting(){
	if (Input.GetKey(KeyCode.LeftShift)){
		run=0.2;
	}
	else
	{
		run=0.0;
	}
}