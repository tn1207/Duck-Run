import socket

# Localhost and port
HOST = "127.0.0.1"
PORT = 5005
BUFFER_SIZE = 1024

# Simple socket example
# Using socket, create a UDP socket

def send_input(input):
    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as server_socket:
        # send data
        message = f"COMMAND: {input}".encode('utf-8')
        server_socket.sendto(message, (HOST, PORT))




