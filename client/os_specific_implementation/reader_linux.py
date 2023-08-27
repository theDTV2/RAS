import fileinput
import queue
import string
from evdev import InputDevice, categorize, ecodes

__read_queue = queue.Queue()

__run = True

def translate_evdev_to_ascii(evdev_event):
    if evdev_event == 11:
        return '0'
    elif evdev_event == 2:
        return '1'
    elif evdev_event == 3:
        return '2'
    elif evdev_event == 4:
        return '3'
    elif evdev_event == 5:
        return '4'
    elif evdev_event == 6:
        return '5'
    elif evdev_event == 7:
        return '6'
    elif evdev_event == 8:
        return '7'
    elif evdev_event == 9:
        return '8'
    elif evdev_event == 10:
        return '9'
    elif evdev_event == 28:
        return '\n'
    else:
        return '0'
        
def get_next_input() -> string:
    if __read_queue.empty():
        return ""
    return __read_queue.get_nowait()


def shut_down_task():
    global __run
    __run = False


def reader_task():
    dev = InputDevice('/dev/input/by-id/usb-OEM_RFID_Device__Keyboard_-event-kbd')
    string_to_pass = ""
    
    for event in dev.read_loop():
     if event.type == ecodes.EV_KEY and event.value == 1:
         read_value = translate_evdev_to_ascii(event.code)
         
         if read_value == '\n':
         	string_to_pass = string_to_pass + read_value
         	__read_queue.put_nowait(string_to_pass)
         	string_to_pass = ""
         else:	
         	string_to_pass = string_to_pass + read_value
        
        

    
 