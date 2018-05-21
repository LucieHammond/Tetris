using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDesign : MonoBehaviour {
	
	public Text levelText;
	public Text scoreText;
	public Text linesText;
    
	private int score;
	private int level = 1;
	private int linesSingle = 0;
	private int linesDouble = 0;
	private int linesTriple = 0;
	private int linesTetris = 0;
	private int linesTetrisB2B = 0;
	private int Lines {
        get {
            return linesSingle + linesDouble * 3 + linesTriple * 5
                + linesTriple * 8 + linesTetrisB2B * 12;
        }
    }
	private bool lastTetris = false;
    
    public void NewLines(List<int> lines)
	{
		int consecutives = 0;
		for (int i = 0; i <= 20; i++){
			if (lines.Contains(i)){
				consecutives++;
			}
			else if (consecutives > 0){
				switch (consecutives){
					case 1:
						linesSingle++;
						score += 10 * level;
						break;
					case 2:
						linesDouble++;
						score += 30 * level;
						break;
					case 3:
						linesTriple++;
						score += 50 * level;
						break;
					case 4:
						if (!lastTetris) {
							linesTetris++;
							score += 80 * level;
						}
						else {
							linesTetrisB2B++;
							score += 120 * level;
						}
						break;
				}
				lastTetris = (consecutives == 4);
				consecutives = 0;
			}
		}
		level = (int) Lines / 5 + 1;
		PrintScore();
	}

	private void PrintScore(){
		levelText.text = level.ToString();
		scoreText.text = score.ToString();
		linesText.text = Lines.ToString();
	}
    
    public float GetTimePerRaw()
	{
		float temp = 0.8f - (level - 1) * 0.007f;
		temp = Mathf.Pow(temp, level - 1);
		return Mathf.Round(temp * 1000000) / 1000000;
	}

	public void IncreaseScore()
	{
		score++;
		PrintScore();
	}

    public void SendStatistics()
	{
		int[] lines = new int[5] { linesSingle, linesDouble, linesTriple, linesTetris, linesTetrisB2B };
		Statistics.UpdateStatistics(level, score, lines);
	}
}
