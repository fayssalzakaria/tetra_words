/** 
 * @file UiManager.cs
 * fichier contenant la classe UiManager
 * @author Fayssal
 
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
/** 
 * @class UiManager
 * Gère les interaction générales avec l'utilsateur
*/
public class UiManager : MonoBehaviour
{
    public Text displayText;
    public Image backgroundImage;
    public GameManager gameManager;
    public DictionnaryManager dicoManager;
    public Coroutine flashCoroutine; 
    public Text ScoreText;
    public Board board;
    public StartManager start;
    public int score = 0;
    public bool isFlashing = false; // Variable pour vérifier si la coroutine de clignotement est en cours d'exécution
    private float flashCooldown = 1f;
    //rendre la variable visible
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject languageMenu;
    
   
    //initaliser l'audio manager
    AudioManager audioManager;
    //enregistrer les score par default
    public Button offMusicButton;
    public Button onMusicButton;
    private  static  Color iceColor = new Color(0.5f, 0.8f, 1f, 1f);
    /**
    * @brief cherche l'audioManager, initalise le score
    */
    public void Awake(){
        audioManager=GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        ScoreText.text=score.ToString();
        
        ScoreText.text="";
        ScoreText.text+=score.ToString();
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }
    /**
    * @brief initalise le bouton musique
    */
    void Start(){
        audioManager.PlayMusicLooped(audioManager.game);
        onMusicButton.gameObject.SetActive(false);
        offMusicButton.gameObject.SetActive(true);
    }
    
    
    /**
    * @brief Coroutine pour le clignotement de l'image en rouge
    */
   public IEnumerator FlashBackgroundImage()
    {
        isFlashing = true; 
        Color originalColor = backgroundImage.color;
        Color redColor = Color.red;
        float flashDuration = 1f; 
        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            backgroundImage.color = redColor;
            yield return new WaitForSeconds(0.1f);
            backgroundImage.color = originalColor;
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.2f;
        }
        backgroundImage.color = originalColor;
        isFlashing = false; 
    }
    
    /**
    * @brief  Coroutine pour gérer le temps de récupération avant qu'une nouvelle coroutine puisse être lancée
    */
    public IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(flashCooldown);
        flashCoroutine = null; 
    }
    /**
    * @brief  Efface la dernière lettre sélectionnée.
    */
    public void erase()
    {
        if (gameManager.selectedLetterColliders.Count > 0)

        {
            
            int lastIndex = gameManager.selectedLetterColliders.Count - 1;
            Collider2D colliderToRemove = gameManager.selectedLetterColliders[lastIndex];
            //si la lettre est de neige, ne pas changer la couleur
            GameObject letterObject = colliderToRemove.gameObject;
            if (board.iceLetters.ContainsKey(letterObject)){
                gameManager.ChangeLetterColor(letterObject, iceColor);
            }
            else{
                 // Réinitialiser la couleur de la dernière lettre sélectionnée
                gameManager.ChangeLetterColor(colliderToRemove.gameObject, Color.white);
            }
            gameManager.selectedLetterColliders.RemoveAt(lastIndex);
            
          
            displayText.text = displayText.text.Substring(0, displayText.text.Length - 1);

        }
       
            else
        {
            Debug.Log("longueur négatif");
            if (!isFlashing) // Vérifier si aucune coroutine n'est en cours d'exécution

            {
                flashCoroutine = StartCoroutine(FlashBackgroundImage());

                StartCoroutine(CooldownCoroutine());
            }
            audioManager.PlaySfx(audioManager.lost);
        }
            
        

        
    }

    
     /**
    * @brief  Vérifier qu'un mot est valide et si c'est le cas le supprimer(bouton valid).
    */
    public void valid()
    {
        //enlever les espaces des mots de la liste issue du dictionnaire
        for (int i = 0; i < dicoManager.getWords().Count; i++)
        {
            dicoManager.getWords()[i] = dicoManager.getWords()[i].Replace(" ", "").Trim();
        }

        string lowercaseWord = (displayText.text).ToLower().Replace(" ", "").Trim();

        if (dicoManager.getWords().Contains(lowercaseWord)&&(lowercaseWord.Length>=gameManager.minWordSize))
        {
            audioManager.PlaySfx(audioManager.WinSound);
            
            gameManager.RemoveSelectedLetters();
            
            displayText.text = "";

            Debug.Log("Mot trouvé !");

            score+=lowercaseWord.Length*10;

            Update_score();
            // Réinitialiser les couleurs des lettres après validation
            gameManager.ResetLetterColors();
            

        }

        else
        {
            Debug.Log("MOT NON TROUVÉ");

            gameManager.ResetLetterColors();
            gameManager.selectedLetters.Clear();
            gameManager.selectedLetterColliders.Clear();

            displayText.text = "";
             if (!isFlashing) 
            {
                flashCoroutine = StartCoroutine(FlashBackgroundImage());

                StartCoroutine(CooldownCoroutine());
            }
            audioManager.PlaySfx(audioManager.lost);
        }
    }
     /**
    * @brief  mettre à jour le score
    */
    public void Update_score(){
        ScoreText.text="";
        ScoreText.text+=score.ToString();
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }
     /**
    * @brief  mettre le jeu en pause
    */
    public void pause(){
        GameManager.isPaused=true;
        pauseMenu.SetActive(true);
        Time.timeScale=0f;
      
        
    }
      /**
    * @brief  reprendre le jeu
    */
    public void resume(){
        GameManager.isPaused=false;
        pauseMenu.SetActive(false);
        Time.timeScale=1f;
      
    }
    /**
    * @brief  se rendre au menu
    */
    public void home(){
        pauseMenu.SetActive(false);
        Time.timeScale=1f;
        SceneManager.LoadScene("Home");
    }
     /**
    * @brief  Lancer le menu de langue
    */
    public void language(){
        languageMenu.SetActive(true);
        Time.timeScale=0f;
    }
      /**
    * @brief Charger le dictionnaire français
    */
     public void french(){
        Board.language="words";
        start.updateButtonSprite();
        languageMenu.SetActive(false);
        Time.timeScale=1f;
    
      /**
    * @brief Charger le dictionnaire anglais
    */
    }public void english(){
        Board.language="words2";
        start.updateButtonSprite();
        languageMenu.SetActive(false);
        Time.timeScale=1f;
        
    }
      /**
    * @brief Quitter le menu de langue
    */
    public void retour(){
       
        languageMenu.SetActive(false);
        Time.timeScale=1f;
        
    }
     /**
    * @brief Arrêter la musique
    */
    public void MusicOff(){
        audioManager.StopMusic();
        offMusicButton.gameObject.SetActive(false);
        onMusicButton.gameObject.SetActive(true);
    }
     /**
    * @brief Reprendre la musique
    */
    public void MusicOn(){
        audioManager.PlayMusicLooped(audioManager.game);
        onMusicButton.gameObject.SetActive(false);
        offMusicButton.gameObject.SetActive(true);
    }
   
    
    
}