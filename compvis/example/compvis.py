import os
import urllib.request
from pathlib import Path
import cv2
import mediapipe as mp
from mediapipe.tasks import python
from mediapipe.tasks.python import vision

# 1. Dynamically resolve paths and download the model if missing
SCRIPT_DIR = Path(__file__).resolve().parent
MODEL_PATH = SCRIPT_DIR / "pose_landmarker.task"
MODEL_URL = "https://storage.googleapis.com/mediapipe-models/pose_landmarker/pose_landmarker_heavy/float16/1/pose_landmarker_heavy.task"

if not MODEL_PATH.exists():
    print(f"Downloading model to {MODEL_PATH}...")
    urllib.request.urlretrieve(MODEL_URL, MODEL_PATH)
    print("Download complete.")

# 2. Define custom pose connections for pure OpenCV drawing (no mp.solutions)
POSE_CONNECTIONS = [
    (11, 12), (11, 13), (13, 15), (12, 14), (14, 16), # Arms and Shoulders
    (11, 23), (12, 24), (23, 24),                     # Torso
    (23, 25), (25, 27), (27, 29), (29, 31),           # Left Leg
    (24, 26), (26, 28), (28, 30), (30, 32),           # Right Leg
    (0, 1), (1, 2), (2, 3), (3, 7),                   # Left Face
    (0, 4), (4, 5), (5, 6), (6, 8), (9, 10)           # Right Face
]

def draw_custom_skeleton(frame, pose_landmarks):
    """Draws the pose skeleton using pure OpenCV."""
    h, w, _ = frame.shape
    
    # Convert normalized coordinates to pixel coordinates
    landmark_px = {}
    for idx, landmark in enumerate(pose_landmarks):
        cx, cy = int(landmark.x * w), int(landmark.y * h)
        landmark_px[idx] = (cx, cy)
        
        # Draw joints
        cv2.circle(frame, (cx, cy), 4, (0, 255, 0), -1)

    # Draw bones
    for connection in POSE_CONNECTIONS:
        start_idx, end_idx = connection
        if start_idx in landmark_px and end_idx in landmark_px:
            cv2.line(frame, landmark_px[start_idx], landmark_px[end_idx], (255, 0, 0), 2)

def run_tracker():
    # 3. Initialize Pose Landmarker Task
    base_options = python.BaseOptions(model_asset_path=str(MODEL_PATH))
    options = vision.PoseLandmarkerOptions(
        base_options=base_options,
        running_mode=vision.RunningMode.VIDEO,
        min_pose_detection_confidence=0.5,
        min_tracking_confidence=0.5
    )
    landmarker = vision.PoseLandmarker.create_from_options(options)

    # 4. Setup Webcam
    cap = cv2.VideoCapture(0)
    cap.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
    cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)

    print("Starting body tracking stream. Press 'q' to exit.")

    while cap.isOpened():
        success, frame = cap.read()
        if not success:
            continue

        # Flip horizontally for a selfie-view display
        frame = cv2.flip(frame, 1)

        # Convert OpenCV BGR frame to MediaPipe Image
        rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=rgb_frame)

        # Generate timestamp in milliseconds for VIDEO mode
        timestamp_ms = int(cv2.getTickCount() / cv2.getTickFrequency() * 1000)

        # Perform detection
        results = landmarker.detect_for_video(mp_image, timestamp_ms)

        # 5. Render landmarks manually
        if results.pose_landmarks:
            for pose_landmarks in results.pose_landmarks:
                draw_custom_skeleton(frame, pose_landmarks)

        # Display window
        cv2.imshow('Body Tracking (No mp.solutions)', frame)

        # Exit on 'q' press
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    # Resource cleanup
    landmarker.close()
    cap.release()
    cv2.destroyAllWindows()

if __name__ == "__main__":
    run_tracker()