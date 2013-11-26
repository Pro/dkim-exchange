write-host " *** Exchange DkimSigner UNINSTALL Script ***" -f "blue"

$EXDIR="C:\Program Files\Exchange DkimSigner" 
 
Net Stop MSExchangeTransport 
 
write-host "Disabling agent..."  -f "green"
Disable-TransportAgent -Identity "Exchange DkimSigner" 

write-host "Uninstalling agent..."  -f "green"
Uninstall-TransportAgent -Identity "Exchange DkimSigner" 
 
write-host "Deleting directories and files..." -f "green"
Remove-Item $EXDIR\* -Recurse -Force -ErrorAction SilentlyContinue 
Remove-Item $EXDIR -Recurse -Force -ErrorAction SilentlyContinue 

write-host "Removing registry key for EventLog" -f "green"
if (Test-Path "HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DkimSigner") {
	Remove-Item -Path "HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DkimSigner"
} else {
	write-host "Key already removed. Continuing..." -f "yellow"
}


write-host "Starting Transport..."  -f "green"
Net Start MSExchangeTransport 
 
write-host "Uninstallation complete. Check previous outputs for any errors!"  -f "yellow"