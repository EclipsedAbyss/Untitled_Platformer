using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private TMP_InputField FOVInput;
    [SerializeField] private Slider FOVSlider;
    [SerializeField] private TMP_InputField sensitivityInput;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_InputField targetFPSInput;
    [SerializeField] private Slider targetFPSSlider;
    [SerializeField] private Toggle VSYNCToggler;
    [SerializeField] private Toggle UNCAPToggler;
    private float setFOV;
    private float setSens;
    private float setFPS;
    private StoredSettings settingsStored;
    private PlayerCamera settingsData;
    private FPSLock FPSData;

    private void Start()
    {
     FPSData = FindFirstObjectByType<FPSLock>();
     settingsData = FindFirstObjectByType<PlayerCamera>();
     settingsStored = FindFirstObjectByType<StoredSettings>();
        if (!settingsStored.FPSUncap)
        {
            targetFPSInput.interactable = true;
            targetFPSSlider.interactable = true;
            VSYNCToggler.interactable = true;
            UNCAPToggler.isOn = false;
        }
        else
        {
            targetFPSInput.interactable = false;
            targetFPSSlider.interactable = false;
            VSYNCToggler.interactable = false;
            UNCAPToggler.isOn = true;
        }

        if (settingsStored.VSYNC)
        {
            VSYNCToggler.isOn = true;
            targetFPSInput.interactable = false;
            targetFPSSlider.interactable = false;
            UNCAPToggler.interactable = false;
        }
        else
        {
            VSYNCToggler.isOn = false;
            targetFPSInput.interactable = true;
            targetFPSSlider.interactable = true;
            UNCAPToggler.interactable = true;
        }

    }

    public void VSYNCToggle()
    {
        if (settingsStored.VSYNC)
        {
            settingsStored.VSYNC = false;
        }
        else
        {
            settingsStored.VSYNC = true;
        }
    }

    public void UNCAPToggle()
    {
        if (settingsStored.FPSUncap)
        {
            settingsStored.FPSUncap = false;
            targetFPSInput.interactable = true;
            targetFPSSlider.interactable = true;
        }
        else
        {
            settingsStored.FPSUncap = true;
            targetFPSInput.interactable = false;
            targetFPSSlider.interactable = false;
        }
    }
    
    public void UpdateSettings()
    {
        if (FOVInput.text != "")
            setFOV = float.Parse(FOVInput.text);
        else
        {
            FOVInput.text = settingsData.storedCamFOV.ToString("");
            setFOV = float.Parse(FOVInput.text);
        }

        if (setFOV != settingsStored.storedFOV)
        {
            settingsStored.storedFOV = setFOV;
        }

        if (sensitivityInput.text != "")
            setSens = float.Parse(sensitivityInput.text);
        else
        {
            sensitivityInput.text = settingsData.sensitivity.ToString("");
            setSens = float.Parse(sensitivityInput.text);
        }
          
        if (setSens != settingsStored.storedSens)
        {
            settingsStored.storedSens = setSens;
        }

        if (targetFPSInput.text != "")
            setFPS = float.Parse(targetFPSInput.text);
        else
        {
            targetFPSInput.text += FPSData.defaultTarget.ToString("");
            setFPS = float.Parse(targetFPSInput.text);
        }

        if (setFPS != settingsStored.storedFPS)
        {
            settingsStored.storedFPS = setFPS;
        }

        if (settingsStored.VSYNC == true)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    public void SliderChange()
    {
        if (FOVSlider.value != float.Parse(FOVInput.text))
        {
            FOVInput.text = FOVSlider.value.ToString("");
        }

        if (sensitivitySlider.value != float.Parse(sensitivityInput.text))
        {
            sensitivityInput.text = sensitivitySlider.value.ToString("");
        }

        if (targetFPSSlider.value != float.Parse(targetFPSInput.text))
        {
            targetFPSInput.text = targetFPSSlider.value.ToString("");
        }
    }

    public void InputChange()
    {
        if (float.Parse(FOVInput.text) != FOVSlider.value)
        {
            FOVSlider.value = float.Parse(FOVInput.text);
        }

        if (float.Parse(sensitivityInput.text) != sensitivitySlider.value)
        {
            sensitivitySlider.value = float.Parse(sensitivityInput.text);
        }

        if (float.Parse(targetFPSInput.text) != targetFPSSlider.value)
        {
            targetFPSSlider.value = float.Parse(targetFPSInput.text);
        }
    }

    public void InputEnter()
    {
        if (float.Parse (FOVInput.text) > 200)
        {
            FOVInput.text = "200";
        }

        if (float.Parse (sensitivityInput.text) > 100)
        {
            sensitivityInput.text = "100";
        }

        if (float.Parse(targetFPSInput.text) > 120)
        {
            targetFPSInput.text = "120";
        }
    }
}
