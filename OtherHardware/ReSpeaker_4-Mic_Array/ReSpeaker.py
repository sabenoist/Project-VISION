import paho.mqtt.client as mqtt
import time
import pyaudio
import numpy as np

RESPEAKER_RATE = 16000
RESPEAKER_CHANNELS = 4
# run getDeviceInfo.py to get index
RESPEAKER_INDEX = 2  # refer to input device id
RESPEAKER_FORMAT = pyaudio.paInt16
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
            format=RESPEAKER_FORMAT,
            channels=RESPEAKER_CHANNELS,
            input=True,
            input_device_index=RESPEAKER_INDEX,
            frames_per_buffer=CHUNK)


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
	i = 0
	while(True and i < 5):
		frames = []
		data = stream.read(CHUNK)

		mic0 = np.fromstring(data,dtype=np.int16)[0::2][0]
		mic1 = np.fromstring(data,dtype=np.int16)[1::2][0]
		mic2 = np.fromstring(data,dtype=np.int16)[2::2][0]
		mic3 = np.fromstring(data,dtype=np.int16)[3::2][0]

    	frames.append(mic0)
    	frames.append(mic1)
    	frames.append(mic2)
    	frames.append(mic3)

    	print(frames)
    	i += 1
    	

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