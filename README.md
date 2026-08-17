# mstsc2

`mstsc2.exe` is a wrapper around Microsoft Remote Desktop Connection (`mstsc.exe`). It consumes the optional `/dpi:` argument, forwards all remaining arguments to `mstsc.exe`, and injects an EasyHook DLL that overrides the DPI returned by `GetDpiForMonitor()`.

## Usage

Launch Remote Desktop with the default DPI (**96**, 100% scaling):

```cmd
mstsc2.exe
```

Launch with a custom DPI:

```cmd
mstsc2.exe /dpi:144
```

Launch with standard MSTSC arguments:

```cmd
mstsc2.exe /dpi:144 /v:server01 /f
```

Open an RDP file:

```cmd
mstsc2.exe /dpi:192 MyConnection.rdp
```

`mstsc2.exe` processes only the `/dpi:` argument.

All other command-line arguments are passed unchanged to `mstsc.exe`.

## Common DPI Values

| DPI  | Scaling |
|------|---------|
|   96 |    100% |
|  120 |    125% |
|  144 |    150% |
|  168 |    175% |
|  192 |    200% |
|  288 |    300% |

## How It Works

1. `mstsc2.exe` parses the command-line arguments.
2. The `/dpi:` argument is extracted.
3. The `/dpi:` argument is removed from the command line.
4. All remaining arguments are forwarded to `mstsc.exe`.
5. `mstsc.exe` is started.
6. EasyHook injects the hook DLL.
7. The hook intercepts calls to `GetDpiForMonitor()`.
8. The original DPI value is replaced with the configured DPI.

## Build Requirements

- Visual Studio 2019
- .NET Framework 4.8
- EasyHook 2.7.7097

---

Michael Wollensack - 17.08.2026
