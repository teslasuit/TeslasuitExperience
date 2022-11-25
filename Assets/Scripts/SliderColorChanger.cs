using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderColorChanger : MonoBehaviour {
    [SerializeField] private Image img;
    [SerializeField] private Gradient gradient;
    [SerializeField]private Slider slider;
    private void Awake() {
        if(slider==null)slider = GetComponent<Slider>();
        if(img==null)img = slider.fillRect.GetComponent<Image>();
    }


    public void UpdateColor () {
        if (slider) { 
            img.color = gradient.Evaluate(slider.value);
        }
    }
    [EasyButtons.Button]
    public void NewColor() {
        img.color = gradient.Evaluate(slider.value);
    }

    private void Reset() {
        slider = GetComponent<Slider>();
        img = slider.fillRect.GetComponent<Image>();
    }
}
