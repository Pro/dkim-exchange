write-host "*** Exchange DkimSigner Install Script ***" -f "blue"

# Exchange 2007 SP3 (8.3.*)
# Exchange 2007 SP3 RU21 (8.3.298.*)
# Exchange 2010     (14.0.*)
# Exchange 2010 SP1 (14.1.*)
# Exchange 2010 SP2 (14.2.*)
# Exchange 2010 SP3 (14.3.*)
# Exchange 2013     (15.0.516.32)
# Exchange 2013 CU1 (15.0.620.29)
# Exchange 2013 CU2 (15.0.712.24)
# Exchange 2013 CU3 (15.0.775.38)
# Exchange 2013 SP1 CU4 (15.0.847.32)
# Exchange 2013 SP1 CU5 (15.0.913.22)
# Exchange 2013 SP1 CU6 (15.0.995.29)
# Exchange 2013 SP1 CU7 (15.0.1044.25)
# Exchange 2013 SP1 CU8 (15.0.1076.9)
# Exchange 2013 SP1 CU9 (15.0.1104.5)
# Exchange 2013 SP1 CU10 (15.0.1130.7)
# Exchange 2013 SP1 CU11 (15.0.1156.6)
# Exchange 2013 SP1 CU12 (15.0.1178.4)
# Exchange 2013 SP1 CU13 (15.0.1210.3)
# Exchange 2013 SP1 CU14 (15.0.1236.3)
# Exchange 2013 SP1 CU15 (15.0.1263.5)
# Exchange 2013 SP1 CU16 (15.0.1293.2)
# Exchange 2013 SP1 CU17 (15.0.1320.4)
# Exchange 2013 SP1 CU18 (15.0.1347.2)
# Exchange 2013 SP1 CU19 (15.0.1365.1)
# Exchange 2013 SP1 CU20 (15.0.1367.3)
# Exchange 2013 SP1 CU21 (15.0.1395.4)
# Exchange 2016 Preview	 (15.1.225.17)
# Exchange 2016 RTM	     (15.1.225.42)
# Exchange 2016 CU1	     (15.1.396.30)
# Exchange 2016 CU2	     (15.1.466.34)
# Exchange 2016 CU3	     (15.1.544.27)
# Exchange 2016 CU4	     (15.1.669.32)
# Exchange 2016 CU5	     (15.1.845.34)
# Exchange 2016 CU6	     (15.1.1034.26)
# Exchange 2016 CU7	     (15.1.1261.35)
# Exchange 2016 CU8	     (15.1.1415.2)
# Exchange 2016 CU9	     (15.1.1466.3)
# Exchange 2016 CU10	     (15.1.1531.3)

