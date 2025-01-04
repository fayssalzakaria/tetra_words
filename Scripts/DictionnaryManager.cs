/** 
 * @file DictionnaryManager.cs
 * fichier contenant la classe DictionnaryManager
 * @author Fayssal
*/
using UnityEngine;
using System.Collections.Generic;
/** 
 * @class DictionnaryManager
 * Gère les dictionnaire (anglais, francais)
*/
public class DictionnaryManager : MonoBehaviour
{
    // Liste des mots chargés depuis la source de données
    private List<string> words = new List<string>();
   
    //dictionnaire pour faire correspondre les charactere de la chainequ'on charge avec DictionnaryManager et les letter.
    private Dictionary<char, Letters> charToLetter = new Dictionary<char, Letters>() {
        {'A', Letters.A},
        {'B', Letters.B},
        {'C', Letters.C},
        {'D', Letters.D},
        {'E', Letters.E},
        {'F', Letters.F},
        {'G', Letters.G},
        {'H', Letters.H},
        {'I', Letters.I},
        {'J', Letters.J},
        {'K', Letters.K},
        {'L', Letters.L},
        {'M', Letters.M},
        {'N', Letters.N},
        {'O', Letters.O},
        {'P', Letters.P},
        {'Q', Letters.Q},
        {'R', Letters.R},
        {'S', Letters.S},
        {'T', Letters.T},
        {'U', Letters.U},
        {'V', Letters.V},
        {'W', Letters.W},
        {'X', Letters.X},
        {'Y', Letters.Y},
        {'Z', Letters.Z},
        {'a', Letters.A},
        {'b', Letters.B},
        {'c', Letters.C},
        {'d', Letters.D},
        {'e', Letters.E},
        {'f', Letters.F},
        {'g', Letters.G},
        {'h', Letters.H},
        {'i', Letters.I},
        {'j', Letters.J},
        {'k', Letters.K},
        {'l', Letters.L},
        {'m', Letters.M},
        {'n', Letters.N},
        {'o', Letters.O},
        {'p', Letters.P},
        {'q', Letters.Q},
        {'r', Letters.R},
        {'s', Letters.S},
        {'t', Letters.T},
        {'u', Letters.U},
        {'v', Letters.V},
        {'w', Letters.W},
        {'x', Letters.X},
        {'y', Letters.Y},
        {'z', Letters.Z}
        
    };
    /**
    * @brief charge les mots depuis une source de données
    */
    public void LoadWords(string file)
    {
             
        TextAsset wordFile = Resources.Load<TextAsset>(file);
        string[] wordList = wordFile.text.Split('\n');
        words.AddRange(wordList);
    }

    /**
    * @brief Méthode pour obtenir un mot aléatoire depuis la liste de mots chargés
    */
    public List<string> GetRandomWords(int count, int minWordSize)
{   
    List<string> randomWords = new List<string>();

    if (count <= 0)
    {
        Debug.LogWarning("Le nombre de mots demandé doit être supérieur à zéro.");
        return randomWords; // Retourne une liste vide si le nombre demandé est inférieur ou égal à zéro
    }

    if (words.Count == 0)
    {
        Debug.LogWarning("Aucun mot n'a été chargé.");
        return randomWords; // Retourne une liste vide si aucun mot n'a été chargé
    }   

    // Filtrez les mots en fonction de la taille minimale spécifiée
    List<string> filteredWords = words.FindAll(word => word.Length >= minWordSize);

    // Vérifiez si le nombre de mots filtrés est suffisant pour répondre à la demande
    if (filteredWords.Count < count)
    {
        Debug.LogWarning("Pas assez de mots disponibles avec la taille minimale spécifiée.");
        return randomWords; // Retourne une liste vide si le nombre de mots filtrés est insuffisant
    }
    filteredWords = filteredWords.ConvertAll(word => word.ToLower());
 
    for (int i = 0; i < count; i++)
    {
        int randomIndex = Random.Range(0, filteredWords.Count);
        randomWords.Add(filteredWords[randomIndex]);
        // Supprimez le mot sélectionné pour éviter les doublons
        filteredWords.RemoveAt(randomIndex);
    }

    return randomWords;
}
  
    /**
    * @brief avoir le dico char/letter
    */
    public Dictionary<char, Letters> getDictionary(){
        return charToLetter;
    }  
     /**
    * @brief avoir la liste de mots
    */ 
    public List<string> getWords(){
        return words;
    }

}
