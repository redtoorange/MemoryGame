using System;
using Godot;

namespace MemoryGame;

public partial class GameController : Node
{
    [Export] private TileController tileController;
    [Export] private int guesses = 6;

    [Export(PropertyHint.File, "*.tscn")] private string winScreen;
    [Export(PropertyHint.File, "*.tscn")] private string loseScreen;

    public event Action<int> OnGuessesChanged;

    public override void _Ready()
    {
        tileController.OnAllTilesGone += HandleAllTilesGone;
        tileController.OnFailedGuess += HandleFailedGuess;
    }

    private void HandleFailedGuess()
    {
        if (guesses == 0)
        {
            GetTree().ChangeSceneToFile(loseScreen);
        }
        else
        {
            guesses -= 1;
            OnGuessesChanged?.Invoke(guesses);
        }
    }

    private void HandleAllTilesGone()
    {
        GetTree().ChangeSceneToFile(winScreen);
    }
}