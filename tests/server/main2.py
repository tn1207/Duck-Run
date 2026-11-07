# This sends example commands every 3 seconds using the send_input function from command_sender.py
# When testing, ensure that the server is running first or it has nothing to send to

from server.command_sender import send_input
import time

commands = ["JUMP", "SLIDE", "TILT_LEFT", "TILT_RIGHT", "TURN_LEFT", "TURN_RIGHT"]
    

send_input("TEST COMMANDS")
for k in range(len(commands)):
    time.sleep(3)
    send_input(commands[k])


