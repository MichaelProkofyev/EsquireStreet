using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using System;
using System.Text;


public class Social : MonoBehaviour {

	public void ShareScreenShot () {
		if (!FB.IsInitialized) {
			FB.Init(this.OnInitComplete);
		}
		// else {
		// 	if (!FB.IsLoggedIn) {
		// 		FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends", "publish_actions" }, this.HandleResult);
		// 	}else {
		// 		StartCoroutine(ShareImageShot());
		// 	}
		// }
	}

	private void OnInitComplete()
        {
            // this.Status = "Success - Check log for details";
            // this.LastResponse = "Success Response: OnInitComplete Called\n";
            string logMessage = string.Format(
                "OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
                FB.IsLoggedIn,
                FB.IsInitialized);
            Debug.Log("FB " + logMessage);
			StartCoroutine(ShareImageShot());
        }

	IEnumerator ShareImageShot() {
 
		yield return new WaitForEndOfFrame();

		var width = Screen.width;
		var height = Screen.height;
		var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		// Read screen contents into the texture
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		byte[] screenshot = tex.EncodeToPNG();
		string encodedText = Convert.ToBase64String (screenshot);

		FB.FeedShare(
				link: new System.Uri("https://esquire.ru/"),
				linkName: "The Larch",
				linkCaption: "I thought up a witty tagline about larches",
				linkDescription: "There are a lot of larch trees around here, aren't there?",
				picture: new System.Uri( "data:image/png;base64," + Convert.ToBase64String( screenshot ) ),
				callback: null
		);


		// var wwwForm = new WWWForm();
		// wwwForm.AddBinaryData("image", screenshot, "Screenshot.png");
		// wwwForm.AddField("og:title", "http://www.esquire.ru/");



		// FB.API("me/photos", HttpMethod.POST, null, wwwForm);
 
     }

}
