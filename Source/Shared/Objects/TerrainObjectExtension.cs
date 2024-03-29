﻿using System.Drawing;
using GoRogue;

namespace CastleOfTheWinds.Objects
{
    public class TerrainObjectExtension : CastleObject
    {
        public TerrainObjectExtension(Coord position, TerrainObject parent)
            : base(parent.Layer, position, parent.Description, isStatic: parent.IsStatic, isWalkable: parent.IsWalkable, isTransparent: parent.IsTransparent)
        {
            ParentTerrainObject = parent;
        }

        public TerrainObject ParentTerrainObject { get; }

        public override Image? GetImage() => null;
    }
}