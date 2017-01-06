## ARM-Analyzer : Backward Taint Analyzer for ARM architecture ##

### Abstract ###

We have developed a set of tools for analyzing crashes occurred on Linux OS and ARM architecture to determine exploitability with taint analysis for practical use. For the analysis, Dynamic Binary Instrumentation (DBI) is required to extract context information of each instruction at runtime. Unfortunately, most existing DBI tools have been developed for x86 architecture. Therefore, we have developed a dedicated tool called [ARM-Tracer](https://github.com/scotty-kdw/ARM-Tracer#arm-tracer "ARM-Tracer") based on ptrace system call. ARM-Tracer can dynamically trace a specific thread in the multi-threaded environment and generate a trace log until the target gets crashed. Then, the trace log is analyzed by another tool that we have developed to perform the backward taint analysis on Desktop for efficiency. The tool is named [ARM-Analyzer](https://github.com/scotty-kdw/ARM-Analyzer#arm-analyzer "ARM-Analyzer"), a stand-alone GUI application developed in C# language, which lets us know whether the crash is affected by the input data. For this, we analyzed ARM instructions to identify taint objects of each instruction. 

### Our Tools ###

[**▼ ARM-Tracer**](https://github.com/scotty-kdw/ARM-Tracer#arm-tracer "ARM-Tracer")  - Trace Log Generation (CLI) on a target device or emulator : Generating context information of every instruction from a specific point (input data) to a crash point

![ARM-Tracer](https://raw.githubusercontent.com/scotty-kdw/ARM-Tracer/master/tutorial/ARM_Tracer.png)

[**▼ ARM-Analyzer**](https://github.com/scotty-kdw/ARM-Analyzer#arm-analyzer "ARM-Analyzer") - Backward Taint Analysis (GUI) on Desktop : Analyzing trace log to determine exploitability by tracking data propagation 

![ARM-Analyzer](https://raw.githubusercontent.com/scotty-kdw/ARM-Tracer/master/tutorial/ARM_Analyzer.png)

##

**Compile for ARM-Tracer** 

    $ sudo apt-get install gcc-arm-linux-androideabi
    
    $ ./make


##

**Thanks to**

- VDT (Visual Data Tracer) : Crash analyzer for user level applications on Windows OS (x86) `Our motivation`
- Capstone : Pretty good multi-architecture disassembler

