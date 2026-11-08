import socket
import time

# Godot UDP Server configuration
UDP_IP = "127.0.0.1"
UDP_PORT = 5000

print(f"UDP Test Client targeting {UDP_IP}:{UDP_PORT}")
print("Commands: [a] Tilt Left, [d] Tilt Right, [q] Turn Left, [e] Turn Right, [space] Jump, [s] Slide, [x] Exit")

# Create a UDP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

command_map = {
    'a': "TILT_LEFT",
    'd': "TILT_RIGHT",
    'q': "TURN_LEFT",
    'e': "TURN_RIGHT",
    ' ': "JUMP",
    'space': "JUMP",
    's': "SLIDE"
}

try:
    while True:
        # Get input from the console to simulate gestures
        key = input("Enter command key (a/d/q/e/space/s) or 'x' to quit: ").strip().lower()
        
        if key == 'x':
            print("Exiting test client...")
            break
            
        if key in command_map:
            message = command_map[key]
            sock.sendto(message.encode('utf-8'), (UDP_IP, UDP_PORT))
            print(f"Sent UDP packet: {message}")
        else:
            print("Unknown key! Use 'a', 'd', 'q', 'e', 'space', 's', or 'x'.")
            
except KeyboardInterrupt:
    print("\nClient closed.")
finally:
    sock.close()