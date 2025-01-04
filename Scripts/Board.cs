/** 
 * @file Board.cs
 * fichier contenant la classe Board.cs qui gère ce qui se passe dans la fenêtre de jeu
 * @author Fayssal 

*/
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text;
//using System;
/** 
 * @class Board
 * class gèrant ce qui se passe dans la grille où les lettres chutent
*/
public class Board : MonoBehaviour
{
   
    // Liste des lettres disponibles avec leurs GameObjects associés
    public List<LettersData> lettersDataList = new List<LettersData>();
    private Vector3 lastPlacedPosition; // Dernière position où une lettre a été placée 
    private Vector3 topLetterPosition;
    private Vector3 currentLetterPosition;
    public Letters currentLetter;
    private LettersData data;
    private char currentCharacter;
    private float previousFallTime;
    
    private string words=new string("");
    private List<string> motsList;

    public GameObject currentObject;
    private DictionnaryManager dico;
    public Collider2D currentFallingCollider;
    public GameManager game;

    public LevelManager level;

    //initialiser le dictionnaire la premiere fois
    public int totalLinesToFill;

    public static string language;
   
    public List<Collider2D> allColliders;
    private  static  Color iceColor = new Color(0.5f, 0.8f, 1f, 1f);
    public  Dictionary<GameObject, int> iceLetters = new Dictionary<GameObject, int>();
    
     /**
   
    * @brief charge le type dictionnaire (français ou anglais) et enlève les espace des mots du dictionnaire

    */
    private void Awake()
    {
       
        dico = GetComponent<DictionnaryManager>(); 
        //choisir le dictionnaire selon la langue
        if (Board.language=="words"){
             dico.LoadWords("words");
        }else if(Board.language =="words2"){
            dico.LoadWords("words2");
        }
        //enlever les espace des mots du dictionnaire
        for (int i = 0; i < dico.getWords().Count; i++)
        {
            dico.getWords()[i] = dico.getWords()[i].Replace(" ", "").Trim(); // Supprime les espaces de chaque mot
        }
        recharge();

    }
     /**
    
    * @brief Place une lettre à une certaine position entrée en paramètre

    */
    public void PlaceLetter(Letters letter, Vector3 position)
{
    // Trouver le bon data pour le caractère
    foreach (var lettersData in lettersDataList)
    {
        if (lettersData.letter == letter)
        {
            data=lettersData;
            GameObject newLetter = Instantiate(lettersData.gameObject, position, Quaternion.identity);
            currentObject = newLetter;
            currentFallingCollider = newLetter.AddComponent<BoxCollider2D>();
            allColliders.Add(currentFallingCollider);

            

            break;
        }
    }
}


    /**
    
    * @brief Place des lettres dans les première ligne (1 ligne si le niveau est facile, 2 si moyen et 3 si difficile)

    */

