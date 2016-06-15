using UnityEngine;
// using System.Collections.Generic;
// using System.Collections;
using Facebook.Unity;
// using System;
// using System.Net;
// using System.Xml.Linq;
//  using System.IO;


public class Social : MonoBehaviour {


	// bool uploadImageSuccess = false;
	// string uploadedImgUrl;
	public GameController gameController;

	public void ShareScreenShot () {
		if (!FB.IsInitialized) {
			FB.Init(this.OnInitComplete);	
		}else {
		

			FB.FeedShare(
			link: new System.Uri("http://esquire.ru/mystreet3D"),
			linkName: string.Format("Вы прошли «Мою улицу» всего за " + gameController.timeString),
			linkCaption: "esquire.ru/mystreet3D",
			linkDescription: "Выберитесь из перекопанной Москвы в игре Esquire",
			picture: new System.Uri("http://i.imgur.com/FhPbOm5.png"),
			callback: null
			);
		}
		// else {
		// 	StartCoroutine(AppScreenshotUpload());		
		// }

	}

	private void OnInitComplete()
        {
            // this.Status = "Success - Check log for details";
            // this.LastResponse = "Success Response: OnInitComplete Called\n";
            Debug.Log("FB " + string.Format("IsLoggedIn='{0}' IsInitialized='{1}'", FB.IsLoggedIn, FB.IsInitialized));
			// StartCoroutine(AppScreenshotUpload());

			FB.FeedShare(
			link: new System.Uri("http://esquire.ru/mystreet3D"),
			linkName: string.Format("Вы прошли «Мою улицу» всего за " + gameController.timeString),
			linkCaption: "esquire.ru/mystreet3D",
			linkDescription: "Выберитесь из перекопанной Москвы в игре Esquire",
			picture: new System.Uri("http://i.imgur.com/c1dcAI1.png"),
			callback: null
			);   

			// FB.ShareLink(
			// 	new System.Uri("https://esquire.ru/"),
			// 	"CONTENT TITLE",
			// 	"DESCRIPTION",
			// 	new System.Uri("http://imgur.com/" + "5L8Y9v6")
				
			// );
        }


// IEnumerator AppScreenshotUpload()
//      {
//          yield return new WaitForEndOfFrame();

// 		var width = Screen.width;
// 		var height = Screen.height;
// 		var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
// 		// Read screen contents into the texture
// 		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
// 		tex.Apply();
// 		byte[] rawImage = tex.EncodeToPNG();
 
//          //Before we try uploading it to Imgur we need a Server Certificate Validation Callback
//         //  ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;


// 		var wwwForm = new WWWForm();
// 		wwwForm.AddBinaryData("image", rawImage, "Screenshot.png");
// 		wwwForm.AddField("type", "base64" );
// 		// Dictionary<string,string> headers = wwwForm.headers;
// 		// headers["Authorization"] = "Client-ID " + "5a8a9f1a6e3f7ac";
// 		wwwForm.headers.Add("Authorization", "Client-ID " + "5a8a9f1a6e3f7ac");

// 		WWW www = new WWW("https://api.imgur.com/3/image.xml", wwwForm);
// 		yield return www;

// 		// Debug.Log(www.bytes);
// 		// FB.API("me/photos", HttpMethod.POST, null, wwwForm);


     
//         //  using (var w = new WebClient())
//         //  {
//         //     //  w.Headers.Add("Authorization", "Client-ID " + "5a8a9f1a6e3f7ac");
//         //     //  var values = new System.Collections.Specialized.NameValueCollection
//         //     //  {
//         //     //      { "image", Convert.ToBase64String(rawImage) },
//         //     //      { "type", "base64" },
//         //     //  };
 
//         //     //  byte[] response = w.UploadValues("https://api.imgur.com/3/image.xml", values);
//  			 XDocument xdoc = XDocument.Load(new MemoryStream(www.bytes));
// 			//  foreach (var item in xdoc.Descendants("id")) {
// 			// 	uploadedImgUrl = item.Value; 
// 			// 	uploadImageSuccess = true;
// 			// 	Debug.Log("IMGUR IMG: " + uploadedImgUrl);
// 			//  }

// 			//  if (uploadImageSuccess && FB.IsInitialized) {
// 				// FB.FeedShare(
// 				// link: new System.Uri("https://esquire.ru/"),
// 				// linkName: "НАЗВАНИЕ ССЫЛКИ",
// 				// linkCaption: "ЗАГОЛОВОК ССЫЛКИ",
// 				// linkDescription: "ОПИСАНИЕ",
// 				// picture: new System.Uri("http://imgur.com/" + uploadedImgUrl),
// 				// callback: null
// 				// );}   
//              Debug.Log(xdoc.Descendants("id").ToString());
//         ////  }
//      }

}
