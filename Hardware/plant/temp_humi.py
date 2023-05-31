import time
from grove.grove_temperature_humidity_aht20 import GroveTemperatureHumidityAHT20

class TempHumi:

    def __init__(self, bus = 4) -> None:
        self._ath20 = GroveTemperatureHumidityAHT20(bus=bus)

    def read(self):
        temp_humi = self._ath20.read()
        return {'temperature': temp_humi[0], 'humidity': temp_humi[1]}

def main():
    temp_humi = TempHumi()

    while True:
        print(temp_humi.read())
        time.sleep(3)

if __name__ == "__main__":
    main()
