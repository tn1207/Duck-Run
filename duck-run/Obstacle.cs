using Godot;
using System;


public partial class Obstacle : Area3D
{
    [Export] public float Speed = 15.0f;
    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector3(0, 0, -Speed * (float)delta);

        if (Position.Z < -10)
        {
            QueueFree();
        }
    }

}