write-host "Detecting Exchange version ... " -f "cyan"
$hostname = hostname
$exchserver = Get-ExchangeServer -Identity $hostname
$EXDIR="C:\Program Files\Exchange DkimSigner"
$EXVER="Unknown"
if (($exchserver.admindisplayversion).major -eq 8 -and ($exchserver.admindisplayversion).minor -eq 3 -and ($exchserver.admindisplayversion).build -eq 298) {
	$EXVER="Exchange 2007 SP3 RU21"
} elseif (($exchserver.admindisplayversion).major -eq 8 -and ($exchserver.admindisplayversion).minor -eq 3) {
	$EXVER="Exchange 2007 SP3"
} elseif (($exchserver.admindisplayversion).major -eq 14 -and ($exchserver.admindisplayversion).minor -eq 0) {
	$EXVER="Exchange 2010"
} elseif (($exchserver.admindisplayversion).major -eq 14 -and ($exchserver.admindisplayversion).minor -eq 1) {
	$EXVER="Exchange 2010 SP1"
} elseif (($exchserver.admindisplayversion).major -eq 14 -and ($exchserver.admindisplayversion).minor -eq 2) {
	$EXVER="Exchange 2010 SP2"
} elseif (($exchserver.admindisplayversion).major -eq 14 -and ($exchserver.admindisplayversion).minor -eq 3) {
	$EXVER="Exchange 2010 SP3"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 516) {
	$EXVER="Exchange 2013"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 620) {
	$EXVER="Exchange 2013 CU1"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 712) {
	$EXVER="Exchange 2013 CU2"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 775) {
	$EXVER="Exchange 2013 CU3"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 847) {
	$EXVER="Exchange 2013 SP1 CU4"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 913) {
	$EXVER="Exchange 2013 SP1 CU5"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 995) {
	$EXVER="Exchange 2013 SP1 CU6"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1044) {
	$EXVER="Exchange 2013 SP1 CU7"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1076) {
	$EXVER="Exchange 2013 SP1 CU8"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1104) {
	$EXVER="Exchange 2013 SP1 CU9"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1130) {
	$EXVER="Exchange 2013 SP1 CU10"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1156) {
	$EXVER="Exchange 2013 SP1 CU11"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1178) {
	$EXVER="Exchange 2013 SP1 CU12"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1210) {
	$EXVER="Exchange 2013 SP1 CU13"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1236) {
	$EXVER="Exchange 2013 SP1 CU14"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1263) {
	$EXVER="Exchange 2013 SP1 CU15"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1293) {
	$EXVER="Exchange 2013 SP1 CU16"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1320) {
	$EXVER="Exchange 2013 SP1 CU17"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1347) {
	$EXVER="Exchange 2013 SP1 CU18"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1365) {
	$EXVER="Exchange 2013 SP1 CU19"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1367) {
	$EXVER="Exchange 2013 SP1 CU20"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 1395) {
	$EXVER="Exchange 2013 SP1 CU21"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 1 -and ($exchserver.admindisplayversion).build -eq 225 -and ($exchserver.admindisplayversion).revision -eq 17) {
	$EXVER="Exchange 2016 Preview"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 1 -and ($exchserver.admindisplayversion).build -eq 225 -and ($exchserver.admindisplayversion).revision -eq 42) {
	$EXVER="Exchange 2016 RTM"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 1 -and ($exchserver.admindisplayversion).build -eq 396) {
	$EXVER="Exchange 2016 CU1"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 1 -and ($exchserver.admindisplayversion).build -eq 466) {
	$EXVER="Exchange 2016 CU2"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 1 -and ($exchserver.admindisplayversion).build -eq 544) {
	$EXVER="Exchange 2016 CU3"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 1 -and ($exchserver.admindisplayversion).build -eq 669) {
	# same as CU3
	$EXVER="Exchange 2016 CU3"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 1 -and ($exchserver.admindisplayversion).build -eq 845) {
	$EXVER="Exchange 2016 CU5"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 1 -and ($exchserver.admindisplayversion).build -eq 1034) {
	$EXVER="Exchange 2016 CU6"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 1 -and ($exchserver.admindisplayversion).build -eq 1261) {
	$EXVER="Exchange 2016 CU7"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 1 -and ($exchserver.admindisplayversion).build -eq 1415) {
	$EXVER="Exchange 2016 CU8"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 1 -and ($exchserver.admindisplayversion).build -eq 1466) {
	$EXVER="Exchange 2016 CU9"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 1 -and ($exchserver.admindisplayversion).build -eq 1531) {
	$EXVER="Exchange 2016 CU10"
} else {
	throw "The exchange version is not yet supported: " + $exchserver.admindisplayversion
}

$SRCDIR="Src\Exchange.DkimSigner\bin\$EXVER"

write-host "Found $EXVER" -f "green"

write-host "Creating registry key for EventLog" -f "green"
if (Test-Path "HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM") {
	write-host "Registry key for EventLog already exists. Continuing..." -f "yellow"
} else {
	New-Item -Path "HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM"
	New-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM" -Name EventMessageFile -PropertyType String -Value "C:\Windows\Microsoft.NET\Framework\v4.0.30319\EventLogMessages.dll"
}


net stop MSExchangeTransport

write-host "Creating install directory: '$EXDIR' and copying data from '$SRCDIR'"  -f "green"
new-item -Type Directory -path $EXDIR -ErrorAction SilentlyContinue

copy-item "$SRCDIR\*" $EXDIR -force
copy-item "Src\Configuration.DkimSigner\bin\Release\*" $EXDIR -force
$overwrite = read-host "Do you want to copy (and overwrite) the config file: '$SRCDIR\settings.xml'? [Y/N]"
if ($overwrite -eq "Y" -or $overwrite -eq "y") {
	copy-item "Src\Exchange.DkimSigner\settings.xml" $EXDIR -force
} else {
	write-host "Not copying config file" -f "yellow"
}

# Unblocks files that were downloaded from the Internet.
unblock-file "$EXDIR\ExchangeDkimSigner.dll"
unblock-file "$EXDIR\ExchangeDkimSigner.pdb"
unblock-file "$EXDIR\settings.xml"

read-host "Now open '$EXDIR\Configuration.DkimSigner.exe' to configure Exchange DkimSigner. Don't forget to setup all the keys! When done and saved press 'Return'"

write-host "Registering agent" -f "green"
Install-TransportAgent -Name "Exchange DkimSigner" -TransportAgentFactory "Exchange.DkimSigner.DkimSigningRoutingAgentFactory" -AssemblyPath "$EXDIR\ExchangeDkimSigner.dll"

write-host "Enabling agent" -f "green"
enable-transportagent -Identity "Exchange DkimSigner"
get-transportagent

write-host "Starting Edge Transport" -f "green"
net start MSExchangeTransport

write-host "Installation complete. Check previous outputs for any errors!" -f "yellow"
