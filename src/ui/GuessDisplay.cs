using Godot;
using MemoryGame;

public partial class GuessDisplay : Label
{
    [Export] private GameController gameController;

    public override void _Ready()
    {
        gameController.OnGuessesChanged += UpdateGuessText;
    }

    private void UpdateGuessText(int count)
    {
        Text = $"Guesses Remaining: {count}";
    }
}