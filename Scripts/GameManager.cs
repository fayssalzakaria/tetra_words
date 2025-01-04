/** 
 * @file GameManager.cs
 * fichier contenant la classe GameManager qui gere l'etat global du jeu
 * @author Fayssal Zakaria
*/
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public Board board;
    public DictionnaryManager dico;
    public Text displayText;
    public List<GameObject> selectedLetters = new List<GameObject>();
    public List<Collider2D> selectedLetterColliders = new List<Collider2D>();
    public UiManager ui;
    //private float fallTime = 0.1f; //temps de chute
    private Collider2D lastSelectedCollider;
    public int minWordSize;
    public float fallSpeed;
    public Text minWordSizeText;
    AudioManager audioManager;
     public float iceProba;//acceder a la probabilite d'avoir une lettre enneige depuis board
    public static bool isPaused;
    private  static  Color iceColor = new Color(0.5f, 0.8f, 1f, 1f);
    
    /**
    * @
    * @brief cherche l'audioManager qui contient un tag assigne dans l'inspecteur de unity
    */
    private void Awake(){
        audioManager=GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
    }
    /**
    * @brief commence le jeu
    */
    
    void Start()
    {  
        isPaused=false;
        audioManager.PlayMusicLooped(audioManager.game);
        Debug.Log(Board.language);
        // Charger les paramètres du niveau précédent depuis PlayerPrefs
        int savedMinWordSize = PlayerPrefs.GetInt("MinWordSize", 4); // Valeur par défaut : 4
        float savedFallSpeed = PlayerPrefs.GetFloat("FallSpeed", 0.5f); // Valeur par défaut : 0.5f
        float savedIceProba=PlayerPrefs.GetFloat("iceLetterProba", 0.05f);
        // Utiliser ces paramètres pour configurer le niveau
        SetLevelParameters(savedMinWordSize, savedFallSpeed,savedIceProba);
        board.PlaceLetterBeforeStart();
        board.SpawnNewLetter();
        board.setPreviousFallTime(Time.time);
        
    }
    //mettre a jour le temp pour les deplacements
    void Update()
    {
        // Vérifier si le temps écoulé dépasse le temps de chute ou si la lettre est valide
        
        if ((Time.time - (board.getPreviousFallTime()) > fallSpeed))
        {
            // Déplacer la lettre vers le bas
            board.MoveLetterDown();
            board.setPreviousFallTime(Time.time); // Réinitialiser le compteur du temps de chute
            board.DropAllLetters();
        }
    }
    /**
    * @brief quand une lettre est cliqué avec OnMouseDown de la class LetterCollider, cette fonction est appele afin
     d'inscrire le char dans l'interface de selection si celle-ci est validée
    */
 public void SelectLetter(char letter)
    {   
        if(GameManager.isPaused==false){
         // Vérifier si la longueur du texte atteint 10 caractères
        if (displayText.text.Length >= 15)
        {
            if (!ui.isFlashing) 
            {
                ui.flashCoroutine = StartCoroutine(ui.FlashBackgroundImage());
                StartCoroutine(ui.CooldownCoroutine());
            } 
            audioManager.PlaySfx(audioManager.lost);
            return; // Ne pas ajouter de caractère si la limite est atteinte
        }
        // Recherche du collider associé à la lettre sélectionnée
        Collider2D[] colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        foreach (Collider2D collider in colliders)
        {
            // Vérification que le collider correspond à une lettre
            if (collider.GetComponent<LetterCollider>() != null)
            {
                // Vérifie si le collider actuel n'est pas dans les colliders selectionner
                if (!selectedLetterColliders.Contains(collider) )
                {
                    GameObject letterObject = collider.gameObject;
                    
                    // Ajout de la lettre à la liste des lettres sélectionnées
                    selectedLetters.Add(letterObject);
                    // Changer la couleur du SpriteRenderer de la lettre sélectionnée
                     ChangeLetterColor(letterObject, Color.blue);
                    
                    // Ajout du collider de la lettre sélectionnée à la liste
                    selectedLetterColliders.Add(collider);
                    
                    // Affichage de la lettre sélectionnée
                    displayText.text += (letter.ToString()).ToLower();
                    
                    // Met à jour le dernier collider sélectionné
                    lastSelectedCollider = collider;
                    
                    break; // Sortie de la boucle après avoir traité la première lettre trouvée
                }
            else{
                Debug.Log("lettre non valide");
                if (!ui.isFlashing) 
                {
                    ui.flashCoroutine = StartCoroutine(ui.FlashBackgroundImage());
                    StartCoroutine(ui.CooldownCoroutine());
                }  
                audioManager.PlaySfx(audioManager.lost); 
               
                }
            }
        }
        }
        else{
            return;
        }
    }
    /**
    * @brief supprimer les lettre selectionner par l'utilisateur
    */
    public void RemoveSelectedLetters()
    {

        // Creer une copie de la liste des colliders sélectionnes

        List<Collider2D> collidersToRemove = new List<Collider2D>(selectedLetterColliders);
        // Parcourir la liste des colliders à supprimer
      
        foreach (Collider2D collider in collidersToRemove)
        {
            GameObject letterObject = collider.gameObject;
            // Récupérer la position de la lettre à supprimer
           // String lowercaseWord = (displayText.text).ToLower().Replace(" ", "").Trim();
        // Vérifier si c'est une lettre de glace
        if (board.iceLetters.ContainsKey(letterObject))

       
        {
            int validationsLeft = board.iceLetters[letterObject];
            validationsLeft -= 1; // Décrémenter le compteur de validations
            board.iceLetters[letterObject] = validationsLeft;
            if (validationsLeft ==1 )//la lettre enneige a deja ete valider
            {
                //mettre a jour les lettre enneige
                board.iceLetters.Remove(letterObject);
                //remettre le sprite de la lettre comme avant
                SpriteRenderer spriteRenderer = letterObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.white;
                }
               //s'assurer que le collider de la lettre de glace est retire de la liste des colliders
                if (selectedLetterColliders.Contains(collider))
                {
                    selectedLetterColliders.Remove(collider);
                    Debug.Log("Supprimée des colliders sélectionnés : " + letterObject.name);
                }
                continue;
            }
            
        }
            board.allColliders.Remove(collider);
            Vector3 position = collider.transform.position;

            // Supprimer le GameObject associé au collider
            Destroy(collider.gameObject);
            
            
            // Retirer le collider de la liste des colliders sélectionnés

            selectedLetterColliders.Remove(collider);
        
        }
        
        board.DropAllLetters();
        selectedLetters.Clear();

    }
    
    /**
    * @brief Change la couleur de la lettre selctionner par l'utilisateur selon une couleur specifié en paramètre
    */
    public void ChangeLetterColor(GameObject letterObject, Color color)
    {
        SpriteRenderer spriteRenderer = letterObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }
    /**
    * @brief Réinitialiser les couleur de toute les lettre si le mot n'est pas valide
    */
    public void ResetLetterColors()
    {
        foreach (GameObject letterObject in selectedLetters)
        {   //si la lettre est en glace, la remettre en glace, sinon la remettre par default
            if(!board.iceLetters.ContainsKey(letterObject)){
            ChangeLetterColor(letterObject, Color.white);
           
            }
            else{
               ChangeLetterColor(letterObject, iceColor); 
            }
        }

        // Effacer la liste des lettres sélectionnées
        selectedLetters.Clear();
    }   
      /**
    * @brief Initalise les paramètres de taille minimal et de vitesse selon le niveau sélectionné
    */
     public void SetLevelParameters(int minWordSize, float fallSpeed,float iceproba)
    {
        this.minWordSize = minWordSize;
        this.fallSpeed = fallSpeed;
        this.iceProba = iceproba;

        // Vous pouvez ajouter d'autres actions ici en fonction des paramètres du niveau
        Debug.Log("Paramètres du niveau configurés : Taille minimale des mots = " + minWordSize + ", Vitesse de chute = " + fallSpeed);
        if (Board.language=="words")
        {
            minWordSizeText.text = "Taille min mots: " + minWordSize.ToString();
        }else if(Board.language=="words2"){
            minWordSizeText.text = "Min Word Size: " + minWordSize.ToString();
        }
        



    }
}