using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleObject : MonoBehaviour
{
    private UnityEvent<Toggle> toggleEvent;

    private Toggle m_Toggle;
    

    private void OnDestroy()
    {
        m_Toggle.onValueChanged.RemoveListener(OnValueChanged);
    }
    private void OnValueChanged(bool value)
    {
        if (value == true)
        {
            toggleEvent?.Invoke(m_Toggle);
        }
    }
    public void Initialize(Toggle toggle, UnityEvent<Toggle> toggleEvent)
    {
        this.toggleEvent = toggleEvent;
        m_Toggle = toggle;
        m_Toggle.onValueChanged.AddListener(OnValueChanged);
    }


}