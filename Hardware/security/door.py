from gpiozero import Button, AngularServo
import time

class Door:
    def __init__(self, switch_pin_number=16, servo_pin_number=12) -> None:
        self._magnetic_switch = Button(switch_pin_number)
        self.servo = AngularServo(servo_pin_number, min_angle=-90, max_angle=90, min_pulse_width=0.5/1000, max_pulse_width=2.5/1000)
        self.lock()
    
    @property
    def is_closed(self):
        return self._magnetic_switch.is_active
    
    def unlock(self):
        self.servo.angle = 90
        time.sleep(2)
        self.is_locked = False

    def lock(self):
        self.servo.angle = 0
        time.sleep(2)
        self.is_locked = True

    @property
    def is_locked(self):
        return self._is_locked
    
    @is_locked.setter
    def is_locked(self, value:bool):
        self._is_locked = value


def main():
    door = Door()

    while True:
        door.lock()
        time.sleep(5)
        door.unlock()
        time.sleep(5)



if __name__ == "__main__":
    main()