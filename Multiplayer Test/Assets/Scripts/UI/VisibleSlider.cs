using UnityEngine;
using UnityEngine.UI;

public class VisibleSlider : MonoBehaviour
{
    [SerializeField] private Text _textCount;
    [SerializeField] private Slider _slider;

    public void SliderValue()
    {
        _textCount.text = _slider.value.ToString();
    }
}
