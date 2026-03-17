using UnityEngine;

public class HitStopEffects : MonoBehaviour
{
    [SerializeField] private GameObject hitStopLight;
    [SerializeField] private AudioClip hitStopSFXAudio;
    [SerializeField] private AudioSource hitStopSFX;
    [SerializeField] private float hitStopLength;
    private float HitStopLengthStored;
    public bool hitStop;

    private void Start()
    {
        hitStopLight.SetActive(false);
        HitStopLengthStored = hitStopLength;// saves the stored value
    }
    public void OnHitStopStart()
    {
        hitStopLength = HitStopLengthStored;// resets the stored value
        hitStop = true;
        Debug.Log("hitstop");
        hitStopLight.SetActive(true);
        Time.timeScale = 0;
    }
    private void Update()
    {
        if (hitStop)
        {
            hitStopLength -= Time.unscaledDeltaTime;
            if (hitStopLength < 0)
            {
                OnHitStopEnd();
            }
        }
    }
    private void OnHitStopEnd()
    {
        hitStop = false;
        hitStopLight.SetActive(false);
        Time.timeScale = 1;
        hitStopSFX.PlayOneShot(hitStopSFXAudio, 0.7f);
        Debug.Log("hitstop end");

    }
}
