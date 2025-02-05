# Bitmap Converter
Aptly named, Bitmap Converter ingests a Bitmap image file, extracts the color data, and exports said data to a RGB array in JSON. The primary purpose of this is a compliment to my game program Eldoria. Eldoria will ingest the JSON files and "recreate" them into string-based images. The current code reflects this purpose.

Bitmap Converter accepts as input the following:

* The path to a single Bitmap file.
* The paths to multiple Bitmap files, separated by a whitespace character.
* A directory containing Bitmap files. Files other than those with the BMP extension will be ignored.

The JSON file will be output in the directory where the source Bitmap image lives.

## Conversion
Bitmap Converter has code that specifically targets my intended use case. It can, with some modifications, be augmented to work in different contexts. Specifically:

* The variable dims is a constant that could be discarded and dynamically calculated after a Bitmap is loaded (this would be required; it's simply width * height).
* The JsonSerializer could have additional options appended to it if you need more complex output (or simpler).
