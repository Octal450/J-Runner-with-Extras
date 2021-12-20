@Echo off

:: update_build_datetime.bat <path to build.txt>
:: Updates Build.txt with current datetime in a locale-independent format (always US format)

:: Based off https://superuser.com/a/316153, which in turn is based off http://ss64.com/nt/syntax-getdate.html
:: This version by DaCukiMonsta 20th Dec 2021


:: Check WMIC is available
WMIC.EXE Alias /? >NUL 2>&1 || GOTO s_error

:: Use WMIC to retrieve date and time
FOR /F "skip=1 tokens=1-6" %%G IN ('WMIC Path Win32_LocalTime Get Day^,Hour^,Minute^,Month^,Second^,Year /Format:table') DO (
   IF "%%~L"=="" goto s_done
      Set _yyyy=%%L
      Set _mm=00%%J
      Set _dd=00%%G
      Set _hour=00%%H
      SET _minute=00%%I
      SET _second=00%%K
)
:s_done

:: Pad digits with leading zeros
      Set _mm=%_mm:~-2%
      Set _dd=%_dd:~-2%
      Set _hour=%_hour:~-2%
      Set _minute=%_minute:~-2%
      Set _second=%_second:~-2%

:: Get the datetime in our format:
Set _datetime=%_mm%-%_dd%-%_yyyy% %_hour%:%_minute%.%_second%
Echo %_datetime% > %1