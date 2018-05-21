using UnityEngine;

public static class Statistics {
    
	public static int nbGames = 0;
    
	// Score
	public static float avgScore {
		get {
			if (nbGames > 0)
			    return ((float)totalScore) / nbGames;
			return 0;
		}
	}
	public static int totalScore = 0;
	public static int lastScore = 0;
	public static int maxScore = 0;

    // Level
	public static int lastLevel = 0;
    public static int maxLevel = 0;

    // Lines
	public static int lastLines = 0;
    public static int maxLines = 0;

	// Lines detail
	public static int[] lastLinesDetail = new int[5];
	public static int[] maxLinesDetail = new int[5];

    public static void UpdateStatistics(int level, int score, int[] lines)
	{
		nbGames++;

		lastScore = score;
		totalScore += score;
		maxScore = Mathf.Max(maxScore, score);

		lastLevel = level;
		maxLevel = Mathf.Max(maxLevel, level);

		lastLines = lines[0] + lines[1] * 3 + lines[2] * 5 + lines[3] * 8 + lines[4] * 12;
		maxLines = Mathf.Max(maxLines, lastLines);

		for (int i = 0; i < lines.Length; i++)
		{
			lastLinesDetail[i] = lines[i];
			maxLinesDetail[i] = Mathf.Max(maxLinesDetail[i], lines[i]);
		}      
	}

}
