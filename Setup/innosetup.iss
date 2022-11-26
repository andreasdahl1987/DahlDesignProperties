#define PluginVersion '1.11.0'
#define LEDVersion '1.1.0'

[Setup]
AppName=Dahl Design Dashboard & Plugin
AppVersion={#PluginVersion}
OutputBaseFilename=DahlDesignSetup
WizardStyle=modern
DefaultDirName={autopf}\SimHub
Compression=lzma2
SolidCompression=yes

;Disable warning that output directory already exists
DirExistsWarning=no 
;OutputDir= userdocs:Inno Setup Examples Output

[Files]
Source: "..\obj\Debug\DahlDesign.dll"; DestDir: "{app}"
Source: "{tmp}\DahlDesignLED.dll"; DestDir: "{app}"; Flags: external
Source: "{tmp}\DahlDesignDDU.simhubdash"; DestDir: "{app}"; Flags: external deleteafterinstall

[Run]
Filename: "{app}\DahlDesignDDU.simhubdash"; Flags: shellexec waituntilterminated

[Code]
var
  DownloadPage: TDownloadWizardPage;
function OnDownloadProgress(const Url, FileName: String; const Progress, ProgressMax: Int64): Boolean;
begin
  if Progress = ProgressMax then
    Log(Format('Successfully downloaded file to {tmp}: %s', [FileName]));
  Result := True;
end;
procedure InitializeWizard;
begin
  DownloadPage := CreateDownloadPage(SetupMessage(msgWizardPreparing), SetupMessage(msgPreparingDesc), @OnDownloadProgress);
end;
function NextButtonClick(CurPageID: Integer): Boolean;
begin
  if CurPageID = wpReady then begin
    DownloadPage.Clear;
    DownloadPage.Add('https://github.com/andreasdahl1987/DahlDesignDash/releases/download/{#PluginVersion}/DahlDesignDDU.simhubdash', 'DahlDesignDDU.simhubdash', '');    
    DownloadPage.Add('https://github.com/andreasdahl1987/DahlDesignLED/releases/download/{#LEDVersion}/DahlDesignLED.dll', 'DahlDesignLED.dll', '');    
    
    DownloadPage.Show;
    try
      try
        DownloadPage.Download; // This downloads the files to {tmp}
        Result := True;
      except
        if DownloadPage.AbortedByUser then
          Log('Aborted by user.')
        else
          SuppressibleMsgBox(AddPeriod(GetExceptionMessage), mbCriticalError, MB_OK, IDOK);
        Result := False;
      end;
    finally
      DownloadPage.Hide;
    end;
  end else
    Result := True;
end;