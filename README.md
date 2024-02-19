[![CI/CD](https://github.com/TcHaxx/snappy/actions/workflows/cicd.yml/badge.svg)](https://github.com/TcHaxx/snappy/actions/workflows/cicd.yml)
[![NuGet version (TcHaxx.Snappy.CLI)](https://img.shields.io/nuget/v/TcHaxx.Snappy.CLI.svg)](https://www.nuget.org/packages/TcHaxx.Snappy.CLI/)
---

# snappy
Snappy is a Snapshot Testing framework, like [Verify](https://github.com/VerifyTests/Verify),  designed for TwinCAT 3.  

During the assertion phase, Snappy performs the following steps:

1. It serializes the test result and saves it in a file that corresponds to the test-suite and test name.
2. In subsequent test executions, it re-serializes the result and compares it to the existing file.
3. If the two snapshots do not match, the test fails.  
This discrepancy could indicate either an unexpected change or the need to update the reference snapshot to reflect the new result
4. The validated files can the be added to [source control](#source-control-received-and-verified-files).


## Example using `TcUnit`
```
FUNCTION_BLOCK FB_TcUnitExample EXTENDS FB_TestSuite
VAR
	hr				: HRESULT;		
	fbTcUnitAdapter : FB_TcUnitAdapter;
	
	stActual		: ST_DemoDataType;
	{attribute 'analysis' := '-33'}
	stResult		: ST_VerificationResult;
END_VAR
```
```
TEST('Test some stuff with TcHaxx.Snappy');

// ARRANGE

// ACT
stActual := F_CreateDemoData();

// ASSERT
hr := fbTcUnitAdapter.Verify(anyArg:= stActual);

IF NOT PENDING(hr) THEN
	TEST_FINISHED_NAMED('Test some stuff with TcHaxx.Snappy');
END_IF
```

> Find more examples in the `examples` PLC project.

## Install

Snappy consists of two parts:
* A dotnet CLI tool, called `TcHaxx.Snappy.CLI`,
  > See [How to manage .NET tools](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools)
* and the TwinCAT library `snappy`.

### Prerequisites

* [Verify - Supported Tool](https://github.com/VerifyTests/DiffEngine?tab=readme-ov-file#supported-tools) is installed.

### TcHaxx.Snappy.CLI

To install the CLI tool execute following command:
```sh
dotnet tool install -g TcHaxx.Snappy.CLI
```
#### Update
To update to the latest version from [Nuget](https://www.nuget.org/packages/TcHaxx.Snappy.CLI/), run:
```sh
dotnet tool update -g  TcHaxx.Snappy.CLI
```
#### Uninstall
```sh
dotnet tool uninstall -g TcHaxx.Snappy.CLI
``` 

### Run install command

To install the required TwinCAT libraries, simply run the following command:
```sh
TcHaxx.Snappy.CLI install
```
This command will install all required dependencies:
```
************************************************************************************************************************
                                               TcHaxx.Snappy.CLI V0.1.0.0
                                           Copyright (c) 2024 densogiaichned
                                            https://github.com/TcHaxx/snappy
************************************************************************************************************************

[00:00:01 INF] Using TwinCAT profile "C:\TwinCAT\3.1\Components\Plc\Profiles\TwinCAT PLC Control_Build_4024.55.profile"
[00:00:01 INF] Installing TwinCAT libraries ...
[00:00:20 INF] RepTool.exe: Repository Tool
Copyright © 1994-2020 by 3S-Smart Software Solutions GmbH. All rights reserved.

Arguments:  --profile='TwinCAT PLC Control_Build_4024.55' --installLibsRecursNoOverwrite "%USERPROFILE%\.dotnet\tools\.store\tchaxx.snappy.cli"

Library installed: %USERPROFILE%\.dotnet\tools\.store\tchaxx.snappy.cli\0.1.0\tchaxx.snappy.cli\0.1.0\content\.dist\rplc.library
Library installed: %USERPROFILE%\.dotnet\tools\.store\tchaxx.snappy.cli\0.1.0\tchaxx.snappy.cli\0.1.0\content\.dist\snappy.library
```

## Usage
Before any tests are being run, `TcHaxx.Snappy.CLI` hast to be started, prior.  
Run manually via [dotnet tools](#dotnet-tools) or [TwinCAT deployment](#twincat-3-plc-deployment).

### dotnet tools
Run `snappy`-CLI with following command:
```sh
TcHaxx.Snappy.Verify verify [OPTIONS]
```
> See [Command verfiy](#command-verify) for more options.

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

### Commands and Options

Snappy.CLI has following CLI commands and options.

#### Command `install`

This command installs all dependencies, such as [rplc.library](https://github.com/tchaxx/rplc) and of course `snappy.library`.
```sh
TcHaxx.Snappy.CLI install [OPTIONS]
```
Option  | Required | Default | Description  
--- | --- | --- |  ---
`--tc-profile` | `no` | `latest` | TwinCAT profile to use, e.g. `latest` or specific version `TwinCAT PLC Control_Build_4024.54`, defaults to `latest`.
`--tool-path` | `no` | `%USERPROFILE%\.dotnet\tools\.store\tchaxx.snappy.cli` | Directory, where `TcHaxx.Snappy.CLI ` was installed, e.g. `dotnet tool install -g TcHaxx.Snappy.CLI`.
 `-l`<br/>`--log-level` | `no` | `Information`| Minimum `LogEventLevel`, defaults to `Information`.
`--help` | `no` | | Display help screen.
`--version` | `no` | | Display version information.

#### Command `verify`

```sh
TcHaxx.Snappy.CLI verify [OPTIONS]
```
Option  | Required | Default | Description  
--- | --- | --- |  ---
`-d`<br/>`--dir` | `no` | `./TcHaxx.Snappy.Verified` | Directory of verified snapshot files.
`-c`<br/>`--compact-diff` | `no` | `true` | Diff output as compact as possible.
`-p`<br/>`--port` | `no` | `25000` | AmsPort of the Server (snappy).
`-f`<br/>`--fpp` | `no` | `5` | Floating point precision for `REAL`/`LREAL` values.
 `-l`<br/>`--log-level` | `no` | `Information`| Minimum `LogEventLevel`, defaults to `Information`.
`--help` | `no` | | Display help screen.
`--version` | `no` | | Display version information.

# Acknowledgments

* [TcUnit](https://github.com/tcunit/TcUnit) - A unit testing framework for Beckhoff's TwinCAT 3
* [CommandLineParser](https://github.com/commandlineparser/commandline) - A command line parsing library for .NET applications.
* [Verify](https://github.com/VerifyTests/Verify) - A library used for snapshot testing.
* [Serilog](https://github.com/serilog/serilog) - A logging library for .NET applications.