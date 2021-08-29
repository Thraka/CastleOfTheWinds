using GoRogue;
using GoRogue.GameFramework;

using System.Drawing;

namespace CastleOfTheWinds
{
    public class CastleObject : GameObject
    {
        public CastleObject(int layer, Coord position, string description, string? imagePath = null, IGameObject? parentObject = null, bool isStatic = false, bool isWalkable = false, bool isTransparent = true)
            : base(position, layer, parentObject, isStatic, isWalkable, isTransparent)
        {
            Description = description;
            ImagePath = imagePath;
        }

        public string Description { get; set; }

        public string? ImagePath { get; set; }

        public virtual Image? GetImage() => ImagePath == null ? null : Bitmaps.Read(ImagePath);

        public override string ToString()
        {
            return $"{Position}: {Description}";
        }
    }
}