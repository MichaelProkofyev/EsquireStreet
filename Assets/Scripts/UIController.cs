using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public Text loadingText;
	public Image loadingImage, headImage;
	float fadeToWhiteSpeed = 6f;

	private bool fadingToClear = false;
	private bool fadingToWhite = false;
	private bool movingHead = false;
	private bool fadingHead = false;


	public void FinishedLoading() {
		loadingText.gameObject.SetActive(false);
		fadingToClear = true;
		// StartFadeToWhite();
		movingHead = true;
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
			headImage.color = Color.Lerp(headImage.color, Color.white, Time.deltaTime * 2.5f);
			headImage.rectTransform.localPosition = Vector3.up * Mathf.Lerp(headImage.rectTransform.localPosition.y, 0, Time.deltaTime);
			if (-5 <= headImage.rectTransform.localPosition.y) {
				movingHead = false;
				fadingHead = true;
			}
		}else if(fadingHead) {
			headImage.color = Color.Lerp(headImage.color, Color.clear, Time.deltaTime * 5f);
			if (headImage.color.a <= 0.01f) {
				fadingHead = false;
				Destroy(headImage);
			}
		}


		if (fadingToClear) {
			loadingImage.color = Color.Lerp(loadingImage.color, Color.clear, Time.deltaTime/2.5f);
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
