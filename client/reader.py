import fileinput
import queue
import string

__read_queue = queue.Queue()

__run = True


def get_next_input() -> string:
    if __read_queue.empty():
        return ""
    return __read_queue.get_nowait()


def shut_down_task():
    global __run
    __run = False


def reader_task():
    input_handle = fileinput.input()

    while __run:
        # If there is input, send request to api

        input_str = input_handle.readline()
        if input_str != "":
            __read_queue.put_nowait(input_str)

