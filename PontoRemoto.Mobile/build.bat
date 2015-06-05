@echo off

if "%~1"=="clean" (
	rmdir platforms /s /q
	mkdir platforms

	rmdir plugins /s /q
	mkdir plugins

	call phonegap plugin add https://git-wip-us.apache.org/repos/asf/cordova-plugin-dialogs.git
	call phonegap plugin add https://git-wip-us.apache.org/repos/asf/cordova-plugin-device.git
	call phonegap plugin add https://git-wip-us.apache.org/repos/asf/cordova-plugin-geolocation.git
	call phonegap plugin add org.apache.cordova.network-information
	call phonegap plugin add https://github.com/EddyVerbruggen/Toast-PhoneGap-Plugin.git
)

call phonegap build android