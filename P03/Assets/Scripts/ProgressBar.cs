using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;

    public void SetMinProgress(int progress)
    {
        slider.minValue = progress;
        slider.value = progress;
    }
    
    public void SetProgress(int progress)
    {
        slider.value = progress;
    }
}
