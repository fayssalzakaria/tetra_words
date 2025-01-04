/** 
 * @file AudioManager.cs
 * fichier contenant la classe AudioManager
 * @author Fayssal
 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/** 
 * @class AudioManaager
 * Gère les sons du jeu (musique et effets sonores)
*/
public class AudioManager : MonoBehaviour
{
    //gerel les music et les "sound effect" separement
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip game;
    public AudioClip WinSound;
    
    public AudioClip startMusic;

    public AudioClip gameOver;

    public AudioClip lost;
    

     private void Awake()
    {
        // Marquer cet objet pour qu'il ne soit pas détruit lors du chargement d'une nouvelle scène
        DontDestroyOnLoad(this.gameObject);
    }
    /** 
    * @brief Joue une musique en boucle
    */
    public void PlayMusicLooped(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();
    }
      /** 
    * @brief Arrete la musique en cours
    */
    public void StopMusic()
    {
        musicSource.Stop();
    }
      /** 
    * @brief Joue un effet sonore une foi
    */
    public void PlaySfx(AudioClip clip){

        SFXSource.PlayOneShot(clip);
    }
    
}
