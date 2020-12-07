from pathlib import Path
import re
import math

input_file = Path("input.txt").read_text()
all_entries = list(input_file.split('\n'))

### Incorrect answers
# 6760
## Part two incorrect
# 115 (too high)

def part_one():
    valid_passport_count = 0
    current_set = set()
    for entry in all_entries:
        if entry is None or entry == '':
            if len(current_set) == 7:
                valid_passport_count += 1
            current_set = set()
        else:
            if "byr:" in entry: current_set.add("byr")
            if "iyr:" in entry: current_set.add("iyr")
            if "eyr:" in entry: current_set.add("eyr")
            if "hgt:" in entry: current_set.add("hgt")
            if "hcl:" in entry: current_set.add("hcl")
            if "ecl:" in entry: current_set.add("ecl")
            if "pid:" in entry: current_set.add("pid")

    if len(current_set) == 7:
        valid_passport_count += 1
    print("Part 1 count of valid passports=" + str(valid_passport_count))
    return 0

def part_two():
    valid_passport_count = 0
    current_set = set()
    for entry in all_entries:
        if entry is None or entry == '':
            if len(current_set) == 7:
                valid_passport_count += 1
            current_set = set()
        else:
            if is_valid_byr(entry): current_set.add("byr")
            if is_valid_iyr(entry): current_set.add("iyr")
            if is_valid_eyr(entry): current_set.add("eyr")
            if is_valid_hgt(entry): current_set.add("hgt")
            if is_valid_hcl(entry): current_set.add("hcl")
            if is_valid_ecl(entry): current_set.add("ecl")
            if is_valid_pid(entry): current_set.add("pid")
    print("Part 2 count of valid passports=" + str(valid_passport_count))
    return 0

# byr (Birth Year) - four digits; at least 1920 and at most 2002.
def is_valid_byr(entry):
    if re.search(r"(byr:200[0-2]|byr:19[2-9][0-9])", entry) is not None:
        return True
    return False

# iyr (Issue Year) - four digits; at least 2010 and at most 2020.
def is_valid_iyr(entry):
    if re.search(r"(iyr:201[0-9]|iyr:2020)", entry):
        return True
    return False

# eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
def is_valid_eyr(entry):
    if re.search(r"(eyr:202[0-9]|eyr:2030)", entry):
        return True
    return False

# hgt (Height) - a number followed by either cm or in:
    # If cm, the number must be at least 150 and at most 193.
    # If in, the number must be at least 59 and at most 76.
def is_valid_hgt(entry):
    if re.search(r"(1[5-8][0-9]cm|19[0-3]cm|59in|6[0-9]in|7[0-6]in)", entry):
        return True
    return False

# hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
def is_valid_hcl(entry):
    if re.search(r"(#[0-9a-f]{6})", entry):
        return True
    return False

# ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
def is_valid_ecl(entry):
    if re.search(r"(amb|blu|brn|gry|grn|hzl|oth)", entry):
        return True
    return False

# pid (Passport ID) - a nine-digit number, including leading zeroes.
def is_valid_pid(entry):
    if re.search(r"([0-9]{9})", entry):
        return True
    return False

part_one()
part_two()