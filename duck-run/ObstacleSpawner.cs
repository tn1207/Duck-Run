using Godot;
using System;

public partial class ObstacleSpawner : Node3D
{
    [Export] public PackedScene ObstacleScene;
    private Timer _spawnTimer;
    private float[] _lanes = { -2.0f, 0.0f, 2.0f };
    private Random _random = new Random();

    public override void _Ready()
    {
        // Create and configure a timer dynamically to spawn obstacles every 1.5 seconds
        _spawnTimer = new Timer();
        _spawnTimer.WaitTime = 1.5f;
        _spawnTimer.Autostart = true;
        _spawnTimer.Timeout += OnSpawnTimeout;
        AddChild(_spawnTimer);
    }

    private void OnSpawnTimeout()
    {
        if (ObstacleScene == null) return;

        // Instantiate the obstacle
        var obstacle = (Node3D)ObstacleScene.Instantiate();

        // Pick a random lane X coordinate
        int laneIndex = _random.Next(_lanes.Length);
        float spawnX = _lanes[laneIndex];

        // Add it to the main world tree
        GetTree().Root.AddChild(obstacle);

        // Set spawn position far down the Z track (e.g., Z = -40)
        obstacle.Position = new Vector3(spawnX, 1.75f, -40.0f);
    }
}