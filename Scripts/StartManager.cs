/** 
 * @file StartManager.cs
 * fichier contenant la classe StartManager
 * @author fayssal
 
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
/** 
 * @class StartManager
 * Gère ce qui se passe dans l'interface d'acceuil du jeu
 *@author fayssal
*/
public class StartManager : MonoBehaviour
{
    public Text bestScore;
    AudioManager audioManager;
    public Button button;
    public Sprite jouer;
    public Sprite starT;
    private void Awake(){
        audioManager=GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
     /** 
    * Charger la musique et le meilleur score
    */
    void Start(){
        audioManager.StopMusic();
        audioManager.PlayMusicLooped(audioManager.startMusic);
        bestScore.text=LoseManager.bestScore.ToString();
        if (Board.language == "words")
        {
            button.image.sprite = jouer;
        }
        else if (Board.language == "words2")
        {
            button.image.sprite = starT;
        }
        else
        {
            button.image.sprite = starT; 
        }
       
        
    }
    /** 
    * @brief se rendre à l'interface de niveaux
    */
    public void start(){
        SceneManager.LoadScene("Levels");
    }
    //mettre a jour le bouton jouer(l'image selon la langue)
    public void updateButtonSprite()
    {
        if (Board.language == "words")
        {
            button.image.sprite = jouer;
        }
        else if (Board.language == "words2")
        {
            button.image.sprite = starT;
        }
        else
        {
            button.image.sprite = starT; 
        }
    }
}