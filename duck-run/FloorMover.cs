using Godot;
using System;

public partial class FloorMover : StaticBody3D
{
    [Export] public float speed = 15.0f;

    private float _resetThreshold = 50.0f;
    private float _startZ = -50.0f;

    public override void _PhysicsProcess(double delta)
    {
        Vector3 currentPosition = Position;
        currentPosition.Z += speed * (float)delta;
        Position = currentPosition;

        if (Position.Z >= _resetThreshold)
        {
            currentPosition.Z = _startZ;
            Position = currentPosition;
        }
    }

}
