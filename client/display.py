import queue
import typing
import datetime
from os.path import exists
from time import sleep

import pygame

__message_queue = queue.Queue()
__default_message = ""
__default_color = [255, 255, 255]
__keep_last_value = False
__run = True

def write_success_message(message, duration):
    __write_message(message, duration, (0, 255, 0))
    return


def write_error_message(message, duration):
    __write_message(message, duration, (255, 0, 0))
    return


def write_info_message(message, duration):
    __write_message(message, duration, (255, 255, 255))
    return


def set_default_message(message, color):
    global __default_message
    global __default_color
    __default_message = message
    default_color = color
    return


def shut_down_task():
    global __run
    __run = False


def set_keep_last_message(value):
    global __keep_last_value
    __keep_last_value = value
    return


def __write_message(message, duration, color):
    global __message_queue
    __message_queue.put_nowait([message, duration, color])
    return


def display_task():
    global __run
    print("Hello from display")
    pygame.init()

    screen = pygame.display.set_mode((256, 256))
    font = pygame.font.Font(None, 24)

    cutoff_time = datetime.datetime.now()

    _displaying_default_message = False

    while __run:

        # cache current time to prevent repeated recalculation
        _time_now = datetime.datetime.now()

        if cutoff_time > _time_now:
            sleep(0.2)
            continue
        else:
            # If we show the default message, we display it here
            if not __keep_last_value and __message_queue.empty() and not _displaying_default_message:
                screen.fill([0, 0, 0])
                text = font.render(__default_message, True, __default_color)
                screen.blit(text, [30, 20])
                _displaying_default_message = True

        if not __message_queue.empty():
            screen.fill([0, 0, 0])
            (_message, _duration, _color) = __message_queue.get_nowait()
            cutoff_time = _time_now + datetime.timedelta(seconds=_duration)
            text = font.render(_message, True, _color)
            screen.blit(text, [30, 20])
            _displaying_default_message = False

        pygame.display.update()

        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                __run = False

        sleep(0.2)