     public void PlaceLetterBeforeStart()
{
    
    List<float> availableColumns = new List<float>();

    
    //ajouter des lignes selon la difficultee choisis
    if(game.minWordSize==4){
        for (float i = -4.5f; i < 5; i++)
    {
        availableColumns.Add(i);
    }
        int lettersPlaced = 0;
      

        
      
        while (lettersPlaced < 10)
        {
            
            int randomIndex = Random.Range(0, words.Length);
            char currentChar = words[randomIndex];

            if (dico.getDictionary().ContainsKey(currentChar))
            {
                Letters letter = dico.getDictionary()[currentChar];

                int randomColumnIndex = Random.Range(0, availableColumns.Count);
                float randomColumn = availableColumns[randomColumnIndex];
                availableColumns.RemoveAt(randomColumnIndex);
              
                Vector3 lastPlacedPosition = new Vector3(randomColumn,-9.5f, 0); // Décalage vertical pour chaque ligne
                bool isIceLetter = Random.Range(0, 100) < (game.iceProba * 100);
                PlaceLetter(letter, lastPlacedPosition);
                if (isIceLetter){
                    SpriteRenderer spriteRenderer = currentObject.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = iceColor;
                        iceLetters[currentObject] = 2;
                    }
                }
                // Ajouter un composant pour détecter les clics ou les toucher sur le collider

                LetterCollider collider = currentObject.AddComponent<LetterCollider>();
                // Assigner le caractère associé à la lettre
                collider.character = data.character;
                // Retire la lettre utilisée de la chaîne
                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();

                lettersPlaced += 1;// Incrémente le compteur de lettres placées
                 if (string.IsNullOrEmpty(words))
                {
                    recharge();
                }
            }
            else
            {
                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();
            }
        }

    }else if(game.minWordSize==5){

        int lettersPlaced = 0;
        for (float i = -4.5f; i < 5; i++)
        {
            availableColumns.Add(i);
        }

        
      
        while (lettersPlaced < 10)
        {
        
            int randomIndex = Random.Range(0, words.Length);
            char currentChar = words[randomIndex];

            if (dico.getDictionary().ContainsKey(currentChar))
            {
                Letters letter = dico.getDictionary()[currentChar];

                int randomColumnIndex = Random.Range(0, availableColumns.Count);
                float randomColumn = availableColumns[randomColumnIndex];
                availableColumns.RemoveAt(randomColumnIndex);
              
                Vector3 lastPlacedPosition = new Vector3(randomColumn,-9.5f, 0); // Décalage vertical pour chaque ligne

                PlaceLetter(letter, lastPlacedPosition);
                
                bool isIceLetter2 = Random.Range(0, 100) < (game.iceProba * 100);
                  if (isIceLetter2){
                    SpriteRenderer spriteRenderer = currentObject.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = iceColor;
                        iceLetters[currentObject] = 2;
                    }
                }
                // Ajouter un composant pour détecter les clics ou les toucher sur le collider

                LetterCollider collider = currentObject.AddComponent<LetterCollider>();
                // Assigner le caractère associé à la lettre
                collider.character = data.character;
                // Retire la lettre utilisée de la chaîne
                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();

                lettersPlaced += 1;// Incrémente le compteur de lettres placées
                 if (string.IsNullOrEmpty(words))
                {
                    recharge();
                }
            }
            else
            {
                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();
            }
        }
        if (availableColumns.Count == 0)
            {
                Debug.LogWarning("Toutes les colonnes ont été remplies.");
                availableColumns.Clear();
                 for (float i = -4.5f; i < 5; i++)
                {   
                    availableColumns.Add(i);
                }
              
               
            }

        int lettersPlaced2 = 0;
      

        
      
