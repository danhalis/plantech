import time
from grove.adc import ADC

class SoundDetector:
    def __init__(self, channel = 4) -> None:
        self._adc = ADC()
        self._channel = channel

    def read_sound(self):
        return self._adc.read(self._channel)

def main():
    sound_detector = SoundDetector()

    while True:
        print(sound_detector.read_sound())
        time.sleep(1)

if __name__ == "__main__":
    main()
