from tkinter.messagebox import NO
from serial import Serial
import pynmea2 # Python library for the NMEA 0183 protocol. Source: https://github.com/Knio/pynmea2
from geopy.geocoders import Nominatim # Python geocoder class for OpenStreetMap Nominatim geocoding web service. Source: https://github.com/geopy/geopy
from geopy.location import Location # Object that contains a parsed geocoder response. Source: https://github.com/geopy/geopy

class GPSSensor:
    def __init__(self) -> None:
        self.connect()
        self._geo_locator = Nominatim(user_agent="PlanTech")

    def read_gps_data(self) -> pynmea2.NMEASentence:
        """
        Reads the GPS data of the device from a GPS sensor on Grove UART port.
            Returns:
                nmea_sentence (NMEASentence): A `NMEASentence` object that contains properties like `latitude`, `longitude`, etc.\n
                or `None` if no data is read (possibly due to GPS connectivity).
        """

        line = ""
        data = None

        while True:
            try:
                line = self._serial.readline().decode('utf-8').rstrip()
                
                if len(line.strip()) == 0:
                    return None

                data = pynmea2.parse(line)
                break
        
            # There's a random chance the first byte being read is part way through a character.
            # Read another full line and continue.
            except UnicodeDecodeError:
                continue
            except pynmea2.nmea.ParseError:
                continue
        
        return data
    
    def get_coordinates(self) -> pynmea2.NMEASentence:
        """
        Gets the geographical coordinates of the device read by a GPS sensor on Grove UART port.
            Returns:
                nmea_sentence (NMEASentence): A `NMEASentence` object that contains properties like `latitude`, `longitude`, etc.\n
                or `None` if no data is read (possibly due to GPS connectivity).

            Example:
            >>> coordinates = geo_location.get_coordinates()
            >>> print(f"{coordinates.latitude}°{coordinates.latitude_minutes}′{coordinates.latitude_seconds}″{coordinates.lat_dir}")
            >>> print(f"{coordinates.longitude}°{coordinates.longitude_minutes}′{coordinates.longitude_seconds}″{coordinates.lon_dir}")
        """

        data = self.read_gps_data()

        if data is None:
            return None

        while data.identifier() != "GNGLL,":
            data = self.read_gps_data()

        return data

    def get_location(self) -> Location:
        """
        Gets the location of the device read by a GPS sensor on Grove UART port.
            Returns:
                location (Location): A `Location` object that contains properties:\n
                • `address`: the human-readable address of the location\n
                • `latitude`: the latitude of the location\n
                • `longitude`: the longitude of the location\n
                • `raw`: the dictionary of the geocoder's response for the location\n

                or `None` if no data is read (possibly due to GPS connectivity). 
            Example:
            >>> location = geo_location.get_location()
            >>> print(location.address)
        """

        coordinates = self.get_coordinates()

        if coordinates is None:
            return None

        return self._geo_locator.reverse(f"{coordinates.latitude},{coordinates.longitude}")

    def __clear_gps_serial_connection__(self):
        self._serial.reset_input_buffer()
        self._serial.flush()

    def connect(self):
        """Opens GPS sensor connection."""

        # Open GPS sensor connection on Grove UART port
        self._serial = Serial('/dev/ttyAMA0', baudrate=9600, timeout=1)
        self.__clear_gps_serial_connection__()

    def disconnect(self):
        """Closes GPS sensor connection."""

        # Close GPS sensor connection
        self.__clear_gps_serial_connection__()
        self._serial.close()    