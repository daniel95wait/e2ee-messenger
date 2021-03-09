@ECHO OFF
CD "%~dp0%"
TITLE Update Markdown

SET input=writeup.docx
SET output=README.md

REM Checks if the document exists
IF NOT EXIST %input% (
    ECHO Error: No file %input% in current directory
    ECHO Move or rename your file and try again
    GOTO CommonExit
)

REM Runs the command to update the markdown 
pandoc %input% ^
    --standalone ^
    --extract-media . ^
    --to gfm-smart ^
    --wrap=none ^
    --reference-links ^
    -o %output%

REM Check if the command threw any errors
IF %ERRORLEVEL% NEQ 0 (
    ECHO Error: Check Pandoc is installed
    ECHO Pandoc can be downloaded from https://pandoc.org/installing.html
    GOTO CommonExit
)

REM When all is successful
ECHO Successfully updated %output%

:CommonExit
ECHO.
PAUSE
EXIT /b
