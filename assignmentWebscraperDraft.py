from pprint import pprint
from typing import NamedTuple, Optional, List

import requests  # pip install requests
from bs4 import BeautifulSoup, Tag  # pip install bs4

paper_code = input("Whats your paper code? ")
sem = input("What semester? (A|B|H|C|G) ")
if sem.upper() not in ("A", "B", "H", "C", "G"):
    raise RuntimeError(f"{sem.upper()} is not a supported semester.")

url = f"https://paperoutlines.waikato.ac.nz/outline/{paper_code.upper()}-22{sem.upper()}%20(HAM)"

r = requests.get(url)
if r.status_code != 200:
    raise RuntimeError(r.text)

soup: BeautifulSoup = BeautifulSoup(r.content, "html.parser")

table: Optional[Tag] = soup.find("div", id="module-Assessment-grid")
if not table:
    raise RuntimeError(
        f"'{paper_code}' in {sem} semester does not appear to be a valid paper code."
    )

table_data: Tag = table.find("tbody")


class Entry(NamedTuple):
    name: str
    percentage: int


assessments: List[Entry] = []
for row in table_data.children:
    cols: List[str] = [ele.text.strip() for ele in row]
    name = cols[0].split(".")[1].lstrip("\xa0")
    entry: Entry = Entry(name=name, percentage=int(cols[3]))
    assessments.append(entry)

pprint(assessments)
