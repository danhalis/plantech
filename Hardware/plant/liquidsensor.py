from grove.adc import ADC
import time

class LiquidSensor:
    DEFAUL_CHANNEL = 0

    def __init__(self, bus=DEFAUL_CHANNEL) -> None:
        self.adc = ADC()            
        self.channel = bus      

    def get_level(self):
        water_level = self.adc.read(self.channel) 
        return water_level


def main():
    water_sensor = LiquidSensor()

    while True:
        print(water_sensor.get_level())
        time.sleep(3)

if __name__ == "__main__":
    main()