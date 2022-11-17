using MathGameMAUI.Models;
using System.Runtime.ExceptionServices;

namespace MathGameMAUI;

public partial class GamePage : ContentPage
{
	public string GameType { get; set; }
	
	int firstNumber;
	int secondNumber;
	int score;
	private const int totalQuestions = 2;
	int gamesLeft = totalQuestions;
	public GamePage(string gameType)
	{
		InitializeComponent();
		GameType = gameType;
		BindingContext = this;
		CreateQuestion();
	}
	private void CreateQuestion()
	{
		Random random = new();
		firstNumber = random.Next(1,500);
        secondNumber = random.Next(1, 500);
		if (GameType=="+")
		{
			while (firstNumber+secondNumber>1000)
			{
                firstNumber = random.Next(1, 500);
                secondNumber = random.Next(1, 500);
            }
		}
        else if (GameType=="/")
		{
            firstNumber = GameType != "/" ? random.Next(1, 9) : random.Next(1, 99);
            secondNumber = GameType != "/" ? random.Next(1, 9) : random.Next(1, 10);
            while (firstNumber<secondNumber||firstNumber%secondNumber!=0||firstNumber/secondNumber>10)
			{
				firstNumber = random.Next(1, 100);
				secondNumber = random.Next(1, 10);
			}
		}
		else if (GameType == "X")
		{
            firstNumber = random.Next(1, 10);
            secondNumber = random.Next(1, 10);
        }
		else if (GameType == "-")
		{
                firstNumber = random.Next(1, 500);
                secondNumber = random.Next(1, firstNumber);  
        }
		QuestionLabel.Text = $"{firstNumber} {GameType} {secondNumber}";
    }
	private void OnAnswerSubmitted(object sender, EventArgs e)
	{
		
		int answer = int.Parse(AnswerEntry.Text);
		bool isCorrect = false;
		switch (GameType)
		{
			case "+":
				isCorrect = answer == firstNumber + secondNumber;
				break;
            case "-":
                isCorrect = answer == firstNumber - secondNumber;
                break;
            case "X":
                isCorrect = answer == firstNumber * secondNumber;
                break;
            case "/":
                isCorrect = answer == firstNumber / secondNumber;
                break;
            default:
				break;

		}
        ProcessAnswer(isCorrect);
		gamesLeft--;
		AnswerEntry.Text ="";
		if (gamesLeft>0)
		{
			CreateQuestion();
		}
		else
		{
			GameOver();
		}
    }

    private void GameOver()
    {
		GameOperation gameOperation = GameType switch
		{
			"+" => GameOperation.Addition,
			"-" => GameOperation.Substraction,
            "X" => GameOperation.Multiplication,
			"/" => GameOperation.Division
		};
		QuestionArea.IsVisible = false;
		BackToMenuBtn.IsVisible = true;
		GameOverLabel.Text = $"Game over! You got {score} out of {totalQuestions} right";

		App.GameRepository.Add(new Game
		{
			DatePlayed = DateTime.Now,
			Type = gameOperation,
			Score = score
		});
    }	

    private void ProcessAnswer(bool isCorrect)
    {
        if (isCorrect)
		{
			score++;
		}
		AnswerLabel.Text = isCorrect ? "Correct" : "Incorrect";
    }
	private void OnBackToMenu(object sender, EventArgs e)
	{
		score = 0;
		gamesLeft = totalQuestions;
		Navigation.PushAsync(new MainPage());
	}
}