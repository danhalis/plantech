import dotenv
from plant.plant import Plant
from security.security import Security
from geo_location.geo_location import GeoLocation
from azure.iot.device import IoTHubDeviceClient, Message
from telemetry_message import TelemetryMessage
import os
import time

class Container:
    DEFAULT_TELEMETRY_INTERVAL = 10

    def __init__(self, device_id, device_connection_string) -> None:
        self.device_id = device_id
        self.iothub_device_connection_string = device_connection_string
        self.telemetry_interval = Container.DEFAULT_TELEMETRY_INTERVAL
        self.client = self.__create_client__()
        self.plant = Plant()
        self.security = Security()
        self.geo_location = GeoLocation()
        self.__TELEMETRY_INTERVAL_KEY = "telemetry_interval"
        self.__LED_KEY = "led"
        self.__FAN_KEY = "fan"
        self.__BUZZER_KEY = "buzzer"
        self.__DOORLOCK_KEY = "door_lock"

        self.__report_current_properties()


    def __report_current_properties(self, responds_to_desired_props: bool = True):
        """
        Reports current states of actuators to device twin.

            Parameters:
                • responds_to_desired_props `bool` (default: `True`): Indicates whether to respond to desired properties.\n
                If `True`, the device will update actuators' states according to the received desired properties and report to device twin.\n
                If `False`, the device will only report the current actuators' states to device twin.
        """

        # Getting current prop values
        patch = {}
        patch[self.__TELEMETRY_INTERVAL_KEY] = self.telemetry_interval
        patch[self.__LED_KEY] = "on" if self.plant.light_on == True else "off"
        patch[self.__FAN_KEY] = "on" if self.plant.fan_on == True else "off"
        patch[self.__BUZZER_KEY] = "on" if self.security.buzzer_on == True else "off"
        patch[self.__DOORLOCK_KEY] = "locked" if self.security.door_locked == True else "unlocked"

        # If responding to desired properties
        if responds_to_desired_props:
            
            desired_props = self.client.get_twin()["desired"]

            for key in desired_props:
                if key.startswith("$"):
                    continue

                desired_value = desired_props[key]
                value_is_set = False

                if key == self.__TELEMETRY_INTERVAL_KEY:
                    try:
                        float_value = float(desired_value)
                        self.telemetry_interval = int(float_value)
                        value_is_set = True
                        desired_value = self.telemetry_interval
                    except:
                        pass

                if key == self.__LED_KEY:
                    if str(desired_value).lower() == "on":
                        self.plant.light_on = True
                        value_is_set = True
                    elif str(desired_value).lower() == "off":
                        self.plant.light_on = False
                        value_is_set = True

                if key == self.__FAN_KEY:
                    if str(desired_value).lower() == "on":
                        self.plant.fan_on = True
                        value_is_set = True
                    elif str(desired_value).lower() == "off":
                        self.plant.fan_on = False
                        value_is_set = True
                
                if key == self.__BUZZER_KEY:
                    if str(desired_value).lower() == "on":
                        self.security.buzzer_on = True
                        value_is_set = True
                    elif str(desired_value).lower() == "off":
                        self.security.buzzer_on = False
                        value_is_set = True
                
                if key == self.__DOORLOCK_KEY:
                    if str(desired_value).lower() == "unlocked":
                        self.security.door_locked = False
                        value_is_set = True
                    elif str(desired_value).lower() == "locked":
                        self.security.door_locked = True
                        value_is_set = True

                if value_is_set == True:
                    patch[key] = desired_value

        print("Patch:", patch)
        # Report back props
        self.client.patch_twin_reported_properties(patch)

    def __create_client__(self):
        
        client = IoTHubDeviceClient.create_from_connection_string(self.iothub_device_connection_string)
        
        # Define behavior for receiving twin desired property patches
        def twin_desired_properties_received_handler(desired_props):
            print("Desired props received:")
            print(desired_props)

            reported_props = client.get_twin()["reported"]

            patch = {}
            for key in desired_props:
                if key.startswith("$"):
                    continue

                if key not in reported_props or reported_props[key] != desired_props[key]:
                    desired_value = desired_props[key]
                    value_is_set = False

                    if key == self.__TELEMETRY_INTERVAL_KEY:
                        try:
                            float_value = float(desired_value)
                            self.telemetry_interval = int(float_value)
                            value_is_set = True
                            desired_value = self.telemetry_interval
                        except:
                            pass

                    if key == self.__LED_KEY:
                        if str(desired_value).lower() == "on":
                            self.plant.light_on = True
                            value_is_set = True
                        elif str(desired_value).lower() == "off":
                            self.plant.light_on = False
                            value_is_set = True

                    if key == self.__FAN_KEY:
                        if str(desired_value).lower() == "on":
                            self.plant.fan_on = True
                            value_is_set = True
                        elif str(desired_value).lower() == "off":
                            self.plant.fan_on = False
                            value_is_set = True
                    
                    if key == self.__BUZZER_KEY:
                        if str(desired_value).lower() == "on":
                            self.security.buzzer_on = True
                            value_is_set = True
                        elif str(desired_value).lower() == "off":
                            self.security.buzzer_on = False
                            value_is_set = True
                    
                    if key == self.__DOORLOCK_KEY:
                        if str(desired_value).lower() == "unlocked":
                            self.security.door_locked = False
                            value_is_set = True
                        elif str(desired_value).lower() == "locked":
                            self.security.door_locked = True
                            value_is_set = True

                    if value_is_set == True:
                        patch[key] = desired_value

            print("Patch:", patch)
            client.patch_twin_reported_properties(patch)

        try:
            client.on_twin_desired_properties_patch_received = twin_desired_properties_received_handler
        except:
            # Clean up in the event of failure
            client.shutdown()
            raise Exception("An error has occurred when setting up IoT Hub device connection.")

        return client

    def send_telemetry(self):
        message = TelemetryMessage()
       
        location = self.geo_location.location
        message.address = location if location is not None else None
        message.angles = self.geo_location.angles
        message.vibration = self.geo_location.vibration

        message.temperature = self.plant.temp_humi["temperature"]
        message.humidity = self.plant.temp_humi["humidity"]
        message.water_level = self.plant.water_level
        message.soil_moisture = self.plant.soil_moisture

        message.luminosity = self.security.luminosity
        message.motion = self.security.motion
        message.sound = self.security.sound
        message.door_is_closed = self.security.door.is_closed

        azure_iot_message = Message(message.to_str())
        # Attach device id
        azure_iot_message.custom_properties["device_id"] = self.device_id
        self.client.send_message(azure_iot_message)
        
        print(azure_iot_message)

    def run(self):
        while True:
            self.send_telemetry()
            self.__report_current_properties(responds_to_dsesired_props=False)
            time.sleep(self.telemetry_interval)

def load_env():
    fileNameToFind = '.env'

    cwd = os.path.dirname(os.path.realpath(__file__))
    rightEnvPath = cwd + "/../" + fileNameToFind
    rightEnvPath = os.path.realpath(rightEnvPath)

    envFound = dotenv.find_dotenv(fileNameToFind)

    if(envFound != rightEnvPath):
        print("Missing .env file. Please see file .env.example.")
        raise KeyError("Missing .env file. Please see file .env.example.")

    dotenv.load_dotenv(envFound)

def main():
    load_env()
    IOTHUB_DEVICE_ID = os.getenv("IOTHUB_DEVICE_ID")
    IOTHUB_DEVICE_CONNECTION_STRING = os.getenv("IOTHUB_DEVICE_CONNECTION_STRING")

    container = Container(IOTHUB_DEVICE_ID, IOTHUB_DEVICE_CONNECTION_STRING)
    container.run()

if __name__ == "__main__":
    main()
