$Arguments = "/uninstall /startManually /serviceName:{0} /displayName:{0}  NServiceBus.Production /username:{1} /password:{2} /description:{3}' -f 'lacjam.servicebus', 'MyUsername', 'MyPassword', 'www.mckelt.com'  "

$nServiceBus = Resolve-Path -Path nServiceBus.Host.exe;

Write-Host -Object ('Argument string is: {0}' -f $Arguments);
Write-Host -Object ('Path to nServiceBus.Host.exe is: {0}' -f $nServiceBus);
Start-Process -Wait -NoNewWindow -FilePath $nServiceBus -ArgumentList $Arguments -RedirectStandardOutput tmp.txt;

## output - nservicebus.host.exe /install /startManually /serviceName:lacjam.servicebus /displayName:lacjam.servicebus  NServiceBus.Production /username:"cmckelt" /password:password /description:www.mckelt.com
                                                                                                                                                                             
