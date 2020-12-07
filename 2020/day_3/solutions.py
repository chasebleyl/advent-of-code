from pathlib import Path

input_file = Path("input.txt").read_text()
all_entries = list(input_file.split('\n'))

# Since the patterns repeats to the right an infinite number of times, the last char index before a repeat is listed below
MAX_WIDTH_BASE_0 = 31

def part_one():
    print("Part one; total number of trees encountered: " + str(trees_encountered_by_slopes(3, 1)))
    return 0

def part_two():
    count_1 = trees_encountered_by_slopes(1, 1)
    count_2 = trees_encountered_by_slopes(3, 1)
    count_3 = trees_encountered_by_slopes(5, 1)
    count_4 = trees_encountered_by_slopes(7, 1)
    count_5 = trees_encountered_by_slopes(1, 2)

    print("Part two; all counts multiplied together: " + str(count_1 * count_2 * count_3 * count_4 * count_5))

def trees_encountered_by_slopes(x_slope, y_slope):
    trees_encountered_count = 0
    current_x = 0
    current_y = 0
    total_lines = len(all_entries)

    while current_y < total_lines:
        if all_entries[current_y][current_x] == '#': trees_encountered_count += 1
        current_x += x_slope
        current_y += y_slope
        if current_x >= MAX_WIDTH_BASE_0: current_x = current_x - MAX_WIDTH_BASE_0

    return trees_encountered_count

part_one()
part_two()