import os
import fnmatch
import re

directories_to_traverse = ["..\\control\\"]

files_to_search = []

result_strings = []

for dire in directories_to_traverse:
    for file in os.listdir(dire):
        if os.path.isdir(os.path.join(dire, file)):
            directories_to_traverse.append(os.path.join(dire, file))

        if file.endswith(".cshtml") or file.endswith(".cs"):
            files_to_search.append(os.path.join(dire, file))

for file in files_to_search:
    _opened_file = open(file, 'r')

    _read_lines = _opened_file.readlines()

    for line in _read_lines:

        if line.count("LanguageManager.GetLocalizedString") > 0:
            for occ in re.finditer("LanguageManager[.]GetLocalizedString[(]\"", line):
                found = line[occ.end():line.find('"', occ.end())]
                if found not in result_strings:
                    result_strings.append(found)

    _opened_file.close()


_result_file = open("result.txt", 'w')

for line in result_strings:
    _result_file.write(line + "\t\n")

_result_file.close()
