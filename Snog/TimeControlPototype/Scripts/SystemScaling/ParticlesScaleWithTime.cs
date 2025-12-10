using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlesScaleWithTime : MonoBehaviour
{
    private ParticleSystem _ps;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (TimeController.Instance == null)
            return;

        _ps.main.simulationSpeed = TimeController.Instance.CurrentScale;
    }
}
