from gpiozero import OutputDevice
import time

class Fan:
    def __init__(self, pin = 5) -> None:
        self.fan = OutputDevice(pin)
        self._is_on = False
        self.fan.off()

    def on(self):
        print('Turning fan on...')
        self.fan.on()
        self._is_on = True

    def off(self):
        print('Turning fan off...')
        self.fan.off()
        self._is_on = False

    @property
    def state(self):
        return self._is_on

    @state.setter
    def state(self, value: bool):
        if value == True:
            self.on()
        elif value == False:
            self.off()

def main():
    fan = Fan()

    while True:
        fan.on()
        print(fan.state)
        time.sleep(3)
        fan.off()
        print(fan.state)
        time.sleep(3)

if __name__ == "__main__":
    main()
