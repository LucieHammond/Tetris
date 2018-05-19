using UnityEngine;

public class SquareBehaviour : MonoBehaviour {

	private void OnDestroy()
	{
		GameObject parentTetrimino = transform.parent.gameObject;
        if (parentTetrimino.transform.childCount == 1)
		{
			Destroy(parentTetrimino);
		}
	}
}
