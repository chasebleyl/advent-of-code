from pathlib import Path
import re
import math

input_file = Path("input.txt").read_text()
all_entries = list(input_file.split('\n'))

# ROWS AND SEATS ARE BASE 0
MAX_ROWS = 127
MAX_COLUMNS = 7

def parts_one_and_two():
    seat_ids = list()
    highest_seat_id = 0
    for entry in all_entries:
        match = re.search(r"([a-zA-Z]{7})([a-zA-Z]{3})", entry)
        match_groups = match.groups()
        assigned_row = binary_search_from_string(match_groups[0], 0, MAX_ROWS)
        assigned_column = binary_search_from_string(match_groups[1], 0, MAX_COLUMNS)
        seat_id = assigned_row * 8 + assigned_column
        seat_ids.append(seat_id)
    seat_ids.sort()
    print("Highest seat id=" + str(seat_ids[-1]))

    for i in range(0, len(seat_ids) - 2):
        if seat_ids[i+1] - seat_ids[i] > 1:
            print("My seat_id is=" + str(seat_ids[i] + 1))
            break
    return 0

def binary_search_from_string(string_code, bottom, top):
    if bottom == top:
        return bottom 
    focus_char = string_code[0]
    if focus_char == 'F' or focus_char == 'L':
        return binary_search_from_string(string_code[1:], bottom, calculate_new_top(bottom, top))
    return binary_search_from_string(string_code[1:], calculate_new_bottom(bottom, top), top)

def calculate_new_bottom(bottom, top):
    return math.ceil(bottom + ((top - bottom) / 2))

def calculate_new_top(bottom, top):
    return math.floor(top - ((top - bottom) / 2))

parts_one_and_two()