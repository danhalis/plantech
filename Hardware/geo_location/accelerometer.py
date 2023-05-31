from urllib import response
import seeed_python_reterminal.core as rt
import seeed_python_reterminal.acceleration as rt_accel
import math
import time

class Accelerometer:
    def __init__(self) -> None:
        self._device = rt.get_acceleration_device()

    def read_angles(self):
        x = None
        y = None
        z = None

        for event in self._device.read_loop():
            accelEvent = rt_accel.AccelerationEvent(event)
        
            if str(accelEvent.name) == 'AccelerationName.X':
                x = accelEvent.value
            elif str(accelEvent.name) == 'AccelerationName.Y':
                y = accelEvent.value
            elif str(accelEvent.name) == 'AccelerationName.Z':
                z = accelEvent.value

            if x != None and y != None and z != None:
                pitch = round(180 * math.atan2(x, math.sqrt(y*y + z*z))/math.pi, 2)
                roll = round(180 * math.atan2(y, math.sqrt(x*x + z*z))/math.pi, 2) 
                
                return {'pitch': pitch, 'roll': roll}

    def read_vibration(self): 
        x = x2 = y = y2 = z = z2 = None

        for event in self._device.read_loop():
            accelEvent = rt_accel.AccelerationEvent(event)
        
            if str(accelEvent.name) == 'AccelerationName.X':
                if x == None:
                    x = accelEvent.value
                elif x2 == None:
                    x2 = accelEvent.value
            elif str(accelEvent.name) == 'AccelerationName.Y':
                if y == None:
                    y = accelEvent.value
                elif y2 == None:
                    y2 = accelEvent.value
            elif str(accelEvent.name) == 'AccelerationName.Z':
                if z == None:
                    z = accelEvent.value
                elif z2 == None:
                    z2 = accelEvent.value

            if x2 != None and y2 != None and z2 != None:
                return (x2 - x) + (y2 - y) + (z2 - z)

def main():
    accelerometer = Accelerometer()

    while True:
        print(f"ANGLES: {accelerometer.read_angles()}")
        print(f"VIBRATION: {accelerometer.read_vibration()}")
        time.sleep(1)

if __name__ == "__main__":
    main()     