using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioScaleWithTime : MonoBehaviour
{
    public float minPitch = 0.4f;
    private AudioSource _src;

    private void Awake()
    {
        _src = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (TimeController.Instance == null)
            return;

        float s = TimeController.Instance.CurrentScale;
        _src.pitch = Mathf.Lerp(minPitch, 1f, s);
    }
}
