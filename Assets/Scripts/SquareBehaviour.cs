using UnityEngine;

public class SquareBehaviour : MonoBehaviour {

	private void OnDestroy()
	{
		Transform parentTetrimino = transform.parent;
		if (parentTetrimino && parentTetrimino.transform.childCount == 2)
		{
			Destroy(parentTetrimino.gameObject);
		}
	}
    
    public int GetID()
	{
		switch(gameObject.name)
		{
			case ("I-Square"):
				return 1;
			case ("J-Square"):
				return 2;
			case ("L-Square"):
                return 3;
			case ("O-Square"):
                return 4;
			case ("S-Square"):
                return 5;
			case ("T-Square"):
                return 6;
			case ("Z-Square"):
                return 7;
			default:
				return -1;

		}
	}
}
