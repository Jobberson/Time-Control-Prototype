using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorScaleWithTime : MonoBehaviour
{
    public AnimationCurve scaleToSpeed = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _anim.updateMode = AnimatorUpdateMode.Normal;
    }

    private void Update()
    {
        if (TimeController.Instance == null)
            return;

        float s = TimeController.Instance.CurrentScale;
        _anim.speed = Mathf.Max(0.0f, scaleToSpeed.Evaluate(s));
    }
}
