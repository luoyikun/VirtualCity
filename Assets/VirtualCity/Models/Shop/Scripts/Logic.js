var skies : Material[];
var currentMaterial : int = 0;
var skyNameGuiText : GUIText;

function Start() {
	currentMaterial = 0;
	RenderSettings.skybox = skies[currentMaterial];
	skyNameGuiText.text = skies[currentMaterial].name;
	
	// Hide + lock the cursor
	Cursor.visible = false;
	Screen.lockCursor = true;
}

function Update () {

	if (Input.GetButtonDown("Fire1")) {
		ToggleMaterial();
	}
	if (Input.GetButtonDown("Jump")) {
		ToggleMaterial();
	}

	if (Input.GetKeyDown ("escape")) {
		Screen.lockCursor = false;
		Cursor.visible = true;
	}	
}

function ToggleMaterial() {
	currentMaterial++;
	if (currentMaterial == skies.length) {
		currentMaterial = 0;
	}
	RenderSettings.skybox = skies[currentMaterial];
	skyNameGuiText.text = skies[currentMaterial].name;
}