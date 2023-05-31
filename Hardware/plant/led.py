import time
import RPi.GPIO as GPIO

class rgb_led:
	r_all=[]
	g_all=[]
	b_all=[]
	
	def __init__(self,led=1):
		GPIO.setwarnings(False)
		self.num_led=led
		self.r_all=[0] * self.num_led
		self.g_all=[0] * self.num_led
		self.b_all=[0] * self.num_led
		             
		GPIO.setmode(GPIO.BCM)  
		self.clk_pin= 24 #RX pin BCM
		self.data_pin= 25 # TX pin BCM 
		self.tv_nsec= 100
		GPIO.setup(self.clk_pin, GPIO.OUT)
		GPIO.setup(self.data_pin, GPIO.OUT)

	def sendByte(self,b):
		# print b
		for loop in range(8):
			# digitalWrite(CLKPIN, LOW);
			GPIO.output(self.clk_pin,0)
			time.sleep(self.tv_nsec/1000000.0)
			# nanosleep(&TIMCLOCKINTERVAL, NULL);
			
			# The  ic will latch a bit of data when the rising edge of the clock coming, And the data should changed after the falling edge of the clock; 
			# Copyed from P9813 datasheet
			
			if (b & 0x80) != 0:
				# digitalWrite(DATPIN, HIGH)
				GPIO.output(self.data_pin,1)
			else:
				# digitalWrite(DATPIN, LOW):
				GPIO.output(self.data_pin,0)
			
			# digitalWrite(CLKPIN, HIGH);
			GPIO.output(self.clk_pin,1)
			# nanosleep(&TIMCLOCKINTERVAL, NULL);
			time.sleep(self.tv_nsec/1000000.0)
			# //usleep(CLOCKINTERVAL);
			
			b <<= 1
			
	def sendColor(self,r, g, b):
		prefix = 0b11000000;
		if (b & 0x80) == 0:
			prefix |= 0b00100000
		if (b & 0x40) == 0:	
			prefix |= 0b00010000
		if (g & 0x80) == 0:	
			prefix |= 0b00001000
		if (g & 0x40) == 0:	
			prefix |= 0b00000100
		if (r & 0x80) == 0:	
			prefix |= 0b00000010
		if (r & 0x40) == 0:	
			prefix |= 0b00000001
	
		self.sendByte(prefix)
		self.sendByte(b)
		self.sendByte(g)
		self.sendByte(r)
		
	def setColorRGB(self,r,g,b):
		for i in range(4):
			self.sendByte(0)
		
		self.sendColor(r, g, b);
		
		for i in range(4):
			self.sendByte(0)
		
	def setColorRGBs(self,r,g,b,count):
		for i in range(4):
				self.sendByte(0)
		for i in range(count):
			self.sendColor(r[i], g[i], b[i])
		for i in range(4):
				self.sendByte(0)
	
	def setOneLED(self,r,g,b,led_num):
		self.r_all[led_num]=r
		self.g_all[led_num]=g
		self.b_all[led_num]=b
		
		self.setColorRGBs(self.r_all,self.g_all,self.b_all,self.num_led)


class Led:
    def __init__(self, num_led = 2) -> None:
        self.num_led = num_led
        self.led = rgb_led(num_led)
        self.off_all()

        
    def set_all(self, r=255, g=255, b=255):
        r = [r] * self.num_led
        g = [g] * self.num_led
        b = [b] * self.num_led
        self.led.setColorRGBs(r=r, g=g, b=b, count=self.num_led)
        self._is_on = True

    def off_all(self):
        r = [0] * self.num_led
        g = [0] * self.num_led
        b = [0] * self.num_led
        self.led.setColorRGBs(r=r, g=g, b=b, count=self.num_led)
        self._is_on = False

    @property
    def is_on(self):
        return self._is_on



if __name__ == "__main__":	
    led = Led()
    