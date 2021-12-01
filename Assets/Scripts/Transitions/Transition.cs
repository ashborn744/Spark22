using UnityEngine;

public enum TransitionType
{
	FadeIn,
	FadeOut,
	SwipeIn,
	SwipeOut
}

public abstract class Transition : MonoBehaviour
{
	[field: SerializeField] public Material Material { get; private set; }

	[field: SerializeField] public float Length { get; set; } = 1.0f;

	[SerializeField] EasingFunction.Ease easingType = EasingFunction.Ease.Linear;

	public EasingFunction.Ease EasingType
	{
		get => easingType;
		set
		{
			easingType = value;
			easingFunction = EasingFunction.GetEasingFunction(easingType);
		}
	}

	protected EasingFunction.Function easingFunction;

	[field: SerializeField, ReadOnly] public float Time { get; protected set; }

	public bool IsFinished() => Time <= 0.0f;
	public virtual void Restart() => Time = Length;
	public virtual void Step(float delta) => Time -= delta;
	public virtual bool IsBlockingClicks() => false;
}
