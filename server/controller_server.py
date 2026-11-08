import socket

# Localhost and port
HOST = "127.0.0.1"
PORT = 5005
BUFFER_SIZE = 1024

# Controller socket -> Constantly listens for commands that will be sent by command_sender.py
# Using socket, create a UDP socket


def run_server():
    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as server_socket:
        # Pair the socket with the address and port
        server_socket.bind((HOST, PORT))
        print(f"UDP server is running | listening to {HOST}:{PORT} for input")

        # Contantly halts program to recieve next input (polling -> may cause issues)
        while True:
            # Recieve data 
            data, client_address = server_socket.recvfrom(BUFFER_SIZE)
            data = data.decode('utf-8')
            print(f"Recieved '{data}' from {client_address}")





