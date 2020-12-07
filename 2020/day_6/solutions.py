from pathlib import Path

input_file = Path("input.txt").read_text()
all_entries = list(input_file.split('\n'))

### Incorrect answers
# 6760

def part_one():
    all_group_counts = list()
    current_set = set()
    for entry in all_entries:
        if entry is None or entry == '':
            all_group_counts.append(len(current_set))
            current_set = set()
        else:
            for char in entry:
                current_set.add(char)
    all_group_counts.append(len(current_set))
    print("Part one total count of YES from all groups=" + str(sum(all_group_counts)))
    return 0

def part_two():
    all_group_counts = list()
    original_set = set()
    is_new_group = True
    for entry in all_entries:
        if entry is None or entry == '':
            all_group_counts.append(len(original_set))
            original_set = set()
            is_new_group = True
        elif is_new_group:
            is_new_group = False
            for char in entry: original_set.add(char)
        else:
            chars_to_remove = list()
            for char in original_set:
                if char not in entry:
                    chars_to_remove.append(char)
            for char in chars_to_remove:
                original_set.remove(char)
    all_group_counts.append(len(original_set))
    print("Part two total count of YES from all groups=" + str(sum(all_group_counts)))
    return 0

part_one()
part_two()