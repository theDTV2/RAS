import datetime
from time import sleep

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


def __close_door():
    # Here the closing function can be called, that sends the close request to the door. This can be anything
    # required, but it has to be NON BLOCKING
    print("Closing door...")


def shut_down_task():
    global __run
    __run = False


def door_task():
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
