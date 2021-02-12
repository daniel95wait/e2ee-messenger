@ECHO OFF
CD "%~dp0%"
TITLE Update Markdown

REM Checks if the document exists
IF NOT EXIST writeup.docx (
    ECHO Error: No file "writeup.docx" in current directory
    ECHO Move or rename your file and try again
    GOTO CommonExit
)

REM Runs the command to update the markdown 
pandoc writeup.docx ^
    --standalone ^
    --extract-media . ^
    --to gfm-smart ^
    --wrap=none ^
    --reference-links ^
    -o writeup.md

REM Check if the command threw any errors
IF %ERRORLEVEL% NEQ 0 (
    ECHO Error: Check Pandoc is installed
    ECHO Pandoc can be downloaded from https://pandoc.org/installing.html
    GOTO CommonExit
)

REM When all is successful
ECHO Successfully updated writeup.md

:CommonExit
ECHO.
PAUSE
EXIT /b
