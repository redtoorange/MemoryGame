using Godot;

namespace MemoryGame;

[GlobalClass]
public partial class TileTextureMapping : Resource
{
   [Export] public string key;
   [Export] public Texture2D texture;
}