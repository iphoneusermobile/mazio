; setup.nsi
;
; This script is based on example1.nsi, but it remember the directory, 
; has uninstall support and (optionally) installs start menu shortcuts.
;
; It will install mazio.nsi into a directory that the user selects,

!include WordFunc.nsh
!insertmacro VersionCompare
 
!include LogicLib.nsh

!include MUI.nsh

!include x64.nsh

;--------------------------------

; The name of the installer
Name "Mazio"

LicenseData LICENSE

; The file to write
OutFile "bin\Release\mazio-setup.exe"

; The default installation directory
InstallDir $PROGRAMFILES\Kalamon\Mazio

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\Kalamon\Mazio" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------

; Functions

Function .onInit
  Call GetDotNETVersion
  Pop $0
  ${If} $0 == "not found"
    MessageBox MB_OK|MB_ICONSTOP ".NET runtime library is not installed.$\n$\n\
                                 Please update your computer at http://windowsupdate.microsoft.com/"
    Abort
  ${EndIf}
 
  StrCpy $0 $0 "" 1 # skip "v"
 
  ${VersionCompare} $0 "2.0" $1
  ${If} $1 == 2
    MessageBox MB_OK|MB_ICONSTOP ".NET runtime library v2.0 or newer is required. You have $0.$\n$\n\
                                  Please update your computer at http://windowsupdate.microsoft.com/"
    Abort
  ${EndIf}
FunctionEnd

Function GetDotNETVersion
  Push $0
  Push $1
 
  System::Call "mscoree::GetCORVersion(w .r0, i ${NSIS_MAX_STRLEN}, *i) i .r1 ?u"
  StrCmp $1 0 +2
    StrCpy $0 "not found"
 
  Pop $1
  Exch $0
FunctionEnd

Function LaunchLink
  ExecShell "" "$INSTDIR\mazio.exe"
FunctionEnd

; Pages

!define MUI_ICON res\TrayIcon.Icon.ico
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE LICENSE
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
 
!define MUI_FINISHPAGE_NOAUTOCLOSE
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_CHECKED
!define MUI_FINISHPAGE_RUN_TEXT "Start Mazio"
!define MUI_FINISHPAGE_RUN_FUNCTION "LaunchLink"
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_LANGUAGE "English"

;--------------------------------

; The stuff to install
Section "Mazio (required)"

  SectionIn RO
  
  ${If} ${RunningX64}
    SetRegView 64
  ${EndIf}

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "bin\Release\mazio.exe"
  ;File "bin\Release\mazio.exe.manifest"
  File "bin\Release\mazio.exe.config"
  File "bin\Release\mazio.XmlSerializers.dll"
  File "bin\Release\FlickrNet.dll"
  File "bin\Release\Google.GData.Client.dll"
  File "bin\Release\Google.GData.Photos.dll"
  File "bin\Release\Google.GData.Extensions.dll"

  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\Kalamon\Mazio "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Mazio" "DisplayName" "Mazio"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Mazio" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Mazio" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Mazio" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
  ; Register protocol
  WriteRegStr HKCR "mazio" "" "URL:mazio Protocol"
  WriteRegStr HKCR "mazio" "URL Protocol" ""
  WriteRegStr HKCR "mazio\shell\open\command" "" '"$INSTDIR\mazio.exe" "%1"'
  WriteRegStr HKCU "Software\Classes\mazio" "" "URL:mazio Protocol"
  WriteRegStr HKCU "Software\Classes\mazio" "URL Protocol" ""
  WriteRegStr HKCU "Software\Classes\mazio\shell\open\command" "" '"$INSTDIR\mazio.exe" "%1"'

  ; Register file type
  WriteRegStr HKCR ".maz\shell\open\command" "" '"$INSTDIR\mazio.exe" mazio:openfile:"%1"'
  WriteRegStr HKCR ".maz\DefaultIcon" "" "$INSTDIR\mazio.exe,0"
  WriteRegStr HKCR ".maz" "TypeOverlay" "$INSTDIR\mazio.exe,0"
  WriteRegDWORD HKCR ".maz" "Treatment" 2
  ;${If} ${RunningX64}
  ;  WriteRegStr HKCR ".maz\ShellEx\{e357fccd-a995-4576-b01f-234630154e96}" "" "{2A288F88-BF2E-4872-BCA3-B2462C18A811}"
  ;  WriteRegStr HKCR "CLSID\{2A288F88-BF2E-4872-BCA3-B2462C18A811}" "" "DeskBeam Thumbnail Handler"
  ;  WriteRegStr HKCR "CLSID\{2A288F88-BF2E-4872-BCA3-B2462C18A811}\InProcServer32" "" "$INSTDIR\thumbnailhandler64.dll" 
  ;${Else}
  ;  RegDLL $INSTDIR\thumbnailhandler.dll
  ;${EndIf}
  System::Call 'shell32.dll::SHChangeNotify(i, i, i, i) v (0x08000000, 0, 0, 0)'
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\Mazio"
  CreateShortCut "$SMPROGRAMS\Mazio\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\Mazio\Mazio.lnk" "$INSTDIR\mazio.exe" "" "$INSTDIR\mazio.exe" 0
  CreateShortCut "$SMPROGRAMS\Mazio\Mazio in Grabbing Mode.lnk" "$INSTDIR\mazio.exe" "-sg" "$INSTDIR\mazio.exe" 0
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ${If} ${RunningX64}
    SetRegView 64
  ${EndIf}

  ; uninstall file handler
  ${If} ${RunningX64}
    DeleteRegKey HKCR "CLSID\{2A288F88-BF2E-4872-BCA3-B2462C18A811}"
  ${Else}
    UnRegDLL $INSTDIR\thumbnailhandler.dll
  ${endIf}
	
  DeleteRegKey HKCR ".maz"

  System::Call 'shell32.dll::SHChangeNotify(i, i, i, i) v (0x08000000, 0, 0, 0)'

  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Mazio"
  DeleteRegKey HKLM SOFTWARE\Mazio

  ; Unregister protocol
  DeleteRegKey HKCR "mazio"
  DeleteRegKey HKCU "Software\Classes\Mazio"
  
  ; Remove files and uninstaller
  Delete $INSTDIR\mazio.exe
  Delete $INSTDIR\FlickrNet.dll
  Delete $INSTDIR\mazio.exe.config
  ;Delete $INSTDIR\mazio.exe.manifest
  Delete $INSTDIR\mazio.XmlSerializers.dll
  Delete $INSTDIR\GoogleGDataClient.dll
  Delete $INSTDIR\GoogleGDataPhotos.dll
  Delete $INSTDIR\GoogleGDataExtensions.dll

  Delete $INSTDIR\uninstall.exe

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\Mazio\*.*"

  ; Remove directories used
  RMDir "$SMPROGRAMS\Mazio"
  RMDir "$INSTDIR"

SectionEnd