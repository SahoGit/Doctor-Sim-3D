using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    [Header("For Sources")]
    public AudioSource effectSource;
    public AudioSource loopSource;

    [Header("For Clips")]
    public AudioClip checkPointSound;
    public AudioClip hitPSSound;
    public AudioClip buttonClickSound;
    public AudioClip popUpSound;

    void Awake() {
        instance = this;
    }

    //for playing the effect
    public void PlayEffect(AudioClip x) {
        effectSource.PlayOneShot(x);
    }

    //for playing the loop sound
    public void PlayLoop(AudioClip x){
        loopSource.clip = x;
        loopSource.Play();
    }
    
    public void StopLoop(){
        loopSource.Stop();
        loopSource.clip = null;
    }
}
