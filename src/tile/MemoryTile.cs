using System;
using Godot;
using MemoryGame;

[GlobalClass]
public partial class MemoryTile : StaticBody3D
{
    public event Action<MemoryTile> OnTileClicked;

    [Export] private Sprite3D tileFace;
    private string textureKey;
    private bool isHovered;

    public override void _Input(InputEvent @event)
    {
        if (!isHovered) return;


        if (@event is InputEventMouseButton iemb && iemb.Pressed && iemb.ButtonIndex == MouseButton.Left)
        {
            GD.Print("Clicked!");
            OnTileClicked?.Invoke(this);
        }
    }

    public void Initialize(TileTextureMapping textureMapping)
    {
        textureKey = textureMapping.key;
        tileFace.Texture = textureMapping.texture;
    }

    public string GetTextureKey()
    {
        return textureKey;
    }

    public override void _MouseEnter()
    {
        isHovered = true;
    }

    public override void _MouseExit()
    {
        isHovered = false;
    }

    public FlipController GetFlipController()
    {
        return GetNode<FlipController>("FlipController");
    }
}