        while (lettersPlaced2 < 10)
        {
            

            int randomIndex = Random.Range(0, words.Length);
            char currentChar = words[randomIndex];

            if (dico.getDictionary().ContainsKey(currentChar))
            {
                Letters letter = dico.getDictionary()[currentChar];

                int randomColumnIndex = Random.Range(0, availableColumns.Count);
                float randomColumn = availableColumns[randomColumnIndex];
                availableColumns.RemoveAt(randomColumnIndex);
            
                Vector3 lastPlacedPosition = new Vector3(randomColumn,-8.5f, 0); // Décalage vertical pour chaque ligne

                PlaceLetter(letter, lastPlacedPosition);
                bool isIceLetter3 = Random.Range(0, 100) < (game.iceProba * 100);
                  if (isIceLetter3){
                    SpriteRenderer spriteRenderer = currentObject.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = iceColor;
                        iceLetters[currentObject] = 2;
                    }
                }
                // Ajouter un composant pour détecter les clics ou les toucher sur le collider

                LetterCollider collider = currentObject.AddComponent<LetterCollider>();
                // Assigner le caractère associé à la lettre
                collider.character = data.character;
                // Retire la lettre utilisée de la chaîne
                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();

                lettersPlaced2 += 1;// Incrémente le compteur de lettres placées
                 if (string.IsNullOrEmpty(words))
                {
                    recharge();
                }
            }
            else
            {
                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();
            }
        }

    }else{

         int lettersPlaced = 0;
          for (float i = -4.5f; i < 5; i++)
         {
            availableColumns.Add(i);
        }

        
      
        while (lettersPlaced < 10)
        {
            if (availableColumns.Count == 0)
            {
                Debug.LogWarning("Toutes les colonnes ont été remplies.");
                availableColumns.Clear();
                 for (float i = -4.5f; i < 5; i++)
                {   
                    availableColumns.Add(i);
                }
                lettersPlaced=0;
                break;
            }

            int randomIndex = Random.Range(0, words.Length);
            char currentChar = words[randomIndex];

            if (dico.getDictionary().ContainsKey(currentChar))
            {
                Letters letter = dico.getDictionary()[currentChar];

                int randomColumnIndex = Random.Range(0, availableColumns.Count);
                float randomColumn = availableColumns[randomColumnIndex];
                availableColumns.RemoveAt(randomColumnIndex);
                
                Vector3 lastPlacedPosition = new Vector3(randomColumn,-9.5f, 0); // Décalage vertical pour chaque ligne

                PlaceLetter(letter, lastPlacedPosition);
                bool isIceLetter4 = Random.Range(0, 100) < (game.iceProba * 100);
                  if (isIceLetter4){
                    SpriteRenderer spriteRenderer = currentObject.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = iceColor;
                        iceLetters[currentObject] = 2;
                    }
                }
                // Ajouter un composant pour détecter les clics ou les toucher sur le collider

                LetterCollider collider = currentObject.AddComponent<LetterCollider>();
                // Assigner le caractère associé à la lettre
                collider.character = data.character;
                // Retire la lettre utilisée de la chaîne
                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();

                lettersPlaced += 1;// Incrémente le compteur de lettres placées
                 if (string.IsNullOrEmpty(words))
                {
                    recharge();
                }
            }
            else
            {
                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();
            }
        }
         if (availableColumns.Count == 0)
            {
                Debug.LogWarning("Toutes les colonnes ont été remplies.");
                availableColumns.Clear();
                 for (float i = -4.5f; i < 5; i++)
                {   
                    availableColumns.Add(i);
                }
              
          
            }
        int lettersPlaced2 = 0;
      

        
      
        while (lettersPlaced2 < 10)
        {
           

            int randomIndex = Random.Range(0, words.Length);
            char currentChar = words[randomIndex];

            if (dico.getDictionary().ContainsKey(currentChar))
            {
                Letters letter = dico.getDictionary()[currentChar];

                int randomColumnIndex = Random.Range(0, availableColumns.Count);
                float randomColumn = availableColumns[randomColumnIndex];
                availableColumns.RemoveAt(randomColumnIndex);
               
                Vector3 lastPlacedPosition = new Vector3(randomColumn,-8.5f, 0); // Décalage vertical pour chaque ligne

                PlaceLetter(letter, lastPlacedPosition);
                bool isIceLetter5 = Random.Range(0, 100) < (game.iceProba * 100);
                  if (isIceLetter5){
                    SpriteRenderer spriteRenderer = currentObject.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = iceColor;
                        iceLetters[currentObject] = 2;
                    }
                }
                // Ajouter un composant pour détecter les clics ou les toucher sur le collider

                LetterCollider collider = currentObject.AddComponent<LetterCollider>();
                // Assigner le caractère associé à la lettre
                collider.character = data.character;
                // Retire la lettre utilisée de la chaîne
                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();

                lettersPlaced2 += 1;// Incrémente le compteur de lettres placées
                 if (string.IsNullOrEmpty(words))
                {
                    recharge();
                }
            }
            else
            {
                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();
            }
            
        }
         if (availableColumns.Count == 0)
            {
                Debug.LogWarning("Toutes les colonnes ont été remplies.");
                availableColumns.Clear();
                 for (float i = -4.5f; i < 5; i++)
                {   
                    availableColumns.Add(i);
                }
              
          
            }
        int lettersPlaced3 = 0;
      

        
      
        while (lettersPlaced3 < 10)
        {
            if (availableColumns.Count == 0)
            {
                Debug.LogWarning("Toutes les colonnes ont été remplies.");
                availableColumns.Clear();
                 for (float i = -4.5f; i < 5; i++)
                {   
                    availableColumns.Add(i);
                }
              
          
            }

            int randomIndex = Random.Range(0, words.Length);
            char currentChar = words[randomIndex];

            if (dico.getDictionary().ContainsKey(currentChar))
            {
                Letters letter = dico.getDictionary()[currentChar];

                int randomColumnIndex = Random.Range(0, availableColumns.Count);
                float randomColumn = availableColumns[randomColumnIndex];
                availableColumns.RemoveAt(randomColumnIndex);
              
                Vector3 lastPlacedPosition = new Vector3(randomColumn,-7.5f, 0); // Décalage vertical pour chaque ligne

                PlaceLetter(letter, lastPlacedPosition);
                bool isIceLetter6 = Random.Range(0, 100) < (game.iceProba * 100);
                  if (isIceLetter6){
                    SpriteRenderer spriteRenderer = currentObject.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = iceColor;
                        iceLetters[currentObject] = 2;
                    }
                }
                // Ajouter un composant pour détecter les clics ou les toucher sur le collider

                LetterCollider collider = currentObject.AddComponent<LetterCollider>();
                // Assigner le caractère associé à la lettre
                collider.character = data.character;
                // Retire la lettre utilisée de la chaîne
                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();

                lettersPlaced3 += 1;// Incrémente le compteur de lettres placées
                 if (string.IsNullOrEmpty(words))
                {
                    recharge();
                }
            }
            else
            {
                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();
            }
            
        }
    }
    

    
}
     /**
    
    * @brief Supprime une lettre selon une position

    */
    public void RemoveLetter(Vector3 position)
    {
    
    // Récupère tous les colliders 2D qui se trouvent aux coordonnées spécifiées
    Collider2D[] colliders = Physics2D.OverlapPointAll(new Vector2(position.x, position.y));

    foreach (Collider2D collider in colliders)
        {
            Destroy(collider.gameObject);
        }
    }
    //verifie si la position en dessous de la lettre est occupe en detectant un eventuel collider
    public bool IsPositionOccupied(Vector3 position)
    {
        //vérifier la position en dessous
        Vector3 positionBelow = position + new Vector3(0, -0.1f, 0);

        // Vérifier si la position en dessous est occupée en vérifiant s'il y a des collisions avec les autres lettres
        Collider2D[] colliders = Physics2D.OverlapPointAll(new Vector2(positionBelow.x, positionBelow.y));

        // Si des collisions sont détectées, cela signifie que la position est occupée
        return colliders.Length > 0;
    }
      /**
    
    * @brief Gère la chute des lettres et l'appartion de nouvelle

    */
     public void MoveLetterDown()
    {
        Vector3 newPosition = currentLetterPosition + new Vector3(0, -1, 0); // Déplacement d'une unité vers le bas
        
        // Vérifier si la nouvelle position est valide et si la position en dessous est occupée
        if (!IsPositionOccupied(newPosition))
        {
       
            // Déplacer l'objet sans le détruire et le recréer
        currentObject.transform.position = newPosition;
        currentLetterPosition = newPosition;
     
       
        }
        //lettre ayant atteint le sommet: gameOver
        else
        {   
            if(currentLetterPosition.y ==9.5){
            SceneManager.LoadScene("Lose");
            return;
        }
        
        // Ajouter un composant pour détecter les clics ou les toucher sur le collider
        LetterCollider collider = currentObject.AddComponent<LetterCollider>();
    
        // Assigner le caractère associé à la lettre

        collider.character = data.character;

        // Mettre à jour la position actuelle de la lettre et redémarrer le compteur du temps de chute

        currentLetterPosition = topLetterPosition;

        previousFallTime = Time.time;
        

        SpawnNewLetter();
        
        
        }

    }
     // faire chutter toute les lettre jusqu'a en rencontrer une autre
    public void DropAllLetters(){
         
        foreach(Collider2D cols in allColliders){
            if(cols!=currentFallingCollider){
            Vector3 pos = cols.transform.position;
            Vector3 newPosition = pos + new Vector3(0, -1, 0);
            while(!IsPositionOccupied(newPosition)){
                
           {
       
            
            cols.transform.position = newPosition;
            newPosition += new Vector3(0, -1, 0);
          
           }
            }
            } 
            
        }
    }

    /**
    
    * @brief Fait apparaître une nouvelle lettre dans une colonne aléatoire de la première ligne de la grille du jeu

    */
   
   public void SpawnNewLetter()

    {    
        //liste des cordonne x (colonnes)
        List<float> floatValues = new List<float> { -4.5f, -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f, 4.5f};

        float randomValue = floatValues[Random.Range(0, floatValues.Count)];
        //genere une position tout en haut a une colonne aleatoire
        topLetterPosition = new Vector3(randomValue, 9.5f, 0);
    
        // Tant que le caractère choisi aléatoirement n'appartient pas au dictionnaire, choisir un nouveau
        while (true)
        {
            int randomIndex = Random.Range(0, words.Length);
            char currentChar = words[randomIndex];

            if (dico.getDictionary().ContainsKey(currentChar))
            {
                Letters letter = dico.getDictionary()[currentChar];
                currentLetter = letter;

                currentCharacter = currentChar;
                bool isIceLetter = Random.Range(0, 100) < (game.iceProba * 100);
                if (isIceLetter){
                    // Placer la lettre de glace
                currentLetterPosition = topLetterPosition;
                PlaceLetter(letter, topLetterPosition);

                // Changer la couleur pour indiquer qu'il s'agit d'une lettre de glace
                SpriteRenderer spriteRenderer = currentObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = iceColor;
                }

                // Ajouter la lettre à la liste des lettres de glace avec un compteur de validation
                iceLetters[currentObject] = 2; // 2 validations nécessaires

                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();
                if (string.IsNullOrEmpty(words))
                {
                    recharge();
                }

                break; // Quittez la boucle
                }
                else{
                      currentLetterPosition = topLetterPosition;

                PlaceLetter(letter, topLetterPosition);

                // Retire la lettre utilisée de la chaîne

                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();

                if (string.IsNullOrEmpty(words))
                {
                    recharge();
                }
                Debug.Log(words + "taille" + words.Length);

                break; // Sort de la boucle une fois que la lettre est trouvée dans le dictionnaire
                }
              
            }
            else
            {
                // Retire la lettre utilisée de la chaîne et réessaie
                words = words.Remove(randomIndex, 1).Replace(" ", "").Trim();

                if (string.IsNullOrEmpty(words))

                {
                    recharge();

                }

                Debug.Log(words + "taille" + words.Length);
            }
        }
    }
     /**
    
    * @brief Si la liste de mot est vide, la recharger avec 3 nouveaux mots

    */
   
    public void recharge() {
    // Recharge les mots
        //dico.LoadWords();
        motsList = dico.GetRandomWords(3,game.minWordSize);

    // Réinitialise la chaîne de mots
        words = "";

    // Concatène les mots rechargeés
        foreach (string word in motsList) {
            words += word;
        }


        // Supprime les espaces
        words = words.Replace(" ", "").Trim();


        Debug.Log(words + "  taille" + words.Length);
    }
     /**
    
    * @brief Avoir le temps de chute précédent

    */
    public float getPreviousFallTime(){
        return previousFallTime;
    }
    /**
    * @brief modifier le temps de chute précédent

    */
    public void setPreviousFallTime(float time){
        previousFallTime=time;
    }
    
    
    
}
