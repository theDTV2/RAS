import queue
import typing
from os.path import exists
from time import sleep

import pygame

message_queue = queue.Queue()
default_message = ""
default_color = [255, 255, 255]


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
    default_message = message
    default_color = color
    return


def __write_message(message, duration, color):
    message_queue.put_nowait([message, duration, color])
    return


def display_task():
    print("Hello from display")
    pygame.init()

    screen = pygame.display.set_mode((256, 256))
    font = pygame.font.Font(None, 24)
    i = 0
    while 1:
        pygame.event.get()
        screen.fill([0, 0, 0])
        if not message_queue.empty():
            (message, duration, color) = message_queue.get_nowait()

            text = font.render(message, True, color)
            screen.blit(text, [30, 20])

        pygame.display.update()
        sleep(0.5)
