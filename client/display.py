import queue
import string
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
__displaying_default_message = False


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
    global __displaying_default_message
    __default_message = message
    __default_color = color
    __displaying_default_message = False
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
    global __displaying_default_message
    print("Hello from display")
    pygame.init()

    screen = pygame.display.set_mode((512, 384))
    [_w_screen, _h_screen] = screen.get_size()

    font = pygame.font.Font(None, 48)

    cutoff_time = datetime.datetime.now()

    while __run:

        # cache current time to prevent repeated recalculation
        _time_now = datetime.datetime.now()

        if cutoff_time > _time_now:
            sleep(0.2)
            continue
        else:
            # If we show the default message, we display it here
            if not __keep_last_value and __message_queue.empty() and not __displaying_default_message:
                screen.fill([0, 0, 0])
                _default_text = font.render(__default_message, True, __default_color)
                _width_line_d = _default_text.get_width()
                screen.blit(_default_text,  [(0.5 * _w_screen) - (0.5 * _width_line_d), (0.3 * _h_screen)])
                _displaying_default_message = True

        if not __message_queue.empty():
            screen.fill([0, 0, 0])
            (_message, _duration, _color) = __message_queue.get_nowait()
            cutoff_time = _time_now + datetime.timedelta(seconds=_duration)

            # If the message contains a newline, we create a second line
            if '\n' in _message:
                _pos = _message.find('\n')

                _text_second_line = font.render(_message[_pos+1:], True, _color)

                [_width_line_2, _height_line_2] = _text_second_line.get_size()
                screen.blit(_text_second_line,
                            [(0.5 * _w_screen) - (0.5 * _width_line_2), (0.3 * _h_screen) + (_height_line_2*2)])
                # If we used the second line, we modify the message for the first line
                _message = _message[0:_pos]

            _text_first_line = font.render(_message, True, _color)

            _width_line_1 = _text_first_line.get_width()
            screen.blit(_text_first_line,
                        [(0.5 * _w_screen) - (0.5 * _width_line_1), (0.3 * _h_screen)])

            _displaying_default_message = False

        pygame.display.update()

        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                __run = False

        sleep(0.2)
