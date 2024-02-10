[![CI/CD](https://github.com/TcHaxx/snappy/actions/workflows/cicd.yml/badge.svg)](https://github.com/TcHaxx/snappy/actions/workflows/cicd.yml)
[![NuGet version (TcHaxx.Snappy.CLI)](https://img.shields.io/nuget/v/TcHaxx.Snappy.CLI.svg)](https://www.nuget.org/packages/TcHaxx.Snappy.CLI/)
---
> 🚧 WIP
# snappy
Snappy is a Snapshot Testing framework, like [Verify](https://github.com/VerifyTests/Verify),  designed for TwinCAT 3.  

During the assertion phase, Snappy performs the following steps:

1. It serializes the test result and saves it in a file that corresponds to the test-suite and test name.
2. In subsequent test executions, it re-serializes the result and compares it to the existing file.
3. If the two snapshots do not match, the test fails.  
This discrepancy could indicate either an unexpected change or the need to update the reference snapshot to reflect the new result
4. The validated files can the be added to [source control](#source-control-received-and-verified-files).

## Install

Snappy consists of two parts:
* A dotnet CLI tool, called `TcHaxx.Snappy.CLI`,
  > See [How to manage .NET tools](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools)
* and the TwinCAT library `snappy`.

### TcHaxx.Snappy.CLI

To install the CLI tool execute following command:
```sh
dotnet tool install -g TcHaxx.Snappy.CLI
```
#### Update
To update to the latest verson from [Nuget](https://www.nuget.org/packages/TcHaxx.Snappy.CLI/), run:
```sh
dotnet tool update -g  TcHaxx.Snappy.CLI
```
#### Uninstall
```sh
dotnet tool uninstall -g TcHaxx.Snappy.CLI
``` 

### snappy.library
Download latest releases, either `snappy.libary` and/or `snappy.compiled-library` and [install](https://infosys.beckhoff.com/english.php?content=../content/1033/tc3_plc_intro/4218300427.html).

## Usage
Before any tests are being run, `TcHaxx.Snappy.CLI` hast to be started, prior.
Run manually via [dotnet tools](#dotnet-tools) or [TwinCAT deployment](#twincat-3-plc-deployment).

### dotnet tools
```sh
TcHaxx.Snappy.Verify verify [OPTIONS]
```

### TwinCAT 3 PLC Deployment
> See [Category Deployment](https://infosys.beckhoff.com/english.php?content=../content/1033/tc3_plc_intro/3260050187.html)

To automatically start `TcHaxx.Snappy.CLI` you may want to add following event to your PLC project properties:

Event | Command Type | Parameter 1 | Parameter 2
--- | --- | --- | ---
Activate Configuration | Execute | `cmd /c start TcHaxx.Snappy.CLI verify -d \"%SOLUTIONPATH%.snappy-verified\"` |

## Source control: Received and Verified files
When dealing with source control, consider the following guidelines for handling **Received** and **Verified** files:

1. **Exclusion**:
   - Exclude all files with the pattern `*.received.*` from source control.
   - To achieve this, add the following line to your `.gitignore` file:
     ```
     *.received.*
     ```

2. **Commitment**:
   - On the other hand, **commit** all files with the pattern `*.verified.*` to source control.

> See [Verify/README](https://github.com/VerifyTests/Verify?tab=readme-ov-file#source-control-received-and-verified-files)

# Acknowledgments

* [TcUnit](https://github.com/tcunit/TcUnit) - A unit testing framework for Beckhoff's TwinCAT 3
* [CommandLineParser](https://github.com/commandlineparser/commandline) - A command line parsing library for .NET applications.
* [Verify](https://github.com/VerifyTests/Verify) - A library used for snapshot testing.
* [Serilog](https://github.com/serilog/serilog) - A logging library for .NET applications.