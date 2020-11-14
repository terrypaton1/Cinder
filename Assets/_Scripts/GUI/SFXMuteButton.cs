using UnityEngine;

public class SFXMuteButton : MonoBehaviour {
	/// <summary>
	/// The on graphic.
	/// </summary>
	[SerializeField]
	GameObject onGraphic;

	/// <summary>
	/// The off graphic.
	/// </summary>
	[SerializeField]
	GameObject offGraphic;

	/// <summary>
	/// The animator.
	/// </summary>
	Animator _animator;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		_animator = GetComponent<Animator>();
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable() {
		EvaluateSFXState();
	}

	/// <summary>
	/// Raises the click event.
	/// </summary>
	void OnClick() {
//		Debug.Log(_animator);
		if (_animator != null) {
			_animator.Play("ButtonPressAnimation");
		}
		if (GameVariables.instance.SFXEnabled == 0) {
			GameVariables.instance.SFXEnabled = 1;
		} else {
			GameVariables.instance.SFXEnabled = 0;
		}
		PlayerPrefs.SetInt(DataVariables.SFXEnabled, GameVariables.instance.SFXEnabled);
		EvaluateSFXState();
	}

	/// <summary>
	/// Evaluates the state of the SFX.
	/// </summary>
	void EvaluateSFXState() {
		if (GameVariables.instance.SFXEnabled == 0) {
			onGraphic.SetActive(false);
			offGraphic.SetActive(true);
		} else {
			onGraphic.SetActive(true);
			offGraphic.SetActive(false);
		}			
	}
}


		