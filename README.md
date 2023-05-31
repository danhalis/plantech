# PlanTech: Design Document

## Documentation
https://docs.google.com/document/d/1j6F9BgPlJrNpdfWOaX0L7nxYUMMyMZjASFxbAsi426Q/edit?usp=sharing

## Functional Overview

PlantTech is a mobile application designed to automate and fully control a fleet of container farms through IoT technologies. It will help to facilitate the jobs of farm technicians by allowing them to specifically adjust the conditions of individual plants, as well as fleet managers who have the difficult task of ensuring that all containers are properly installed and kept secure. In doing this, PlanTech will provide all the tools necessary to make sure that every plant, no matter their needs, is grown to their full potential.

## Design Overview

*   Login page:

    If the user is not identified yet, the app will prompt the user's authentication. This can be done by having a log-in system if time permits. Otherwise, the app will only ask the user for their job, Fleet Manager or Farm Technician. From there, the app can render the corresponding views for that user.

![image](https://user-images.githubusercontent.com/60619445/164607585-04ee479e-adce-438d-a2f8-7a0126788102.png)

*   Side/Burger Menu:

    This is a general navigation component that provides access to Container List page, Containers Map (future work), temperature comparison graph page and log out button.

![image](https://user-images.githubusercontent.com/60619445/164607519-703b1fdb-59c2-48e7-a142-4add88e570d6.png)

*   Container List page (Homepage):

    The homepage of the app is the Container List page. This page is accessible via the burger menu. Each list item displays the name and the location of the container. The app can prioritize the containers that are in bad states. These bad states are determined by default thresholds. As a future work, the user can define their preferred thresholds for different container’s properties. The user can choose a container item to access its dashboard page.

    *   Fleet manager’s view: this page displays all the containers including rented and available ones. The fleet manager can also view the rental state (Rented or Available) and security state (Alarming or Normal) of the container. Alarming state involves bad pitch/roll/banking angles, high level of vibration or events like when the buzzer is triggered. In the event of an alarming state, the fleet manager gets a notification. Containers are ordered by security state, then by rental state, then by name.

    *   Farm technician’s view: this page displays only the containers where they work. The farm technician can view the farming environment state of the container (Warning or Normal) Warning state involves harmful levels of temperature, humidity, water or soil moisture. In the event of an warningstate, the farm manager gets a notification. Containers are ordered by farming environment state, then by name.

| ![image](https://user-images.githubusercontent.com/60619445/164607642-abab541b-a3ce-4071-aa2f-d021a09360fa.png) | ![image](https://user-images.githubusercontent.com/60619445/164607683-f084a663-5c5d-49ab-863c-257bf1c97d90.png) |
|--|--|
| Fleet Manager’s view | Farm Technician’s view |

*   Container Dashboard page:

    This page displays the name, the location of the container and overall properties of the chosen container in different tiles. From here, the user can directly control some hardware components. As for the data that can be represented in graphs, the user can tap on one of the tiles to navigate to the corresponding graph page.

    *   Fleet manager’s view: this page shows pitch/roll/banking angles, vibration, door lock state, buzzer state of the container and noise level, luminosity, motion in the container. The fleet manager can also toggles door lock and buzzer directly on the security tile.
    *   Farm technician’s view: this page shows temperature, humidity, water level, soil moisture and fan state in the container. The technician can also turn on/off the fan directly on the fan tile.

| ![image](https://user-images.githubusercontent.com/60619445/164607734-72683167-1a93-4e7e-8226-3aab1078b0ad.png) | ![image](https://user-images.githubusercontent.com/60619445/164607791-6a73c5ef-9b51-44ae-8fc6-d6246962fab3.png) |
|--|--|
| Fleet Manager’s view | Farm Technician’s view |

*   Graph pages:

    Each type of data that can be represented in a graph has its own graph page with the corresponding title. On the temperature and humidity graph page in particular, the user can also control the fan.

    All the data-comparing graphs are accessible via the burger menu. For now, the app only provides temperatures of different containers in different locations.

| ![image](https://user-images.githubusercontent.com/60619445/164607840-47a8d9a5-9255-45e1-8d0e-0b97c2ffca0a.png) | ![image](https://user-images.githubusercontent.com/60619445/164607886-a8855f3e-a844-413f-ad84-da2e29cb44f5.png) |
|--|--|
| Temperature of a single container | Temperatures of different containers |

## Screen Analysis & UI Wireframing
Link to draw.io: https://drive.google.com/file/d/11EZhlpKpvT_HOAE53KSHgyF1KEp_aliK/view?usp=sharing
![plantech drawio](https://user-images.githubusercontent.com/60619445/164609727-2a3f38ec-6b79-43b0-8083-c74568e396f5.png)

## Prioritization of Features

*   Must develop

	*   Fleet manager specific dashboard to display all relevant farming operations
	*   Farm technician specific dashboard to display all relevant farming operations
	*   Ability to read GPS location, luminosity, motion level, temperature, humidity, moisture, water level, vibration, sound level, fan status, door status, buzzer status, and pitch, roll, and banking angles for each container
	*   Ability to control the fan status, door status, and buzzer status for each container
	*   An admin settings page where the user can set threshold values for each sensor
	*   Alert and notification system

*   Would like to develop
	*   Sharing data
	*   A graphical representation of each of the sensor values to better demonstrate to the user whether the sensor is reading a value that is safe, concerning, or dangerous
	*   An alarm system where the farm technician can activate/deactivate the alarm, and the fleet manager will only be sent an alert/notification if the alarm is activated
	*   The ability to filter and search the container list

*   Could develop if time permits

	*   Save the users role the first time that they use to application so that they don’t have to enter each time whether they are a fleet manager or a farm technician
	*   Save the users last page to be able to open the app where they left off the next time that they use it
	*   Integration of a map to visually demonstrate the location of each deployed container farm

*   Likely would not develop because of lack of time or knowledge

	*   3D model of each container to be able to graphically demonstrate the containers current pitch, roll, and banking angles
	*   Display the current time according to the location of each container

## User Stories

1.  As the farm technician, I want to know the environmental conditions inside the container in near real-time so that I can make necessary adjustments.

	a.  As a farm technician, I can read the measurement of the temperature and humidity in the container so that I know if the temperature or the humidity is too high or too low and from there, perform necessary adjustments.

	*Acceptance criteria:*

	*   The temperature and humidity is measured by Grove’s AHT20 sensors

	b.  As a farm technician, I can read the measurement of the relative water level so that I know if the plants are submerged by the right amount of water and from there, perform necessary adjustments.

	*Acceptance criteria:*

	*   The water level is read is measured by Grove’s Water Level Sensor

	c.  As a farm technician, I can read the measurement of the soil moisture so that I know if the plants need more or less watering and from there, perform necessary adjustments.

	*Acceptance criteria:*

	*   The soil moisture is measured by Grove’s Capacitive Soil Moisture Sensor

	d.  As a farm technician, I can read the fan state (on/off) so that I can perform actions accordingly.

	*Acceptance criteria:*

	*   The state of the 5V cooling fan is read by reading the state of the relay that controls the fan.

2.  As the farm technician, I need to be able to control the environmental conditions inside the container to make sure that plants are healthy.

	a.  As a farm technician, I can turn on/off the fan so that I can better control the temperature inside the container.

	*Acceptance criteria:*

	*   5V cooling fan will be turned on/off using a relay.

3.  As a farm technician, I want to have access to a mobile application that provides me a user interface that answers my needs so that I can do my job more efficiently

	a.  As a farm technician, I would like to see a list of containers I operate so that I can easily have access to the environment information of each container and help me do a better job.

	*Acceptance criteria:*

	*   The mobile app must have a screen giving the options to select a container from a list of containers the farm technician operates.

	*   Selecting a container must navigate to a screen showing relevant information about the environment in each container.

	b.  As a farm technician, I would like to have access to a dashboard with environmental information of the chosen container so that it can help me do a better job.

	*Acceptance criteria:*

	*   The dashboard should show information about the fan state, soil moisture information, water level info, and temperature & humidity information.
	*   The dashboard should also allow the farm technician to control the fan state (on/off).
	*   There must be a button/switch to turn the fan on and off.
	
	c.  As a farm technician, I would like to be able to receive a notification when a container environment is in an warning state so that I can be informed right away of any issue and provide better environmental care for the plants.
	
	*Acceptance criteria:*
	
	*   The farm technician responsible for the container in question should receive a notification when the container environment is in a state of warning.
	*   State of warning include any sensor measurement that is considered harmful.
	*   More specifically, a warning state involves harmful levels of temperature, humidity, water or soil moisture.
	*   The farm technician gets notified when the container has a harmful level of temperature.
	*   The farm technician gets notified when the container has a harmful level of humidity.
	*   The farm technician gets notified when the container has a harmful level of water.
	*   The farm technician gets notified when the container has a hamrful level of soil moisture.
	
	d.  As a farm technician, I would like to be able to share the dashbord information by email or message so that I can easily get access to important information outside of the application.
	
	*Acceptance criteria*
	
	*   There should be a sharing button available on the dashboard page.
	*   Sharing should be done using a sharing bottom sheet, ideally using the operating system's built in sharing method.
	*   The sensor data available in the dashboard for farm technician at the current moment should all be shared using a json file.

	e.  As a farm technician, I would like to be able to access a page where I can view a graph of the container temperature and humidity levels, so that I can visually compare the container temperature and humidity levels over time.
	
	*Acceptance criteria*
	
	*   Clicking on the temperature and humidity tile in the farm technician dashboard will navigate the user to the charts page, where the charts will be populated with the container temperature and humidity data

	f.  As a farm technician, I would like to be able to access a page where I can view a graph of the container water levels, so that I can visually compare the container water levels over time.
	
	*Acceptance criteria*
	
	*   Clicking on the water level tile in the farm technician dashboard will navigate the user to the charts page, where the charts will be populated with the container water level data

	g.  As a farm technician, I would like to be able to access a page where I can view a graph of the container soil moisture, so that I can visually compare the container moisture levels over time.
	
	*Acceptance criteria*
	
	*   Clicking on the soil moisture tile in the farm technician dashboard will navigate the user to the charts page, where the charts will be populated with the soil moisture level data.

4.  As the fleet manager, I want to know the location and placement of the container farms so that I can track company assets.

	a.  As a fleet manager, I want to be able to read the GPS location of each container, so that I can monitor the location of all deployed container farms remotely.

	*Acceptance criteria:*

	*   The GPS location of the container farm will be read using the Air530 Grove GPS sensor.
	*   The location can be displayed in a user-friendly manner (human readable address)

	b.  As a fleet manager, I want to be able to read the pitch, roll, and banking angles of each container, so that I can ensure that the deployed container farms are installed properly from a remote location.

	*Acceptance criteria:*

	*   The pitch, roll, and banking angles will be read by the reTerminals STMicroelectronics LIS3DHTR 3-axis accelerometer.

	c.  As a fleet manager, I want to be able to read the vibration levels of each container, so that I can see if a container is ever in a potentially unstable environment and at risk of being damaged.

	*Acceptance criteria:*

	*   The vibration levels will be read by the reTerminals STMicroelectronics LIS3DHTR 3-axis accelerometer.

	d.  As a fleet manager, I want to be able to read if the buzzer is on or off, so that I can be informed if any deployed container is in a location or position that is not optimal or if there are any security risks.

	*Acceptance criteria:*

	*   The state of the buzzer will be read using the reTerminals built-in buzzer by reading the value stored in the usr\_buzzer file.

	e.  As a fleet manager, I want to be able to control the buzzer (on or off), so that I can warn all farm technicians working at a given container farm if the location or position of their container is not optimal, or to turn off the buzzer if it was wrongly activated.

	*Acceptance criteria:*

	*   The state of the buzzer will be controlled using the reTerminals built-in buzzer by setting the value stored in the usr\_buzzer file.
	*   The buzzer will be activated if the vibration level meets a set threshold.
	*   The buzzer will be activated if the pitch, roll, or bank level meets a set threshold.

	f.  As a fleet manager, I want  to be able to visually see the location of all deployed containers, so that I can get a better idea of where all the containers are exactly, as well as where they are relative to other containers

	*Acceptance criteria:*

	*   The GPS location of the container farm will be read using the Air530 Grove GPS sensor.
	*   This map should ideally be available from almost anywhere throughout the application. (The side hamburger menu would be a good place to put it)

	*   A map interface will be integrated into the application and a pin will be added at the location of each container

5.  As the fleet manager, I want to be informed of security issues inside the container farm so that I can mobilize an appropriate response.

	a.  As a fleet manager, I want to be able to monitor the noise level in the container so that it helps me make a better assumption of any intruders in the container.

	*Acceptance criteria:*

	*   Noise detection should be done using a compatible noise detector sensor.

	b.  As a fleet manager, I want to be able to monitor the luminosity level in the container so that I can know if the lights are on or off, and therefore make a better assumption of any intruders in the container.

	*Acceptance criteria:*

	*   Luminosity level should be read using the reTermnial’s built-in sensor.

	c.  As a fleet manager, I want to be able to detect motion in the container so that I can provide better security and know if there’s any intruder in the container.

	*Acceptance criteria:*

	*   Motion detection should be done using the Adjustable PIR Motion Sensor.

	d.  As a fleet manager, I want to be able to know whether the container door is locked or unlocked so that I can provide better security.

	*Acceptance criteria:*

	*   The container door-lock state should be read from the Micro Servo.

	e.  As a fleet manager, I want to be able to lock and unlock the container door so that I can prevent any security issues.

	*Acceptance criteria:*

	*   The container door-lock state should be modified using the Micro Servo.

	f.  As a fleet manager, I want to be able to turn the buzzer on and off so that I can provide better security

	*Acceptance criteria:*

	*   The state of the buzzer will be controlled using the reTerminals built-in buzzer by setting the value stored in the usr\_buzzer file.
	*   Once the noise level is higher than an acceptable threshold, the buzzer alarm should turn on.
	*   Once the luminosity level is higher than an acceptable threshold, the buzzer alarm should turn on.
	*   Once the motion level is higher than an acceptable threshold, the buzzer alarm should turn on.
	*   Once the container door-lock state is changed, turn on the buzzer.

	g. As a fleet manager, I want to read the door state (opened/closed) so that I can perform any necessary actions.

	*Acceptance criteria:*

	*   The door state is read by reading the magnetic switch used by the door.

6.  As a fleet manager, I want to have access to a mobile application that provides me a user interface that answers my needs so that I can do my job more efficiently

	a.  As a fleet manager, I would like to see a list of containers I operate and a dashboard for each container so that I can easily have access to the container information and help me in locating the container & providing efficient security.

	*Acceptance criteria:*

	*   The mobile app must have a screen giving the options to select a container from a list of containers the fleet manager operates.

	*   Selecting a container must navigate to a screen showing relevant information about each container.

	b.  As a fleet manager, I would like to have access to a dashboard with container information so that it can help me do a better job and more efficient job.

	*Acceptance criteria:*

	*   The dashboard must display information about the buzzer’s state, the door-lock state, motion information, luminosity level, noise level, vibration level, the GPS location of the container and the pitch, roll, and banking angles of the container.
	*   The dashboard must also allow the fleet manager to control the buzzer’s state (on/off) & the door-lock state (locked/unlocked).
	*   The noise information should be displayed with some sort of indicator to know the noise level. (low, medium, or high).
	*   The luminosity information should be displayed with some sort of indicator to know the luminosity level. (low, medium, or high).
	*   The motion information should be displayed with some sort of indicator to know whether the motion is low, medium or high.
	*   There must be a button/switch to turn the buzzer on and off.
	*   There must be a button/switch to lock & unlock the door.

	c.  As a fleet manager, I would like to be able to visually see the data that is being captured by the container sensors through the use of charts, so that I can get a better understanding of the container conditions over time

	*Acceptance criteria:*

	*   There will be a page dedicated to displaying charts that represent the data that is being captured by the sensors
	*   Charts will be implemented using Microcharts.Forms

	d.  As a fleet manager, I would like to be able to visually compare the sensor data of different containers, so that I can get a better idea of which containers have conditions that aren’t the norm

	*Acceptance criteria:*

	*   There will be a page dedicated to displaying charts that represent the comparison of the data that is being captured by the sensors of different containers
	*   Charts will be implemented using Microcharts.Forms
	
	e.  As a fleet manager, I would like to be able to receive a notification when a container is in an alert state so that I can be informed right away of any issue and provide security.
	
	*Acceptance criteria:*
	
	*   The fleet manager responsible for the container in question should receive a notification when the container is in a state of alert.
	*   State of alert include any sensor measurement that is considered invalid.
	*   More specifically, an alert state involves bad pitch/roll/banking angles, high level of vibration or events like when the buzzer is triggered.
	*   The fleet manager gets notified when the container has a bad pitch.
	*   The fleet manager gets notified when the container has a bad roll.
	*   The fleet manager gets notified when the container has a bad banking angle.
	*   The fleet manager gets notified when the container has a high level of vibration.
	*   The fleet manager gets notified when the buzzer is triggered.
	
	f.  As a fleet manager, I would like to be able to share the dashbord information by email or message so that I can easily get access to important information outside of the application.
	
	*Acceptance criteria*
	
	*   There should be a sharing button available on the dashboard page.
	*   Sharing should be done using a sharing bottom sheet, ideally using the operating system's built in sharing method.
	*   The sensor data available in the dashboard for fleet manager at the current moment should all be shared using a json file.

	g.  As a fleet manager, I would like to be able to access a page where I can view a graph of the container angles, so that I can visually compare the container angles over time.
	
	*Acceptance criteria*
	
	*   Clicking on the angles tile in the fleet manager dashboard will navigate the user to the charts page, where the charts will be populated with the container angle data.

	h.  As a fleet manager, I would like to be able to access a page where I can view a graph of the container vibration levels, so that I can visually compare the container vibration levels over time.
	
	*Acceptance criteria*
	
	*   Clicking on the vibration tile in the fleet manager dashboard will navigate the user to the charts page, where the charts will be populated with the container vibration data.

	i.  As a fleet manager, I would like to be able to access a page where I can view a graph of the container noise levels, so that I can visually compare the container noise levels over time.
	
	*Acceptance criteria*
	
	*   Clicking on the noise tile in the fleet manager dashboard will navigate the user to the charts page, where the charts will be populated with the container noise data.

	j.  As a fleet manager, I would like to be able to access a page where I can view a graph of the container luminosity levels, so that I can visually compare the container luminosity levels over time.
	
	*Acceptance criteria*
	
	*   Clicking on the luminosity tile in the fleet manager dashboard will navigate the user to the charts page, where the charts will be populated with the container luminosity data.

	k.  As a fleet manager, I would like to be able to access a page where I can view a graph of the container motion levels, so that I can visually compare the container motion levels over time.
	
	*Acceptance criteria*
	
	*   Clicking on the motion tile in the fleet manager dashboard will navigate the user to the charts page, where the charts will be populated with the container motion data.

7.  As a user, I want to be able to authenticate in the mobile application

	a.  As a user, I would like to be able to login to the mobile application so that I can have access to the information about the containers I operate

	*Acceptance criteria:*

	*   The app should have a “login” screen that either asks for the user’s credentials (username/password) to log them in and display them information relevant to their role, OR a screen that simply asks the user what role they belong to.
	*   After successfully “logging in”, the appropriate screen should be shown for their role. For the fleet manager, the screen with the list of containers he operates should be shown. For the farm technician, the screen with the list of containers he is assigned to should be shown.

8.  As a user, I want the navigation of the app to be easy to follow

	a.  As a user, I would like to have a hamburger menu with important links so that I can navigate to those screens with ease throughout the app.

	*Acceptance criteria:*

	*   There should be a hamburger menu with relevant links to the logged in user.
	*   The fleet manager & the farm technician should both have access to a container list link, the logout/login button, and a link to the temperature in different locations.
	*   The hamburger menu must be available throughout the entire app.

## Proposed Implementation Schedule

*   Sprint 2 (Apr 25 - May 2)

	*   3\. a
	*   3\. b
	*   6\. a
	*   6\. b
	*   6\. c
	*   6\. d
	*   7\. a
	*   8\. a

*   Sprint 3 (May 3 - May 9)

	*   1\. a
	*   1\. b
	*   1\. c
	*   1\. d
	*   2\. a

*   Sprint 4 (May 10 - May 16)

	*   4\. a
	*   4\. b
	*   4\. c
	*   4\. d
	*   5\. b
	*   5\. d

*   Sprint 5 (May 17 - May 23)

	*   4\. e
	*   5\. a
	*   5\. c
	*   5\. e
	*   5\. f

## Potential Showstoppers and Open Questions
*   Using Python back-end code with Xamarin Forms (Create a Python API for the front-end?)
*   Integration of Google Maps
*   Customizing the UI rather than using default templates
*   The potential use of MAUI
*   How we will efficiently stream data to the charts so that they're updated in realtime 
