/** 
 * @file ShareOnSocialMedia.cs
 * fichier contenant la classe ShareOnSocialMedia
 * @author Fayssal
 
*/
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
/** 
 * @class ShareOnSocialMedia
 * Gère le partage de score via un plugin externe
 * @author fayssal
*/
public class ShareOnSocialMedia : MonoBehaviour
{
	[SerializeField] GameObject Panel_share;
	[SerializeField] Text txtPanelScore;
	
	//[SerializeField] Text txtDate;


	//partager le score
	/**
	* @brief partager le score via un "screenshot"
	*/
	public void ShareScore ()
	{
		
		Panel_share.SetActive (true);
		StartCoroutine ("TakeScreenShotAndShare");
	}
	//screen l'ecran a partager
	/**
	* @brief enregistre l'ecran ("screenshot")
	*/
	IEnumerator TakeScreenShotAndShare ()
	{
		yield return new WaitForEndOfFrame ();

		Texture2D tx = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
		tx.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
		tx.Apply ();

		string path = Path.Combine (Application.temporaryCachePath, "sharedImage.png");
		File.WriteAllBytes (path, tx.EncodeToPNG ());

		Destroy (tx); 
		//appele NativeShare(du plugin du meme nom)
		new NativeShare ()
			.AddFile (path)
			.SetSubject ("This is my score XD")
			.SetText ("Do you also want to try to play tetra-mots ? install it now !!")
			.Share ();


		Panel_share.SetActive (false);
	}
}