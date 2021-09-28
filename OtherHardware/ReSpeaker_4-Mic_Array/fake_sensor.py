import paho.mqtt.client as mqtt
import time
import random

verbose_logging = False

broker_address = "localhost" 
topic_mic0 = "0"
topic_mic1 = "1"
topic_mic2 = "2"
topic_mic3 = "3"

print("Creating new instance.")
client = mqtt.Client("P1") 


def connect():
	print("Connecting to broker on.")
	client.connect(broker_address)
	print("Succesfully connected to broker.")

	fake_sensors();


def publish(topic, msg):
	client.publish(topic, msg)

	if (verbose_logging):
		print("Publishing on topic: " + topic + ", message: " + msg)


def fake_sensors():
	print("Faking sensor input...")

	while (True):
		time.sleep(0.25)

		publish(topic_mic0, str(random.randint(0,500)))
		publish(topic_mic1, str(random.randint(0,500)))
		publish(topic_mic2, str(random.randint(0,500)))
		publish(topic_mic3, str(random.randint(0,500)))


if __name__ == "__main__":
	connect()
