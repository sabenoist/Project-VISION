import paho.mqtt.client as mqtt
import time
import pyaudio
import numpy as np

RESPEAKER_RATE = 16000
RESPEAKER_CHANNELS = 4
# run getDeviceInfo.py to get index
RESPEAKER_INDEX = 1  # refer to input device id
RESPEAKER_FORMAT = pyaudio.paInt16
CHUNK = 1024

AMPLITUDE_MODIFIER = 1

verbose_logging = False

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
		frames = []
		data = stream.read(CHUNK)
		data_numbers = np.fromstring(data,dtype=np.int16)

		mic0 = avg(data_numbers[0::RESPEAKER_CHANNELS])/float(AMPLITUDE_MODIFIER)
		mic1 = avg(data_numbers[1::RESPEAKER_CHANNELS])/float(AMPLITUDE_MODIFIER)
		mic2 = avg(data_numbers[2::RESPEAKER_CHANNELS])/float(AMPLITUDE_MODIFIER)
		mic3 = avg(data_numbers[3::RESPEAKER_CHANNELS])/float(AMPLITUDE_MODIFIER)

		publish(topic_mic0, str(mic0))
		publish(topic_mic1, str(mic1))
		publish(topic_mic2, str(mic2))
		publish(topic_mic3, str(mic3))


def avg(values):
	return sum(values)/len(values)


if __name__ == "__main__":
	connect()
