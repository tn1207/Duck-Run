using Godot;
using System;

public partial class FloorMover : Node3D
{
	[Export] public float Speed = 15.0f;

	// Set this to the exact length of your river_map model (e.g., 50.0f)
	[Export] public float TileLength = 30.0f;

	// The Z coordinate where the tile recycles back to the start
	private float _resetZ = 50.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector3 pos = Position;
		pos.Z += Speed * (float)delta;
		Position = pos;

		// When a tile scrolls past the screen threshold, push it back by (TileLength * TotalTiles)
		if (Position.Z >= _resetZ)
		{
			pos.Z -= TileLength * 3.0f; // Multiplied by 3 since you have 3 map chunks
			Position = pos;
		}
	}
}
