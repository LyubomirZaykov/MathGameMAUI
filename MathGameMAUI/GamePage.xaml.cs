using System.Runtime.ExceptionServices;

namespace MathGameMAUI;

public partial class GamePage : ContentPage
{
	public string GameType { get; set; }
	int firstNumber;
	int secondNumber;
	int score;
	const int totalQuestions = 2;
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
		string gameOperand = null;
		switch (GameType)
		{
			case "Addition":
				gameOperand = "+";
				break;
			case "Substraction":
				gameOperand = "-";
				break;
			case "Multiplication":
				gameOperand = "*";
				break;
			case "Division":
				gameOperand = "/";
				break;
			default: gameOperand = "";
				break;
		}
        Random random = new Random();
		firstNumber = GameType != "Division" ? random.Next(1, 9) : random.Next(1, 99);
		secondNumber = GameType != "Division" ? random.Next(1, 9) : random.Next(1, 99);
		if (GameType=="Division")
		{
			while (firstNumber<secondNumber||firstNumber%secondNumber!=0)
			{
				firstNumber = random.Next(1, 99);
				secondNumber = random.Next(1, 99);
			}
		}
		else if (GameType == "Substraction")
		{
            
                firstNumber = random.Next(1, 99);
                secondNumber = random.Next(1, firstNumber);
            
        }
		QuestionLabel.Text = $"{firstNumber} {gameOperand} {secondNumber}";
    }
	private void OnAnswerSubmitted(object sender, EventArgs e)
	{
		int answer = int.Parse(AnswerEntry.Text);
		bool isCorrect = false;
		switch (GameType)
		{
			case "Addition":
				isCorrect = answer == firstNumber + secondNumber;
				break;
            case "Substraction":
                isCorrect = answer == firstNumber - secondNumber;
                break;
            case "Multiplication":
                isCorrect = answer == firstNumber * secondNumber;
                break;
            case "Division":
                isCorrect = answer == firstNumber / secondNumber;
                break;
            default:
				break;

		}
        ProcessAnswer(isCorrect);
		gamesLeft--;
		AnswerEntry.Text = "";
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
		GameOverLabel.Text = $"Game over! You got {score} out of {totalQuestions} right";
    }

    private void ProcessAnswer(bool isCorrect)
    {
        if (isCorrect)
		{
			score++;
		}
		AnswerLabel.Text = isCorrect ? "Correct" : "Incorrect";
    }
}