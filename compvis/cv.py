import os
import urllib.request
from pathlib import Path
import cv2
import time
import mediapipe as mp
from mediapipe.tasks import python
from mediapipe.tasks.python import vision

# 1. Dynamically resolve paths and download the model if missing
SCRIPT_DIR = Path(__file__).resolve().parent
MODEL_PATH = SCRIPT_DIR / "blaze_face_short_range.tflite"
MODEL_URL = "https://storage.googleapis.com/mediapipe-models/face_detector/blaze_face_short_range/float16/latest/blaze_face_short_range.tflite"

if not MODEL_PATH.exists():
    print(f"Downloading model to {MODEL_PATH}...")
    urllib.request.urlretrieve(MODEL_URL, MODEL_PATH)
    print("Download complete.")

latest_result = None;
    
def update_last_result(result, output_image, timestamp_ms):
    global last_result 
    last_result = result

def run_tracker():
    # Setup Face Detector
    BaseOptions = mp.tasks.BaseOptions
    FaceDetector = mp.tasks.vision.FaceDetector
    FaceDetectorOptions = mp.tasks.vision.FaceDetectorOptions
    VisionRunningMode = mp.tasks.vision.RunningMode
     
    options = FaceDetectorOptions(
        base_options = BaseOptions(model_asset_path = str(MODEL_PATH)),
        running_mode = VisionRunningMode.LIVE_STREAM,
        min_detection_confidence = .5,
        result_callback = update_last_result
    )

    # Setup Webcam
    cap = cv2.VideoCapture(0)
    cap.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
    cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)

    with FaceDetector.create_from_options(options) as detector:
        while cap.isOpened():
            success, frame = cap.read()

            if not success:
                continue

            # Flip horizontall for Selfie display
            frame = cv2.flip(frame, 1)

        # Convert to openCV's BGR grame to Mediapipe image
        
            rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data = rgb_frame)

            timestamp_ms = int(time.time() * 1000)
            detector.detect_async(mp_image, timestamp_ms)

            if latest_result and latest_result.detections:
                for detection in latest_result.detections:
                    bbox = detection.bounding_box

                    # Calculate center coords
                    center_x = int(bbox.origin_x + bbox.width/2)
                    center_y = int(bbox.origin_y + bbox.height/2)

                    # Calculate the radius for the face
                    radius = int(max(bbox.width, bbox.height) / 2)

                    # Draw the circle
                    cv2.circle(frame, (center_x, center_y), radius, (0, 255, 0), 2)

            cv2.imshow("Face", frame)

            if(cv2.waitKey(1) & 0xFF == ord('q')):
                break

    cap.release()
    cv2.destroyAllWindows()

   



if __name__ == "__main__":
    run_tracker()