using Godot;
using System;
using System.Text;

public partial class UDPServerConnection : CharacterBody3D
{
    // UDP server variables
    private PacketPeerUdp _udp = new PacketPeerUdp();
    private string _ip = "127.0.0.1";
    private int _port = 5000;

    // Lane management variables
    private float[] _lanes = new float[] { -2.0f, 0.0f, 2.0f }; // Example lane positions
    private int _currentLaneIndex = 1; // Start in the middle lane
    private float _targetLanePosition = 0.0f; // Target position for smooth movement

    private float _lerpSpeed = 20.0f; // Speed of interpolation

    // Jump variables
    [Export] public float JumpVelocity = 4.5f;

     // Debounce variables
    private float _debounceTime = 0.2f; // Time in seconds to debounce jump input
    private float _timeSinceLastEvent = 0f; // Last time the jump
    private bool _isPending = false;

    public override void _Ready()
    {
        Error err = _udp.Bind(_port);
        if (err != Error.Ok)
        {
            GD.PrintErr($"Failed to listen on UDP port {_port}: {err}");
            return;
        }

        GD.Print($"Listening for UDP packets on {_ip}:{_port}");

    }

    public override void _PhysicsProcess(double delta)
    {
        if (_isPending)
        {
            _timeSinceLastEvent += (float)delta;
            if (_timeSinceLastEvent >= _debounceTime)
            {
                _isPending = false;
            }
        }
        int packetCount = _udp.GetAvailablePacketCount();
        Vector3 currentVelocity = Velocity;
        currentVelocity.Y -= 9.8f * (float)delta; // Apply gravity if not on the floor
        Velocity = currentVelocity;
        if (packetCount > 0 && !_isPending)
        {
            byte[] packet = _udp.GetPacket();
            string message = Encoding.UTF8.GetString(packet).Trim();
            switch (message)
            {
                case "TILT_LEFT":
                    GD.Print("Received tilt_left command");
                    if (_currentLaneIndex > 0)
                    {
                        _currentLaneIndex--;
                        _targetLanePosition = _lanes[_currentLaneIndex];

                    }
                    OnEventTriggered();
                    break;
                case "TILT_RIGHT":
                    GD.Print("Received tilt_right command");
                    // Handle tilt_right command
                    if (_currentLaneIndex < _lanes.Length - 1)
                    {
                        _currentLaneIndex++;
                        _targetLanePosition = _lanes[_currentLaneIndex];
                    }
                    OnEventTriggered();
                    break;
                case "TURN_LEFT":
                    GD.Print("Received turn_left command");
                    // Handle turn_left command
                    OnEventTriggered();
                    break;
                case "TURN_RIGHT":
                    GD.Print("Received turn_right command");
                    // Handle turn_right command
                    OnEventTriggered();
                    break;
                case "JUMP":
                    GD.Print("Received jump command");
                    if (!IsOnFloor())
                    {
                        GD.Print("Cannot jump, not on the floor");
                    }
                    if (IsOnFloor())
                    {
                        currentVelocity.Y = JumpVelocity; // Set jump velocity
                        GD.Print("Jumping");
                    }
                    Velocity = currentVelocity;
                    OnEventTriggered();
                    break;
                case "SLIDE":
                    GD.Print("Received slide command");
                    // Handle slide command
                    OnEventTriggered();
                    break;
                default:
                    GD.Print($"Received unknown command: {message}");
                    break;
            }

            GD.Print($"Received UDP packet: {message}");
        }
        Vector3 currentPosition = Position; // Assuming this script is attached to a Node3D
        currentPosition.X = Mathf.Lerp(currentPosition.X, _targetLanePosition, (float)delta * _lerpSpeed);
        Position = currentPosition;
        MoveAndSlide();
    }

    public void OnEventTriggered()
    {
        _timeSinceLastEvent = 0f;
        _isPending = true;
    }
}
