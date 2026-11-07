import socket

# Localhost and port
HOST = "127.0.0.1"
PORT = 5000
BUFFER_SIZE = 1024

# Simple socket example
# Using socket, create a UDP socket

def create_server():
    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as server_socket:
        # Pair the socket with the address and port
        server_socket.bind((HOST, PORT))
        print(f"UDP server is running | listening to {HOST}:{PORT}")
    
        # send data
        message = b"Hello Server, my name is dev Ricky"
        server_socket.sendto(message, (HOST, PORT))

        # Recieve data 
        data, client_address = server_socket.recvfrom(BUFFER_SIZE)
        print(f"Recieved '{data.decode('utf-8')}' from {client_address}")



