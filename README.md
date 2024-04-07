# File UTF8 Encoder

This is a C# program that detects the character encoding of a file and converts it to UTF-8 (without BOM) using the Ude (Universal Detector) library.

## Prerequisites

- .NET 8.0
- System.Text.Encoding.CodePages NuGet package

## Usage

1. Build the project.
2. Run the program from the command line, passing the path of the file you want to convert as an argument. For example: `ConvertToUTF8.exe path/to/your/file`

The program will detect the file's character encoding, convert it to UTF-8 (without BOM), and overwrite the original file with the converted content.

## License

Copyright 2024 Qadiff LLC

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.