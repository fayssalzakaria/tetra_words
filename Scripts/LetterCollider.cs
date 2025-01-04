/** 
 * @file LetterCollider.cs
 * fichier contenant la classe LetterCollider
 * @author Fayssal
*/
using UnityEngine;
/**
 * @class LetterCollider
 *Appelle la fonction de selection de lettre quand on appuie sur une lettre
*/
public class LetterCollider : MonoBehaviour
{
    public char character; 
    /**
 * @brief Appelle la fonction de selection de lettre quand on appuie sur une lettre
 
*/
     void OnMouseDown()
    {
        // Lorsqu'un clic est détecté, sélectionnez la lettre
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.SelectLetter(character); // Utilisez le caractère associé à la lettre
        }
    }
}
