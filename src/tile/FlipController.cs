using System;
using Godot;

namespace MemoryGame;

public partial class FlipController : Node
{
    public event Action doneFlippingToFront;

    [Export] private Node3D targetNode;
    [Export] private Vector3 frontRotation;
    [Export] private Vector3 backRotation;
    [Export] private float transitionTime = 1.5f;

    [Export] private Tween.EaseType easeType = Tween.EaseType.InOut;
    [Export] private Tween.TransitionType transitionType = Tween.TransitionType.Elastic;
    
    private bool showingFront = false;
    private Tween rotationTween;

    public void FlipToFront()
    {
        if (showingFront) return;

        StopCurrentTween();

        GD.Print("Starting Tween");

        rotationTween = CreateTween();
        rotationTween.TweenProperty(targetNode, "rotation_degrees", frontRotation, transitionTime)
            .SetEase(easeType)
            .SetTrans(transitionType);
        rotationTween.Finished += () =>
        {
            showingFront = true;
            doneFlippingToFront?.Invoke();
        };
    }


    public void FlipToBack()
    {
        if (!showingFront) return;

        StopCurrentTween();

        showingFront = false;
        rotationTween = CreateTween();
        rotationTween.TweenProperty(targetNode, "rotation_degrees", backRotation, transitionTime)
            .SetEase(easeType)
            .SetTrans(transitionType);
    }

    private void StopCurrentTween()
    {
        if (rotationTween != null && rotationTween.IsRunning())
        {
            rotationTween.Kill();
        }
    }
}