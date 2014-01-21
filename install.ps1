write-host " *** Exchange DkimSigner Install Script ***" -f "blue"

# Exchange 2007 SP3 (8.3.*)
# Exchange 2010     (14.0.*)
# Exchange 2010 SP1 (14.1.*)
# Exchange 2010 SP2 (14.2.*)
# Exchange 2010 SP3 (14.3.*)
# Exchange 2013     (15.0.516.32)
# Exchange 2013 CU1 (15.0.620.29)
# Exchange 2013 CU2 (15.0.712.24)
# Exchange 2013 CU3 (15.0.775.38)

$hostname = hostname
$exchserver = Get-ExchangeServer -Identity $hostname
$EXDIR="C:\Program Files\Exchange DkimSigner" 
if (($exchserver.admindisplayversion).major -eq 8 -and ($exchserver.admindisplayversion).minor -eq 3) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2007 SP3"
} elseif (($exchserver.admindisplayversion).major -eq 14 -and ($exchserver.admindisplayversion).minor -eq 0) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2010"
} elseif (($exchserver.admindisplayversion).major -eq 14 -and ($exchserver.admindisplayversion).minor -eq 1) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2010 SP1"
} elseif (($exchserver.admindisplayversion).major -eq 14 -and ($exchserver.admindisplayversion).minor -eq 2) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2010 SP2"
} elseif (($exchserver.admindisplayversion).major -eq 14 -and ($exchserver.admindisplayversion).minor -eq 3) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2010 SP3"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 516) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2013"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 620) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2013 CU1"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 712) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2013 CU2"
} elseif (($exchserver.admindisplayversion).major -eq 15 -and ($exchserver.admindisplayversion).minor -eq 0 -and ($exchserver.admindisplayversion).build -eq 775) {
	$SRCDIR="Src\Exchange.DkimSigner\bin\Exchange 2013 CU3"
}
else {
	throw "The exchange version is undefine."
}

write-host "Creating registry key for EventLog" -f "green"
if (Test-Path "HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM") {
	write-host "Registry key for EventLog already exists. Continuing..." -f "yellow"
} else {
	New-Item -Path "HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM"
}


net stop MSExchangeTransport 
 
write-host "Creating install directory: '$EXDIR' and copying data from '$SRCDIR'"  -f "green"
new-item -Type Directory -path $EXDIR -ErrorAction SilentlyContinue 

copy-item "$SRCDIR\ExchangeDkimSigner.dll" $EXDIR -force 
$overwrite = read-host "Do you want to copy (and overwrite) the config file: '$SRCDIR\ExchangeDkimSigner.dll'? [Y/N]"
if ($overwrite -eq "Y" -or $overwrite -eq "y") {
	copy-item "$SRCDIR\ExchangeDkimSigner.dll.config" $EXDIR -force
} else {
	write-host "Not copying config file" -f "yellow"
}

read-host "Now open '$EXDIR\ExchangeDkimSigner.dll.config' to configure Exchange DkimSigner.\nDon't forget to setup all the keys! When done and saved press 'Return'"

write-host "Registering agent" -f "green"
Install-TransportAgent -Name "Exchange DkimSigner" -TransportAgentFactory "Exchange.DkimSigner.DkimSigningRoutingAgentFactory" -AssemblyPath "$EXDIR\ExchangeDkimSigner.dll"

write-host "Enabling agent" -f "green" 
enable-transportagent -Identity "Exchange DkimSigner" 
get-transportagent 
 
write-host "Starting Edge Transport" -f "green" 
net start MSExchangeTransport 
 
write-host "Installation complete. Check previous outputs for any errors!" -f "yellow" 
