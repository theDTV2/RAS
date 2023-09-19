import datetime
from time import sleep
import RPi.GPIO as GPIO

__closing_time = datetime.datetime.now()
__run = True


def request_open_door(duration=5):
    global __closing_time
    __closing_time = datetime.datetime.now() + datetime.timedelta(seconds=duration)


def request_close_door():
    global __closing_time
    __remaining_opening_time = datetime.datetime.now()


def __open_door():
    # Here the opening function can be called, that sends the open request to the door. This can be anything
    # required, but it has to be NON BLOCKING
    print("Opening door...")
    GPIO.output(12, GPIO.LOW)


def __close_door():
    # Here the closing function can be called, that sends the close request to the door. This can be anything
    # required, but it has to be NON BLOCKING
    print("Closing door...")
    GPIO.output(12, GPIO.HIGH)


def shut_down_task():
    global __run
    __run = False


def door_task():
    ###################
    ###### Setup ######

    # GPIO-Pins wie nach Pin-Nummer verwenden
    GPIO.setmode(GPIO.BOARD)

    # GPIO In-/Outputs
    GPIO.setup(12, GPIO.OUT)  # Pin 12
    GPIO.setup(10, GPIO.IN, pull_up_down=GPIO.PUD_DOWN)  # Pin 10, interner Pull-Down
    GPIO.setup(8, GPIO.IN, pull_up_down=GPIO.PUD_DOWN)  # Pin 8, interner Pull-Down

    global __closing_time
    _door_is_open = False

    while __run:
        _time_now = datetime.datetime.now()

        if __closing_time > _time_now and not _door_is_open:

            __open_door()
            _door_is_open = True

        if __closing_time < _time_now and _door_is_open:

            __close_door()
            _door_is_open = False

        sleep(0.1)
