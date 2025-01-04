/** 
 * @file LevelManager.cs
 * fichier contenant la classe LevelManager qui gère les niveau du jeu
 * @author Fayssal
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/** 
 * @class LevelManager
 * Gère les niveau du jeu (facile,moyen et difficile)
*/
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Text chooseLevel;
    private int minWordSize;
    private float fallSpeed;
    private float iceLetterProba;
    private int linesToFill;
    public Sprite frenchIcon;
    public Sprite englishIcon;

    public Image iconImage;

    AudioManager audioManager;
    public Button button_easy;
    public Button button_medium;
    public Button button_hard;
    public Sprite facile;
    public Sprite moyen;
    public Sprite difficile;
    public Sprite easy;
    public Sprite medium;
    public Sprite difficult;

    
    /**
     * @brief charge l'audio
    */
    private void Awake()
    {
        audioManager=GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.StopMusic();
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    /**
     * @brief charge l'icone du dictionnaire, change les icones selon la langue choisis
    */
    void Start(){

        if (Board.language=="words"){
            Image buttonImageEasy = button_easy.GetComponent<Image>();
            Image buttonImageMED = button_medium.GetComponent<Image>();
            Image buttonImageHARD = button_hard.GetComponent<Image>();
            
            iconImage.sprite = frenchIcon;
            chooseLevel.text="CHOISIR UN NIVEAU";
            buttonImageEasy.sprite=facile;
            buttonImageMED.sprite=moyen;
            buttonImageHARD.sprite=difficile;
        }else if(Board.language=="words2"){
            Image buttonImageEasy = button_easy.GetComponent<Image>();
            Image buttonImageMED = button_medium.GetComponent<Image>();
            Image buttonImageHARD = button_hard.GetComponent<Image>();
            iconImage.sprite = englishIcon;
            chooseLevel.text="CHOOSE A LEVEL ";
            buttonImageEasy.sprite=easy;
            buttonImageMED.sprite=medium;
            buttonImageHARD.sprite=difficult;
        }
        else{
            Image buttonImageEasy = button_easy.GetComponent<Image>();
            Image buttonImageMED = button_medium.GetComponent<Image>();
            Image buttonImageHARD = button_hard.GetComponent<Image>();
            Board.language="words2";
            iconImage.sprite = englishIcon;
            chooseLevel.text="CHOOSE A LEVEL ";
            buttonImageEasy.sprite=easy;
            buttonImageMED.sprite=medium;
            buttonImageHARD.sprite=difficult;
        }
    }
    /**
     * @brief charge le niveau 1
    */
    public void LoadLevel1()
    {
        minWordSize = 4;
        fallSpeed = 0.5f;
        linesToFill = 1;
        iceLetterProba= 0.02f;
        StartGame();
    }
    /**
     * @brief charge le niveau 2
    */
    public void LoadLevel2()
    {
        minWordSize = 5;
        fallSpeed = 0.25f;
        linesToFill = 2;
        iceLetterProba= 0.07f;
        StartGame();
    }
    /**
     * @brief charge le niveau 3
    */
    public void LoadLevel3()
    {
        minWordSize = 6;
        fallSpeed = 0.08f;
        linesToFill = 3;
        iceLetterProba= 0.13f;
        StartGame();
    }
    
    /**
     * @brief charger la scène game de manière asynchrone a la scène levelManager 
    */
    private void StartGame()
    {
        // Sauvegarder les paramètres du niveau actuel dans PlayerPrefs
        PlayerPrefs.SetInt("MinWordSize", minWordSize);
        PlayerPrefs.SetFloat("FallSpeed", fallSpeed);
        PlayerPrefs.SetFloat("iceLetterProba", iceLetterProba);
        SceneManager.LoadSceneAsync("game", LoadSceneMode.Single).completed += OnGameSceneLoaded;
    }
    /**
     * @brief initialise les parametre du niveau selectionner avant que la scene game soit charge grace a l'operation asynchrone
    */
    private void OnGameSceneLoaded(AsyncOperation operation)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.SetLevelParameters(minWordSize, fallSpeed,iceLetterProba);
        }
        else
        {
            Debug.LogError("GameManager non trouvé dans la scène du jeu !");
        }
    }
}