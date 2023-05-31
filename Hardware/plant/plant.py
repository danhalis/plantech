from plant.temp_humi import TempHumi
from plant.fan import Fan
from plant.liquidsensor import LiquidSensor
from plant.moisturesensor import MoistureSensor
from plant.led import Led
import argparse

class Plant:
    def __init__(self) -> None:
        self._temp_humi = TempHumi()
        self._fan = Fan()
        self._water_sensor = LiquidSensor()
        self._moisture_sensor = MoistureSensor()
        self._light = Led()
        

    @property
    def fan_on(self) -> bool:
        return self._fan.state

    @fan_on.setter
    def fan_on(self, value: bool):
        self._fan.state = value

    @property
    def temp_humi(self) -> dict:
        return self._temp_humi.read()

    @property
    def water_level(self) -> int:
        return self._water_sensor.get_level()

    @property
    def soil_moisture(self) -> int:
        return self._moisture_sensor.read()

    @property
    def light_on(self) -> bool:
        return self._light.is_on
    
    @light_on.setter
    def light_on(self, value:bool):
        if value == True:
            self._light.set_all()
        else:
            self._light.off_all()

def main():
    # Parse arguments
    parser = argparse.ArgumentParser()
    parser.add_argument("--temp_humi", action='store_true')
    parser.add_argument("--water", action='store_true')
    parser.add_argument("--moisture", action='store_true')
    parser.add_argument("--fan", action='store_true')
    parser.add_argument("--set_fan", type=str)
    parser.add_argument("--light", action='store_true')
    parser.add_argument("--set_light", type=str)
    args = parser.parse_args()

    # Create new Plant instance
    plant = Plant()

    # Call the appropriate method(s) based on passed arguments
    if args.temp_humi:
        print(f"TEMPERATURE & HUMIDITY: {plant.temp_humi}")

    if args.fan:
        print(f"FAN STATE: {plant.fan}")
    elif args.set_fan:
        print("Setting fan state...")
        if args.set_fan.lower() == 'true' or args.set_fan.lower() == 'on':
            plant.fan_on = True
        elif args.set_fan.lower() == 'false' or args.set_fan.lower() == 'off':
            plant.fan_on = False

    if args.water:
        water_level = plant.water_level
        print("WATER LEVEL: " + str(water_level))
    
    if args.moisture:
        soil_moisture = plant.soil_moisture
        print("SOIL MOISTURE: " + str(soil_moisture))
    
    if args.set_light == "on":
        print("Turning on lights...")
        plant.light_on = True
    elif args.set_light == 'off':
        print("Turning off lights...")
        plant.light_on = False
    
    if args.light:
        print({"LIGHT ON": plant.light_on})

if __name__ == "__main__":
    main()