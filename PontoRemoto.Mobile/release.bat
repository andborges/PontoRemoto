@echo off

set appname="PontoRemoto"

call build
copy build\ant.properties platforms\android\ant.properties

cd platforms\android

rmdir bin /s /q
call ant release

cd bin
jarsigner -verbose -sigalg SHA1withRSA -digestalg SHA1 -keystore ..\..\..\build\mobileit-release-key.keystore %appname%-release-unsigned.apk mobileit
zipalign -v 4 %appname%-release-unsigned.apk %appname%.apk

rmdir ..\..\..\release /s /q
mkdir ..\..\..\release

copy %appname%.apk ..\..\..\release\%appname%.apk

cd ..\..\..