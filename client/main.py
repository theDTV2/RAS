import asyncio
import fileinput
import json
from os.path import exists
from ssl import SSLError
import threading
import sys
from time import sleep
import requests
from datetime import datetime, timedelta

import display

verify_ssl_cert = False
# In Debug mode we don't check for the ssl certificate
if __debug__:
    verify_ssl_cert = False

# How many seconds request delay we allow, before we assume the message is too old
timeout = 5

# Return constants
RETURN_OK = 0
RETURN_CONFIG_ERROR = 1
RETURN_CONNECTION_ERROR = 2
RETURN_SSL_ERROR = 3
RETURN_PARSE_ERROR = 4
RETURN_API_REGISTER_ERROR = 5
RETURN_API_ACCESS_ERROR = 6
RETURN_API_HEARTBEAT_ERROR = 7

display_task = threading.Thread(target=display.display_task)
display_task.start()
display.set_keep_last_message(True)
display.write_info_message("System starting up...", 1)

print("Trying to open config.cfg...")
# If there is no config.cfg-file, we cannot continue
if not exists("config.cfg"):
    print("config.cfg not found")
    exit(RETURN_CONFIG_ERROR)

print("Trying to read config.cfg...")
with open('config.cfg', 'r') as config_file_r:
    config_data = json.load(config_file_r)
    # Load properties from config
    dns_name = config_data['dns_name']
    api_endpoint = config_data['api_endpoint']
    door_id = config_data['door_id']
    door_name = config_data['door_name']
    secret = config_data['secret']
    print("done")
    config_file_r.close()

if dns_name == "" or api_endpoint == "":
    print("endpoint or dns name not set, check config file")
    exit(RETURN_CONFIG_ERROR)

if door_id == "":
    print("door id not set, check config file")
    exit(RETURN_CONFIG_ERROR)

total_address = dns_name + api_endpoint

# Test, if api is reachable
print("testing api connection...")
try:
    request_test = requests.get(total_address,
                                verify=verify_ssl_cert)
except ConnectionError:
    print("API not reachable")
    exit(RETURN_CONNECTION_ERROR)
except SSLError:
    print("Certificate error, check target server")
    exit(RETURN_SSL_ERROR)

if request_test.status_code != 200:
    print("Connection test failed")
    exit(RETURN_CONNECTION_ERROR)

print("api connection successful")
# If secret is not set, we assume that this door is not registered
if secret == "":
    print("secret not set, trying to register door in API...")
    # Register first door
    data_register = {'doorID': door_id,
                     'timeStamp': datetime.now()}
    request_register = requests.post(total_address + '/Register',
                                     data=data_register, verify=verify_ssl_cert)
    print("received register response, parsing...")

    if request_register.status_code != 200:
        print("Register failed, check Door ID in config")
        exit(RETURN_API_REGISTER_ERROR)

    # convert time stamp to time obj
    time_stamp_str = request_register.json()['timeStamp']
    try:
        time_stamp = datetime.strptime(time_stamp_str, '%d.%m.%Y %H:%M:%S')
    except ValueError:
        print("Time Stamp conversion failed, check for data manipulation")
        exit(RETURN_API_REGISTER_ERROR)

    if time_stamp + timedelta(seconds=timeout) < datetime.now():
        print("Register operation too old, check connection or check for manipulation")
        exit(RETURN_API_REGISTER_ERROR)

    secret = request_register.json()['secret']
    door_name = request_register.json()['displayName']

    print("Door registered successfully, saving new parameters to config.cfg...")
    config_data['secret'] = secret
    config_data['door_name'] = door_name

    with open('config.cfg', 'w') as config_file_w:
        json.dump(config_data, config_file_w)
        config_file_w.close()
        print("register done")

else:
    print("secret found, assuming door is already registered")

last_check_in = datetime.now()
heartbeat_frequency_seconds = 60

display.set_keep_last_message(False)
display.set_default_message(message="System ready", color=(0,255,0))

while True:
    sleep(1)

    # Send Heartbeat to api
    if last_check_in + timedelta(seconds=heartbeat_frequency_seconds) < datetime.now():

        print("sending heartbeat...")
        data_heartbeat = {'doorID': door_id,
                          'timeStamp': datetime.now(),
                          'secret': secret}
        try:
            request_heartbeat = requests.post(total_address + "/Heartbeat",
                                              data=data_heartbeat, verify=verify_ssl_cert)
        except ConnectionError:
            print("API not reachable")
            exit(RETURN_CONNECTION_ERROR)
        print("heartbeat done")
        last_check_in = datetime.now()

    # If there is input, send request to api
    input_handle = fileinput.input()

    input_str = input_handle.readline()
    if input_str != "":
        cardCode = input_str.strip()
        time_stamp = datetime.now()

        data_request_access = {'doorID': door_id,
                               'cardCode': cardCode,
                               'timeStamp': datetime.now(),
                               'secret': secret}
        try:
            return_access_request = requests.post(total_address + "/Access",
                                                  data=data_request_access, verify=verify_ssl_cert)
        except ConnectionError:
            print("API not reachable")
            break

        if return_access_request.status_code != 200:
            print("API communication error")
            exit(RETURN_API_ACCESS_ERROR)

        time_stamp_str = return_access_request.json()['timeStamp']
        door_response = return_access_request.json()['doorResponse']

        if time_stamp + timedelta(seconds=timeout) < datetime.now():
            print("Register operation too old, check connection or check for manipulation")
        if door_response:
            display.write_success_message("Access Granted", 4)
            print("doorResponse Access Granted")
        else:
            display.write_error_message("Access Denied", 4)
            print("doorResponse Access denied")
        print("ResponseText: " + return_access_request.json()['responseText'])

    # TODO: Send Response Text to Display
    input_handle.close()
