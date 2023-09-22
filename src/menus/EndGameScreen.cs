using Godot;

public partial class EndGameScreen : Control
{
    [Export] private Button playAgain;
    [Export] private Button quitButton;
    [Export(PropertyHint.File, "*.tscn")] private string gameScene;

    public override void _Ready()
    {
        playAgain.Pressed += () => { GetTree().ChangeSceneToFile(gameScene); };
        quitButton.Pressed += () => { GetTree().Quit(); };
    }
}