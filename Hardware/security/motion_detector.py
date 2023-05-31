import time
from gpiozero import MotionSensor

class MotionDetector:
    def __init__(self, pin = 22) -> None:
        self._detector = MotionSensor(pin)

    def read_motion(self):
        return self._detector.motion_detected

def main():
    motion_detector = MotionDetector()

    while True:
        motion_detector.read_motion()
        time.sleep(1)

if __name__ == "__main__":
    main()
