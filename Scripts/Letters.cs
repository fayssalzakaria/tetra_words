/** 
 * @file Letters.cs
 * fichier contenant la l'enumeration des lettres un structure LettersData
 * @author Fayssal
*/
using UnityEngine;
/**
* @brief Énumération des lettres disponibles
*/
public enum Letters {
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J,
    K,
    L,
    M,
    N,
    O,
    P,
    Q,
    R,
    S,
    T,
    U,
    V,
    W,
    X,
    Y,
    Z
}

[System.Serializable]

 /**
* @brief Structure pour stocker les données associées à chaque lettre dans un game object
*/
public struct LettersData {
    public Letters letter;
    public char character;
    public GameObject gameObject;

}
