using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlesScaleWithTime : MonoBehaviour
{
    private ParticleSystem _ps;
    private ParticleSystem.MainModule _main;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
        _main = _ps.main;
    }

    private void Update()
    {
        if (TimeController.Instance == null)
            return;

        _main.simulationSpeed = TimeController.Instance.CurrentScale;
    }
}
