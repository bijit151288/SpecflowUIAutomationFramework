@echo off
rem   just kills stray local chromedriver.exe instances.
rem   useful if you are trying to clean your project, and your ide is complaining.

taskkill /F /IM chromeDriver.exe /T
taskkill /F /IM geckodriver.exe /T
taskkill /F /IM MicrosoftWebDriver.exe /T
taskkill /f /IM msedgedriver.exe