using UnityEngine;
using UnityEngine.UI;

public class DisplayStatistics : MonoBehaviour {

	public Text nbGames;
	public Text avgScore;
	public Text totalScore;

	public Text lastLevel;
    public Text maxLevel;
	public Text lastScore;
	public Text maxScore;
	public Text lastLines;
	public Text maxLines;

	public Text[] lastLinesDetails;
	public Text[] maxLinesDetails;

	private void Start () 
	{
		nbGames.text = Statistics.nbGames.ToString("n0");
		avgScore.text = Statistics.avgScore.ToString("n");
		totalScore.text = Statistics.totalScore.ToString("n0");

		lastLevel.text = Statistics.lastLevel.ToString();
		maxLevel.text = Statistics.maxLevel.ToString();
		lastScore.text = Statistics.lastScore.ToString("n0");
		maxScore.text = Statistics.maxScore.ToString("n0");
		lastLines.text = Statistics.lastLines.ToString("n0");
		maxLines.text = Statistics.maxLines.ToString("n0");

		for (int i = 0; i < lastLinesDetails.Length; i++)
		{
			lastLinesDetails[i].text = Statistics.lastLinesDetail[i].ToString("n0");
			maxLinesDetails[i].text = Statistics.maxLinesDetail[i].ToString("n0");
		}
	}
}
