from pathlib import Path
import re 

input_file = Path("input.txt").read_text()
all_entries = list(input_file.split('\n'))

def part_one_and_two():
    part_one_and_two_valid_count = 0
    part_two_valid_count = 0
    for entry in all_entries:
        match = re.search(r"(\d*)-(\d*)\ ([a-z])\:\ ([a-z]*)", entry)
        match_groups = match.groups()
        if is_valid_password_one(int(match_groups[0]), int(match_groups[1]), match_groups[2], match_groups[3]):
            part_one_and_two_valid_count += 1
        if is_valid_password_two(int(match_groups[0]), int(match_groups[1]), match_groups[2], match_groups[3]):
            part_two_valid_count += 1
    print("Part 1 total valid passwords: " + str(part_one_and_two_valid_count))
    print("Part 2 total valid passwords: " + str(part_two_valid_count))
    return 0

def is_valid_password_one(low_count, high_count, target_char, password):
    char_count = password.count(target_char)
    if char_count < low_count or char_count > high_count:
        return False
    return True

def is_valid_password_two(low_pos, high_pos, target_char, password):
    pos_contains_count = 0
    if password[low_pos - 1] == target_char: pos_contains_count += 1
    if password[high_pos - 1] == target_char: pos_contains_count += 1

    return True if pos_contains_count == 1 else False

part_one_and_two()
