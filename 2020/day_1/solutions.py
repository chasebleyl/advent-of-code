from pathlib import Path 

input_file = Path("input.txt").read_text()
all_entries = list(map(int, input_file.split('\n')))
all_entries.sort()

def part_one():
    for i in range(0, len(all_entries)):
        for j in range(i + 1, len(all_entries)):
            if all_entries[i] + all_entries[j] == 2020:
                print("Part one solution is: " + str(all_entries[i] * all_entries[j]))
                return

def part_two():
    for i in range(0, len(all_entries)):
        for j in range(i + 1, len(all_entries)):
            for k in range(j + 1, len(all_entries)):
                if all_entries[i] + all_entries[j] + all_entries[k] == 2020:
                    print("Part two solution is: " + str(all_entries[i] * all_entries[j] * all_entries[k]))
                    return


part_one()
part_two()
