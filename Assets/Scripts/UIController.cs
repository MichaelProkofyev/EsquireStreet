using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public Text loadingText;
	public Image loadingImage, headImage;
	public Transform walls;
	public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsController;
	float fadeToWhiteSpeed = 6f;

	private bool fadingToClear = false;
	private bool movingWalls = false;
	private bool fadingToWhite = false;
	private bool movingHead = false;
	private bool fadingHead = false;


	public void StartAnimation() {
		loadingText.gameObject.SetActive(false);
		fadingToClear = true;
		// StartFadeToWhite();
		movingHead = true;
		movingWalls = true;
	}

	public void StartFadeToWhite() {
		fadingToWhite = true;
		loadingImage.color = new Color(1f, 1f, 1f, 0f);	
	}

	// Use this for initialization
	void Start () {

	}
	
	void Update () {

		if (movingHead) {
			headImage.color = Color.Lerp(headImage.color, Color.white, Time.deltaTime);
			headImage.rectTransform.localPosition = Vector3.up * Mathf.Lerp(headImage.rectTransform.localPosition.y, 3.5f, Time.deltaTime);
			if (3.4f <= headImage.rectTransform.localPosition.y) {
				movingHead = false;
				fadingHead = true;
				fpsController.enabled = true;
			}
		}else if(fadingHead) {
			headImage.color = Color.Lerp(headImage.color, Color.clear, Time.deltaTime * 5f);
			if (headImage.color.a <= 0.01f) {
				fadingHead = false;
			}
		}

		if (movingWalls) {
			walls.transform.position = Vector3.up * Mathf.Lerp(walls.transform.position.y, 0f, Time.deltaTime*0.7f);
			if (-0.05f <= walls.transform.position.y) {
				walls.transform.position = Vector3.zero;
				movingWalls = false;
			}
		}


		if (fadingToClear) {
			loadingImage.color = Color.Lerp(loadingImage.color, Color.clear, Time.deltaTime);
			if (loadingImage.color.a <= 0.01f) {
				fadingToClear = false;
				loadingImage.color = Color.clear;
			}
		}else if (fadingToWhite) {
			loadingImage.color = Color.Lerp(loadingImage.color, Color.white, Time.deltaTime * fadeToWhiteSpeed);
			if (0.99f <= loadingImage.color.a) {
				fadingToWhite = false;
				loadingImage.color = Color.white;
			}
		} 
	}
}
