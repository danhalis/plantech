import argparse
import seeed_python_reterminal.core as rt
from security.door import Door
from security.motion_detector import MotionDetector
from security.sound_detector import SoundDetector

class Security:
    """The security subsystem is responsible for monitoring access and safety of the container."""

    def __init__(self) -> None:
        self._reTerninal = rt
        self._motion_detector = MotionDetector()
        self._sound_detector = SoundDetector()
        self._door = Door()

    @property
    def buzzer_on(self) -> bool:
        return self._reTerninal.buzzer

    @buzzer_on.setter
    def buzzer_on(self, value: bool):
        self._reTerninal.buzzer = value

    @property
    def luminosity(self) -> int:
        return self._reTerninal.illuminance

    @property
    def sound(self) -> int:
        return self._sound_detector.read_sound()

    @property
    def motion(self) -> bool:
        return self._motion_detector.read_motion()

    @property
    def door(self):
        return self._door
    
    @property
    def door_locked(self) -> bool:
        return self._door.is_locked
    
    @door_locked.setter
    def door_locked(self, lock:bool):
        if lock:
            self._door.lock()
            
        elif not lock:
            self._door.unlock()

def main():
    # Parse arguments
    parser = argparse.ArgumentParser()
    parser.add_argument("--noise", action='store_true')
    parser.add_argument("--lumi", action='store_true')
    parser.add_argument("--motion", action='store_true')
    parser.add_argument("--lock", action='store_true')
    parser.add_argument("--set_lock", type=str)
    parser.add_argument("--buzzer", action='store_true')
    parser.add_argument("--set_buzzer", type=str)
    parser.add_argument("--door", action='store_true')
    args = parser.parse_args()
   
    security = Security()

    # Call the appropriate method(s) based on passed arguments
    if args.noise:
        print(f"NOISE LEVEL: {security.sound}")
    if args.motion:
        print(f"MOTION STATE: {security.motion}")
    if args.lumi:
        print(f"LUMINOSITY: {security.luminosity}")
    if args.set_buzzer == "on":
        print(f"Turning buzzer on...")
        security.buzzer_on = True
    elif args.set_buzzer == "off":
        print(f"Turning buzzer off...")
        security.buzzer_on = False
    if args.buzzer:
        print(f"BUZZER STATE: {security.buzzer_on}")
    if args.door:
        door_state_str = "CLOSED" if security.door.is_closed else "OPENED"
        print(f"DOOR STATE: {door_state_str}")
    if args.set_lock == 'closed':
        print(f"Locking door...")
        security.door_locked = True
    elif args.set_lock == 'open':
        print(f"Unlocking door...")
        security.door_locked = False
    if args.lock:
        door_lock_state = "CLOSED" if security.door_locked else "OPEN"
        print(f"DOOR LOCK: {door_lock_state}")


if __name__ == "__main__":
    main()