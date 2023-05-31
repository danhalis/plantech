
class TelemetryMessage:

    def __init__(self) -> None:
        self._geo_location = {
            "address": None,
            "angles": {
                "pitch": None,
                "roll": None
            },
            "vibration": None
        }
        self._security = {
            "luminosity": None,
            "motion": None,
            "sound": None,
            "door_is_closed": None
        }
        self._plant = {
            "water_level": None,
            "soil_moisture": None,
            "humidity": None,
            "temperature": None
        }

    def __str__(self) -> str:
        return f"{{'geo_location': {self._geo_location}, 'security': {self._security}, 'plant': {self._plant}}}"

    def to_str(self) -> str:
        return self.__str__()

    @property
    def address(self):
        return self._geo_location["address"]

    @address.setter
    def address(self, value):
        self._geo_location["address"] = value

    @property
    def angles(self):
        return self._geo_location["angles"]
    
    @angles.setter
    def angles(self, value):
        self._geo_location["angles"]["pitch"] = value["pitch"]
        self._geo_location["angles"]["roll"] = value["roll"]

    @property 
    def vibration(self):
        return self.geo_location["vibration"]
    
    @vibration.setter
    def vibration(self, value):
        self._geo_location["vibration"] = value
    
    @property
    def luminosity(self):
        return self._geo_location["luminosity"]

    @luminosity.setter
    def luminosity(self, value):
        self._security["luminosity"] = value

    @property 
    def motion(self):
        return self._security["motion"]
    
    @motion.setter
    def motion(self, value):
        self._security["motion"] = value

    @property
    def sound(self):
        return self._security["sound"]

    @sound.setter
    def sound(self, value):
        self._security["sound"] = value
    
    @property
    def door_is_closed(self):
        return self._security["door_is_closed"]

    @door_is_closed.setter
    def door_is_closed(self, value):
        self._security["door_is_closed"] = value

    @property
    def water_level(self):
        return self._plant["water_level"]
    
    @water_level.setter
    def water_level(self, value):
        self._plant["water_level"] = value
    
    @property 
    def soil_moisture(self):
        return self._plant["soil_moisture"]
    
    @soil_moisture.setter
    def soil_moisture(self, value):
        self._plant["soil_moisture"] = value

    @property
    def humidity(self):
        return self._plant["humidity"]
    
    @humidity.setter
    def humidity(self, value):
        self._plant["humidity"] = round(value, 2)
    
    @property
    def temperature(self):
        return self._plant["temperature"]

    @temperature.setter
    def temperature(self, value):
        self._plant["temperature"] = round(value, 2)