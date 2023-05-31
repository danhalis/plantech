from grove.adc import ADC
import smbus2 as smbus
import time

class MoistureSensor:
    DEFAUL_CHANNEL = 2

    def __init__(self, bus=DEFAUL_CHANNEL) -> None:
        self.adc = ADC()
        self.channel = bus      


    def read(self):
        moisture_level = self.adc.read(self.channel) 
        return moisture_level


def main():
    moisture_sensor = MoistureSensor()

    while True:
        print(moisture_sensor.read())
        time.sleep(3)

if __name__ == "__main__":
    main()