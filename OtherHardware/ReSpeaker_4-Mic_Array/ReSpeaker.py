import paho.mqtt.client as mqtt
import time
import pyaudio
import numpy as np

RESPEAKER_RATE = 16000
RESPEAKER_CHANNELS = 2
RESPEAKER_WIDTH = 2
# run getDeviceInfo.py to get index
RESPEAKER_INDEX = 1  # refer to input device id
CHUNK = 1024

verbose_logging = True

broker_address = "localhost" 
topic_mic0 = "0"
topic_mic1 = "1"
topic_mic2 = "2"
topic_mic3 = "3"

print("Creating new instance.")
client = mqtt.Client("P1") 

p = pyaudio.PyAudio()

stream = p.open(
            rate=RESPEAKER_RATE,
            format=p.get_format_from_width(RESPEAKER_WIDTH),
            channels=RESPEAKER_CHANNELS,
            input=True,
            input_device_index=RESPEAKER_INDEX,)


def connect():
	print("Connecting to broker on.")
	client.connect(broker_address)
	print("Succesfully connected to broker.")

	read_sensors();


def publish(topic, msg):
	client.publish(topic, msg)
	
	if (verbose_logging):	
		print("Publishing on topic: " + topic + ", message: " + msg)


def read_sensors():
	while(True):
		time.sleep(1)
		frames = []
		data = stream.read(CHUNK)
    	frames.append(data)

    	print(frames)
    	

def fake_sensors():
	print("Faking sensor input...")

	topic_id = 0

	while (True):
		time.sleep(0.25)

		publish(str(topic_id), "10")
		
		topic_id += 1
		if (topic_id > int(topic_mic3)):
			topic_id = int(topic_mic0)


if __name__ == "__main__":
	connect()