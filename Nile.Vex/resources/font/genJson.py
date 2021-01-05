import json

alphabet = "abcdefghijklmnopqrstuvwxyz"
ALPHABET = alphabet.upper();

margin = 1;

glyphs = {}

accw = 0;

acch = 14;

fw = 305.0

fh = 48.0

for i in ALPHABET:
    accw += margin;
    width = int(input(f"Enter width for character {i}\n"))
    height = int(input(f"Enter height for character {i}\n"))
    glyphs.update({i : {"x" : accw / fw, "y" : acch / fh, "w" : width / fw, "h" : height / fh}})
    accw += width;

acch = 30;
accw = 0;

for i in alphabet:
    accw += margin;
    width = int(input(f"Enter width for character {i}\n"))
    height = int(input(f"Enter height for character {i}\n"))
    glyphs.update({i : {"x" : accw / fw, "y" : acch / fh, "w" : width / fw, "h" : height / fh}})
    accw += width;

accw = 0;

print("The generated json is")
print(json.dumps(glyphs))

