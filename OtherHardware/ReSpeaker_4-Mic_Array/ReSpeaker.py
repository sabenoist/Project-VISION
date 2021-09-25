import paho.mqtt.client as mqtt
import time

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

	fake_sensors()


def publish(topic, msg):
	client.publish(topic, msg)
	
	if (verbose_logging):	
		print("Publishing on topic: " + topic + ", message: " + msg)


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