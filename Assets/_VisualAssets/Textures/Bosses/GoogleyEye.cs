using UnityEngine;

public class GoogleyEye : MonoBehaviour {

	Ball currentBallTracking;
	[SerializeField] Transform eyeBall;

	void Update() {
		// look at the players bat
		if (currentBallTracking == null) {
			// todo ARRRGH this code is terrible, this needs to be changed to requesting a ball from the BallManager.
			currentBallTracking = (Ball)FindObjectOfType(typeof(Ball));
		}

		if (currentBallTracking == null)
		{
			return;
		}
		Vector3 dir = (currentBallTracking.transform.position - this.transform.position).normalized * .08f;
		eyeBall.localPosition = Vector3.Lerp(eyeBall.localPosition, dir, Time.deltaTime * 5f);
	}
}
