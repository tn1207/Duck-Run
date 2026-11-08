using Godot;


public partial class Obstacle : Area3D
{
    [Export] public float Speed = 15.0f;

    public override void _Ready()
    {
        // Connect the body entered signal to detect collisions with the player
        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 currentPosition = GlobalPosition;
        currentPosition.Z += Speed * (float)delta;
        GlobalPosition = currentPosition;

        if (GlobalPosition.Z > 30.0f) // Adjust this value based on your game's requirements
        {
            QueueFree(); // Remove the obstacle from the scene when it goes off-screen
        }

    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is CharacterBody3D)
        {
            GD.Print("Player hit an obstacle!");
            // Handle player collision with the obstacle (e.g., reduce health, end game, etc.)
            GetTree().ReloadCurrentScene();
        }
    }

}
