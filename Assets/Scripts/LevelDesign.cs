using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDesign : MonoBehaviour {
    
	public int level {
		get { return (int) (lines / 10) + 1; }
	}
	public int lines {
		get { return linesSingle + linesDouble * 3 + linesTriple * 5 + linesTriple * 8; }
	}
	public Text levelText;
	public Text scoreText;
	public Text linesText;

	private int score;
	private int linesSingle;
	private int linesDouble;
	private int linesTriple;
	private int linesTetris;
    
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
						break;
					case 2:
						linesDouble++;
						break;
					case 3:
						linesTriple++;
						break;
					case 4:
						linesTetris++;
						break;
				}
				consecutives = 0;
			}
		}
		score = linesSingle * 10 + linesDouble * 25 + linesTriple * 40 + linesTetris * 60;
		PrintScore();
	}

	private void PrintScore(){
		levelText.text = level.ToString();
		scoreText.text = score.ToString();
		linesText.text = lines.ToString();
	}
    
    public void GetSpeed()
	{
		
	}
}
