using Godot;
using System;
using System.Text;

public partial class UDPServerConnection : Node
{
    private PacketPeerUdp _udp = new PacketPeerUdp();
    private string _ip = "127.0.0.1";
    private int _port = 4242;

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
            string message = Encoding.UTF8.GetString(packet);
            GD.Print($"Received UDP packet: {message}");
        }
    }
}
