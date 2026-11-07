using Godot;
using System;
using System.Text;

public partial class UDPServerConnection : Node3D
{
    private PacketPeerUdp _udp = new PacketPeerUdp();
    private string _ip = "127.0.0.1";
    private int _port = 5000;
    private float[] _lanes = new float[] { -2.0f, 0.0f, 2.0f }; // Example lane positions
    private int _currentLaneIndex = 1; // Start in the middle lane
    private float _targetLanePosition = 0.0f; // Target position for smooth movement

    private float _lerpSpeed = 20.0f; // Speed of interpolation

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

    public override void _Process(double delta)
    {
        int packetCount = _udp.GetAvailablePacketCount();
        if (packetCount > 0)
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
                    break;
                case "TILT_RIGHT":
                    GD.Print("Received tilt_right command");
                    // Handle tilt_right command
                    if (_currentLaneIndex < _lanes.Length - 1)
                    {
                        _currentLaneIndex++;
                        _targetLanePosition = _lanes[_currentLaneIndex];
                    }
                    break;
                case "TURN_LEFT":
                    GD.Print("Received turn_left command");
                    // Handle turn_left command
                    break;
                case "TURN_RIGHT":
                    GD.Print("Received turn_right command");
                    // Handle turn_right command
                    break;
                case "JUMP":
                    GD.Print("Received jump command");
                    // Handle jump command
                    break;
                case "SLIDE":
                    GD.Print("Received slide command");
                    // Handle slide command
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
    }
}
