import argparse
from geo_location.gps_sensor import GPSSensor
from geo_location.accelerometer import Accelerometer
import seeed_python_reterminal.core as rt

class GeoLocation:
    """The Geo-Location subsystem is responsible for monitoring the transportation and placement of the container."""

    def __init__(self) -> None:
        self._gps_sensor = GPSSensor()
        self._accelerometer = Accelerometer()
        self._reTerninal = rt

    @property
    def gps_data(self):
        return self._gps_sensor.read_gps_data()

    @property
    def coordinates(self):
        return self._gps_sensor.get_coordinates()

    @property
    def location(self):
        return self._gps_sensor.get_location()

    @property
    def angles(self) -> dict:
        return self._accelerometer.read_angles()

    @property
    def vibration(self) -> float:
        return self._accelerometer.read_vibration()

    @property
    def buzzer_on(self) -> bool:
        return self._reTerninal.buzzer

    @buzzer_on.setter
    def buzzer_on(self, value: bool):
        self._reTerninal.buzzer = value

def main():
    # Parse arguments
    parser = argparse.ArgumentParser()
    parser.add_argument("--gps", action='store_true')
    parser.add_argument("--angles", action='store_true')
    parser.add_argument("--vibration", action='store_true')
    parser.add_argument("--buzzer", action='store_true')
    parser.add_argument("--set_buzzer", type=str)
    args = parser.parse_args()

    geo_location = GeoLocation()

    # Call the appropriate method(s) based on passed arguments
    if args.gps:
        coordinates = geo_location.coordinates

        if coordinates is None:
            print("Cannot read coordinates. Please relocate the GPS sensor.")
        else:
            latitude_str = f"{coordinates.latitude:.2f}°{coordinates.latitude_minutes:.2f}′{coordinates.latitude_seconds:.2f}″{coordinates.lat_dir}"
            longitude_str = f"{coordinates.longitude:.2f}°{coordinates.longitude_minutes:.2f}′{coordinates.longitude_seconds:.2f}″{coordinates.lon_dir}"
            print(f"COORDINATES: {latitude_str},{longitude_str}")
            print(f"ADDRESS: {geo_location.location.address}")
    if args.angles:
        print(f"ANGLES: {geo_location.angles}")
    if args.vibration:
        print(f"VIBRATION: {geo_location.vibration}")
    if args.set_buzzer == "on":
        print(f"Turning buzzer on...")
        geo_location.buzzer_on = True
    elif args.set_buzzer == "off":
        print(f"Turning buzzer off...")
        geo_location.buzzer_on = False
    if args.buzzer:
        print(f"BUZZER STATE: {geo_location.buzzer_on}")

if __name__ == "__main__":
    main()
