using System;
using Godot;
using Godot.Collections;

namespace MemoryGame;

public partial class TileController : Node3D
{
    public event Action OnAllTilesGone;
    public event Action OnFailedGuess;

    [Export] private Array<TileTextureMapping> materialMapping;
    [Export] private Node3D tileParent;
    private Array<MemoryTile> controlledTiles;

    [Export] private bool processing = false;
    [Export] private MemoryTile firstClickedTile;
    [Export] private MemoryTile secondClickedTile;

    public override void _Ready()
    {
        controlledTiles = new Array<MemoryTile>();
        foreach (Node child in tileParent.GetChildren())
        {
            if (child is MemoryTile mt)
            {
                controlledTiles.Add(mt);
            }
        }
        controlledTiles.Shuffle();

        Array<TileTextureMapping> materialMappingCopy = new Array<TileTextureMapping>(materialMapping);

        for (int i = 0; i < controlledTiles.Count; i += 2)
        {
            int keyIndex = GD.RandRange(0, materialMappingCopy.Count - 1);

            MemoryTile firstTile = controlledTiles[i];
            MemoryTile secondTile = controlledTiles[i + 1];

            firstTile.Initialize(materialMappingCopy[keyIndex]);
            firstTile.OnTileClicked += HandleTileClicked;

            secondTile.Initialize(materialMappingCopy[keyIndex]);
            secondTile.OnTileClicked += HandleTileClicked;

            materialMappingCopy.RemoveAt(keyIndex);
        }
    }


    private void HandleTileClicked(MemoryTile tile)
    {
        // Currently waiting on an animation
        if (processing) return;

        // Clicked same tile again
        if (tile == firstClickedTile || tile == secondClickedTile) return;

        if (firstClickedTile == null)
        {
            firstClickedTile = tile;
            firstClickedTile.GetFlipController().FlipToFront();
        }
        else if (secondClickedTile == null)
        {
            processing = true;
            secondClickedTile = tile;
            secondClickedTile.GetFlipController().FlipToFront();
            secondClickedTile.GetFlipController().doneFlippingToFront += HandleDoneFlipping;
        }
    }

    private void HandleDoneFlipping()
    {
        secondClickedTile.GetFlipController().doneFlippingToFront -= HandleDoneFlipping;

        if (firstClickedTile.GetTextureKey() == secondClickedTile.GetTextureKey())
        {
            GD.Print("Found Match!");

            controlledTiles.Remove(firstClickedTile);
            controlledTiles.Remove(secondClickedTile);

            if (controlledTiles.Count == 0)
            {
                OnAllTilesGone?.Invoke();
            }

            firstClickedTile.QueueFree();
            secondClickedTile.QueueFree();
        }
        else
        {
            GD.Print("No Match!");

            firstClickedTile.GetFlipController().FlipToBack();
            secondClickedTile.GetFlipController().FlipToBack();

            OnFailedGuess?.Invoke();
        }

        firstClickedTile = null;
        secondClickedTile = null;
        processing = false;
    }
}