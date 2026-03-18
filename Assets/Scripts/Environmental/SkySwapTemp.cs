using System.Collections;
using UnityEngine;

public class SkySwapTemp : MonoBehaviour
{
    Renderer skyRenderer;
    [SerializeField] float sky1ChangeTimeValue;
    [SerializeField] float sky2ChangeTimeValue;
    [SerializeField] float sky1Value;
    [SerializeField] float sky2Value;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skyRenderer = GetComponent<Renderer>();
        StartCoroutine("SkyChange");
    }

   IEnumerator SkyChange()
    {

        while (sky2Value <= 1)
        {
            sky1Value = 0.1f / sky1ChangeTimeValue;
            sky2Value = 0.1f / sky2ChangeTimeValue;
            skyRenderer.material.SetFloat("_Sky01Amount", sky1Value);
            skyRenderer.material.SetFloat("_Sky02Amount", sky2Value);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
