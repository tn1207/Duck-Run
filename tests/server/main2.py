# This sends example commands every 3 seconds using the send_input function from command_sender.py
# When testing, ensure that the server is running first or it has nothing to send to
from pathlib import Path
import time
import sys

root_dir = Path(__file__).resolve().parents[2]
if str(root_dir) not in sys.path:
    sys.path.insert(0, str(root_dir))

from server.command_sender import send_input






commands = ["JUMP", "SLIDE", "TILT_LEFT", "TILT_RIGHT", "TURN_LEFT", "TURN_RIGHT"]
    


send_input("TEST COMMANDS")
for k in range(len(commands)):
    time.sleep(3)
    send_input(commands[k])


