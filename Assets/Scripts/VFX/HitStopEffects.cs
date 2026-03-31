using UnityEngine;

public class HitStopEffects : MonoBehaviour
{//start of hitstop effect script
    [SerializeField] private GameObject hitStopLight;//used to store the screen effect
    [SerializeField] private AudioClip hitStopSFXAudio;//used to store the audio clip for the sfx
    //[SerializeField] private AudioSource hitStopSFX;//used to store the audio source for the sfx
    [SerializeField] private float hitStopLength;//used to manually set the length of the hitstop
    private float HitStopLengthStored;//used to store the manually set length so it can be reset
    public bool hitStop;//used to tell update if a hitstp has occured

    private void Start()//start of start
    {
        hitStopLight.SetActive(false);//deactivates the hit stop light as it is only needed in brief increments.
        HitStopLengthStored = hitStopLength;// saves the stored value
    }//end of start
    public void OnHitStopStart()//fired from the external script, calls the hitstop VFX.
    {
        hitStopLength = HitStopLengthStored;// resets the stored value
        hitStop = true;//tells the script that there is currently an active hitstop
        hitStopLight.SetActive(true);//sets the screen effect to true. this consists of a screen overlay and a light hitting that.
        Time.timeScale = 0;//freezes time
    }
    private void Update()//this allows the hitstop time to actually decay and thus, end.
    {
        if (hitStop)//chekcs if there is an active hitstop
        {//if there is
            hitStopLength -= Time.unscaledDeltaTime;//decrements the timer of the hitstop in real time to avoid it being frozen by the hitstop
            if (hitStopLength < 0)//once that value goes below zero
            {
                OnHitStopEnd();//end the hitstop.
            }
        }
    }//end of update
    private void OnHitStopEnd()// ends the hitstop and resumes normal play.
    {
        hitStop = false;//tells the script there is no longer a hitstop
        hitStopLight.SetActive(false);//disables the screen efffect
        Time.timeScale = 1;//resumes time
        //hitStopSFX.PlayOneShot(hitStopSFXAudio, 0.7f);//plays the hitstop sfx
    }//end of hitstopend
}//end of hitstop effect script
