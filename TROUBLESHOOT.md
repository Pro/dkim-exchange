Exchange DKIM Signer Installation Troubleshooting
=============

# Installation script problems

## Could not load file or assembly... HRESULT: 0x80131515

### Error message

```
Registering agent
Could not load file or assembly 'file:///C:\Program Files\Exchange DkimSigner\ExchangeDkimSigner.dll' or one of its
dependencies. Operation is not supported. (Exception from HRESULT: 0x80131515)
    + CategoryInfo          : InvalidArgument: (:) [Install-TransportAgent], FileLoadException
    + FullyQualifiedErrorId : [Server=EXCHANGE2013,RequestId=c4faf8ee-f91c-4548-bd1f-4ebaf6bb0e0d,TimeStamp=02/03/2014
    11:39:42] [FailureCategory=Cmdlet-FileLoadException] E9E2E55C,Microsoft.Exchange.Management.AgentTasks.InstallTra
  nsportAgent
    + PSComputerName        : exchange2013.xxx.xxx.xxx
```

### Solution

check if the .dll's are unlocked:

1. open the folder containing the downloaded .zip contents
2. open the folder `.\Src\Exchange.DkimSigner\bin\`
3. select the corresponding Exchange version
4. for all the .dll files in this folder (`ExchangeDkimSigner.dll`) make sure that they are unlocked:
5. Click Properties
6. Go to the "General" tab
7. Click on "Unlock file"
8. Click OK

![unlock-software-unblock](https://f.cloud.github.com/assets/251973/2304090/064ecbbe-a203-11e3-9b06-892b70bf380e.png)

## The process cannot access the file ...

### Error message

```
copy-item : The process cannot access the file 'C:\Program Files\Exchange DkimSigner\ExchangeDkimSigner.dll' because it is
being used by another process.
```

### Solution

1. Open Powershell
2. Execute `net stop W3SVC`
3. Delete the file `C:\Program Files\Exchange DkimSigner\ExchangeDkimSigner.dll`
4. Execute `net start W3SVC`
5. Rerun the install script