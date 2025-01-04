/** 
 * @file LoseManager.cs
 * fichier contenant la classe LoseManager
 * @author Fayssal
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
/** 
 * @class LoseManager
 * Gère les l'interface de fin de jeu
*/
public class LoseManager : MonoBehaviour
{
    [SerializeField] Text finalScore;
    public static int bestScore=0;
    AudioManager audioManager;
    
  /** 
 * @brief Cherche l'audioManager
  
*/
    private void Awake(){
        audioManager=GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
    }
    /** 
 * @brief Fait les intitialisatons necessaire : music, score
  
*/
     void Start()
    {
        audioManager.StopMusic();
        audioManager.PlaySfx(audioManager.gameOver);
        // Charger le score depuis PlayerPrefs
        int savedScore = PlayerPrefs.GetInt("Score", 0);
        if(savedScore>bestScore){
            bestScore=savedScore;
        }
        Debug.Log("Saved Score: " + savedScore);
        // Mettre à jour le texte avec le score chargé
        finalScore.text = "Your Score: " + savedScore.ToString();
    }
      /** 
 * @brief Rejouer un niveau
  
*/
    public void replay(){
      
        SceneManager.LoadScene("Game");

    }
    /** 
    * @brief Aller au menu
  
    */
    public void menu(){
        SceneManager.LoadScene("Home");
    }
 
}
